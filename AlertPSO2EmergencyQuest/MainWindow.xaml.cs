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

        public MainWindow(EventPSO2 pso)
        {
            InitializeComponent();

            pso.TimeTick+=this.timer_Tick;

            this.messageText.Text = pso.GetCurentEventMessageTxt();
            this.StateChanged+=new EventHandler(MainWindow_StateChanged);
        }
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // クローズ処理をキャンセルして、タスクバーの表示も消す
            e.Cancel = true;
            this.WindowState = System.Windows.WindowState.Minimized;
            this.ShowInTaskbar = false;
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
            this.textBlock1.Text = (sender as AlertPSO2EmergencyQuest.Model.EventPSO2).TiemTick;
            this.messageText.Text = (sender as AlertPSO2EmergencyQuest.Model.EventPSO2).MessageTxt;
        }

        private void MaxButton_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow setWindow = new SettingWindow();

            setWindow.ShowDialog();
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            
            switch (this.WindowState)
            {
                case System.Windows.WindowState.Normal:
                    Console.WriteLine("普通の大きさ");
                    break;
                case System.Windows.WindowState.Minimized:
                    Console.WriteLine("最小化");
                    break;
                case System.Windows.WindowState.Maximized:
                    Console.WriteLine("最大化");
                    break;
            }
        }
        public static RoutedEvent SampleEvent = EventManager.RegisterRoutedEvent(
        "Sample", RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(MainWindow));

        public event RoutedEventHandler Sample
        {
            add { this.AddHandler(SampleEvent, value); }
            remove { this.RemoveHandler(SampleEvent, value); }
        }


    }
}
