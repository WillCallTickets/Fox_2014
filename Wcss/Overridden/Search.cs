using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class Search_Principal : _PrincipalBase.Principaled
    {
        public Search Search { get; set; }

        public Search_Principal(Search search) : base(search)
        {
            Search = search;
        }
    }

    public partial class Search : _PrincipalBase.IPrincipal
    {
    }
}
