using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class VdShowTicketOutlet
    {
        public int Allotment
        {
            get
            {
                return this.IAllotment;
            }
            set
            {
                this.IAllotment = value;
            }
        }

        public int Sold
        {
            get
            {
                return this.ISold;
            }
            set
            {
                this.ISold = value;
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

    public partial class VdShowTicketOutletCollection : Utils._Collection.IOrderable<VdShowTicketOutlet>
    {
        public VdShowTicketOutlet AddActToCollection(int tShowId, int tVdShowTicketId, string outletName, int allotment, int sold)
        {
            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
            args.Add(new System.Web.UI.Pair("TShowId", tShowId));
            args.Add(new System.Web.UI.Pair("TVdShowTicketId", tVdShowTicketId));
            args.Add(new System.Web.UI.Pair("OutletName", outletName));
            args.Add(new System.Web.UI.Pair("IAllotment", allotment));
            args.Add(new System.Web.UI.Pair("ISold", sold));

            VdShowTicketOutlet added = AddToCollection(args);

            return added;
        }

        public VdShowTicketOutlet AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
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

        public VdShowTicketOutlet ReorderItem(int idx, string direction)
        {
            return Utils._Collection.ReorderOrderedCollection(this.GetList(), idx, direction, "IOrdinal");
        }
    }
}
