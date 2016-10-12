<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CollectionContextPager.ascx.cs" EnableViewState="true" Inherits="wctMain.Admin._customControls.CollectionContextPager" %>
<%@ Register assembly="WctControls" namespace="WctControls.WebControls" tagprefix="cc1" %>
<%@ Register src="/Admin/_customControls/CollectionContextPrincipalPicker.ascx" tagname="CollectionContextPrincipalPicker" tagprefix="uc1" %>

<div class="panel-heading panel-pager">
    <div class="badge-container">
        <span class="badge" ><%= PagerTitle %></span> 
    </div>                    
    <div class="coll-search pull-right">
        <input type="text" class="form-control typeahead" placeholder="Search..." id="coll-searchterms" autocomplete="off" />
        <a href="#" id="coll-sitesearch"><span class="glyphicon glyphicon-search"></span></a> 
    </div>
    <div class="coll-template pull-right">
        <asp:PlaceHolder ID="placeValidation" runat="server" />
    </div>    
</div>
<div class="clearfix"></div>
<div class="panel-actions panel-pager">
    <div class="cc-status">
        <a id="criteriatoggle" title="Click to edit filter criteria" class="btn">
            <span class="glyphicon glyphicon-filter"></span>
            <strong><asp:Literal ID="litStatus" runat="server" EnableViewState="false" /></strong>
            <code>click this row for filter options</code>
        </a>
    </div>
    <table class="context-pager" border="0" cellspacing="0" cellpadding="0">
        <tbody>           
            <tr class="cc-pager-row">
                <td class="cc-principal">
                    <uc1:CollectionContextPrincipalPicker ID="CollectionContextPrincipalPicker1" runat="server" />
                </td>
                <td class="cc-paging">
                    <ul class="pagination pagination-sm">
                        <li>
                            <asp:LinkButton CssClass="" ID="btnFirst" CausesValidation="false" runat="server" Tooltip="first" CommandName="firstpage" OnClick="nav_Click">
                                <span class="glyphicon glyphicon-fast-backward"></span>
                            </asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton CssClass="" ID="btnPrev" CausesValidation="false" runat="server" Tooltip="prev" CommandName="prevpage" OnClick="nav_Click" >
                                <span class="glyphicon glyphicon-backward"></span>
                            </asp:LinkButton>
                        </li>
                        <asp:Repeater ID="rptPageLink" runat="server" OnDataBinding="rptPageLink_DataBinding" OnItemDataBound="rptPageLink_ItemDataBound"
                            EnableViewState="true">
                            <ItemTemplate>
                                <asp:Literal ID="litLiStart" runat="server" EnableViewState="false" />
                                <asp:LinkButton CssClass="" ID="btnPage" CausesValidation="false" runat="server" Tooltip="go to specified page" CommandName="page" Text='<%#Eval("Text") %>' 
                                    CommandArgument='<%#Eval("Value") %>' OnClick="nav_Click" EnableViewState="true" />
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                        <li>
                            <asp:LinkButton CssClass="" ID="btnNext" CausesValidation="false" runat="server" ToolTip="next" CommandName="nextpage" OnClick="nav_Click">
                                <span class="glyphicon glyphicon-forward"></span>
                            </asp:LinkButton></li>
                        <li>
                            <asp:LinkButton CssClass="" ID="btnLast" CausesValidation="false" runat="server" ToolTip="last" CommandName="lastpage" OnClick="nav_Click">
                                <span class="glyphicon glyphicon-fast-forward"></span>
                            </asp:LinkButton></li>
                    </ul>
                </td>
                <td class="cc-viewing">
                    <span class="badge badge-primary"><asp:Literal ID="litViewing" runat="server" EnableViewState="false" /></span>
                </td>
                <td class="cc-page-size">
                    <strong>Results</strong>
                    <asp:DropDownList ID="ddlPageSize" CssClass="form-control page-size-control" runat="Server" AutoPostBack="True" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged" 
                        OnDataBinding="ddlPageSize_DataBinding" />
                </td>
            </tr>
        </tbody>
    </table>      
    <div id="coll-criteria-editor" class="well-behind" style="display:none;">
        <div class="well well-outfront well-outfront-wide fade fade-med" >
            <h3>Edit Search Criteria</h3>
            <div class="row context-edit-row">
                <div class="status-portal col-xs-4" style="display:inline-block;">
                    <!-- all, active, notactive -->
                    <h4 >Status</h4>
                    <asp:RadioButtonList ID="rdoStatus" runat="server" ClientIDMode="Static" CssClass="form-control form-control-vertical" OnDataBinding="rdoStatus_DataBinding"
                        OnDataBound="rdoStatus_DataBound">
                    </asp:RadioButtonList>
                </div>
                <div class="status-portal col-xs-6" style="display:inline-block;">
                    <h4 >Run Time</h4>
                    <cc1:BootstrapDateTimePicker ID="txtDateStart" Label="Start" Date='' DateCompareEmpty="min" CssClass="banner-dtpicker" runat="server" />
                    <br />
                    <cc1:BootstrapDateTimePicker ID="txtDateEnd" Label="End" Date='' DateCompareEmpty="max" CssClass="banner-dtpicker" runat="server" />
                </div>
                <div class="status-portal col-xs-10">
                    <br/>
                    <asp:LinkButton ID="criteriaapply" runat="server" ClientIDMode="Static" CssClass="btn btn-primary" Text="Apply Criteria" OnClick="lnkCriteria_Click" />
                    <a id="criteriacancel" class="btn btn-primary" style="margin-left:25px;">Cancel</a>
                </div>
            </div>
        </div>
    </div>
</div>