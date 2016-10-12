using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class VdShowInfo
    {
        [XmlAttribute("DateStamp")]
        public DateTime DateStamp
        {
            get { return this.DtStamp; }
            set { this.DtStamp = value; }
        }

        [XmlAttribute("DateModified")]
        public DateTime DateModified
        {
            get { return this.DtModified; }
            set { this.DtModified = value; }
        }

        public decimal TicketGross
        {
            get
            {
                return (this.MTicketGross.HasValue) ? decimal.Round(this.MTicketGross.Value, 2) : 0;
            }
            set
            {
                this.MTicketGross = decimal.Round(value, 2);
            }
        }

        public int TicketsSold
        {
            get
            {
                return (this.ITicketsSold.HasValue) ? this.ITicketsSold.Value : 0;
            }
            set
            {
                this.ITicketsSold = value;
            }
        }

        public int CompsIn
        {
            get
            {
                return (this.ICompsIn.HasValue) ? this.ICompsIn.Value : 0;
            }
            set
            {
                this.ICompsIn = value;
            }
        }

        public decimal FacilityFee
        {
            get
            {
                return (this.MFacilityFee.HasValue) ? decimal.Round(this.MFacilityFee.Value, 2) : 0;
            }
            set
            {
                this.MFacilityFee = decimal.Round(value, 2);
            }
        }

        public decimal Concessions
        {
            get
            {
                return (this.MConcessions.HasValue) ? decimal.Round(this.MConcessions.Value, 2) : 0;
            }
            set
            {
                this.MConcessions = decimal.Round(value, 2);
            }
        }

        public decimal BarTotal
        {
            get
            {
                return (this.MBarTotal.HasValue) ? decimal.Round(this.MBarTotal.Value, 2) : 0;
            }
            set
            {
                this.MBarTotal = decimal.Round(value, 2);
            }
        }

        public decimal BarPerHead
        {
            get
            {
                return (this.MBarPerHead.HasValue) ? decimal.Round(this.MBarPerHead.Value, 2) : 0;
            }
            set
            {
                this.MBarPerHead = decimal.Round(value, 2);
            }
        }

        public int MarketingDays
        {
            get
            {
                return (this.IMarketingDays.HasValue) ? this.IMarketingDays.Value : 0;
            }
            set
            {
                this.IMarketingDays = value;
            }
        }

        public decimal ProductionLabor
        {
            get
            {
                return (this.MProdLabor.HasValue) ? decimal.Round(this.MProdLabor.Value, 2) : 0;
            }
            set
            {
                this.MProdLabor = decimal.Round(value, 2);
            }
        }

        public decimal SecurityLabor
        {
            get
            {
                return (this.MSecurityLabor.HasValue) ? decimal.Round(this.MSecurityLabor.Value, 2) : 0;
            }
            set
            {
                this.MSecurityLabor = decimal.Round(value, 2);
            }
        }

        public decimal Hospitality
        {
            get
            {
                return (this.MHospitality.HasValue) ? decimal.Round(this.MHospitality.Value, 2) : 0;
            }
            set
            {
                this.MHospitality = decimal.Round(value, 2);
            }
        }

        public int MarketPlays
        {
            get
            {
                return (this.IMarketPlays.HasValue) ? this.IMarketPlays.Value : 0;
            }
            set
            {
                this.IMarketPlays = value;
            }
        }

    }
}
