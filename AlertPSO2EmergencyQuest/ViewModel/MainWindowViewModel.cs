using System.Diagnostics;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows;
using System.Windows.Interactivity;

namespace AlertPSO2EmergencyQuest.ViewModel
{
    public class MainWindowViewModel : NotificationObject
    {
        // これが今回の肝
        private InteractionRequest<Confirmation> alertRequest;

        // DelegateCommandもPrismに定義されてる
        private DelegateCommand alertCommand;

        private string message;

        public MainWindowViewModel()
        {
            this.alertRequest = new InteractionRequest<Confirmation>();
            this.alertCommand = new DelegateCommand(Alert);
        }

        public IInteractionRequest AlertRequest { get { return alertRequest; } }

        public ICommand AlertCommand { get { return this.alertCommand; } }

        // ただのメッセージを表すプロパティ
        public string Message
        {
            get
            {
                return this.message;
            }

            set
            {
                this.message = value;
                // Expressionを渡すタイプのRaisePropertyChangedメソッドも定義されてる
                this.RaisePropertyChanged(() => Message);
            }
        }

        private void Alert()
        {
            // Viewにリクエストを投げる
            alertRequest.Raise(
                new Confirmation { Title = "こんにちは", Content = "Hello world" },
                // コールバックで続きの処理をやる
                n =>
                {
                    // コールバックで結果を受け取り処理ができる
                    this.Message = n.Confirmed ? "OKが押されました" : "キャンセルが押されました";
                });
        }
    }    

    public class ConfirmAction : TriggerAction<DependencyObject>
    {
        protected override void Invoke(object parameter)
        {
            // イベント引数とContextを取得する
            var args = parameter as InteractionRequestedEventArgs;
            var ctx = args.Context as Confirmation;

            // ContextのConfirmedに結果を格納する
            ctx.Confirmed = MessageBox.Show(
                args.Context.Content.ToString(),
                args.Context.Title,
                MessageBoxButton.OKCancel) == MessageBoxResult.OK;

            // コールバックを呼び出す
            args.Callback();
        }
    }
}
