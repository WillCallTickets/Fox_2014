<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Editor_Genre.ascx.cs" Inherits="wctMain.Admin._editors._controls.Editor_Genre" %>
<%@ Register assembly="WctControls" namespace="WctControls.WebControls" tagprefix="cc1" %>

<div id="genreseditor" class="show-mgmt">
    <div class="panel-header-supplement">
        <asp:LinkButton Id="btnNew" CssClass="btn btn-default btn-add-new" CausesValidation="false" runat="server" OnClick="linkNew_Click" Text="new" />
    </div>
    <div class="panel-body">
        <asp:ListView ID="listEnt" ClientIDMode="Static" runat="server" EnableViewState="true" DataKeyNames="Id" 
            onDataBinding="listEnt_DataBinding" 
            OnItemDataBound="listEnt_ItemDataBound"
            OnItemEditing="listEnt_ItemEditing" 
            OnItemInserting="listEnt_ItemInserting" 
            OnItemDeleting="listEnt_ItemDeleting"
            OnItemCanceling="listEnt_ItemCanceling" 
            OnItemUpdating="listEnt_ItemUpdating" 
            OnItemCommand="listEnt_ItemCommand"
            ItemPlaceholderID="ListViewContent" 
            >
            <EmptyDataTemplate>
                no genres available
            </EmptyDataTemplate>
            <LayoutTemplate>  
                <div id="genre-items" class="orderable-items">
                    <ul>
                    <asp:Panel ID="ListViewContent" runat="server" />
                    </ul>
                </div>
            </LayoutTemplate>
            <EditItemTemplate>
                <div class="well-behind">
                    <div class="well well-outfront fade fade-slow in">
                        <cc1:ErrorDisplayLabel ID="ErrorDisplayLabel1" runat="server" />
   
                        <div class="form-group">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    Name
                                </span>
                                <asp:TextBox ID="txtName" runat="server" maxlength="256" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("Name") %>' />
                            </div>
                        </div>     
                        <div class="form-group">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    Description
                                </span>
                                <asp:TextBox ID="txtDescription" runat="server" maxlength="256" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("Description") %>' />
                            </div>
                        </div>     
                        <div class="form-group">
                            <br />
                            <asp:LinkButton ID="btnSave" runat="server" Text="Save" CommandName="Update" CssClass="btn btn-primary btn-command" />
                            <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CommandName="Cancel" CssClass="btn btn-primary btn-command" />
                        </div>
                    </div>
                </div>
            </EditItemTemplate>
            <InsertItemTemplate>
                <div class="well-behind">
                    <div class="well well-outfront fade fade-slow in">

                        <cc1:ErrorDisplayLabel ID="ErrorDisplayLabel1" runat="server" />
   
                        <div class="form-group">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    Name
                                </span>
                                <asp:TextBox ID="txtName" runat="server" maxlength="256" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("Name") %>' />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    Description
                                </span>
                                <asp:TextBox ID="txtDescription" runat="server" maxlength="256" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("Description") %>' />
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
            <ItemTemplate>
                <li id='ent_<%#Eval("Id") %>' class="vdquery-row vdquery-item form-inline vdquery-row-select">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0" class="director-list">
                        <tr>
                            <td class="badge-num">
                                <asp:LinkButton ID="linkEdit" runat="server" CommandArgument='<%#Eval("Id") %>' ToolTip="edit" CommandName="Edit" CssClass="badge" />
                            </td>
                            <td class="item-info" style="vertical-align:middle;">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td style="width:50%;"><%#Eval("Name") %></td>
                                        <td style="width:50%;"><%#Eval("Description") %></td>
                                    </tr>
                                </table>
                            </td>
                            <td class="item-delete">
                                <asp:LinkButton ID="LinkButton2" runat="server" Text="X" CommandName="Delete" CssClass="btn btn-danger btn-xs" 
                                    OnClientClick="return confirm('Are you sure you want to delete this?');" >&nbsp;<span class="glyphicon glyphicon-trash"></span></asp:LinkButton>
                            </td>
                    </table>
                </li>
            </ItemTemplate>
        </asp:ListView>
    </div>
</div>