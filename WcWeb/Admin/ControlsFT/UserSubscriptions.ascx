<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserSubscriptions.ascx.cs" Inherits="wctMain.Admin.ControlsFT.UserSubscriptions" %>
<div class="usersubscription">
    <fieldset>
        <div class="controlheader" style="display:inline;">
            <span class="title">Mail Subscriptions:</span>
              <span class="links">
                <asp:LinkButton ID="btnUpdate" runat="server" CssClass="btnadmin" 
                Text="Update" onclick="btnUpdate_Click" />
              </span>
        </div>
        <ul>
            <li>Check to subscribe. Uncheck to unsubscribe.</li>
        </ul>
        <div class="feedback">
            <asp:Label runat="server" ID="lblFeedbackOK" SkinID="FeedbackOK" Text="Subscriptions updated successfully" Visible="false" />
        </div>
        <asp:CheckBoxList runat="server" RepeatLayout="table" ID="chkSubs" RepeatColumns="1" CellSpacing="4" 
            ondatabinding="chkSubs_DataBinding" ondatabound="chkSubs_DataBound" EnableViewState="true" />
    </fieldset>
</div>
