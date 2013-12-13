using System;
using System.Configuration;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AlertPSO2EmergencyQuest.Model
{
    public  class CommPSO2Bot
    {
        public  enum status{
            /// <summary>
            /// クエスト準備中
            /// </summary>
            GetReady,
            /// <summary>
            /// クエスト中
            /// </summary>
            InQuest,
            /// <summary>
            /// クエスト時間外
            /// </summary>
            OverTimeQuest,
        }
        private CommTwitterizer twitterAccess;

        private String botName;

        private DateTime beforeTime;

        private double intervalMinutes;
        /// <summary>
        /// botクロール間隔[min]
        /// </summary>
        public double IntervalMinutes
        {
            get { return intervalMinutes; }
            set { intervalMinutes = value; }
        }

        private DateTime fromTime;
        /// <summary>
        /// イベント期間：～から
        /// </summary>
        public DateTime FromTime
        {
            get { return fromTime; }
        }

        private DateTime toTime;
        /// <summary>
        /// イベント期間：～まで
        /// </summary>
        public DateTime ToTime
        {
            get { return toTime; }
        }

        private string event1;
        /// <summary>
        /// イベント名：大分類
        /// </summary>
        public string Event1
        {
            get { return event1; }
        }
        private string event2;
        /// <summary>
        /// イベント名：小分類
        /// </summary>
        public string Event2
        {
            get { return event2; }
        }
        private status currentStatus;
        /// <summary>
        /// 現在のステータス
        /// </summary>
        public status CurrentStatus
        {
            get { return currentStatus; }
        }
        
        private string selectShipName;
        /// <summary>
        /// 現在選択されているship名
        /// </summary>
        public string SelectShipName
        {
            get { return selectShipName; }
            set { selectShipName = value; }
        }
        private Dictionary<string, string> botUrls;
        /// <summary>
        /// ship名にひもづけられたbotのUrl
        /// </summary>
        public Dictionary<string, string> BotUrls
        {
            get { return botUrls; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public  CommPSO2Bot()
        {
#warning タイムライン名をデバッグのため変更tritri ‏@sakatri
            //botName = "sakatri";
            botName = "pso2_emg_ship10";
            twitterAccess = new CommTwitterizer();
            beforeTime = DateTime.Now;
            this.intervalMinutes = 1;
            this.currentStatus = status.OverTimeQuest;


            this.selectShipName = Properties.Settings.Default.saveShipName;
            var shipsBotUrl 
                = new List<string>
                    (ConfigurationManager.AppSettings.Get("shipsBotUrl").Split(new char[] { ';' }));
            botUrls = new Dictionary<string, string>();
            int counter=1;
            foreach (var url in shipsBotUrl)
            {
                //botアプリ内でのship名は"ship+ship番号"となります
                botUrls.Add("ship" + counter.ToString(), url);
                counter++;
            }

        }

        /// <summary>
        /// botのステータスを取得します
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        internal bool GetStatus()
        {
            bool output = true;
#warning デバッグ用
            //if (TimeSpan.FromTicks(DateTime.Now.Ticks - beforeTime.Ticks).Seconds
            //    > this.intervalMinutes)
            if (TimeSpan.FromTicks(DateTime.Now.Ticks-beforeTime.Ticks).Minutes
                >= this.intervalMinutes)
            {
                beforeTime = DateTime.Now;
                DateTime before1hour = DateTime.Now.AddHours(-1);
                var tweetDates = from x in twitterAccess.GetUserTimeline(this.botName)
                           orderby x.CreatedDate descending
                           where (before1hour <x.CreatedDate)
                           select x.Text;

                foreach (var tweetData in tweetDates)
                {
#warning デバッグ用
                    this.parseStatus(tweetData);
                    break;
                    /*
                    if ((before5Min < data.CreatedDate) && (data.CreatedDate < after5Min))
                    {
                        this.parseStatus(data.Text);
                    }*/
                }
            }
            return output;
        }

        /// <summary>
        /// botのツイートをパーシングしステータスを取得します
        /// </summary>
        /// <param name="statusStr"></param>
        internal void parseStatus(string statusStr)
        {
            DateTime nowtime = DateTime.Now;
#warning デバッグ用コード
            //nowtime = DateTime.Parse("2013/11/30 22:10:00");

            List<DateTime> perseData =
                perseDateTime(statusStr);
            perseData.Sort((a, b) => a.CompareTo(b));
            if (perseData.Count == 2)
            {
                fromTime = perseData[0];
                DateTime formTimeBefore30min = perseData[0];
                formTimeBefore30min=formTimeBefore30min.AddMinutes(-30.0);
                toTime = perseData[1];
                if ((formTimeBefore30min.Ticks < nowtime.Ticks)
                    && (nowtime.Ticks < fromTime.Ticks))
                {
                    this.currentStatus = status.GetReady;
                }
                else if ((fromTime.Ticks <= nowtime.Ticks)
                   && (nowtime.Ticks <= toTime.Ticks))
                {
                    this.currentStatus = status.InQuest;
                }
                else
                {
                    this.currentStatus = status.OverTimeQuest;
                }

            }
            if (perseEvent(statusStr, out event1, out event2))
            {

            }
            else
            {

            }

        }

        /// <summary>
        /// イベントをツイートからperseします
        /// </summary>
        /// <param name="statusStr"></param>
        /// <param name="bigEvent"></param>
        /// <param name="smallEvent"></param>
        /// <returns></returns>
        internal bool perseEvent(string statusStr, out string bigEvent, out string smallEvent)
        {
            bool output=false;
            bigEvent = "";
            smallEvent = "";

            string event1Match = @"【[^】]+】";//【】で囲まれた部分を抽出する正規表現
            string event2Match = @"（[^）]+）";//（）で囲まれた部分を抽出する正規表現

            if (System.Text.RegularExpressions.Regex.IsMatch(
                statusStr, event1Match))
            {
                System.Text.RegularExpressions.MatchCollection me1 =
                    System.Text.RegularExpressions.Regex.Matches(statusStr, event1Match);

                foreach (var data in me1)
                {
                    bigEvent = data.ToString();
                    output = true;
                }
            }
            if (System.Text.RegularExpressions.Regex.IsMatch(
                statusStr, event2Match))
            {
                System.Text.RegularExpressions.MatchCollection me2 =
                    System.Text.RegularExpressions.Regex.Matches(statusStr, event2Match);

                foreach (var data in me2)
                {
                    smallEvent = data.ToString();
                    output = true;
                }
            }
            return output;
        }
        /// <summary>
        /// ツイートの中にある時間データをperseします
        /// </summary>
        /// <param name="statusStr"></param>
        /// <returns></returns>
        internal List<DateTime> perseDateTime(string statusStr)
        {
            List<DateTime> output = new List<DateTime>();
            string timeMatch = @"([0-9]|([0-1][0-9]|[2][0-3]))[:][0-5][0-9]+";//hh:mm形式の時間抽出正規表現
            string dataMatch = @"(\[([1-9]|[1][0-2])/((3[01]|[1-2][0-9])|[0][1-9])\])+";//[MM:DD]形式の日付抽出正規表現
            if (System.Text.RegularExpressions.Regex.IsMatch(
                statusStr, dataMatch))
            {
                string tDay = "";
                System.Text.RegularExpressions.MatchCollection md =
                    System.Text.RegularExpressions.Regex.Matches(statusStr, dataMatch);

                foreach (var data in md)
                {
                    tDay = data.ToString().Replace(@"[", "");
                    tDay = tDay.Replace(@"]", "");
                    tDay = DateTime.Now.Year.ToString() + "/" + tDay;
                }

                if (System.Text.RegularExpressions.Regex.IsMatch(
                    statusStr, timeMatch))
                {
                    //時間っぽい文字列をすべて抽出する
                    System.Text.RegularExpressions.MatchCollection mh =
                    System.Text.RegularExpressions.Regex.Matches(statusStr, timeMatch);
                    foreach (var time in mh)
                    {
                        output.Add(DateTime.Parse(tDay + " " + time.ToString()));
                    }
                }
            }
            return output;
        }
    }
}
