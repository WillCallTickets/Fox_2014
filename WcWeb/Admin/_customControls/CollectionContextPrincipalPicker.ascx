<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CollectionContextPrincipalPicker.ascx.cs" Inherits="wctMain.Admin._customControls.CollectionContextPrincipalPicker" %>

<span class="btn-group btn-group-justified btn-bold collectioncontextprincipalpicker" >
<asp:Repeater ID="rptPrincipal" runat="server" EnableViewState="false" 
    OnDataBinding="rptPrincipal_DataBinding"
    OnItemDataBound="rptPrincipal_ItemDataBound" 
    OnItemCommand="rptPrincipal_ItemCommand">
    <ItemTemplate>
        <asp:LinkButton ID="linkNav" runat="server" CommandName="select" CausesValidation="false" OnClientClick="return true;" 
            Text="<%#Container.DataItem.ToString() %>" CommandArgument="<%#Container.DataItem.ToString() %>"
            cssClass='<%# (Container.DataItem.ToString() == Atx.CurrentEditPrincipal.ToString()) ? "btn btn-primary" : "btn btn-default" %>'
                />
    </ItemTemplate>
</asp:Repeater>
</span>
