using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xReddit
{
    public static class Helpers
    {
        public static DateTime TimestampToDate ( double ts )
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            epoch = epoch.AddMilliseconds(ts);
            return epoch;
        }

    }
}
