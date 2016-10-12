<%@ Control Language="C#" AutoEventWireup="false" CodeFile="ContactView.ascx.cs" Inherits="z2Main.View.ContactView" %>
    
<div class="contact-container view-container">
    <div class="main-inner">
        <div class="section-header">
            <h2>Contact Us</h2>
        </div>
        <asp:Repeater ID="rptContact" runat="server" OnDataBinding="rptContact_DataBinding" OnItemDataBound="rptContact_ItemDataBound">
            <HeaderTemplate><ul class="list-unstyled contact-listing"></HeaderTemplate>
            <ItemTemplate>
                <li><a href='mailto:<%#Eval("EmailAddress_Derived") %>'><%#Eval("Title") %></a> <%#Eval("EmailAddress_Derived") %></li>
            </ItemTemplate>
            <FooterTemplate></ul></FooterTemplate>
        </asp:Repeater>
    </div>
</div>
