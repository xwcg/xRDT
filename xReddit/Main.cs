using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xReddit
{
    /// <summary>
    /// Reddit API Queue Object
    /// </summary>
    public class RAPIQO
    {
        public RAPIQueue.QueueCallback Callback;
        //public 
    }

    public class RAPIQueue
    {
        public delegate void QueueCallback ( Thing result );


    }

    public class RedditAPI
    {
        private Request _requestObj;

        private string _APIUrl = "http://www.reddit.com";
        private string _UserAgent = "xRDT v1.0 by /u/xwcg";

        private string _Username = "";
        private string _Password = "";

        private uint _Requests = 0;
        private uint _MaxRequests = 30;
        private uint _WaitTime = 2000;

        /// <summary>
        /// In Seconds
        /// </summary>
        private uint _MaxRequestsTimeframe = 60;
                             
        public RedditAPI ()
        {
            this._requestObj = new Request(this._UserAgent);
        }

        public bool Login ( string username, string password, bool remember )
        {
            _requestObj.Make(_APIUrl + "/api/login", new System.Collections.Specialized.NameValueCollection
            {                                   
                {"user", username},    
                {"passwd", password},
                {"api_type", "json"},
                {"rem", remember ? "true" : "false"}
            });

            this._Username = username;
            this._Password = password;

            return true;
        }

        public AccountThing Get_Me ()
        {
            return new AccountThing(_requestObj.Get(_APIUrl + "/api/me.json"));
        }
    }
}
