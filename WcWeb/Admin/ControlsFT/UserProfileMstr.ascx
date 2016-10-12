<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserProfileMstr.ascx.cs" Inherits="wctMain.Admin.ControlsFT.UserProfileMstr"  %>
<%@ Register Src="/Admin/ControlsFT/UserProf.ascx" TagPrefix="uc1" TagName="UserProfile" %>

<div id="account-profile">
    
    <div class="section-header"><%=this.Page.User.Identity.Name %></div>
    
    <div class="account-profile main-inner">

        <div class="userlinks" style="margin-top:15px;margin-bottom:15px;">
            <asp:HyperLink ID="linkEditProfile" runat="server" CssClass="active btn btn-sm btn-primary" NavigateUrl="" Text="edit profile" />
             <%if (Wcss._Config._AllowCustomerInitiatedNameChanges)
              {%>
                <a href="/WebUser/Default.aspx?p=changename" class="btn btn-sm btn-primary">change email</a>
            <%} %>
            <asp:HyperLink ID="linkChangePass" CssClass="btn btn-sm btn-primary" runat="server" NavigateUrl="/PasswordRecovery.aspx" Text="change pass" />
        </div>
        
        <div class="controlbody">
            <table border="0" cellspacing="0" cellpadding="0" width="98%">
                <tr>
                    <td>
                        <asp:ValidationSummary runat="server" ID="valChangePasswordSummary" ValidationGroup="results" />
                        <asp:CustomValidator ID="CustomResult" runat="server" CssClass="invisible" ErrorMessage="CustomValidator" ValidationGroup="results" 
                            Display="Dynamic">&nbsp;</asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td><uc1:UserProfile ID="UserProfile1" runat="server" /></td>
                </tr>
            </table>
        
            <div class="update" style="margin-top:15px;">
                <asp:LinkButton runat="server" ID="btnUpdate" CssClass="btn btn-sm btn-primary" ValidationGroup="editprofile" OnClick="btnUpdate_Click" >Update</asp:LinkButton>
                <asp:Label runat="server" ID="lblFeedbackOK" forecolor="green" CssClass="success" Text="Profile updated successfully" Visible="false" />
            </div>
        </div>
    </div>
</div>