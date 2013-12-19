using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using log4net;
using AlertPSO2EmergencyQuest;

namespace AlertPSO2EmergencyQuest
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {


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

            new AlertPSO2EmaergencyQuestException(AlertPSO2EmaergencyQuestException.ErrorLevelType.LEVEL_INFO,
                "正常に終了しました", "");
            if (mutex != null)
            {
                mutex.ReleaseMutex();
                mutex.Close();
            } 
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // ミューテックスの所有権を要求
            if (mutex.WaitOne(0, false) == false)
            {
                // すでに起動していると判断して終了
                MessageBox.Show("多重起動はできません。");
                mutex.Close();
                mutex = null;
                this.Shutdown();
            }
            new AlertPSO2EmaergencyQuestException(AlertPSO2EmaergencyQuestException.ErrorLevelType.LEVEL_INFO,
                "起動しました", "");
        }
    }
    
}
