using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace AlertPSO2EmergencyQuest
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private NotifyIconWrapper component;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e); 
            this.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;
            
            component = new NotifyIconWrapper(this);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            component.Dispose();
        }
    }
    
}
