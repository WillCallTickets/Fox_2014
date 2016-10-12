using System;
using System.Web;


namespace Wcss
{
    public partial class _Common
    {
        /// <summary>
        /// indicates the most lax of admin access - for non customers
        /// </summary>
        public static bool IsAuthdAdminUser()
        {
            System.Security.Principal.IPrincipal user = HttpContext.Current.User;

            if(user == null)
                return false;

            return (user.Identity.IsAuthenticated &&
                (user.IsInRole("Administrator") ||
                user.IsInRole("ContentEditor") ||
                user.IsInRole("Kiosker") ||
                user.IsInRole("Manifester") ||
                user.IsInRole("MassMailer") ||
                user.IsInRole("OrderFiller") ||
                user.IsInRole("PasswordViewer") ||
                user.IsInRole("Poster") ||
                user.IsInRole("ReportViewer") ||
                user.IsInRole("Super") ||
                user.IsInRole("VenueDataEntry") ||
                user.IsInRole("VenueDataViewer") ||
                user.IsInRole("Master")));
        }
    }
}
