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
using System.Windows.Shapes;
using AlertPSO2EmergencyQuest.Properties;

namespace AlertPSO2EmergencyQuest
{
    /// <summary>
    /// SettingWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow()
        {
            InitializeComponent();
            Settings.Default.botUrl = "はちゃめちゃ大作成";
            
        }

        private void Button_SettingOK_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result= MessageBox.Show("shipを変更しますか", "設定", MessageBoxButton.OKCancel);

            switch (result)
            {
                case MessageBoxResult.OK:
                    
                    this.Close();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void Button_SettingCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
