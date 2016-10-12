<%@ Control Language="C#" AutoEventWireup="true"  CodeFile="ShowLinks.ascx.cs" Inherits="wctMain.Admin._shows._controls.ShowLinks" %>
<%@ Register assembly="WctControls" namespace="WctControls.WebControls" tagprefix="cc1" %>

<div id="showpromoters" class="show-mgmt">
    <asp:HiddenField ID="hdnCollectionTableName" runat="server" ClientIDMode="Static" Value="<%# Wcss.ShowLink.Schema.TableName %>" />
    <div class="panel-actions">
        <asp:LinkButton ID="btnUpdate" runat="server" CausesValidation="false" CommandName="Update" Text="Save" Enabled="false"
            OnClick="btnUpdate_Click" CssClass="btn btn-lg btn-primary btn-command" />
        <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn btn-lg btn-primary btn-command" Enabled="false"
            CommandName="Cancel" Text="Cancel" OnClick="btnCancel_Click" />                    
        <asp:LinkButton ID="btnChangeShowName" runat="server" Text="Sync Name" CssClass="btn btn-lg btn-primary btn-command" Enabled="false" 
            OnClick="btnChangeShowName_Click"
            OnClientClick="return confirm('This will update the show name to reflect the current information. Are you sure you want to continue?');" />
        <a data-toggle="modal" class="btn btn-lg btn-primary btn-command showcopy-modal-launcher disabled" href="#" >Copy Show</a>                              
        <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" CssClass="btn btn-lg btn-danger btn-command" 
            CommandName="Delete" Enabled="false" Text="Delete" OnClick="btnDelete_Click" />
        <a data-toggle="modal" class="btn btn-lg btn-success btn-command showdisplay-modal-launcher" href="#" >Preview Panel</a>
        <a target="_blank" class="btn btn-lg btn-success btn-command" 
            href='http://<%= Wcss._Config._DomainName%>/<%= Atx.CurrentShowRecord.FirstShowDate.ConfiguredUrl %>/?adm=t' >Preview Window</a>            
        <%if(Atx.IsSuperSession(this.Page.User)) {%>
            <small><%= Atx.CurrentShowRecord.Id.ToString()%> / <%= Atx.CurrentShowRecord.FirstShowDate.Id.ToString() %></small>
        <%} %>
    </div>

    <div class="panel-body">
        <asp:ListView ID="listEnt" ClientIDMode="Static" runat="server" EnableViewState="true" DataKeyNames="Id" 
            ItemPlaceholderID="ListViewContent" 
            OnDataBinding="listEnt_DataBinding" 
            OnItemDataBound="listEnt_ItemDataBound" 
            OnItemUpdating="listEnt_ItemUpdating" 
            OnItemInserting="listEnt_ItemInserting" 
            OnItemEditing="listEnt_ItemEditing" 
            OnItemDeleting="listEnt_ItemDeleting"
            OnItemCanceling="listEnt_ItemCanceling"  
            OnItemCommand="listEnt_ItemCommand"
            >
            <EmptyDataTemplate>
                <div class="form-group" style="padding-left:10px;">no listings</div>
            </EmptyDataTemplate>
            <LayoutTemplate>  
                <div id="showlink-items" class="orderable-items">
                    <ul>
                    <asp:Panel ID="ListViewContent" runat="server" />
                    </ul>
                </div>
            </LayoutTemplate>
            <EditItemTemplate>
                <div class="well-behind">
                    <div class="well well-outfront well-outfront-med fade fade-slow in">
                        <cc1:ErrorDisplayLabel ID="ErrorDisplayLabel1" runat="server" />
                        <div class="form-group">
                            <h3>Edit Show Link</h3>
                        </div>
                        <div class="form-group" style="padding-right:0;">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    Active
                                </span>
                                <asp:CheckBox ID="chkActive" runat="server" CssClass="form-control" Checked='<%#Bind("IsActive") %>' />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="input-group">
                                <ul class="list-group" style="margin-bottom:0;">
                                    <li class="list-group-item list-group-item-info"><span class="glyphicon glyphicon-info-sign glyphicon-primary"></span> Show links will show as the id of the linked show.</li>                                        
                                    <li class="list-group-item list-group-item-info"><span class="glyphicon glyphicon-info-sign glyphicon-primary"></span> Remote links will show the full url.</li>                                        
                                </ul>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    Link Url
                                </span>
                                <asp:TextBox ID="txtLinkUrl" MaxLength="300" runat="server" CssClass="form-control" Text='<%#Bind("LinkUrl") %>' />  
                            </div>
                        </div>
                        <div class="form-group" style="padding-right:0;">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    Display Text
                                </span>
                                <asp:TextBox ID="txtDisplayText" MaxLength="200" runat="server" CssClass="form-control" Text='<%#Bind("DisplayText") %>' />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:LinkButton ID="btnUpdate" runat="server" CausesValidation="false" CommandName="Update" Text="Save" 
                                CssClass="btn btn-primary btn-command" />
                            <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn btn-primary btn-command" 
                                CommandName="Cancel" Text="Cancel" />
                        </div>
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
                            <td class="item-info" style="vertical-align:middle;">
                                <asp:Literal ID="litInfo" runat="server" ></asp:Literal>
                            </td>
                            <td class="item-delete">
                                <asp:LinkButton ID="linkDelete" runat="server" Text="X" CommandName="Delete" CssClass="btn btn-danger btn-command" 
                                    OnClientClick="return confirm('Are you sure you want to delete this?');" >&nbsp;<span class="glyphicon glyphicon-trash"></span></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </li>
            </ItemTemplate>
            <InsertItemTemplate>
                <div class="well-behind">
                    <div class="well well-outfront well-outfront-med fade fade-slow in">
                        <cc1:ErrorDisplayLabel ID="ErrorDisplayLabel1" runat="server" />
                            
                        <div class="well">
                            <h3>Add a link to an existing show</h3>
                            <div class="form-group" style="padding-right:0;">
                                <div class="input-group">
                                    <span class="input-group-addon">
                                        Show Link
                                    </span>
                                    <asp:DropDownList ID="ddlShowLinks" runat="server" cssclass="form-control" OnDataBinding="ddlShowLinks_DataBinding" OnDataBound="ddlShowLinks_DataBound" />
                                </div>
                            </div>
                        </div>
                        <h4>- OR - </h4>
                        <div class="well">
                            <h3>Add a custom link</h3>
                            <div class="form-group">
                                <div class="input-group">
                                    <span class="input-group-addon">
                                        Link Url
                                    </span>
                                    <asp:TextBox ID="txtLinkUrl" MaxLength="300" runat="server" CssClass="form-control" Text='<%#Bind("LinkUrl") %>' />  
                                </div>
                            </div>
                            <div class="form-group" style="padding-right:0;">
                                <div class="input-group">
                                    <span class="input-group-addon">
                                        Display Text
                                    </span>
                                    <asp:TextBox ID="txtDisplayText" MaxLength="200" runat="server" CssClass="form-control" Text='<%#Bind("DisplayText") %>' />
                                </div>
                            </div>
                        </div>
                                                     
                        <div class="form-group">
                            <asp:LinkButton ID="btnSave" runat="server" Text="Add Selection" CommandName="Insert" CssClass="btn btn-primary btn-command" />
                            <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CommandName="Cancel" CssClass="btn btn-primary btn-command" />
                        </div>
                    </div>
                </div>
            </InsertItemTemplate>
        </asp:ListView>
    </div>
    <div class="panel-footer">
        <div class="form-group">
            <asp:LinkButton ID="btnNew" runat="server" CausesValidation="False" CssClass="btn btn-lg btn-success btn-command" 
                Text="Add New Item" OnClick="btnNew_Click" />
        </div>
    </div>
</div>