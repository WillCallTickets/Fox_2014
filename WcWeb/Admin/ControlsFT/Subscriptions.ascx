<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Subscriptions.ascx.cs" Inherits="wctMain.Admin.ControlsFT.Subscriptions" %>
<div id="sub" runat="server" class="usersubscription">
    <h6 class="black">
        Mail Subscriptions
          <span class="links">
            <asp:LinkButton ID="btnUpdate" runat="server" CssClass="btnadmin" 
            Text="Update" onclick="btnUpdate_Click" />
          </span>
    </h6>
    <div class="subcontent">
        <ul>
            <li>Check to subscribe. Uncheck to unsubscribe.</li>
        </ul>
        <div class="feedback">
            <asp:Label runat="server" ID="lblFeedbackOK" SkinID="FeedbackOK" Text="Subscriptions updated successfully" Visible="false" />
        </div>
        <asp:CheckBoxList runat="server" RepeatLayout="flow" ID="chkSubs" RepeatColumns="1" CellSpacing="4" 
            ondatabinding="chkSubs_DataBinding" ondatabound="chkSubs_DataBound" EnableViewState="true" />
    </div>
</div>
