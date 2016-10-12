using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class VdShowTicket
    {
        public bool IsReserved
        {
            get { return this.BReserved; }
            set { this.BReserved = value; }
        }

        public decimal BasePrice
        {
            get
            {
                return decimal.Round(this.MBasePrice, 2);
            }
            set
            {
                this.MBasePrice = decimal.Round(value, 2);
            }
        }

        public decimal ServiceCharge
        {
            get
            {
                return decimal.Round(this.MServiceCharge, 2);
            }
            set
            {
                this.MServiceCharge = decimal.Round(value, 2);
            }
        }

        public decimal AdditionalCharge
        {
            get
            {
                return decimal.Round(this.MAdditionalCharge, 2);
            }
            set
            {
                this.MAdditionalCharge = decimal.Round(value, 2);
            }
        }

        public decimal PriceEach
        {
            get
            {
                return this.BasePrice + this.ServiceCharge + this.AdditionalCharge;
            }
        }

        public int Ordinal
        {
            get
            {
                return this.IOrdinal;
            }
            set
            {
                this.IOrdinal = value;
            }
        }
    }

    public partial class VdShowTicketCollection : Utils._Collection.IOrderable<VdShowTicket>
    {
        public VdShowTicket AddActToCollection(int tShowId, string description, string qualifier, bool reserved, decimal basePrice, decimal serviceCharge, 
            string additionalDescription, decimal additionalCharge)
        {
            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
            args.Add(new System.Web.UI.Pair("TShowId", tShowId));

            args.Add(new System.Web.UI.Pair("TicketDescription", description));
            args.Add(new System.Web.UI.Pair("TicketQualifier", qualifier));
            args.Add(new System.Web.UI.Pair("BReserved", reserved));
            args.Add(new System.Web.UI.Pair("MBasePrice", basePrice));
            args.Add(new System.Web.UI.Pair("MServiceCharge", serviceCharge));
            args.Add(new System.Web.UI.Pair("AdditionalDescription", additionalDescription));
            args.Add(new System.Web.UI.Pair("MAdditionalCharge", additionalCharge));

            VdShowTicket added = AddToCollection(args);

            return added;
        }

        public VdShowTicket AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
        {
            return Utils._Collection.AddItemToOrderedCollection(this.GetList(), args, "IOrdinal", false);
        }

        /// <summary>
        /// Delete a JShowAct from the collection by object ID (not ordinal)
        /// </summary>
        public bool DeleteFromCollection(int idx)
        {
            return Utils._Collection.DeleteFromOrderedCollection(this.GetList(), idx, "IOrdinal");
        }

        public VdShowTicket ReorderItem(int idx, string direction)
        {
            return Utils._Collection.ReorderOrderedCollection(this.GetList(), idx, direction, "IOrdinal");
        }
    }
}
