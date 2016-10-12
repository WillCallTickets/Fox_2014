<%@ Control Language="C#" AutoEventWireup="true"  CodeFile="ShowDate_Acts.ascx.cs" Inherits="wctMain.Admin._shows._controls.ShowDate_Acts" %>
<%@ Register Src="/Admin/_editors/_controls/Editor_Act.ascx" TagName="Editor_Act" TagPrefix="uc2" %>
<%@ Register assembly="WctControls" namespace="WctControls.WebControls" tagprefix="cc1" %>

<div id="showacts" class="show-mgmt">    
    <asp:HiddenField ID="hdnCollectionTableName" runat="server" ClientIDMode="Static" Value="<%# Wcss.JShowAct.Schema.TableName %>" />    
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
        <div class="form-group">
            <div class="form-group col-xs-4" style="padding-left:0;">
                <div class="input-group">
                    <span class="input-group-btn input-group-addon-btn dropup">
                        <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
                            <span class="glyphicon glyphicon-info-sign"></span> Display</button>
                        <ul class="dropdown-menu" role="menu">
                            <li>This setting will toggle how the acts are displayed.</li>
                            <li>Auto inserts "with" and ampersands and is recommended.</li>
                            <li>Legacy allows you to control text displayed by pre, featuring and post text.</li>
                        </ul>
	                </span>
                    <asp:RadioButtonList ID="rdoBilling" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" 
                        RepeatLayout="Table" CssClass="form-control radio-horiz"
                        OnDataBound="rdoBilling_DataBound" 
                        OnSelectedIndexChanged="rdoBilling_SelectedIndexChanged" >
                        <asp:ListItem Text=" Auto" Value="Auto" />
                        <asp:ListItem Text=" Legacy" Value="Legacy" />
                    </asp:RadioButtonList>
                </div>
            </div>
            <div class="form-group col-xs-8">
                &nbsp;
            </div>
        </div>
        <asp:ListView ID="listEnt" ClientIDMode="Static" runat="server" EnableViewState="true" DataKeyNames="Id" 
            DataSourceID="SqlShowActs"                     
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
                <div id="jshowact-items" class="orderable-items">
                    <ul>
                    <asp:Panel ID="ListViewContent" runat="server" />
                    </ul>
                </div>
            </LayoutTemplate>
            <EditItemTemplate>
                <div class="well-behind">
                    <div class="well well-outfront well-outfront-med fade fade-slow in">
                            
                        <div class="form-group">
                            <h3>Edit Act</h3>
                        </div>
                        <cc1:ErrorDisplayLabel ID="ErrorDisplayLabel1" runat="server" />
                        <div class="form-group">
                            <div class="form-group col-xs-6" style="padding-left:0;">
                                <div class="input-group">
                                    <span class="input-group-addon">
                                        Is Headliner
                                    </span>
                                    <asp:CheckBox ID="chkHeadline" runat="server" CssClass="form-control" Checked='<%#Bind("TopBilling") %>' />
                                </div>
                            </div>
                        </div>
                        <div class="clearfix"></div>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <uc2:Editor_Act ID="Editor_Acta" runat="server" SelectedIdx='<%#Eval("tActId") %>' HideSelectionPanel="true" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <%if(!Atx.CurrentShowRecord.FirstShowDate.IsAutoBilling){ %>
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
                                        Act Text
                                    </span>
                                    <asp:TextBox ID="txtAct" MaxLength="300" runat="server" CssClass="form-control" Text='<%#Bind("ActText") %>' />
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    Featuring
                                </span>
                                <asp:TextBox ID="txtFeaturing" MaxLength="1000" runat="server" CssClass="form-control" Text='<%#Bind("Featuring") %>' />
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
                        <%} %>
                                
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
                            <!-- convert to headliner check -->
                            <td class="active-check">
                                <input type="checkbox" disabled="disabled" 
                                    name='activus_<%#Eval("Id") %>' id='activus_<%#Eval("Id") %>' 
                                    <%# (Convert.ToBoolean(Eval("TopBilling_Effective"))) ? "checked" : string.Empty %> > 
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
                                    <uc2:Editor_Act ID="Editor_Acti" runat="server" HideSelectionPanel="false" />
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
<asp:SqlDataSource ID="SqlShowActs" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="SELECT j.[Id], j.[tActId], a.[Name] as ActName, j.[PreText], j.[ActText], j.[Featuring], j.[PostText], j.[iDisplayOrder] as DisplayOrder, 
        j.[bTopBilling] as TopBilling, CASE WHEN (j.[iDisplayOrder] = 0 OR j.[bTopBilling] = 1) THEN 1 ELSE 0 END as [TopBilling_Effective]
        FROM [JShowAct] j, [Act] a 
        WHERE j.[TShowDateId] = @ShowDateId AND j.[TActId] = a.[Id] 
        ORDER BY j.[iDisplayOrder]" 
        DeleteCommand="SELECT 0 "
        InsertCommand="SELECT 0"
        UpdateCommand="SELECT 0"
        OnSelecting="SqlShowActs_Selecting" 
        >
    <SelectParameters>
        <asp:Parameter Name="ShowDateId" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>