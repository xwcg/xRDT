using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xLogger;

namespace xReddit
{
    public enum RAPIRequest
    {
        LOGIN,
        GET_ME,
        GET_COMMENTS,
        SEND_COMMENT,
        GET_HOT,
        GET_NEW
    }

    /// <summary>
    /// Reddit API Queue Object
    /// </summary>
    public class RAPIQO
    {
        public RedditAPI.QueueCallback Callback;
        public RAPIRequest type;
        public string data = "";
        public int limit = 25;
        public NameValueCollection payload;
    }

    public class RedditAPI
    {
        public delegate void QueueCallback ( Thing result );
        private Queue<RAPIQO> queue = new Queue<RAPIQO>();

        private Thread QueueThread;

        private Request _requestObj;

        private string _APIUrl = "http://www.reddit.com";
        private string _UserAgent = "xRDT v1.0 by /u/xwcg";

        private string _Username = "";
        private string _Password = "";
        private bool _Remember = false;

        private int _Requests = 0;
        private int _MaxRequests = 30;
        private int _WaitTime = 2000;

        private bool RunQueue = false;

        /// <summary>
        /// In Seconds
        /// </summary>
        private uint _MaxRequestsTimeframe = 60;

        private DateTime TimeframeStart = DateTime.Now;

        public RedditAPI ()
        {
            this._requestObj = new Request(this._UserAgent);
        }

        #region Queue
        private void Request ( RAPIRequest type, QueueCallback callback )
        {
            this.Request(type, "", 25, null, callback);
        }

        private void Request ( RAPIRequest type, string data, QueueCallback callback )
        {
            this.Request(type, data, 25, null, callback);
        }

        private void Request ( RAPIRequest type, NameValueCollection payload, QueueCallback callback )
        {
            this.Request(type, "", 25, payload, callback);
        }

        private void Request ( RAPIRequest type, int limit, QueueCallback callback )
        {
            this.Request(type, "", limit, null, callback);
        }

        private void Request ( RAPIRequest type, string data, int limit, QueueCallback callback )
        {
            this.Request(type, data, limit, null, callback);
        }

        private void Request ( RAPIRequest type, NameValueCollection payload, int limit, QueueCallback callback )
        {
            this.Request(type, "", limit, payload, callback);
        }

        public void Request ( RAPIRequest type, string data, int limit, NameValueCollection payload, QueueCallback callback )
        {
            RAPIQO item = new RAPIQO();
            item.Callback = callback;
            item.data = data;
            item.limit = limit;
            item.payload = payload;
            item.type = type;

            queue.Enqueue(item);
        }

        public void StartQueue ()
        {
            if ( this.RunQueue )
                return;

            Logger.WriteLine("** Starting Request Queue", ConsoleColor.DarkGreen);

            this.RunQueue = true;
            this.QueueThread = new Thread(new ThreadStart(QueueLoop));
            this.QueueThread.IsBackground = true;
            this.QueueThread.Start();
        }

        public void StopQueue ()
        {
            Logger.WriteLine("** Stopping Request Queue", ConsoleColor.DarkGreen);
            this.RunQueue = false;
            this.QueueThread.Join(5000);
            Logger.WriteLine("** Request Queue Stopped", ConsoleColor.Green);
        }

        private void QueueLoop ()
        {
            Logger.WriteLine("** Request Queue Started", ConsoleColor.Green);
            this.TimeframeStart = DateTime.Now;

            while ( RunQueue )
            {
                if ( queue.Count == 0 )
                {
                    Thread.Sleep(_WaitTime);
                    continue;
                }

                if ( this._Requests >= this._MaxRequests )
                {
                    TimeSpan sinceTimeframe = new TimeSpan(DateTime.Now.Ticks);
                    sinceTimeframe.Subtract(new TimeSpan(this.TimeframeStart.Ticks));

                    if ( sinceTimeframe.TotalSeconds > this._MaxRequestsTimeframe )
                    {
                        this.TimeframeStart = DateTime.Now;
                        this._Requests = 0;
                    }
                    else
                    {
                        Thread.Sleep(( (int)( this._MaxRequestsTimeframe - sinceTimeframe.TotalSeconds ) ) * 1000);
                        continue;
                    }
                }

                RAPIQO queueItem = queue.Dequeue();
                Thing result = null;

                switch ( queueItem.type )
                {
                    case RAPIRequest.LOGIN:
                        Logger.WriteLine(String.Format("<< Logging in user '{0}'", queueItem.payload["user"]), ConsoleColor.DarkGreen);
                        result = _requestObj.Make(_APIUrl + "/api/login", queueItem.payload);
                        break;

                    case RAPIRequest.GET_ME:
                        result = _requestObj.Get(_APIUrl + "/api/me.json");
                        break;

                    case RAPIRequest.GET_HOT:
                        result = _requestObj.Get(String.Format("{0}/r/{1}/hot.json?limit={2}", _APIUrl, queueItem.data, queueItem.limit));
                        break;

                    case RAPIRequest.GET_NEW:
                        result = _requestObj.Get(String.Format("{0}/r/{1}/new.json?limit={2}", _APIUrl, queueItem.data, queueItem.limit));
                        break;

                    case RAPIRequest.GET_COMMENTS:
                        result = _requestObj.Get(String.Format("{0}/r/{1}/comments/{2}.json", _APIUrl, queueItem.payload["subreddit"], queueItem.payload["id"]));
                        break;
                }

                if ( queueItem.Callback != null )
                    queueItem.Callback(result);

                this._Requests++;
                Thread.Sleep(_WaitTime);
            }
        }

        #endregion

        public void Login ( string username, string password, bool remember, QueueCallback callback )
        {
            this._Username = username;
            this._Password = password;
            this._Remember = remember;

            this.Login(callback);
        }

        public void Login ( QueueCallback callback )
        {
            this.Request(RAPIRequest.LOGIN, new NameValueCollection
            {    
                {"user", this._Username},    
                {"passwd", this._Password},
                {"api_type", "json"},
                {"rem", this._Remember ? "true" : "false"}
            }, callback);
        }

        public void Get_Me ( QueueCallback callback )
        {
            this.Request(RAPIRequest.GET_ME, callback);
        }

        public void Get_SubredditHot ( string name, int limit, QueueCallback callback )
        {
            this.Request(RAPIRequest.GET_HOT, name, limit, callback);
        }
    }
}
