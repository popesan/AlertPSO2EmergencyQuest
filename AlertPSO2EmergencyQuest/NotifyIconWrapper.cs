using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation; 
using System.Windows.Threading;
using AlertPSO2EmergencyQuest.Model;

namespace AlertPSO2EmergencyQuest
{
    public partial class NotifyIconWrapper : Component
    {
        private App app;
        

        // 常駐させるウィンドウはここで保持する
        private MainWindow win;

        private EventPSO2 pso; 
        /*
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
        }*/
        public NotifyIconWrapper(App app)
        {
            InitializeComponent();

            this.app = app;
            
            // タスクトレイ用のアイコンを設定
            System.IO.Stream iconStream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/Resources/img/Icon1.ico",UriKind.RelativeOrAbsolute )).Stream;
            this.notifyIcon1.Icon = new System.Drawing.Icon(iconStream);
             
            //contextmenustrip内のtextBoxのクリックイベントをハンドルします
            this.EndText.Click+=new EventHandler(EndText_Click);
            this.SettingText.Click+= new EventHandler(SettingText_Click);

            pso = new EventPSO2();
            pso.Quest += Pso2_Event;
            pso.Start();

            win = new MainWindow(pso);

         }
        /*
        public NotifyIconWrapper(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }*/

        private void ShowWindow()
        {
            win.ShowWindow();

        }
        /// <summary>
        /// タスクトレイ上に表示されたアイコンのクリックイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            switch (e.Button)
            {
                case System.Windows.Forms.MouseButtons.Left:
                    ShowWindow();
                    break;
                case System.Windows.Forms.MouseButtons.Right:
                    this.contextMenuStrip1.Show();
                    break;
            }
        }
        /// <summary>
        /// コンテクストメニュー：EndTextのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndText_Click(object sender, EventArgs e)
        {
            DialogResult result=
                System.Windows.Forms.MessageBox.Show("アプリケーションを終了しますか？", "確認", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                app.Shutdown();//アプリケーションを終了します
            }
        }


        /// <summary>
        /// コンテクストメニュー：SettingTextのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingText_Click(object sender, EventArgs e)
        {
            SettingWindow setWindow = new SettingWindow(this.pso.Bot);

            setWindow.ShowDialog();
        }
        /// <summary>
        /// PSO2イベント発生イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pso2_Event(object sender, EventArgs e)
        {
            Console.WriteLine("PSOいべんとはっせい");
            
            ShowWindow();
        }
    }
}
