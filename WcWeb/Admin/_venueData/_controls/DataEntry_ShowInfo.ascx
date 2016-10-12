<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DataEntry_ShowInfo.ascx.cs" Inherits="wctMain.Admin._venueData._controls.DataEntry_ShowInfo" %>
<%@ Register assembly="WctControls" namespace="WctControls.WebControls" tagprefix="cc1" %>

<div class="vd-data-entry">
    
    <cc1:ErrorDisplayLabel ID="ErrorDisplayLabel1" runat="server" />

    <asp:FormView id="formEntry" runat="server" ClientIDMode="Static" EnableViewState="true" DefaultMode="Edit" 
        OnDataBinding="formEntry_DataBinding" 
        OnDataBound="formEntry_DataBound" 
        OnItemCommand="formEntry_ItemCommand"
        OnItemUpdating="formEntry_ItemUpdating"
        OnModeChanging="formEntry_ModeChanging" >
        <EmptyDataTemplate>
            A Show has not been selected.
        </EmptyDataTemplate>
        <HeaderTemplate>
            <div class="form-inline form-inline-section-header" role="form">
                <div class="form-group">
                    <h1>Show Info entry</h1>
                </div>
                <div class="form-group" style="vertical-align:middle;padding-left:20px;padding-top:10px;">
                    <asp:LinkButton ID="btnSave" runat="server" ClientIDMode="Static" Text="Save" CommandName="Update" CssClass="btn btn-info btn-form-command"></asp:LinkButton>
                    <asp:LinkButton ID="btnCancel" runat="server" ClientIDMode="Static" Text="Cancel" CommandName="Cancel" CssClass="btn btn-danger btn-form-command"></asp:LinkButton>
                </div>
            </div>
        </HeaderTemplate>
        <EditItemTemplate>
            <div class="form-inline" role="form" >
                <div class="form-group form-group-header">
                    Agent
                </div>
                <div class="form-group form-group-column">
                    <asp:TextBox ID="txtAgent" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("Agent") %>' />
                </div>
            </div>
            <div class="form-inline" role="form" >
                <div class="form-group form-group-header">
                    Buyer
                </div>
                <div class="form-group form-group-column">
                    <asp:TextBox ID="txtBuyer" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("Buyer") %>' />
                </div>
            </div>
            <div class="form-inline" role="form">
                <div class="form-group form-group-header">
                    MOD
                </div>
                <div class="form-group form-group-column">
                    <asp:TextBox ID="txtMod" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("MOD") %>' />
                </div>
            </div>
            <div class="form-inline" role="form" >
                <div class="form-group form-group-header">
                    Tickets Sold
                </div>
                <div class="form-group form-group-column">
                    <asp:TextBox ID="txtTicketsSold" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("TicketsSold") %>' />
                </div>
            </div>
            <div class="form-inline" role="form" >
                <div class="form-group form-group-header">
                    Comps In
                </div>
                <div class="form-group form-group-column">
                    <asp:TextBox ID="txtCompsIn" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("CompsIn") %>' />
                </div>
            </div>
            <div class="form-inline" role="form" >
                <div class="form-group form-group-header">
                    Ticket Gross
                </div>
                <div class="form-group form-group-column">
                    <asp:TextBox ID="txtTicketGross" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("TicketGross") %>' />
                </div>
            </div>
            <div class="form-inline" role="form">
                <div class="form-group form-group-header">
                    Facility Fee
                </div>
                <div class="form-group form-group-column">
                    <asp:TextBox ID="txtFacilityFee" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("FacilityFee") %>' />
                </div>
            </div>
            <div class="form-inline" role="form">
                <div class="form-group form-group-header">
                    Concessions
                </div>
                <div class="form-group form-group-column">
                    <asp:TextBox ID="txtConcessions" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("Concessions") %>' />
                </div>
            </div>
            <div class="form-inline" role="form" >
                <div class="form-group form-group-header">
                    Hospitality
                </div>
                <div class="form-group form-group-column">
                    <asp:TextBox ID="txtHospitality" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("Hospitality") %>' />
                </div>
            </div>
            <div class="form-inline" role="form">
                <div class="form-group form-group-header">
                    Prod Labor
                </div>
                <div class="form-group form-group-column">
                    <asp:TextBox ID="txtProductionLabor" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("ProductionLabor") %>' />
                </div>
            </div>
            <div class="form-inline" role="form" >
                <div class="form-group form-group-header">
                    Security Labor
                </div>
                <div class="form-group form-group-column">
                    <asp:TextBox ID="txtSecurityLabor" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("SecurityLabor") %>' />
                </div>
            </div>
            <div class="form-inline" role="form">
                <div class="form-group form-group-header">
                    Bar Total
                </div>
                <div class="form-group form-group-column">
                    <asp:TextBox ID="txtBarTotal" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("BarTotal") %>' />
                </div>
            </div>
            <div class="form-inline" role="form" >
                <div class="form-group form-group-header">
                    Bar Per Head
                </div>
                <div class="form-group form-group-column">
                    <asp:TextBox ID="txtBarPerHead" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("BarPerHead") %>' />
                </div>
            </div>
            <div class="form-inline" role="form" >
                <div class="form-group form-group-header">
                    Marketing Days
                </div>
                <div class="form-group form-group-column">
                    <asp:TextBox ID="txtMarketingDays" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("MarketingDays") %>' />
                </div>
            </div>
            <div class="form-inline" role="form" >
                <div class="form-group form-group-header">
                    Market Plays
                </div>
                <div class="form-group form-group-column">
                    <asp:TextBox ID="txtMarketPlays" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("MarketPlays") %>' />
                </div>
            </div>
            <div class="form-inline" role="form">
                <div class="form-group form-group-header">
                    Notes
                </div>
                <div class="form-group form-group-column">
                    <asp:TextBox ID="txtNotes" runat="server" ClientIDMode="Static" CssClass="form-control" TextMode="MultiLine" Rows="10" Text='<%#Bind("Notes") %>' />
                </div>
            </div>
        </EditItemTemplate>
    </asp:FormView>
</div>