using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class PrincipalConfig
    {
        //vcPrincipal
        //datatype 50
        //maxlength
        //context 50
        //description 256
        //name 50
        //value 256

        [XmlAttribute("Principal")]
        public _Enums.Principal Principal
        {
            get
            {
                return (_Enums.Principal)Enum.Parse(typeof(_Enums.Principal), this.VcPrincipal, true);
            }
            set
            {
                if (value == _Enums.Principal.all)
                    throw new ArgumentOutOfRangeException("Cannot use all principal - use siteConfig for sitewide configs");

                this.VcPrincipal = value.ToString();
            }
        }
    }
}
