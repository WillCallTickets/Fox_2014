using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SubSonic;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Wcss
{
    public partial class Z2Subscription
    {
        [XmlAttribute("DateCreated")]
        public DateTime DateCreated
        {
            get
            {
                return this.DtCreated;
            }
            set
            {
                this.DtCreated = value;
            }
        }

        [XmlAttribute("DateModified")]
        public DateTime DateModified
        {
            get
            {
                return (this.DtModified.HasValue) ? this.DtModified.Value : DateTime.MaxValue;
            }
            set
            {
                this.DtModified = value;
            }
        }

        [XmlAttribute("IsSubscribed")]
        public bool IsSubscribed
        {
            get
            {
                return this.BSubscribed;
            }
            set
            {
                this.BSubscribed = value;
            }
        }

        [XmlAttribute("LastHistoryEventId")]
        public int LastHistoryEventId
        {
            get
            {
                return (this.TZ2SubscriptionHistoryId.HasValue) ? this.TZ2SubscriptionHistoryId.Value : 0;
            }
            set
            {
                this.TZ2SubscriptionHistoryId = value;
            }
        }
    }

    public partial class Z2SubscriptionRequest
    {
        [XmlAttribute("DateCreated")]
        public DateTime DateCreated
        {
            get
            {
                return this.DtStamp;
            }
            set
            {
                this.DtStamp = value;
            }
        }
    }    
}
