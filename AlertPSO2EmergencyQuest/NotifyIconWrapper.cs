using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace AlertPSO2EmergencyQuest
{
    public partial class NotifyIconWrapper : Component
    {
        private App app;
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
            
         }
        /*
        public NotifyIconWrapper(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }*/
        /*
        private void contextMenuStrip1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("menu1Clicked");
        }
        */
        // 常駐させるウィンドウはここで保持する
        private MainWindow win = new MainWindow();

        private void ShowWindow()
        {
            // ウィンドウ表示&最前面に持ってくる
            if (win.WindowState == System.Windows.WindowState.Minimized)
                win.WindowState = System.Windows.WindowState.Normal;

            win.Show();
            win.Activate();
            // タスクバーでの表示をする
            win.ShowInTaskbar = true;
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
            //Console.WriteLine("menu1Clicked");
            app.Shutdown();//アプリケーションを終了します
        }


        /// <summary>
        /// コンテクストメニュー：SettingTextのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingText_Click(object sender, EventArgs e)
        {
            Console.WriteLine("menu1Clicked");
        }

    }
}
