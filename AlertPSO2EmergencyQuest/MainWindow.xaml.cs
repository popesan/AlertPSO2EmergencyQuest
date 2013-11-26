using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;
using AlertPSO2EmergencyQuest.Model;

namespace AlertPSO2EmergencyQuest
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        CommPSO2Bot bot;        
        public MainWindow()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();

            bot = new CommPSO2Bot();
            this.messageText.Text = outputStatusTxt();
        }

        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Windows.Shell.SystemCommands.MinimizeWindow(this);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Windows.Shell.SystemCommands.CloseWindow(this);
        }
        void timer_Tick(object sender, EventArgs e)
        {
            this.textBlock1.Text = DateTime.Now.ToString("HH:mm:ss");
            this.messageText.Text = outputStatusTxt();

        }

        private string outputStatusTxt()
        {
            string output = "";
            bot.GetStatus();
            
            switch (bot.CurrentStatus)
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
        private void MaxButton_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow setWindow = new SettingWindow();

            setWindow.ShowDialog();
        }
    }
}
