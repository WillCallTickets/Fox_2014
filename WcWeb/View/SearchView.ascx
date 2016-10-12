<%@ Control Language="C#" AutoEventWireup="false" CodeFile="SearchView.ascx.cs" Inherits="wctMain.View.SearchView" %>

<div class="displayer fade fade-slow search-container static-container">
    <div class="section-header">Search</div>
    <form id="searchresultsform" action="search" class="main-inner" runat="server">        
        <h4><asp:Literal ID="litTitle" runat="server" /></h4>
        <asp:Repeater ID="rptList" runat="server" OnDataBinding="rptList_DataBinding" onitemdatabound="rptList_ItemDataBound">
            <HeaderTemplate><ul class="list-unstyled"></HeaderTemplate>
            <ItemTemplate><li><asp:Literal ID="litItem" runat="server" /></li></ItemTemplate>
            <FooterTemplate></ul></FooterTemplate>
        </asp:Repeater>    
    </form>
</div>

<script type="text/javascript">

    $(document).ready(function () {
        $('.search-container A').address();
    });

</script>