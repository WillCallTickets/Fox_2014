using System;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;
using wctMain.Controller;

namespace wctMain.Admin._editors._controls
{
    public partial class Editor_Customer : MainBaseControl
    {
        private string _criteria { get; set; }

        protected void Page_Load(object sender, EventArgs e) 
        {
            hdnCollectionTableName.DataBind();
        }

        protected void btnAdmins_Click(object sender, EventArgs e)
        {
            List<string> admins = new List<string>();
            List<ListItem> list = new List<ListItem>();
            string sql = "SELECT DISTINCT u.UserName FROM aspnet_users u, aspnet_usersinroles ur, aspnet_roles r WHERE u.userid = ur.userid and ur.roleid = r.roleid and r.rolename <> 'webuser' ORDER BY u.UserName";
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);

            using (IDataReader dr = SubSonic.DataService.GetReader(cmd))
            {
                while (dr.Read())
                {
                    admins.Add(dr.GetValue(dr.GetOrdinal("UserName")).ToString());
                }
            }

            if(admins.Count > 0)
            {
                foreach(string userName in admins)
                {
                    string[] inRoles = Roles.GetRolesForUser(userName);
                    string desc = string.Format("{0} - {1}", userName, string.Join(", ", inRoles));
                    list.Add(new ListItem(desc, userName));
                }
            }

            BindResults("Admins", list);
        }

        private void BindResults(string criteria, List<ListItem> list)
        {
            _criteria = criteria;

            rptResults.DataSource = list;

            rptResults.DataBind();
        }

        protected void rptResults_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                Label lblCriteria = (Label)e.Item.FindControl("lblCriteria");
                lblCriteria.Text = string.Format("Search results for: ... {0} ...", _criteria);
            }
        }

        protected void rptResults_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpt = (Repeater)sender;

            ListItem li = (ListItem)e.Item.DataItem;
            Literal item = (Literal)e.Item.FindControl("litItem");

            if (li != null && li.Text != null && li.Text.Trim().Length > 0 && item != null)
            {
                if (li.Value != null && li.Value.Trim().Length > 0)
                    item.Text = string.Format("<a {0}>{1}</a>", string.Format("href='/Admin/EditUser.aspx?username={0}'", li.Value), li.Text);
                else
                    item.Text = string.Format("<div class=\"{0}\">{1}</div>", "criteria", li.Text);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            List<ListItem> list = new List<ListItem>();

            //decide what to search for
            string email = txtEmail.Text.Trim();
            string lastname = txtLastName.Text.Trim();
            string month = ddlBdMonth.SelectedValue;

            string criteria = string.Empty;

            if(email.Length > 0)
            {
                criteria = string.Format("Email address: {0}", email);

                string search = string.Format("{0}%", email);

                MembershipUserCollection users = Membership.FindUsersByName(search);//we could use find by email here too

                foreach(MembershipUser mu in users)
                    list.Add(new ListItem(mu.UserName));

                //Find previous emails
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("SET NOCOUNT ON SELECT DISTINCT u.[UserName] as 'UserName' FROM [User_PreviousEmail] upe, [Aspnet_Users] u WHERE upe.[EmailAddress] LIKE @search AND ");
                sb.Append("upe.[UserId] = u.[UserId] AND u.[ApplicationId] = @appId; ");
                SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
                cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, DbType.Guid); 
                cmd.Parameters.Add("@search", search);

                using (IDataReader dr = SubSonic.DataService.GetReader(cmd))
                {
                    bool init = false;
                    while (dr.Read())
                    {
                        if (!init)
                        {
                            list.Add(new ListItem(string.Empty));
                            list.Add(new ListItem("...related matches...", string.Empty));
                            
                            init = true;
                        }

                        list.Add(new ListItem(dr["UserName"].ToString()));

                    }

                    dr.Close();
                }
                //end find previous


                //Find email subscribers only
                System.Text.StringBuilder sb2 = new System.Text.StringBuilder();
                sb2.Append("SELECT DISTINCT u.[UserName] as 'UserName' FROM [Aspnet_Users] u WHERE u.[ApplicationId] = @appId AND u.[UserName] LIKE @search AND ");
                sb2.Append("u.[UserId] NOT IN (SELECT m.[UserId] FROM [Aspnet_Membership] m WHERE m.[ApplicationId] = @appId); ");
                SubSonic.QueryCommand cmd2 = new SubSonic.QueryCommand(sb2.ToString(), SubSonic.DataService.Provider.Name);
                cmd2.Parameters.Add("@appId", _Config.APPLICATION_ID, DbType.Guid);
                cmd2.Parameters.Add("@search", search);

                using (IDataReader dr = SubSonic.DataService.GetReader(cmd2))
                {
                    bool init = false;
                    while (dr.Read())
                    {
                        if (!init)
                        {
                            list.Add(new ListItem(string.Empty));
                            list.Add(new ListItem("...email subscribers...", string.Empty));

                            init = true;
                        }

                        list.Add(new ListItem(dr["UserName"].ToString()));

                    }

                    dr.Close();
                }
                //end find subscribers only

                txtEmail.Text = string.Empty;

            }
            else if (lastname.Length > 0)
            {
                criteria = string.Format("LastName: {0}", lastname);
                using (IDataReader dr = SPs.AspnetMembershipFindUsersByProfileParameter(_Config.APPLICATION_NAME, "LastName", lastname, 0, 10000).GetReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new ListItem(dr.GetValue(dr.GetOrdinal("UserName")).ToString()));
                    }

                    dr.Close();
                }

                txtLastName.Text = string.Empty;
            }
            else if (month != "0")
            {
                criteria = string.Format("Birthdays by month : {0}", ddlBdMonth.SelectedItem.Text);
                month = string.Format("{0}/%", month);
                using (IDataReader dr = SPs.AspnetMembershipFindUsersLikeProfileParameter(_Config.APPLICATION_NAME, "DateOfBirth", month, 0, 10000).GetReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new ListItem(dr.GetValue(dr.GetOrdinal("UserName")).ToString()));
                    }

                    dr.Close();
                }

                ddlBdMonth.SelectedIndex = 0;
            }

            BindResults(criteria, list);
        }
       
}
}
