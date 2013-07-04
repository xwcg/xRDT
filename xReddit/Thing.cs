using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using xLogger;

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
        Listing,
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

        public Thing ( JObject rawObject )
        {
            this.raw = rawObject;
            this.ParseFromRaw();
        }

        public Thing ()
        {
        }

        public void ParseFromRaw ()
        {
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

                case "Listing":
                    this._kind = ThingKind.Listing;
                    break;

                default:
                    Logger.WriteLine(String.Format("## (Thing) kind '{0}' unknown", kindStr), ConsoleColor.DarkRed);
                    this._kind = ThingKind.Unknown;
                    break;
            }

            if ( this._kind != ThingKind.Listing )
            {
                this._thingId = raw.Value<string>("id");
                this._thingName = raw.Value<string>("name");
            }
            else
            {
                this._thingId = "";
                this._thingName = "Listing";
            }
                                                            
            this.data = raw.Value<JObject>("data");     
        
            Logger.WriteLine(String.Format(">> (Thing) of kind '{0}' parsed", this._kind.ToString()), ConsoleColor.DarkYellow);
        }

        public void ParseJson ( string json )
        {
            raw = JObject.Parse(json);

            this.ParseFromRaw();
        }

        public UserThing ToUser ()
        {
            return new UserThing(this);
        }

        public LinkThing ToLink ()
        {
            return new LinkThing(this);
        }

        public ListThing ToListing ()
        {
            return new ListThing(this);
        }
    }

    public class UserThing : Thing
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

        public UserThing ( string json )
            : base(json)
        {
            this.ParseData();
        }

        public UserThing ( Thing baseThing )
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

            Logger.WriteLine(String.Format(">& (Thing) for user '{0}' parsed", this._name), ConsoleColor.DarkYellow);
        }
    }

    public class LinkThing : Thing
    {
        #region Private Properties

        /*
         * Commented data not yet added out of laziness
         * 
         * (type)? indicates that I am not sure about the data type
         * ??? indicated that I have no idea about the data type
         * 
         */
        //private string _domain;
        //private string? _bannedBy;
        //private object? _embedded; 
        //private string _subredditId;   
        //private ??? _likes;
        //private string _flair;    
        //private ??? _media;    
        //private string? _approvedBy;
        //private ??? _thumbnail;
        //private bool _edited;         
        //private string _flairCssClass;
        //private string _flairAuthorCssClass;
        //private bool _saved;
        //private bool _isSelf;
        //private string _permalink;   
        //private string _flairAuthor;  
        //private int _reportsCount;
        //private bool? _distinguished; 
        //private string _textHtml;
        private string _subreddit;
        private string _text;
        private string _id;
        private string _name;
        private string _title;
        private int _score;
        private int _votesUp;
        private int _votesDown;
        private bool _nsfw;
        private bool _hidden;
        private double _ts_created;
        private double _ts_createdUTC;
        private string _url;
        private string _author;
        private int _commentsCount;

        #endregion

        #region Public Properties
        public String Subreddit
        {
            get
            {
                return this._subreddit;
            }
        }

        public String Text
        {
            get
            {
                return this._text;
            }
        }

        public String Id
        {
            get
            {
                return this._id;
            }
        }

        public String Name
        {
            get
            {
                return this._name;
            }
        }

        public String Title
        {
            get
            {
                return this._title;
            }
        }

        public Int32 Score
        {
            get
            {
                return this._score;
            }
        }

        public Int32 Upvotes
        {
            get
            {
                return this._votesUp;
            }
        }

        public Int32 Downvotes
        {
            get
            {
                return this._votesDown;
            }
        }

        public Boolean NSFW
        {
            get
            {
                return this._nsfw;
            }
        }

        public Boolean Hidden
        {
            get
            {
                return this._hidden;
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

        public String URL
        {
            get
            {
                return this._url;
            }
        }

        public String AuthorName
        {
            get
            {
                return this._author;
            }
        }

        public Int32 CommentsCount
        {
            get
            {
                return this._commentsCount;
            }
        }

        #endregion

        public LinkThing ( string json )
            : base(json)
        {
            this.ParseData();
        }

        public LinkThing ( Thing baseThing )
            : base(baseThing)
        {
            this.ParseData();
        }

        public void ParseData ()
        {
            this._subreddit = base.ThingData.Value<string>("subreddit");
            this._text = base.ThingData.Value<string>("selftext");
            this._id = base.ThingData.Value<string>("id");
            this._name = base.ThingData.Value<string>("name");
            this._title = base.ThingData.Value<string>("title");
            this._score = base.ThingData.Value<int>("score");
            this._votesUp = base.ThingData.Value<int>("ups");
            this._votesDown = base.ThingData.Value<int>("downs");
            this._nsfw = base.ThingData.Value<bool>("over_18");
            this._hidden = base.ThingData.Value<bool>("hidden");
            this._ts_created = base.ThingData.Value<double>("created");
            this._ts_createdUTC = base.ThingData.Value<double>("created_utc");
            this._url = base.ThingData.Value<string>("url");
            this._author = base.ThingData.Value<string>("author");
            this._commentsCount = base.ThingData.Value<int>("num_comments");
        }
    }

    /// <summary>
    /// ListThing... ListTHING, get it? .. Because... Listing - ListThing?
    /// I'll see myself out...
    /// </summary>
    public class ListThing : Thing
    {
        #region Private Properties
        private List<LinkThing> _children = new List<LinkThing>();
        #endregion

        #region Public Properties

        public List<LinkThing> Links
        {
            get
            {
                return this._children;
            }
        }

        #endregion

        public ListThing ( string json )
            : base(json)
        {
            this.ParseData();
        }

        public ListThing ( Thing baseThing )
            : base(baseThing)
        {
            this.ParseData();
        }

        private void ParseData ()
        {
            JArray array = (JArray)base.ThingData["children"];
            IList<JObject> children = array.ToObject<IList<JObject>>();
           
            foreach ( JObject jo in children )
            {
                this._children.Add(new LinkThing(new Thing(jo)));
            }
        }
    }

    public class CommentThing : Thing
    {
        #region Private Properties
        #endregion

        #region Public Properties
        #endregion

        public CommentThing ( string json )
            : base(json)
        {
            this.ParseData();
        }

        public CommentThing ( Thing baseThing )
            : base(baseThing)
        {
            this.ParseData();
        }

        private void ParseData ()
        {
        }
    }

    public class MessageThing : Thing
    {
        #region Private Properties
        #endregion

        #region Public Properties
        #endregion

        public MessageThing ( string json )
            : base(json)
        {
            this.ParseData();
        }

        public MessageThing ( Thing baseThing )
            : base(baseThing)
        {
            this.ParseData();
        }

        private void ParseData ()
        {
        }
    }
}
