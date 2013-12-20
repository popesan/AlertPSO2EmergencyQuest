using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlertPSO2EmergencyQuest
{
    class AlertPSO2EmaergencyQuestException:Exception
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

        public enum ErrorLevelType
        {
            /// <summary>
            /// レベル1: 重大エラー
            /// </summary>
            LEVEL_FATAL=1,
            /// <summary>
            /// レベル2: エラー
            /// </summary>
            LEVEL_ERROR=2,
            /// <summary>
            /// レベル3: 注意
            /// </summary>
            LEVEL_WARN = 3,
            /// <summary>
            /// レベル4: 情報
            /// </summary>
            LEVEL_INFO = 4,
            /// <summary>
            /// レベル5: デバッグ
            /// </summary>
            LEVEL_DEBUG = 5,
            /// <summary>
            /// レベル無し: 仮エラー(デフォルト)
            /// </summary>
            LEVEL_UNKNOWN = -1
        }
        
        private ErrorLevelType errorLevel= ErrorLevelType.LEVEL_UNKNOWN;
        /// <summary>
        /// エラーレベル
        /// </summary>
        public ErrorLevelType ErrorLevel
        {
            get { return errorLevel; }
        }
        
        private string message=null ;
        /// <summary>
        /// エラーメッセージ
        /// </summary>
        new public string Message
        {
            get { return message; }
        }

        private string stuckTrace=null ;
        /// <summary>
        /// スタックトレース
        /// </summary>
        public string StuckTrace
        {
            get { return stuckTrace; }
        }

        public AlertPSO2EmaergencyQuestException(ErrorLevelType errorLevel ,string message,string traceRoute)
        {
            switch (errorLevel)
            {
                case ErrorLevelType.LEVEL_FATAL:
                    logger.Fatal(message);
                    break;
                case ErrorLevelType.LEVEL_ERROR:
                    logger.Error(message);
                    break;
                case ErrorLevelType.LEVEL_WARN:
                    logger.Warn(message);
                    break;
                case ErrorLevelType.LEVEL_INFO:
                    logger.Info(message);
                    break;
                case ErrorLevelType.LEVEL_DEBUG:
                    logger.Debug(message);
                    break;
                default:
                    break;
            }

        }
    }
}
