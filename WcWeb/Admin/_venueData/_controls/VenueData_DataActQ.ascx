<%@ Control Language="C#" AutoEventWireup="true" CodeFile="VenueData_DataActQ.ascx.cs" Inherits="wctMain.Admin._venueData._controls.VenueData_DataActQ" %>
<%@ Register src="/Admin/_venueData/_controls/Data_VenueSelect.ascx" tagname="Data_VenueSelect" tagprefix="uc2" %>
<%@ Register src="/Admin/_venueData/_controls/DataEntry_Container.ascx" tagname="DataEntry_Container" tagprefix="uc1" %>



<div id="workspace" runat="server" class="venuedata data-view">
    <h1>Act Queries</h1>
    <div style="background-color:#999;padding:5px;">
                Not yet implemented
            <uc2:Data_VenueSelect ID="Data_VenueSelect1" runat="server" />
                </div>
    <br />
    <div class="form-inline">
        <div class="form-group form-search" role="search">
            <asp:TextBox ID="vdsearchterms" ClientIDMode="Static" runat="server" AutoCompleteType="Disabled" Width="250px" CssClass="form-control typeahead" />
            <asp:LinkButton ID="vdsitesearch" ClientIDMode="Static" runat="server"><i class="icon-search"></i></asp:LinkButton>        
        </div>
        <div class="form-group">
            
        </div>
    </div>

    <asp:ListView ID="listCurrentActShows" ClientIDMode="Static" runat="server" EnableViewState="true" 
        onDataBinding="listCurrentActShows_DataBinding"
        OnDataBound="listCurrentActShows_DataBound" 
        OnItemDataBound="listCurrentActShows_ItemDataBound" 
        OnItemEditing="listCurrentActShows_ItemEditing"
        OnItemCanceling="listCurrentActShows_ItemCanceling"
        ItemPlaceholderID="ListViewContent" 
        >
        <EmptyDataTemplate>
            no shows available
        </EmptyDataTemplate>
       <LayoutTemplate>
           <h1><asp:Literal ID="litName" runat="server" ></asp:Literal></h1>
            <div class="vdquery-row vdquery-item">
                <asp:Literal ID="litAggs" runat="server" ></asp:Literal>
            </div>
            <br />
           <asp:Panel ID="ListViewContent" runat="server" />
       </LayoutTemplate>
        <EditItemTemplate>
            <div class="well-behind">
                <div class="well well-outfront fade fade-slow in">                    
                    <uc1:DataEntry_Container ID="DataEntry_Container1" runat="server" />
                </div>
            </div>
        </EditItemTemplate>
        <ItemTemplate>
            <div class="vdquery-row vdquery-item">
                <asp:Literal ID="litItem" runat="server" ></asp:Literal>
            </div>
            <div class="vdquery-row vdquery-info">
                <asp:LinkButton ID="linkEdit" runat="server" Text="Edit" CommandName="Edit" CssClass="btn btn-primary btn-xs" />
                <asp:Literal ID="litInfo" runat="server" ></asp:Literal>
            </div>
            <br />
        </ItemTemplate>
    </asp:ListView>
</div>