<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserProfile.ascx.cs" Inherits="wctMain.Admin.ControlsFT.UserProfile" %>

<%@ Register src="/Admin/ControlsFT/UserSubscriptions.ascx" tagname="UserSubscriptions" tagprefix="uc1" %>

<span class="membersince">Member since: <asp:Literal ID="LiteralSince" runat="server" /></span>

<% if(this.Page.MasterPageFile.ToLower().IndexOf("templateadmin") == -1){ %>
<div style="color: Red; font-weight: bold; margin-bottom: 1.5em;">
    <div>If you need to change a shipping address for an order, please contact <a href="/Contact.aspx">customer support.</a></div>
    <div>Changing your address here will not update the shipping address on your current order(s).</div>
</div>
<%} %>

<asp:ValidationSummary runat="server" ID="valSummary" ValidationGroup="editprofile" DisplayMode="BulletList" HeaderText="Please correct the following errors:" cssclass="validationsummary" />

<table border="0" cellpadding="2" cellspacing="0">
    <tr>
        <td valign="top">
            <table cellpadding="2">
               <tr>
                  <td style="width:80px;" class="label">First name:</td>
                  <td><asp:TextBox ID="txtFirstName" MaxLength="50" runat="server" Width="250px"></asp:TextBox></td>
               </tr>
               <tr>
                  <td class="label">Last name:</td>
                  <td><asp:TextBox ID="txtLastName" runat="server" MaxLength="50" Width="250px"></asp:TextBox></td>
               </tr>
               <tr>
                  <td class="label">Gender:</td>
                  <td>
                     <asp:DropDownList runat="server" ID="ddlGenders" Width="256px">
                        <asp:ListItem Text="Please select one..." Value="" Selected="True" />
                        <asp:ListItem Text="Male" Value="male" />
                        <asp:ListItem Text="Female" Value="female" />
                     </asp:DropDownList>
                  </td>
               </tr>
               <tr>
                  <td class="label">Birth date:</td>
                  <td style="white-space:nowrap;">
                     <asp:TextBox ID="txtBirthDate" runat="server" Width="250px"></asp:TextBox>
                     <asp:CompareValidator runat="server" ID="valBirthDateType" ControlToValidate="txtBirthDate"
                        SetFocusOnError="true" Display="Static" Operator="DataTypeCheck" Type="Date" ValidationGroup="editprofile"
                        ErrorMessage="The format of the birth date is not valid. Please enter a date in the form MM/dd/yyyy." 
                        ToolTip="The format of the birth date is not valid.  Please enter a date in the form MM/dd/yyyy.">*</asp:CompareValidator>
                  </td>
               </tr>
               <tr>
                  <td class="label">Address1:</td>
                  <td><asp:TextBox runat="server" ID="txtAddress1" MaxLength="60" Width="250px" /></td>
               </tr>
                <tr>
                  <td class="label">Address2:</td>
                  <td><asp:TextBox runat="server" ID="txtAddress2" MaxLength="60" Width="250px" /></td>
               </tr>
               
               <tr>
                  <td class="label">City:</td>
                  <td><asp:TextBox runat="server" ID="txtCity" MaxLength="40" Width="250px" /></td>
               </tr>
               <tr>
                  <td class="label">State/Region:</td>
                  <td><asp:TextBox runat="server" ID="txtState" MaxLength="2" Width="25px" />(use 2 char
                      state/province code)</td>
               </tr>
               <tr>
                  <td style="white-space:nowrap;" class="label">Zip/Postal code:</td>
                  <td><asp:TextBox runat="server" ID="txtPostalCode" MaxLength="20" Width="250px" /></td>
               </tr>
               <tr>
                  <td class="label">Country:</td>
                  <td>
                     <asp:DropDownList ID="ddlCountry" runat="server" OnDataBinding="ddlCountry_DataBinding" OnDataBound="ddlCountry_DataBound" />
                  </td>
               </tr>
               <tr>
                  <td class="label">Phone:</td>
                  <td><asp:TextBox runat="server" ID="txtPhone" MaxLength="25" Width="250px" /></td>
               </tr>
            </table>
        </td>
        <%if (this.DisplayUserSubscriptions)
          {%>
            <td valign="top">
                <uc1:UserSubscriptions ID="UserSubscriptions1" AllowAdminSubscriptions="true" runat="server" />
            
            </td>
        <%} %>
    </tr>
</table>

