<%@ Control Language="C#" AutoEventWireup="true"  CodeFile="ShowPromoters.ascx.cs" Inherits="wctMain.Admin._shows._controls.ShowPromoters" %>
<%@ Register Src="/Admin/_editors/_controls/Editor_Promoter.ascx" TagName="Editor_Promoter" TagPrefix="uc2" %>
<%@ Register assembly="WctControls" namespace="WctControls.WebControls" tagprefix="cc1" %>

<div id="showpromoters" class="show-mgmt">
    <asp:HiddenField ID="hdnCollectionTableName" runat="server" ClientIDMode="Static" Value="<%# Wcss.JShowPromoter.Schema.TableName %>" />
    <div class="panel-actions">
        <asp:LinkButton ID="btnUpdate" runat="server" CausesValidation="false" CommandName="Update" Text="Save" Enabled="false"
            OnClick="btnUpdate_Click" CssClass="btn btn-lg btn-primary btn-command" />
        <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn btn-lg btn-primary btn-command" Enabled="false"
            CommandName="Cancel" Text="Cancel" OnClick="btnCancel_Click" />                    
        <asp:LinkButton ID="btnChangeShowName" runat="server" Text="Sync Name" CssClass="btn btn-lg btn-primary btn-command" 
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
            DataSourceID="SqlShowPromoters"                     
            ItemPlaceholderID="ListViewContent" 
            OnDataBinding="listEnt_DataBinding" 
            OnItemDataBound="listEnt_ItemDataBound" 
            OnItemUpdating="listEnt_ItemUpdating" 
            OnItemEditing="listEnt_ItemEditing" 
            OnItemDeleting="listEnt_ItemDeleting"
            OnItemCanceling="listEnt_ItemCanceling"  
            OnItemCommand="listEnt_ItemCommand"
            >
            <EmptyDataTemplate>
                <div class="form-group" style="padding-left:10px;">no listings</div>
            </EmptyDataTemplate>
            <LayoutTemplate>  
                <div id="jshowpromoter-items" class="orderable-items">
                    <ul>
                    <asp:Panel ID="ListViewContent" runat="server" />
                    </ul>
                </div>
            </LayoutTemplate>
            <EditItemTemplate>
                <div class="well-behind">
                    <div class="well well-outfront well-outfront-med fade fade-slow in">
                            
                        <div class="form-group">
                            <h3>Edit Promoter</h3>
                        </div>
                        <cc1:ErrorDisplayLabel ID="ErrorDisplayLabel1" runat="server" />
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <uc2:Editor_Promoter ID="Editor_Promotera" runat="server" SelectedIdx='<%#Eval("tPromoterId") %>' HideSelectionPanel="true" />
                            </ContentTemplate>
                        </asp:UpdatePanel>                            
                        <div class="form-group">
                            <div class="form-group col-xs-6" style="padding-left:0;">
                                <div class="input-group">
                                    <span class="input-group-addon">
                                        Pre Text
                                    </span>
                                    <asp:TextBox ID="txtPre" MaxLength="500" runat="server" CssClass="form-control" Text='<%#Bind("PreText") %>' />  
                                </div>
                            </div>
                            <div class="form-group col-xs-6" style="padding-right:0;">
                                <div class="input-group">
                                    <span class="input-group-addon">
                                        Promoter Text
                                    </span>
                                    <asp:TextBox ID="txtPromo" MaxLength="300" runat="server" CssClass="form-control" Text='<%#Bind("PromoterText") %>' />
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    Post Text
                                </span>
                                <asp:TextBox ID="txtPost" MaxLength="2000" runat="server" CssClass="form-control" Text='<%#Bind("PostText") %>' />
                            </div>
                        </div>
                                
                        <div class="form-group">
                            <asp:LinkButton ID="btnUpdate" runat="server" CausesValidation="false" CommandName="Update" Text="Save" 
                                CssClass="btn  btn-primary" />
                            <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn  btn-primary" 
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
                                <asp:LinkButton ID="linkDelete" runat="server" Text="X" CommandName="Delete" CssClass="btn btn-danger btn-xs" 
                                    OnClientClick="return confirm('Are you sure you want to delete this?');" >&nbsp;<span class="glyphicon glyphicon-trash"></span></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </li>
            </ItemTemplate>
        </asp:ListView>
    </div>
    <div class="panel-footer">
        <asp:FormView ID="FormView1" runat="server" DataKeyNames="Id" Width="100%" 
            OnItemInserting="FormView1_ItemInserting" OnModeChanging="FormView1_ModeChanging" OnItemCommand="FormView1_ItemCommand" >
            <EmptyDataTemplate>
                <asp:LinkButton ID="btnNew" runat="server" CommandName="New" CssClass="btn btn-lg btn-success btn-command" 
                    Text="Add New Item" />
            </EmptyDataTemplate>                
            <InsertItemTemplate>
                <div class="well-behind">
                    <div class="well well-outfront fade fade-slow in">
                        <cc1:ErrorDisplayLabel ID="ErrorDisplayLabel1" runat="server" />
                        <div class="form-group">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <uc2:Editor_Promoter ID="Editor_Promoteri" runat="server" HideSelectionPanel="false" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="form-group">
                            <asp:LinkButton ID="btnAdd" runat="server" CommandName="Insert" CausesValidation="False" CssClass="btn btn-primary btn-command" 
                                Text="Add Selection" />
                            <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" CausesValidation="False" CssClass="btn btn-primary btn-command" 
                                Text="Cancel" />
                        </div>
                    </div>
                </div>
            </InsertItemTemplate>
        </asp:FormView>            
    </div>
</div>
<asp:SqlDataSource ID="SqlShowPromoters" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="SELECT j.[Id], j.[tPromoterId], a.[Name] as PromoterName, j.[PreText], j.[PromoterText], j.[PostText], j.[iDisplayOrder] as DisplayOrder
        FROM [JShowPromoter] j, [Promoter] a 
        WHERE j.[TShowId] = @ShowId AND j.[TPromoterId] = a.[Id] 
        ORDER BY j.[iDisplayOrder]" 
        DeleteCommand="SELECT 0 "
        InsertCommand="SELECT 0"
        UpdateCommand="SELECT 0"
        OnSelecting="SqlShowPromoters_Selecting" 
        >
    <SelectParameters>
        <asp:Parameter Name="ShowId" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>