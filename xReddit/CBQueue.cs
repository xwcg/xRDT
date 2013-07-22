using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xReddit
{
    public class CBQueue
    {
        private List<RAPIQO> _list = new List<RAPIQO>();

        public CBQueue ()
        {
        }

        public void Enqueue ( RAPIQO item )
        {
            this.Enqueue(item, false);
        }

        public void Enqueue ( RAPIQO item, bool top )
        {
            if ( top )
            {
                this._list.Insert(0, item);
            }
            else
            {
                this._list.Add(item);
            }
        }

        public RAPIQO Dequeue ()
        {
            if ( this._list.Count > 0 )
            {
                RAPIQO retitem = this._list[0];
                this._list.RemoveAt(0);
                return retitem;
            }

            return null;
        }

        public int Count
        {
            get
            {
                return this._list.Count;
            }
        }
    }
}
