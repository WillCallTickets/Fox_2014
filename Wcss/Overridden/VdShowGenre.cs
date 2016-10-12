using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class VdShowGenre
    {
        public bool IsMainGenre
        {
            get { return this.BIsMainGenre; }
            set { this.BIsMainGenre = value; }
        }

        public bool IsSubGenre
        {
            get { return (!this.BIsMainGenre); }
        }

        public int Ordinal
        {
            get { return this.IOrdinal; }
            set { this.IOrdinal = value; }
        }
    }

    public partial class VdShowGenreCollection : Utils._Collection.IOrderable<VdShowGenre>
    {
        public VdShowGenre AddGenreToCollection(int tShowId, string genreName, bool isMainGenre)
        {
            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
            args.Add(new System.Web.UI.Pair("TShowId", tShowId));
            args.Add(new System.Web.UI.Pair("GenreName", genreName));
            args.Add(new System.Web.UI.Pair("BIsMainGenre", isMainGenre));

            VdShowGenre added = AddToCollection(args);

            return added;
        }

        public VdShowGenre AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
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

        public VdShowGenre ReorderItem(int idx, string direction)
        {
            return Utils._Collection.ReorderOrderedCollection(this.GetList(), idx, direction, "IOrdinal");
        }
    }
}
