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

using System.Windows.Media.Animation;  
using AlertPSO2EmergencyQuest.Model;

namespace AlertPSO2EmergencyQuest
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        EventPSO2 pso;
        public MainWindow(EventPSO2 pso)
        {
            InitializeComponent();

            pso.TimeTick+=this.timer_Tick;

            this.messageText.Text = pso.GetCurentEventMessageTxt();
            this.StateChanged+=new EventHandler(MainWindow_StateChanged);
            this.pso = pso;
        }
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // クローズ処理をキャンセルして、タスクバーの表示も消す
            e.Cancel = true;

            
            /*
            this.WindowState = System.Windows.WindowState.Minimized;
            this.ShowInTaskbar = false;*/
            ClosingWindow();
        }

        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Windows.Shell.SystemCommands.MinimizeWindow(this);
            this.windowBase.Width = 0;
            this.windowBase.Height = 0;
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

        private void ResizeButton_Click(object sender, RoutedEventArgs e)
        {
            // Double型で10から200までアニメーションさせる  
            var animation = new DoubleAnimation  
            {  
                From = 10,  
                To = 400  
            };
            this.windowBase.BeginAnimation(MainWindow.WidthProperty, animation);
        }
        private Storyboard myStoryboard;
        public void ShowWindow()
        {
            
            this.Show();

            //キーフレームで時間を区切る
            var frame0 = new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(new TimeSpan(1)));
            var frame1 = new EasingDoubleKeyFrame(410, KeyTime.FromTimeSpan(new TimeSpan(5000000)));
            var frame2 = new EasingDoubleKeyFrame(0,   KeyTime.FromTimeSpan(new TimeSpan(5000001)));
            var frame3 = new EasingDoubleKeyFrame(150, KeyTime.FromTimeSpan(new TimeSpan(10000000)));
            //キーフレームをアニメーションとしてまとめる
            var animationWidth = new DoubleAnimationUsingKeyFrames();
            animationWidth.KeyFrames.Add(frame0);
            animationWidth.KeyFrames.Add(frame1);
            //アニメーションをアニメーションさせたいオブジェクトクラスのプロパティにひもづける
            //...ということはオブジェクト毎にアニメーションさせるのはだめ？
            Storyboard.SetTargetName(animationWidth, this.Name);
            Storyboard.SetTargetProperty(animationWidth, new PropertyPath(MainWindow.WidthProperty));

            var animationHeight = new DoubleAnimationUsingKeyFrames();
            animationHeight.KeyFrames.Add(frame2);
            animationHeight.KeyFrames.Add(frame3);
            Storyboard.SetTargetName(animationHeight, this.Name);
            Storyboard.SetTargetProperty(animationHeight, new PropertyPath(MainWindow.HeightProperty));

            //静的にひもづけられたストーリーボードへアニメーションを登録する
            myStoryboard = new Storyboard();
            myStoryboard.Children.Add(animationWidth);
            myStoryboard.Children.Add(animationHeight);
            //アニメーションを実行する
            myStoryboard.Begin(this);


            // ウィンドウ表示&最前面に持ってくる
            if (this.WindowState == System.Windows.WindowState.Minimized)
                this.WindowState = System.Windows.WindowState.Normal;

            this.Activate();
            // タスクバーでの表示をする
            this.ShowInTaskbar = true;
            //myStoryboard.Stop();
        }
        public void ClosingWindow()
        {
            //キーフレームで時間を区切る
            var frame0 = new EasingDoubleKeyFrame(this.Width,  KeyTime.FromTimeSpan(new TimeSpan(1)));
            var frame1 = new EasingDoubleKeyFrame(0,           KeyTime.FromTimeSpan(new TimeSpan(2500000)));
            var frame2 = new EasingDoubleKeyFrame(this.Height, KeyTime.FromTimeSpan(new TimeSpan(2500001)));
            var frame3 = new EasingDoubleKeyFrame(0,           KeyTime.FromTimeSpan(new TimeSpan(5000000)));
            //キーフレームをアニメーションとしてまとめる
            var animationWidth = new DoubleAnimationUsingKeyFrames();
            animationWidth.KeyFrames.Add(frame0);
            animationWidth.KeyFrames.Add(frame1);
            //アニメーションをアニメーションさせたいオブジェクトクラスのプロパティにひもづける
            //...ということはオブジェクト毎にアニメーションさせるのはだめ？
            Storyboard.SetTargetName(animationWidth, this.Name);
            Storyboard.SetTargetProperty(animationWidth, new PropertyPath(MainWindow.WidthProperty));

            var animationHeight = new DoubleAnimationUsingKeyFrames();
            animationHeight.KeyFrames.Add(frame2);
            animationHeight.KeyFrames.Add(frame3);
            Storyboard.SetTargetName(animationHeight, this.Name);
            Storyboard.SetTargetProperty(animationHeight, new PropertyPath(MainWindow.HeightProperty));

            //静的にひもづけられたストーリーボードへアニメーションを登録する
            myStoryboard = new Storyboard();
            myStoryboard.Completed += new EventHandler(endAnimation);
            myStoryboard.Children.Add(animationWidth);
            myStoryboard.Children.Add(animationHeight);
            //アニメーションを実行する
            myStoryboard.Begin(this);
            
            myStoryboard.Stop();
        }

        private void endAnimation(object sender, EventArgs e)
        {
            // クローズ処理をキャンセルして、タスクバーの表示も消す
            this.WindowState = System.Windows.WindowState.Minimized;
            this.ShowInTaskbar = false;
        }
    }
}
