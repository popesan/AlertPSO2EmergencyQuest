using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Threading;

namespace AlertPSO2EmergencyQuest.Model
{
    public class EventPSO2
    {
        private string messageTxt;
        /// <summary>
        /// イベント状況のテキスト出力
        /// </summary>
        public string MessageTxt
        {
            get { return messageTxt; }
        }

        private string timeTick;
        /// <summary>
        /// 現在の時間出力
        /// </summary>
        public string TiemTick
        {
            get { return timeTick; }
        }
        /// <summary>
        /// クエスト発生イベント
        /// </summary>
        public event EventHandler Quest;

        public event EventHandler TimeTick;

        private DispatcherTimer timer;

        private CommPSO2Bot bot;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EventPSO2()
        {
            timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);

            bot = new CommPSO2Bot();
            this.messageTxt= GetCurentEventMessageTxt();
            this.beforeStatus = bot.CurrentStatus;
        }

        public void Start()
        {
            timer.Start();
        }

        /// <summary>
        /// timer_tickerイベント発生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            this.timeTick = DateTime.Now.ToString("HH:mm:ss");
            TimeTick(this, EventArgs.Empty);
            if (checkEvent())
            {
                Quest(this, EventArgs.Empty);
            }
        }

        public string GetCurentEventMessageTxt()
        {
            bot.GetStatus();
            return this.outputStatusTxt(bot.CurrentStatus);
        }

        CommPSO2Bot.status beforeStatus;
        //private int counter = 0;

        /// <summary>
        /// イベント状況の確認をおこないます
        /// </summary>
        /// <returns></returns>
        private bool checkEvent()
        {
            bool output=false;
            bot.GetStatus();

            if (bot.CurrentStatus != beforeStatus)
            {
                beforeStatus=bot.CurrentStatus;
                output=true;
                this.messageTxt = outputStatusTxt(bot.CurrentStatus);
            }
#warning デバック用
            /*
            if (counter == 10)
            {
                counter = 0;
                output = true;
            }
            counter++;
            //ここまで
            */
            return output;
        }
        /// <summary>
        /// イベント状況出力メッセージ作成を行います
        /// </summary>
        /// <returns></returns>
        private string outputStatusTxt(CommPSO2Bot.status currentStatus)
        {
            string output = "";

            switch (currentStatus)
            {
                case CommPSO2Bot.status.OverTimeQuest:
                    output = "現在クエストは行われておりません";
                    break;
                case CommPSO2Bot.status.GetReady:
                    output = bot.FromTime.ToString() + "より" + bot.Event1 + "-" + bot.Event2 + "が行われます";
                    break;
                case CommPSO2Bot.status.InQuest:
                    output = bot.FromTime.ToString() + "～" + bot.ToTime.ToString() + "期間中" + bot.Event1 + "-" + bot.Event2 + "が行われております";
                    break;
            }
            return output;
        }
    }
}
