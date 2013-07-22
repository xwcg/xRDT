using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace xReddit
{
    class Request
    {
        private RequestClient _client;

        public Request (string agent)
        {
            this._client = new RequestClient();
            this._client.Headers.Add("User-Agent", agent);
        }

        public Thing Make ( string url, NameValueCollection data )
        {
            byte[] response = this._client.UploadValues(url, "POST", data);
            string responseString = Encoding.Default.GetString(response);
                                                
            return new Thing(responseString);
        }

        public Thing Get ( string url )
        {
            byte[] response = this._client.DownloadData(url);
            string responseString = Encoding.Default.GetString(response);
            return new Thing(responseString);
        }

        public bool Working
        {
            get
            {
                return this._client.IsBusy;
            }
        }
    }

    public class RequestClient : WebClient
    {
        private CookieContainer _cookieContainer = new CookieContainer();
        public CookieContainer MyCookies
        {
            get
            {
                return this._cookieContainer;
            }
        }

        protected override WebRequest GetWebRequest ( Uri address )
        {
            WebRequest request = base.GetWebRequest(address);
            if ( request is HttpWebRequest )
            {
                ( request as HttpWebRequest ).CookieContainer = _cookieContainer;
            }
            return request;
        }
    }
}
