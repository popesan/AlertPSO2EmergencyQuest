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
using AlertPSO2EmergencyQuest.Model;
namespace AlertPSO2EmergencyQuest
{
    /// <summary>
    /// SettingWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingWindow : Window
    {
        private CommPSO2Bot bot;
        private string currentCheckedRadioButtonTag;
        public SettingWindow(CommPSO2Bot bot)
        {
            InitializeComponent();
            this.bot = bot;

            Settings.Default.botUrl =this.bot.BotUrls[this.bot.SelectShipName];
            //ラジオボタンのタグを選択ship識別用オブジェクトとします
            this.RadioButton_1.Tag = "ship1";
            this.RadioButton_2.Tag = "ship2";
            this.RadioButton_3.Tag = "ship3";
            this.RadioButton_4.Tag = "ship4";
            this.RadioButton_5.Tag = "ship5";
            this.RadioButton_6.Tag = "ship6";
            this.RadioButton_7.Tag = "ship7";
            this.RadioButton_8.Tag = "ship8";
            this.RadioButton_9.Tag = "ship9";
            this.RadioButton_10.Tag = "ship10";
        }

        private void Button_SettingOK_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result= MessageBox.Show("shipを変更しますか", "設定", MessageBoxButton.OKCancel);

            switch (result)
            {
                case MessageBoxResult.OK:
                    this.bot.SelectShipName = this.currentCheckedRadioButtonTag;
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

        private void RadioButton_10_Checked(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton == null)
                return;
            this.currentCheckedRadioButtonTag=(string)radioButton.Tag;
            /*
            int intIndex = Convert.ToInt32(radioButton.Content.ToString());
            MessageBox.Show(intIndex.ToString());*/
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {   
            switch (this.bot.SelectShipName)
            {
                case "ship1":
                    this.RadioButton_1.IsChecked = true;
                    break;
                case "ship2":
                    this.RadioButton_2.IsChecked = true;
                    break;
                case "ship3":
                    this.RadioButton_3.IsChecked = true;
                    break;
                case "ship4":
                    this.RadioButton_4.IsChecked = true;
                    break;
                case "ship5":
                    this.RadioButton_5.IsChecked = true;
                    break;
                case "ship6":
                    this.RadioButton_6.IsChecked = true;
                    break;
                case "ship7":
                    this.RadioButton_7.IsChecked = true;
                    break;
                case "ship8":
                    this.RadioButton_8.IsChecked = true;
                    break;
                case "ship9":
                    this.RadioButton_9.IsChecked = true;
                    break;
                case "ship10":
                    this.RadioButton_10.IsChecked = true;
                    break;
            }
        }
    }
}
