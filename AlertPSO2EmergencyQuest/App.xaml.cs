using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using log4net;

namespace AlertPSO2EmergencyQuest
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// log4netインスタンス
        /// </summary>
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>レベル無し: 仮エラー(デフォルト)</summary>
        private const int LEVEL_UNKNOWN = -1;
        /// <summary>レベル5: デバッグ</summary>
        public const int LEVEL_DEBUG = 5;
        /// <summary>レベル4: 情報</summary>
        public const int LEVEL_INFO = 4;
        /// <summary>レベル3: 注意</summary>
        public const int LEVEL_WARN = 3;
        /// <summary>レベル2: エラー</summary>
        public const int LEVEL_ERROR = 2;
        /// <summary>レベル1: 重大エラー</summary>
        public const int LEVEL_FATAL = 1;

        // 多重起動チェックに使うミューテックス
        private System.Threading.Mutex mutex
            = new System.Threading.Mutex(false, "AppName");


        private NotifyIconWrapper component;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            try
            {
                this.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;
            }
            catch
            {
            }
            component = new NotifyIconWrapper(this);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            component.Dispose();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (mutex != null)
            {
                mutex.ReleaseMutex();
                mutex.Close();
            } 
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            logger.Fatal("起動ok");
            // ミューテックスの所有権を要求
            if (mutex.WaitOne(0, false) == false)
            {
                // すでに起動していると判断して終了
                MessageBox.Show("多重起動はできません。");
                mutex.Close();
                mutex = null;
                this.Shutdown();
            }
        }
    }
    
}
