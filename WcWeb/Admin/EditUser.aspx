<%@ Page Language="C#" MasterPageFile="/View/Masters/TemplateAdmin.master" AutoEventWireup="true" CodeFile="EditUser.aspx.cs" ValidateRequest="false" 
Inherits="wctMain.Admin.EditUser" Title="Admin - Edit User" %>
<%@ Register Src="/Admin/ControlsFT/UserProfile.ascx" TagName="UserProfile" TagPrefix="uc1" %>
<%@ Register src="/Admin/ControlsFT/UserSubscriptions.ascx" tagname="UserSubscriptions" tagprefix="uc2" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <div id="edituser">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="jqhead around">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" >
                        <tr>
                            <th><%=userName %></th>
                            <td>
                                <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="editor" CssClass="invisible" 
                                    Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:ValidationSummary ID="ValidationSummary1" CssClass="validationsummary" HeaderText="" ValidationGroup="editor" runat="server" />                
                <table border="0" cellpadding="0" cellspacing="0" width="800px">
                    <tr>
                        <td style="width:30%;padding-right:6px;">
                            <div class="jqedt around">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <th class="subtitle" style="text-align:left;width:100%;">General Info</th>
                                        <td style="text-align:right;white-space:nowrap;">
                                            <%if (this.Page.User.IsInRole("Administrator")){%>
                                            <asp:Button ID="btnReset" runat="server" ToolTip="This will reset the password and email the customer with the new password." CausesValidation="false" 
                                                Text="Reset Pass" CssClass="btn btn-primary btn-xs" OnClick="btnReset_Click" OnClientClick="return confirm('Are you sure you want to reset this password?')" />
                                            <%} %>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edttbl">
                                    <tr>
                                       <th>User ID</th>
                                       <td style="width:100%;"><asp:Literal runat="server" ID="lblUserID" /></td>
                                    </tr>
                                    <tr>
                                        <th>User Name</th>
                                        <td><asp:Literal runat="server" ID="lblUserName" /></td>
                                    </tr>
                                    <tr>
                                        <th>E-mail</th>
                                        <td><asp:HyperLink runat="server" ID="lnkEmail" /></td>
                                    </tr>
                                    <%if (this.Page.User.IsInRole("PasswordViewer")){%>
                                    <tr>
                                        <th>Hint Question</th>
                                        <td><asp:Literal ID="litQuestion" runat="server" OnDataBinding="litQuestion_DataBinding" /></td>
                                    </tr>
                                    <%if (this.Page.User.IsInRole("Super")) {%>
                                    <tr>
                                        <th>Hint Answer</th>
                                        <td><asp:Literal ID="litHintAnswer" runat="server" OnDataBinding="litHintAnswer_DataBinding" /></td>
                                    </tr>
                                    <%} %>
                                    <tr>
                                        <th>Password</th>
                                        <td><asp:Literal ID="litPassword" runat="server" OnDataBinding="litPassword_DataBinding" /></td>
                                    </tr>                          
                                    <%} %>
                                    <tr>
                                        <th>Registered</th>
                                        <td><asp:Literal runat="server" ID="lblRegistered" /></td>
                                    </tr>
                                    <tr>
                                        <th>Last Login</th>
                                        <td><asp:Literal runat="server" ID="lblLastLogin" /></td>
                                    </tr>
                                    <tr>
                                        <th>Last Activity</th>
                                        <td><asp:Literal runat="server" ID="lblLastActivity" /></td>
                                    </tr>
                                    <tr>
                                        <th>Online Now</th>
                                        <td><asp:CheckBox runat="server" ID="chkOnlineNow" Enabled="false" /></td>
                                    </tr>
                                    <tr>
                                        <th>Active</th>
                                        <td><asp:CheckBox runat="server" ID="chkApproved" AutoPostBack="true" OnCheckedChanged="chkApproved_CheckedChanged" /></td>
                                    </tr>
                                    <tr>
                                        <th>Locked Out</th>
                                        <td><asp:CheckBox runat="server" ID="chkLockedOut" AutoPostBack="true" OnCheckedChanged="chkLockedOut_CheckedChanged" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div class="jqedt around">
                                <table border="0" cellpadding="0" cellspacing="0" class="edttbl">
                                    <tr>
                                        <td>
                                            <uc2:UserSubscriptions ID="UserSubscriptions1" OnUpdated="UserSubscriptions_Updated" AllowAdminSubscriptions="true" runat="server" />
                                            <asp:Literal ID="litUserIsEmail" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td style="width:30%;padding-right:6px;">
                            <div class="jqedt around">
                                <div class="subtitle" style="padding:4px">Previous Usernames</div>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edttbl">
                                    <tr>
                                        <td>
                                            <asp:Repeater ID="rptPreviousUsername" runat="server" OnDataBinding="rptPreviousUsername_DataBinding">
                                                <ItemTemplate>
                                                    <%#Eval("Text") %>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:Panel ID="panelRoles" runat="server">
                                <div class="jqedt around">
                                    <asp:Literal ID="litUserIsEmailRoles" runat="server" />
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <th class="subtitle" style="text-align:left;width:100%;">Edit User Roles</th>
                                            <td style="text-align:right">
                                                <asp:Button runat="server" ID="btnUpdateRoles" CssClass="btn btn-primary btn-xs" Text="Update Roles" 
                                                    OnClick="btnUpdateRoles_Click" CausesValidation="false" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edttbl">
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label runat="server" ID="lblRolesFeedbackOK" SkinID="FeedbackOK" Text="Roles updated successfully" Visible="false" />
                                                <asp:CheckBoxList runat="server" RepeatLayout="table" ID="chkRoles" RepeatColumns="2" CellSpacing="2" CssClass="role-check" 
                                                    ondatabinding="chkRoles_DataBinding" ondatabound="chkRoles_DataBound" />
                                            </td>
                                        </tr>
                                        <%if (this.Page.User.IsInRole("Super")){%>
                                        <tr>
                                            <th>
                                                <asp:RequiredFieldValidator ID="valRequireNewRole" runat="server" ControlToValidate="txtNewRole" SetFocusOnError="true"
                                                    ErrorMessage="Role name is required." ToolTip="Role name is required." ValidationGroup="CreateRole">*</asp:RequiredFieldValidator>
                                                    New Role
                                            </th>
                                            <td style="width:100%;white-space:nowrap;"><asp:TextBox runat="server" ID="txtNewRole" />&nbsp;
                                                <asp:Button runat="server" CssClass="btn btn-primary btn-xs" ID="btnCreateRole" Text="Create" CausesValidation="false"
                                                    OnClick="btnCreateRole_Click" />
                                            </td>
                                        </tr>
                                        <%} %>
                                    </table>
                                </div>
                            </asp:Panel>
                        </td>
                        <td style="width:30%;padding-right:6px;">
                            <div class="jqedt around">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <th class="subtitle" style="text-align:left;">User Settings</th>
                                        <td style="text-align:right;white-space:nowrap;">&nbsp;
                                            <%if (this.Page.User.IsInRole("OrderFiller")){%>
                                            <asp:Button runat="server" ID="btnUpdateProfile" CssClass="btn btn-primary btn-xs" 
                                                causesvalidation="false"
                                                Text="Update User Settings" OnClick="btnUpdateProfile_Click" /></span>
                                            <%} %>
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" cellpadding="0" cellspacing="0" class="edttbl">
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lblProfileFeedbackOK" SkinID="FeedbackOK" Text="Profile updated successfully" Visible="false" />&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="adminprofile">
                                                <uc1:UserProfile ID="UserProfile1" runat="server" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%if (this.Page.User.IsInRole("Super")){%>
                            <div class="jqedt around">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <th class="subtitle" style="text-align:left;">Change Email</th>
                                        <td style="text-align:right">
                                            <asp:Button runat="server" ID="btnChangeEmail" CssClass="btn btn-primary btn-xs"
                                                Text="Change Email" onclick="btnChangeEmail_Click" ValidationGroup="editor" CausesValidation="true" 
                                                OnClientClick="return confirm('Are you sure you want to change this email address/account name?')" />
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edttbl">
                                    <tr>
                                        <th>
                                            <asp:RegularExpressionValidator runat="server" ID="valEmailPattern" ValidationGroup="editor" 
                                                 CssClass="validator" Display="Static" 
                                                SetFocusOnError="true" ControlToValidate="txtNewEmail" 
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                                ErrorMessage="Please enter a valid e-mail address." 
                                                 onload="valEmailPattern_Load">*</asp:RegularExpressionValidator>
                                            New Email</th>
                                        <td style="width:100%;"><asp:TextBox ID="txtNewEmail" runat="server" MaxLength="256" Width="190px" /></td>
                                    </tr>
                                </table>
                            </div>
                            <%} %>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <br />
                            <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" CssClass="lsttbl" EnableViewState="false">
                                <AlternatingRowStyle BackColor="#f1f1f1" />
                                <EmptyDataTemplate>
                                    <div class="">No User Events</div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
        OnSelecting="SqlDataSource1_Selecting"
        SelectCommand="SELECT eq.[DateToProcess], eq.[DateProcessed], eq.[Status], eq.[CreatorName], 
            eq.[UserName], eq.[Verb], eq.[OldValue], eq.[NewValue], eq.[Description], eq.[Context], eq.[Ip] 
            FROM [EventQArchive] eq, [Aspnet_Users] u 
            WHERE u.[ApplicationId] = @appId AND u.[UserName] = @userName AND u.[UserId] = eq.[UserId]
            UNION 
            SELECT eq.[DateToProcess], eq.[DateProcessed], eq.[Status], eq.[CreatorName], 
            eq.[UserName], eq.[Verb], eq.[OldValue], eq.[NewValue], eq.[Description], eq.[Context], eq.[Ip] 
            FROM [EventQ] eq, [Aspnet_Users] u 
            WHERE u.[ApplicationId] = @appId AND u.[UserName] = @userName AND u.[UserId] = eq.[UserId] 
            ORDER BY [DateToProcess] Desc">
        <SelectParameters>
            <asp:Parameter Name="appId" DbType="Guid" />
            <asp:QueryStringParameter Name="userName" QueryStringField="UserName" Type="string" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>