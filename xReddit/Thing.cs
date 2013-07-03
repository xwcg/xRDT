using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace xReddit
{
    public enum ThingKind
    {
        Unknown,
        Comment,
        Account,
        Link,
        Message,
        Subreddit,
        Award,
        PromoCampaign
    }

    public partial class Thing
    {
        #region Private Properties
        private JObject raw;
        private JObject data;

        private ThingKind _kind;
        private string _thingId;
        private string _thingName;
        #endregion

        #region Public Properties
        public ThingKind Kind
        {
            get
            {
                return this._kind;
            }
        }

        public String ThingId
        {
            get
            {
                return this._thingId;
            }
        }

        public String ThingName
        {
            get
            {
                return this._thingName;
            }
        }

        public JObject ThingData
        {
            get
            {
                return this.data;
            }
        }
        #endregion

        public Thing ( string json )
        {
            this.ParseJson(json);
        }

        public Thing ( Thing baseThing )
        {
            this.raw = baseThing.raw;
            this.data = baseThing.data;
            this._kind = baseThing._kind;
            this._thingId = baseThing._thingId;
            this._thingName = baseThing._thingName;
        }

        public Thing ()
        {
        }

        public void ParseJson ( string json )
        {
            raw = JObject.Parse(json);

            this._thingId = raw.Value<string>("id");
            this._thingName = raw.Value<string>("name");
            string kindStr = raw.Value<string>("kind");

            switch ( kindStr )
            {
                case "t1":
                    this._kind = ThingKind.Comment;
                    break;

                case "t2":
                    this._kind = ThingKind.Account;
                    break;

                case "t3":
                    this._kind = ThingKind.Link;
                    break;

                case "t4":
                    this._kind = ThingKind.Message;
                    break;

                case "t5":
                    this._kind = ThingKind.Subreddit;
                    break;

                case "t6":
                    this._kind = ThingKind.Award;
                    break;

                case "t7":
                    this._kind = ThingKind.Unknown;
                    break;

                case "t8":
                    this._kind = ThingKind.PromoCampaign;
                    break;

                default:
                    this._kind = ThingKind.Unknown;
                    break;
            }

            Console.WriteLine(this._kind.ToString());

            this.data = raw.Value<JObject>("data");

            if ( data == null )
                Console.WriteLine(raw.ToString());
            else
                Console.WriteLine(data.ToString());
        }
    }

    public class AccountThing : Thing
    {
        #region Private Properties
        private bool _hasMail;
        private string _name;
        private bool _isFriend;
        private double _ts_created;
        private double _ts_createdUTC;
        private string _modhash;
        private int _karmaLink;
        private int _karmaComment;
        private bool _overEighteen;
        private bool _isGold;
        private bool _isMod;
        private bool _verified;
        private string _userid;
        private bool _hasModMail;
        #endregion

        #region Public Properties

        public Boolean HasEmail
        {
            get
            {
                return this._hasMail;
            }
        }

        public String Name
        {
            get
            {
                return this._name;
            }
        }

        public Boolean IsFriend
        {
            get
            {
                return this._isFriend;
            }
        }

        public DateTime Created
        {
            get
            {
                return Helpers.TimestampToDate(this._ts_created);
            }
        }

        public DateTime CreatedUTC
        {
            get
            {
                return Helpers.TimestampToDate(this._ts_createdUTC);
            }
        }

        public String ModHash
        {
            get
            {
                return this._modhash;
            }
        }

        public Int32 Link_Karma
        {
            get
            {
                return this._karmaLink;
            }
        }

        public Int32 Comment_Karma
        {
            get
            {
                return this._karmaComment;
            }
        }

        public Boolean IsOver18
        {
            get
            {
                return this._overEighteen;
            }
        }

        public Boolean IsGold
        {
            get
            {
                return this._isGold;
            }
        }
        
        public Boolean IsModerator
        {
            get
            {
                return this._isMod;
            }
        }

        public Boolean IsVerified
        {
            get
            {
                return this._verified;
            }
        }

        public Boolean HasModEmail
        {
            get
            {
                return this._hasModMail;
            }
        }

        public String Id
        {
            get
            {
                return this._userid;
            }
        }

        #endregion

        public AccountThing ( string json )
            : base(json)
        {
            this.ParseData();
        }

        public AccountThing ( Thing baseThing )
            : base(baseThing)
        {
            this.ParseData();
        }

        public void ParseData ()
        {
            this._hasMail = base.ThingData.Value<bool>("has_mail");
            this._name = base.ThingData.Value<string>("name");
            this._isFriend = base.ThingData.Value<bool>("is_friend");
            this._ts_created = base.ThingData.Value<double>("created");
            this._ts_createdUTC = base.ThingData.Value<double>("created_utc");
            this._modhash = base.ThingData.Value<string>("modhash");
            this._karmaLink = base.ThingData.Value<int>("link_karma");
            this._karmaComment = base.ThingData.Value<int>("comment_karma");
            this._overEighteen = base.ThingData.Value<bool>("over_18");
            this._isGold = base.ThingData.Value<bool>("is_gold");
            this._isMod = base.ThingData.Value<bool>("is_mod");
            this._verified = base.ThingData.Value<bool>("has_verified_email");
            this._userid = base.ThingData.Value<string>("id");
            this._hasModMail = base.ThingData.Value<bool>("has_mod_mail");
        }
    }
}
