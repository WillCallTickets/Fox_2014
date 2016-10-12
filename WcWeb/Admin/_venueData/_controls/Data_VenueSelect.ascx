<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Data_VenueSelect.ascx.cs" Inherits="wctMain.Admin._venueData._controls.Data_VenueSelect" %>

<div class="data-selector venue-select">
    <div class="form-inline" role="form">
        <div class="form-group" style="padding-right:20px;">
            <asp:DropDownList ID="ddlVenue" runat="server" EnableViewState="true" AppendDataBoundItems="true" Width="300px"
                OnDataBinding="ddlVenue_DataBinding" 
                OnDataBound="ddlVenue_DataBound" 
                AutoPostBack="true" 
                OnSelectedIndexChanged="ddlVenue_SelectedIndexChanged" 
                CssClass="form-control">    
                <asp:ListItem Text="All Venues" Value="0" />            
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <h4>Start Date &nbsp;</h4>
        </div>
        <div class="form-group" style="padding-right:20px;">
            <asp:TextBox ID="txtVenueSelectStart" runat="server" ClientIDMode="Static" CssClass="btdt-picker venue-dtpicker form-control" 
                AutoPostBack="true" OnTextChanged="txtVenueSelectDate_TextChanged"
                Text='' TextMode="DateTime" Width="185px" />
        </div>
        <div class="form-group">
            <h4>End Date &nbsp;</h4>
        </div>
        <div class="form-group" style="padding-right:20px;">
            <asp:TextBox ID="txtVenueSelectEnd" runat="server" ClientIDMode="Static" CssClass="btdt-picker venue-dtpicker form-control" 
                AutoPostBack="true" OnTextChanged="txtVenueSelectDate_TextChanged" 
                Text='' TextMode="DateTime" Width="185px" />
        </div>
    </div>
</div>