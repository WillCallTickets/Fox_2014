<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Banner_Edit.ascx.cs" Inherits="wctMain.Admin._collectionEditors._banner._controls.Banner_Edit" %>
<%@ Register src="~/Admin/_collectionEditors/_banner/_controls/EditBanner_Container.ascx" tagname="EditBanner_Container" tagprefix="uc1" %>
<%@ Register Src="~/Admin/_customControls/CollectionContextPager.ascx" TagPrefix="uc3" TagName="CollectionContextPager" %>
<%@ Register assembly="WctControls" namespace="WctControls.WebControls" tagprefix="cc1" %>

<div id="bannereditor" class="show-mgmt">
    <asp:HiddenField ID="hdnCollectionTableName" runat="server" ClientIDMode="Static" Value="<%# Wcss.SalePromotion.Schema.TableName %>" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="showmgr" >
                <div class="panel panel-primary">
                    <uc3:CollectionContextPager ID="CollectionContextPager1" runat="server" PagerTitle="Banner Editor" >
                        <Template>
                            <asp:LinkButton ID="linkNew" ClientIDMode="Static" runat="server" tooltip="Add a new Banner"
                                OnClick="linkNew_Click" EnableViewState="true" CommandName="New" CssClass="btn btn-default btn-add-new" >New</asp:LinkButton>
                        </Template>
                    </uc3:CollectionContextPager>
                    <div class="panel-body">
                    <asp:ListView ID="listEnt" ClientIDMode="Static" runat="server" EnableViewState="true" DataKeyNames="Id" 
                    DataSourceID="ObjBanners" 
                    onDataBinding="listEnt_DataBinding"
                    OnItemDataBound="listEnt_ItemDataBound" 
                    OnItemEditing="listEnt_ItemEditing" 
                    OnItemInserting="listEnt_ItemInserting" 
                    OnItemDeleting="listEnt_ItemDeleting"
                    OnItemCanceling="listEnt_ItemCanceling"  
                    OnItemCommand="listEnt_ItemCommand"
                    ItemPlaceholderID="ListViewContent" 
                    >
                    <EmptyDataTemplate>
                        no banners available
                    </EmptyDataTemplate>
                    <LayoutTemplate>  
                        <div id="banner-items" class="orderable-items">
                            <ul>
                            <asp:Panel ID="ListViewContent" runat="server" />
                            </ul>
                        </div>
                    </LayoutTemplate>
                    <EditItemTemplate>
                        <div class="well-behind">
                            <div class="well well-outfront fade fade-slow in">
                                <uc1:EditBanner_Container ID="EditBanner_Container1" runat="server" />
                            </div>
                        </div>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <li id='ent_<%#Eval("Id") %>' class="vdquery-row vdquery-item form-inline vdquery-row-select">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0" class="director-list">
                                <tr>
                                    <td class="badge-num">
                                        <asp:LinkButton ID="linkEdit" runat="server" CommandArgument='<%#Eval("Id") %>' ToolTip="edit" CommandName="Edit" CssClass="badge" />
                                    </td>
                                    <td class="active-check">
                                        <input type="checkbox" disabled="disabled" 
                                            name='activus_<%#Eval("Id") %>' id='activus_<%#Eval("Id") %>' 
                                            <%# (bool.Parse(DataBinder.Eval(Container.DataItem, "IsActive").ToString())) ? "checked" : string.Empty %> >
                                    </td>
                                    <td class="item-info">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td rowspan="2" style="width:330px;text-align:center;vertical-align:middle;"><asp:Literal ID="litInfo" runat="server" ></asp:Literal></td>
                                                <td><strong><%#Eval("VcPrincipal") %></strong></td>
                                                <td colspan="2"><strong><%#Eval("Name") %></strong></td>
                                            </tr>
                                            <tr>
                                                <td style="width:90px;"><strong>time</strong> <%#Eval("BannerTimeout").ToString() %></td>
                                                <td style="width:250px;"><strong>start</strong> <%#((DateTime)DataBinder.Eval(Container.DataItem, "DateStart") == DateTime.MinValue) ? string.Empty : 
                                                    ((DateTime)DataBinder.Eval(Container.DataItem, "DateStart")).ToString("MM/dd/yyyy hh:mmtt") %></td>
                                                <td style="width:250px;"><strong>end</strong> <%#((DateTime)DataBinder.Eval(Container.DataItem, "DateEnd") == DateTime.MaxValue) ? string.Empty : 
                                                    ((DateTime)DataBinder.Eval(Container.DataItem, "DateEnd")).ToString("MM/dd/yyyy hh:mmtt") %></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="item-delete">
                                        <asp:LinkButton ID="LinkButton2" runat="server" Text="X" CommandName="Delete" CssClass="btn btn-danger btn-xs" 
                                            OnClientClick="return confirm('Are you sure you want to delete this?');" >&nbsp;<span class="glyphicon glyphicon-trash"></span></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </li>
                    </ItemTemplate>
                    <InsertItemTemplate>
                        <div class="well-behind">
                            <div class="well well-outfront fade fade-slow in">

                                <cc1:ErrorDisplayLabel ID="ErrorDisplayLabel1" runat="server" />
   
                                <div class="form-group">
                                    <div class="input-group">
                                        <span class="input-group-addon alert-danger">
                                            Belongs To
                                        </span>
                                        <asp:CheckBoxList ID="chkPrincipal" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" CssClass="form-control checkbox-control" 
                                            OnDataBinding="chkPrincipal_DataBinding" />
                                    </div>
                                </div>     
                                <div class="form-group">
                                    <div class="input-group">
                                        <span class="input-group-addon alert-danger">
                                            Name
                                        </span>
                                        <asp:TextBox ID="txtName" runat="server" maxlength="256" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("Name") %>' />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="input-group">
                                        <span class="input-group-addon alert-danger">
                                            Display Text
                                        </span>
                                        <asp:TextBox ID="txtDisplayText" runat="server" maxlength="256" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("DisplayText") %>' />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="input-group">
                                        <span class="input-group-addon">
                                            Additional Text
                                        </span>
                                        <asp:TextBox ID="txtAdditionalText" runat="server" maxlength="256" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("AdditionalText") %>' />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <br />
                                    <asp:LinkButton ID="btnSave" runat="server" Text="Save" CommandName="Insert" CssClass="btn btn-primary btn-command" />
                                    <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CommandName="Cancel" CssClass="btn btn-primary btn-command" />
                                </div>
                            </div>
                        </div>
                    </InsertItemTemplate>
                </asp:ListView>
                </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<asp:ObjectDataSource ID="ObjBanners" runat="server" EnablePaging="true" 
    SelectMethod="GetSalePromotionsInContext" EnableCaching="false" 
    InsertMethod="IgnoreDataSourceInsertMethod" OnInserting="Cancel_Insert"
    TypeName="Wcss.SalePromotion" SelectCountMethod="GetSalePromotionsInContextCount"  
    OnSelecting="objData_Selecting" OnSelected="objData_Selected" >
</asp:ObjectDataSource>