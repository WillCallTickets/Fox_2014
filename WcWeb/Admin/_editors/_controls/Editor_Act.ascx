<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Editor_Act.ascx.cs" Inherits="wctMain.Admin._editors._controls.Editor_Act" %>
<%@ Register assembly="WctControls" namespace="WctControls.WebControls" tagprefix="cc2" %>

<div id="acteditor" class="show-mgmt">
    <div class="panel-body">
        <asp:Panel ID="pnlSelect" runat="server">
            <div class="form-group form-group-34-helper" >
                <div class="input-group alpha-searcher">
                    <span class="input-group-addon" >
                        Act
                    </span>
                    <asp:TextBox ID="search_act" runat="server" ClientIDMode="Static" CssClass="form-control typeahead" AutoCompleteType="None" />
                    <span class="input-group-btn">
                        <asp:LinkButton ID="btnInlineNew" CssClass="btn btn-success btn-search-create" CausesValidation="false" runat="server" 
                            OnClick="btnInlineNew_Click" Text="Create new Act" />
                    </span>
                </div>
            </div>
        </asp:Panel>
        <cc2:ErrorDisplayLabel ID="ErrorDisplayLabel1" runat="server" />
        <asp:FormView ID="FormView1" runat="server" DataKeyNames="Id" DataSourceID="SqlDetails" Width="100%" DefaultMode="ReadOnly" 
        OnItemInserted="FormView1_ItemInserted" 
        OnModeChanged="FormView1_ModeChanged"
        OnItemInserting="FormView1_ItemInserting"   
        OnItemUpdating="FormView1_ItemUpdating" 
        OnItemUpdated="FormView1_ItemUpdated" 
        OnDataBound="FormView1_DataBound" 
        OnItemCommand="FormView1_ItemCommand"
        OnItemDeleting="FormView1_ItemDeleting">
        <EmptyDataTemplate>
        </EmptyDataTemplate>
        <EditItemTemplate>
            <div class="well-behind">
                <div class="well well-outfront fade fade-slow in">
                    <h3>Act Editor</h3>
                    <div class="form-group">  
                        <cc2:ErrorDisplayLabel ID="ErrorDisplayLabel1" runat="server" />
                    </div>
                    <div class="form-group">                 
                        <ul class="list-group" style="margin-bottom:0;">
                            <li class="list-group-item list-group-item-info"><span class="glyphicon glyphicon-info-sign"></span> For url and search purposes, name should only contain alpha-numeric characters [A-Z|a-z|0-9] and the special characters "$-_.+!*'()," per <a target="_blank" href="http://www.rfc-editor.org/rfc/rfc1738.txt">the rfc</a>.</li>
                            <li class="list-group-item list-group-item-info"><span class="glyphicon glyphicon-info-sign"></span> Use the Display Name field to include special characters other than what is listed here, umlat, etc.</li>
                        </ul>
                    </div>
                    <div class="form-group">
                        <div class="input-group">
                            <span class="input-group-addon">
                                (<%# Eval("Id") %>) Name
                            </span>
                            <asp:TextBox ID="NameTextBox" MaxLength="256" CssClass="form-control" runat="server" Text='<%# Bind("Name") %>' />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="input-group">
                            <span class="input-group-addon">
                                Display Name
                            </span>
                            <asp:TextBox ID="DisplayNameTextBox" MaxLength="256" CssClass="form-control" runat="server" Text='<%# Bind("DisplayName") %>' />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="input-group">
                            <span class="input-group-addon">
                                Website
                            </span>
                            <asp:TextBox ID="WebsiteTextBox" MaxLength="256" CssClass="form-control" runat="server" 
                                Text='<%# Bind("Website") %>' OnTextChanged="WebsiteTextBox_TextChanged" />
                            <span class="input-group-btn">
                                <asp:HyperLink ID="linkTestWebsite" runat="server" Target="_blank" CssClass="btn btn-primary btn-test" 
                                    NavigateUrl="" OnDataBinding="linkTestWebsite_DataBinding">test</asp:HyperLink>
                            </span>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" ValidationGroup="srceditor" 
                            CommandName="Update" Text="Update" CssClass="btn btn-primary btn-command" />
                        <asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" 
                            Text="Cancel" CssClass="btn btn-primary btn-command" />
                        <asp:LinkButton Id="DeleteButton" CssClass="btn btn-danger" runat="server" CommandName="Delete" 
                            Text="Delete" CommandArgument='<%#Eval("Id") %>' ToolTip="Delete" CausesValidation="false"
                            OnClientClick='return confirm("Are you sure you want to delete this Act?")' >&nbsp;<span class="glyphicon glyphicon-trash"></span></asp:LinkButton>
                    </div>
                </div>
            </div>
        </EditItemTemplate>
        <InsertItemTemplate>
            <div class="well-behind">
                <div class="well well-outfront fade fade-slow in">
                    <h3>Create New Act</h3>
                    <cc2:ErrorDisplayLabel ID="ErrorDisplayLabel1" runat="server" />
                    <div class="form-group">                 
                        <ul class="list-group" style="margin-bottom:0;">
                            <li class="list-group-item list-group-item-info"><span class="glyphicon glyphicon-info-sign"></span> For url and search purposes, name should only contain alpha-numeric characters [A-Z|a-z|0-9] and the special characters "$-_.+!*'()," per <a target="_blank" href="http://www.rfc-editor.org/rfc/rfc1738.txt">the rfc</a>.</li>
                            <li class="list-group-item list-group-item-info"><span class="glyphicon glyphicon-info-sign"></span> Use the Display Name field <code>(will show in edit step)</code> to include special characters other than what is listed here, umlat, etc.</li>                            
                        </ul>
                    </div>
                    <div class="form-group">
                        <div class="input-group">
                            <span class="input-group-addon">
                                Name
                            </span>
                            <asp:TextBox ID="NameTextBox" MaxLength="256" CssClass="form-control" runat="server" Text='<%# Bind("Name") %>' />                            
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:LinkButton ID="InsertButton" runat="server" ValidationGroup="srceditor" CausesValidation="True" 
                            CommandName="Insert" Text="Create a new act from Name" CssClass="btn btn-success btn-command" />
                        <asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" 
                            Text="Cancel" CssClass="btn btn-primary btn-command" />
                    </div>
                </div>
            </div>
        </InsertItemTemplate>        
        <ItemTemplate>
            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-addon">
                        (<%# Eval("Id") %>) Name
                    </span>
                    <asp:TextBox ID="NameTextBox" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("Name") %>' />
                    <span class="input-group-btn">
                        <asp:LinkButton Id="EditButton" ToolTip="Edit" CssClass="btn btn-primary btn-edit" runat="server" CommandName="Edit" 
                            Text="Edit" CommandArgument='<%#Eval("Id") %>' CausesValidation="false"  />                    
                    </span>
                </div>
            </div>
            
            <%if ((!_NameMatchesDisplayName()) || (!HideDisplayWebsiteDelete))
              {%>
            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-addon">
                        Display Name
                    </span>
                    <asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("DisplayName") %>' />
                </div>
            </div>
            <%} %>
            <%if(!HideDisplayWebsiteDelete) {%>
            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-addon">
                        Website
                    </span>
                    <asp:TextBox ID="txtWebsite" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("Website") %>' />
                    <span class="input-group-btn">
                        <asp:HyperLink ID="btnWebsiteUrl" runat="server" Target="_blank" CssClass="btn btn-primary btn-test" Text="Test" />
                    </span>
                </div>
            </div>
            <%} %>
        </ItemTemplate>
    </asp:FormView>

        <asp:Panel ID="PanelHidden" runat="server" Height="0px">
            <input type="hidden" id="hidSelectedValue" name="hidSelectedValue" runat="server" />
            <asp:LinkButton ID="btnLoad" runat="server" Text="Load/Create" CssClass="btnhid" CausesValidation="false" OnClick="btnLoad_Click" />
        </asp:Panel>
    </div>
</div>
<asp:SqlDataSource ID="SqlDetails" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" EnableCaching="false"
    DeleteCommand="SELECT 0" 
    InsertCommand="INSERT INTO [Act] ([ApplicationId], [Name]) VALUES (@appId, @Name); SELECT @NewId = @@IDENTITY; "
    SelectCommand="SELECT [Id], [Name], [DisplayName], [Website] FROM [Act] WHERE ([ApplicationId] = @appId AND [Id] = @Id); "
    UpdateCommand="UPDATE [Act] SET [Name] = @Name, [DisplayName] = @DisplayName, [Website] = @Website WHERE [Id] = @Id "
    OnInserted="SqlDetails_Inserted" OnInserting="SqlDetails_Inserting" OnSelecting="SqlDetails_Selecting"  >
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Name" Type="String" />
        <asp:Parameter Name="DisplayName" Type="String" />
        <asp:Parameter Name="Website" Type="String" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:CookieParameter ConvertEmptyStringToNull="false" CookieName="acid" DefaultValue="0" Name="Id" Type="int32" />
    </SelectParameters>
    <InsertParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:Parameter Name="NewId" Direction="output" DefaultValue="567" Type="Int32" />
        <asp:Parameter Name="Name" Type="String" />
    </InsertParameters>
</asp:SqlDataSource>