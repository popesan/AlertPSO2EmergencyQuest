using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twitterizer;

namespace Model.AlertPSO2EmergencyQuest
{
    class CommTwitterizer
    {

        /// <summary>
        /// 以下トークン文字列は
        /// https://dev.twitter.com/apps にて Create a new application から生成します。
        /// つぶやきを行う場合は Application type 設定で write も可能としておきます。
        /// </summary>
        const string CONSUMER_KEY = "xFk4GzOhM6mzTGYAVrw";
        const string CONSUMER_SECRET = "mNTCIT45WmyK3xeVjdBEapBZJFLIvo7UVS1dXG4IoY";
        const string ACCESS_TOKEN = "173403180-pvq90s928jKF45M40fhIh4mqB7Jf0HmJzWJkAGkL";
        const string ACCESS_TOKEN_SECRET = "7wpwfDW0mONCughIDXW7EJueZWqjdRd6iA49JHmEVFC4i";


        void testTwit()
        {
            try
            {
                int userID;
                //トークンを設定
                OAuthTokens tokens = new OAuthTokens();
                tokens.AccessToken = ACCESS_TOKEN;
                tokens.AccessTokenSecret = ACCESS_TOKEN_SECRET;
                tokens.ConsumerKey = CONSUMER_KEY;
                tokens.ConsumerSecret = CONSUMER_SECRET;

                //ユーザの情報を取得
                String username = "pso2_emg_ship10";
                GetUserDetails(tokens, username);
                userID = GetUserID(tokens, username);
                //ユーザのタイムラインを取得
                //GetUserTimeline(tokens);
                GetUserTimeline(tokens, username);
                TwitterResponse<TwitterSearchResultCollection> ret
        = TwitterSearch.Search(tokens, username, new SearchOptions() { Locale = "jpn", });
                TwitterSearchResultCollection results = ret.ResponseObject;



                //つぶやく
                //DoUpdate(tokens, "つぶやきテスト");
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }

        /// <summary>
        /// ユーザ情報を取得
        /// </summary>
        /// <param name="tokens">トークン</param>
        /// <param name="username">ユーザ名</param>
        private static void GetUserDetails(OAuthTokens tokens, String username)
        {
            try
            {
                TwitterResponse<TwitterUser> showUserResponse = TwitterUser.Show(tokens, username);
                if (showUserResponse.Result == RequestResult.Success)
                {
                    TwitterUser user = showUserResponse.ResponseObject;
                    Console.WriteLine(showUserResponse.ResponseObject.Name);
                    Console.WriteLine(showUserResponse.ResponseObject.Description);
                    Console.WriteLine("ScreenName: {0:s}\r\n", user.ScreenName);
                    Console.WriteLine("ID: {0:d}\r\n", (int)user.Id);
                }
                else
                {
                    Console.WriteLine(showUserResponse.ErrorMessage);
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }

        /// <summary>
        /// 指定したユーザーのユーザ情報を取得
        /// </summary>
        /// <param name="tokens">トークン</param>
        /// <param name="username">スクリーンネーム</param>
        private static int GetUserID(OAuthTokens tokens, String screenname)
        {
            TwitterUser user = null;
            try
            {
                TwitterResponse<TwitterUser> showUserResponse = TwitterUser.Show(tokens, screenname);
                if (showUserResponse.Result == RequestResult.Success)
                {
                    user = showUserResponse.ResponseObject;
                    Console.WriteLine("ScreenName: {0:s}\r\n", user.ScreenName);
                    Console.WriteLine("ID: {0:d}\r\n", (int)user.Id);
                }
                else
                {
                    Console.WriteLine(showUserResponse.ErrorMessage);
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }

            return (int)user.Id;
        }
        /// <summary>
        /// ユーザ本人のタイムラインを取得
        /// </summary>
        /// <param name="tokens">トークン</param>
        private static void GetUserTimeline(OAuthTokens tokens)
        {
            try
            {
                TwitterResponse<TwitterStatusCollection> res = TwitterTimeline.UserTimeline(tokens);
                foreach (TwitterStatus status in res.ResponseObject)
                {
                    Console.WriteLine(status.Text);
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }
        /// <summary>
        /// 指定ユーザのタイムラインを取得
        /// </summary>
        /// <param name="tokens">トークン</param>
        private static void GetUserTimeline(OAuthTokens tokens, string screenName)
        {
            try
            {
                UserTimelineOptions option = new UserTimelineOptions()
                {
                    ScreenName = screenName,

                };

                TwitterResponse<TwitterStatusCollection> res
                    = TwitterTimeline.UserTimeline(tokens, option);

                foreach (TwitterStatus status in res.ResponseObject)
                {
                    Console.WriteLine(status.Text);
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }
        /// <summary>
        /// つぶやく
        /// </summary>
        /// <param name="tokens">トークン</param>
        /// <param name="body">本文</param>
        private static void DoUpdate(OAuthTokens tokens, String body)
        {
            try
            {
                TwitterResponse<TwitterStatus> tweetResponse = TwitterStatus.Update(tokens, body);
                if (tweetResponse.Result == RequestResult.Success)
                {
                    Console.WriteLine("Tweet posted successfully!");
                }
                else
                {
                    Console.WriteLine("Something bad happened");
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }
    }
}
