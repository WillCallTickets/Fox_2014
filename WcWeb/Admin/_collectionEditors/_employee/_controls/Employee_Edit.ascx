<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Employee_Edit.ascx.cs" Inherits="wctMain.Admin._collectionEditors._employee._controls.Employee_Edit" %>
<%@ Register src="~/Admin/_collectionEditors/_employee/_controls/EditEmployee_Container.ascx" tagname="EditEmployee_Container" tagprefix="uc1" %>
<%@ Register Src="~/Admin/_customControls/CollectionContextPager.ascx" TagPrefix="uc3" TagName="CollectionContextPager" %>
<%@ Register assembly="WctControls" namespace="WctControls.WebControls" tagprefix="cc1" %>

<div id="employeeeditor" class="vd-data-entry show-mgmt">
    <asp:HiddenField ID="hdnCollectionTableName" runat="server" ClientIDMode="Static" Value="<%# Wcss.Employee.Schema.TableName %>" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="showmgr" >
                <div class="panel panel-primary">
                    <uc3:CollectionContextPager ID="CollectionContextPager1" runat="server" PagerTitle="Employee Editor" >
                        <Template>
                            <asp:LinkButton ID="linkNew" ClientIDMode="Static" runat="server" tooltip="Add a new Employee"
                                OnClick="linkNew_Click" EnableViewState="true" CommandName="New" CssClass="btn btn-default btn-add-new" >New</asp:LinkButton>
                        </Template>
                    </uc3:CollectionContextPager>                    
                    <div class="panel-body">
                         <asp:ListView ID="listEnt" ClientIDMode="Static" runat="server" EnableViewState="true" DataKeyNames="Id" 
                            DataSourceID="ObjEmployees"
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
                                no employees available
                            </EmptyDataTemplate>
                            <LayoutTemplate>                
                                <div id="employee-items" class="orderable-items">
                                    <ul>
                                    <asp:Panel ID="ListViewContent" runat="server" />
                                    </ul>
                                </div>
                            </LayoutTemplate>
                            <EditItemTemplate>
                                <div class="well-behind">
                                    <div class="well well-outfront fade fade-slow in">
                                        <uc1:EditEmployee_Container ID="EditEmployee_Container1" runat="server" />
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
                                            <td>
                                                <asp:Literal ID="litInfo" runat="server" ></asp:Literal>
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
                                                <ul class="list-group" style="margin-bottom:0;">
                                                    <li class="list-group-item list-group-item-info"><span class="glyphicon glyphicon-info-sign"></span> All fields required.</li>
                                                    <li class="list-group-item list-group-item-info"><span class="glyphicon glyphicon-info-sign"></span> Z2 employees do not display Names - enter placeholder data if names not applicable.</li>
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon alert-danger">
                                                    Belongs To
                                                </span>
                                                <asp:CheckBoxList ID="chkPrincipal" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" 
                                                    CssClass="form-control checkbox-control" OnDataBinding="chkPrincipal_DataBinding" />
                                            </div>
                                        </div>     
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon alert-danger">
                                                    First Name
                                                </span>
                                                <asp:TextBox ID="txtFirstName" runat="server" maxlength="256" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("FirstName") %>' />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon alert-danger">
                                                    Last Name
                                                </span>
                                                <asp:TextBox ID="txtLastName" runat="server" maxlength="256" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("LastName") %>' />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon alert-danger">
                                                    Title
                                                </span>
                                                <asp:TextBox ID="txtTitle" runat="server" maxlength="256" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("Title") %>' />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon alert-danger">
                                                    Email Address
                                                </span>
                                                <asp:TextBox ID="txtEmail" runat="server" maxlength="256" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("EmailAddress") %>' />
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
<asp:ObjectDataSource ID="ObjEmployees" runat="server" EnablePaging="true"  
    SelectMethod="GetEmployeesInContext" EnableCaching="false" 
    InsertMethod="IgnoreDataSourceInsertMethod" OnInserting="Cancel_Insert" 
    TypeName="Wcss.Employee" SelectCountMethod="GetEmployeesInContextCount"  
    OnSelecting="objData_Selecting" OnSelected="objData_Selected" >
</asp:ObjectDataSource>