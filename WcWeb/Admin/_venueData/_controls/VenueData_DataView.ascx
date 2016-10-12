<%@ Control Language="C#" AutoEventWireup="true" CodeFile="VenueData_DataView.ascx.cs" Inherits="wctMain.Admin._venueData._controls.VenueData_DataView" %>
<%@ Register src="/Admin/_venueData/_controls/Data_VenueSelect.ascx" tagname="Data_VenueSelect" tagprefix="uc2" %>

<uc2:Data_VenueSelect ID="Data_VenueSelect1" runat="server" />

<div id="workspace" runat="server" class="venuedata data-view">
    <h1>Data View</h1>

    <h3>start date</h3>
    <h3>end date</h3>

    <h3>list of venues (in that time period?)</h3>

    <h3>Break down by venue or select combo or all venues</h3>

    <h3>Aggregates - see metrics in time period</h3>
    


</div>