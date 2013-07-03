using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace xRDT
{
    class Program
    {
        static xReddit.RedditAPI r;

        static void Main ( string[] args )
        {
            Thread redditThread = new Thread(new ThreadStart(RedditThread));
            redditThread.Start();

            while ( r == null )
                Thread.Sleep(100);

            //xReddit.RedditAPI r = new xReddit.RedditAPI();
            r.Login("", "", true);
            r.Get_Me();
            Console.ReadKey();
        }

        static void RedditThread ()
        {
            r = new xReddit.RedditAPI();
        }
    }
}
