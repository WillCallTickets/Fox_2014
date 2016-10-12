<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DataEntry_Container.ascx.cs" Inherits="wctMain.Admin._venueData._controls.DataEntry_Container" %>
<%@ Register src="/Admin/_venueData/_controls/DataEntry_ShowInfo.ascx" tagname="DataEntry_ShowInfo" tagprefix="uc1" %>

<div class="vd-dataentry-container">
    
    <div class="form-inline">
        <asp:LinkButton ID="btnClose" runat="server" CommandName="Cancel" CausesValidation="false" Text="Close" CssClass="btn btn-primary" />
        <div class="form-group">
            <h4><asp:Literal ID="litName" runat="server" ></asp:Literal></h4>
        </div>
    </div>
    <div class="vd-data-entry entry-select">
    
        <!-- Nav tabs -->
        <asp:Repeater ID="rptNavTabs" runat="server" EnableViewState="false" OnItemDataBound="rptNavTabs_ItemDataBound">
            <HeaderTemplate><ul id="pnlNavTab" class="nav nav-tabs"></HeaderTemplate>
            <ItemTemplate><asp:Literal ID="litItem" runat="server" EnableViewState="false" /></ItemTemplate>
            <FooterTemplate></ul></FooterTemplate>
        </asp:Repeater>

        <div class="tab-content" >
            <asp:Literal ID="divInfo" runat="server" EnableViewState="false" />
            
                <uc1:DataEntry_ShowInfo ID="DataEntry_ShowInfo1" runat="server" />

            <asp:Literal ID="divInfoEnd" runat="server" EnableViewState="false" Text="</div>" />
            <asp:Literal ID="divGenres" runat="server" EnableViewState="false" />
            <div class="form-inline" role="form">
                                <div class="form-group form-group-header-col">
                                    Genres
                                </div>
                                <div class="form-group form-group-column">
                                    list<br />ddlToAdd more
                                </div>
                                <div class="form-group form-group-header-col">
                                    Sub-Genres
                                </div>
                                <div class="form-group form-group-column">
                                    list<br />ddlToAdd more
                                </div>
                            </div>
            <asp:Literal ID="divGenresEnd" runat="server" EnableViewState="false" Text="</div>" />
            <asp:Literal ID="divTickets" runat="server" EnableViewState="false" />
            <div class="form-inline" role="form">
                                <div class="form-group form-group-header-col">
                                    Tickets<br />
                                </div>
                                <div class="form-group form-group-column">
                                    list<br />ddlToAdd more<br /># comps
                                </div>
                                <div class="form-group form-group-header-col">
                                    Outlets
                                </div>
                                <div class="form-group form-group-column">
                                    list<br />ddlToAdd more<br />
                                    totals<br />#comps in
                                </div>
                            </div>
            <asp:Literal ID="divTicketsEnd" runat="server" EnableViewState="false" Text="</div>" />
            <asp:Literal ID="divConcessions" runat="server" EnableViewState="false" />
            <div class="form-inline" role="form">
                                <div class="form-group form-group-header-col">
                                    Concessions
                                </div>
                                <div class="form-group form-group-column">
                                    list<br />ddlToAdd more
                                </div>
                            </div>
            <asp:Literal ID="divConcessionsEnd" runat="server" EnableViewState="false" Text="</div>" />
            <asp:Literal ID="divExpenses" runat="server" EnableViewState="false" />
                <div class="form-inline" role="form">
                    <div class="form-group form-group-header-col">
                        Expenses
                    </div>
                    <div class="form-group form-group-column">
                        list<br />ddlToAdd more
                    </div>
                </div>
            <asp:Literal ID="divExpensesEnd" runat="server" EnableViewState="false" Text="</div>" />
            <asp:Literal ID="divMarketing" runat="server" EnableViewState="false" />
                <div class="form-inline" role="form">
                    <div class="form-group form-group-header-col">
                        Marketing Days
                    </div>
                    <div class="form-group form-group-column">
                        #
                    </div>
                    <div class="form-group form-group-header-col">
                        Marketing Plays
                    </div>
                    <div class="form-group form-group-column">
                        #
                    </div>
                </div>
                <div class="form-inline" role="form">
                    <div class="form-group form-group-header-col">
                        Market Notes
                    </div>
                    <div class="form-group form-group-column">
                        txt
                    </div>
                </div>
            <asp:Literal ID="divMarketingEnd" runat="server" EnableViewState="false" Text="</div>" />
        </div><!-- end tab-content-->
    </div>
</div>