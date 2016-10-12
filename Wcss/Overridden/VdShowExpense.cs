using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class VdShowExpense
    {
        [XmlAttribute("DateIncurred")]
        public DateTime DateIncurred
        {
            get { return (this.DtIncurred.HasValue) ? this.DtIncurred.Value : DateTime.MinValue; }
            set { this.DtIncurred = value; }
        }

        public decimal Amount
        {
            get
            {
                return decimal.Round(this.MAmount, 2);
            }
            set
            {
                this.MAmount = decimal.Round(value, 2);
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

    public partial class VdShowExpenseCollection : Utils._Collection.IOrderable<VdShowExpense>
    {
        public VdShowExpense AddActToCollection(int tShowId, DateTime dtIncurred, string expenseCategory, string expenseName, string notes, decimal amount)
        {
            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
            args.Add(new System.Web.UI.Pair("TShowId", tShowId));
            args.Add(new System.Web.UI.Pair("DtIncurred", dtIncurred));
            args.Add(new System.Web.UI.Pair("ExpenseCategory", expenseCategory));
            args.Add(new System.Web.UI.Pair("ExpenseName", expenseName));
            args.Add(new System.Web.UI.Pair("Notes", notes));
            args.Add(new System.Web.UI.Pair("MAmount", amount));

            VdShowExpense added = AddToCollection(args);

            return added;
        }

        public VdShowExpense AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
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

        public VdShowExpense ReorderItem(int idx, string direction)
        {
            return Utils._Collection.ReorderOrderedCollection(this.GetList(), idx, direction, "IOrdinal");
        }
    }
}
