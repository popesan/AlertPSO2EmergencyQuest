using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AlertPSO2EmergencyQuest.Model
{
    internal class CommPSO2Bot
    {
        internal enum status{
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

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal CommPSO2Bot()
        {
            botName = "pso2_emg_ship10";
            twitterAccess = new CommTwitterizer();
            beforeTime = DateTime.Now;
            this.intervalMinutes = 3;
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
            if (TimeSpan.FromTicks(DateTime.Now.Ticks - beforeTime.Ticks).Seconds
                > this.intervalMinutes)
            //if (TimeSpan.FromTicks(DateTime.Now.Ticks-beforeTime.Ticks).Minutes
            //    > this.intervalMinutes)
            {
                beforeTime = DateTime.Now;
                DateTime before5Min = DateTime.Now.AddMinutes(-5);
                DateTime after5Min = DateTime.Now.AddMinutes(5);
                foreach (var data in twitterAccess.GetUserTimeline(this.botName))
                {
                    this.parseStatus(data.Text);
                    if ((before5Min < data.CreatedDate) && (data.CreatedDate < after5Min))
                    {
                        Console.WriteLine(data.Text);
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// botのツイートをパーシングします
        /// </summary>
        /// <param name="statusStr"></param>
        internal void parseStatus(string statusStr)
        {
            string dataMatch=@"([0-9]|([0-1][0-9]|[2][0-3]))[:][0-5][0-9].*";
            if (System.Text.RegularExpressions.Regex.IsMatch(
                statusStr,dataMatch))
            {
                //時間っぽい文字列をすべて抽出する
                System.Text.RegularExpressions.MatchCollection mc =
                System.Text.RegularExpressions.Regex.Matches(statusStr, dataMatch);
                Console.WriteLine("郵便番号が含まれています");
                foreach (var data in mc)
                {
                    string test = data.ToString();
                    DateTime date1 = DateTime.Parse(test);
                }
            }




        }
    }
}
