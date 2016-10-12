<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Kiosk_Edit.ascx.cs" Inherits="wctMain.Admin._collectionEditors._kiosk._controls.Kiosk_Edit" %>
<%@ Register src="~/Admin/_collectionEditors/_kiosk/_controls/EditKiosk_Container.ascx" tagname="EditKiosk_Container" tagprefix="uc1" %>
<%@ Register Src="~/Admin/_customControls/CollectionContextPager.ascx" TagPrefix="uc3" TagName="CollectionContextPager" %>
<%@ Register assembly="WctControls" namespace="WctControls.WebControls" tagprefix="cc1" %>

<div id="kioskeditor" class="vd-data-entry show-mgmt">
    <asp:HiddenField ID="hdnCollectionTableName" runat="server" ClientIDMode="Static" Value="<%# Wcss.Kiosk.Schema.TableName %>" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="showmgr" >
                <div class="panel panel-primary">
                    <uc3:CollectionContextPager ID="CollectionContextPager1" runat="server" PagerTitle="Kiosk Editor" >
                        <Template>
                            <asp:LinkButton ID="linkNew" ClientIDMode="Static" runat="server" tooltip="Add a new Kiosk"
                                OnClick="linkNew_Click" EnableViewState="true" CommandName="New" CssClass="btn btn-default btn-add-new" >New</asp:LinkButton>
                        </Template>
                    </uc3:CollectionContextPager>
                    <div class="panel-body">
                       <asp:ListView ID="listEnt" runat="server" EnableViewState="true" DataKeyNames="Id"  
                        DataSourceId="ObjKiosks"
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
                            no kiosks available
                        </EmptyDataTemplate>
                        <LayoutTemplate>       
                            <div id="kiosk-items" class="orderable-items">                                                     
                                <ul>
                                <asp:Panel ID="ListViewContent" runat="server" />
                                </ul>
                            </div>
                        </LayoutTemplate>
                        <EditItemTemplate>
                            <div class="well-behind">
                                <div class="well well-outfront fade fade-slow in">                                    
                                    <uc1:EditKiosk_Container ID="EditKiosk_Container1" runat="server" />
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
                                                    <td class="ent-name"><strong><%#Eval("Name") %></strong></td>
                                                    <td class="ent-principal"><strong><%#Eval("VcPrincipal") %></strong></td>
                                                    <td class="ent-timeout"><strong>time</strong> <%#Eval("Timeout").ToString() %></td>
                                                    <td class="ent-date"><strong>start</strong> <%#((DateTime)DataBinder.Eval(Container.DataItem, "DateStart") == DateTime.MinValue) ? string.Empty : 
                                                        ((DateTime)DataBinder.Eval(Container.DataItem, "DateStart")).ToString("MM/dd/yyyy hh:mmtt") %></td>
                                                    <td class="ent-date"><strong>end</strong> <%#((DateTime)DataBinder.Eval(Container.DataItem, "DateEnd") == DateTime.MaxValue) ? string.Empty : 
                                                        ((DateTime)DataBinder.Eval(Container.DataItem, "DateEnd")).ToString("MM/dd/yyyy hh:mmtt") %></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" class="ent-info">
                                                        <asp:Literal ID="litInfo" runat="server" ></asp:Literal>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td class="item-delete">
                                            <asp:LinkButton ID="linkDelete" runat="server" Text="X" CommandName="Delete" CssClass="btn btn-danger btn-xs" 
                                                OnClientClick="return confirm('Are you sure you want to delete this?');" />
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
                                            <asp:TextBox ID="txtName" runat="server" maxlength="256" ClientIDMode="Static" ReadOnly="false" CssClass="form-control form-control-name" Text='<%#Bind("Name") %>' />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="input-group">
                                            <span class="input-group-addon">
                                                <a href="javascript: alert('This will populate date, title, headliners, openers and the image from the selected show.')"><i class="glyphicon glyphicon-info-sign"></i></a>
                                                    Show
                                            </span>
                                            <asp:DropDownList ID="ddlPopulate" runat="server" DataSourceID="SqlShowList" 
                                                DataTextField="ShowName" DataValueField="ShowId" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="input-group">                    
                                            <ul class="list-group" style="margin-bottom:0;">
                                                <li class="list-group-item list-group-item-info"><span class="glyphicon glyphicon-info-sign"></span> Selecting a show will attempt to import images and values from the selected show.</li>
                                                <li class="list-group-item list-group-item-info"><span class="glyphicon glyphicon-info-sign"></span> Selecting a show is not required. You can edit kiosk information after creation.</li>
                                                
                                            </ul>
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
<asp:ObjectDataSource ID="ObjKiosks" runat="server" EnablePaging="true" 
    SelectMethod="GetKiosksInContext" EnableCaching="false" 
    InsertMethod="IgnoreDataSourceInsertMethod" OnInserting="Cancel_Insert" 
    TypeName="Wcss.Kiosk" SelectCountMethod="GetKiosksInContextCount"  
    OnSelecting="objData_Selecting" OnSelected="objData_Selected" >
</asp:ObjectDataSource>
<asp:SqlDataSource ID="SqlShowList" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="SELECT 0"
    onselecting="SqlShowList_Selecting">
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:Parameter Name="startDate" Type="DateTime" />
    </SelectParameters>    
</asp:SqlDataSource>