<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Editor_Venue.ascx.cs" Inherits="wctMain.Admin._editors._controls.Editor_Venue" %>
<%@ Register assembly="WctControls" namespace="WctControls.WebControls" tagprefix="cc2" %>

<div id="venueeditor" class="show-mgmt">
    <div class="panel-body">
        <cc2:ErrorDisplayLabel ID="ErrorDisplayLabel1" runat="server" />
        <asp:Panel ID="pnlSelect" runat="server">
            <div class="form-group form-group-34-helper" >
                <div class="input-group alpha-searcher">
                    <span class="input-group-addon" >
                        Venue
                    </span>
                    <asp:TextBox ID="search_venue" runat="server" ClientIDMode="Static" CssClass="form-control typeahead" AutoCompleteType="None" />
                    <span class="input-group-btn">
                        <asp:LinkButton ID="btnInlineNew" CssClass="btn btn-success" CausesValidation="false" runat="server" 
                            OnClick="btnInlineNew_Click" Text="Create new Venue" />
                    </span>
                </div>
            </div>
        </asp:Panel>
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
                        <h3>Venue Editor</h3>
                        <cc2:ErrorDisplayLabel ID="ErrorDisplayLabel1" runat="server" />
                        <div class="form-group">                 
                            <ul class="list-group" style="margin-bottom:0;">
                                <li class="list-group-item list-group-item-info"><span class="glyphicon glyphicon-info-sign"></span> For url and search purposes, name should only contain alpha-numeric characters [A-Z|a-z|0-9] and the special characters "$-_.+!*'()," per <a target="_blank" href="http://www.rfc-editor.org/rfc/rfc1738.txt">the rfc</a>.</li>
                                <li class="list-group-item list-group-item-info"><span class="glyphicon glyphicon-info-sign"></span> Use the Display Name field to include special characters other than what is listed here, umlat, etc.</li>
                            </ul>
                        </div>
                        <div class="form-group">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    Belongs To
                                </span>
                                <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("vcPrincipal") %>' />
                            </div>
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
                                    Text='<%# Bind("WebsiteUrl") %>' OnTextChanged="WebsiteTextBox_TextChanged" />
                                <span class="input-group-btn">
                                    <asp:HyperLink ID="linkTestWebsite" runat="server" Target="_blank" CssClass="btn btn-primary btn-test" 
                                        NavigateUrl="" OnDataBinding="linkTestWebsite_DataBinding">test</asp:HyperLink>
                                </span>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-group col-xs-6" style="padding-left:0;">
                                <div class="input-group">
                                    <span class="input-group-addon">
                                        Latitude
                                    </span>
                                    <asp:TextBox ID="txtLatitude" runat="server" ClientIDMode="Static" CssClass="form-control" MaxLength="50" Text='<%#Bind("Latitude") %>' />
                                </div>
                            </div>
                            <div class="form-group col-xs-6" style="padding-right:0;">
                                <div class="input-group">
                                    <span class="input-group-addon">
                                        Longitude
                                    </span>
                                    <asp:TextBox ID="txtLongitude" runat="server" ClientIDMode="Static" CssClass="form-control" MaxLength="50" Text='<%#Bind("Longitude") %>' />
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-group col-xs-6" style="padding-left:0;">
                                <div class="input-group">
                                    <span class="input-group-addon">
                                        Capacity
                                    </span>
                                    <asp:TextBox ID="txtCapacity" runat="server" ClientIDMode="Static" CssClass="form-control" MaxLength="100" Text='<%#Bind("iCapacity") %>' />
                                </div>
                            </div>
                            <div class="form-group col-xs-6">
                                &nbsp;
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="input-group">
                                <span class="input-group-btn input-group-addon-btn dropup">
                                    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
                                        <span class="glyphicon glyphicon-info-sign"></span> Full Address</button>
                                    <ul class="dropdown-menu" role="menu">
                                        <li>This should be street, city, state and zip, all on one line.</li>
                                    </ul>
	                            </span>
                                <asp:TextBox ID="txtShortAddress" runat="server" ClientIDMode="Static" CssClass="form-control" MaxLength="500" Text='<%#Bind("ShortAddress") %>' />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    Street Address
                                </span>
                                <asp:TextBox ID="txtStreetAddress" runat="server" ClientIDMode="Static" CssClass="form-control" MaxLength="150" Text='<%#Bind("Address") %>' />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-group col-xs-6" style="padding-left:0;">
                                <div class="input-group">
                                    <span class="input-group-addon">
                                        City
                                    </span>
                                    <asp:TextBox ID="txtCity" runat="server" ClientIDMode="Static" CssClass="form-control" MaxLength="100" Text='<%#Bind("City") %>' />
                                </div>
                            </div>
                            <div class="form-group col-xs-3" style="">
                                <div class="input-group">
                                    <span class="input-group-addon">
                                        State
                                    </span>
                                    <asp:TextBox ID="txtState" runat="server" ClientIDMode="Static" CssClass="form-control" MaxLength="50" Text='<%#Bind("State") %>' />
                                </div>
                            </div>
                            <div class="form-group col-xs-3" style="padding-right:0;">
                                <div class="input-group">
                                    <span class="input-group-addon">
                                        Zip
                                    </span>
                                    <asp:TextBox ID="txtZipCode" runat="server" ClientIDMode="Static" CssClass="form-control" MaxLength="10" Text='<%#Bind("ZipCode") %>' />
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-group col-xs-6" style="padding-left:0;">
                                <div class="input-group">
                                    <span class="input-group-addon">
                                        Box Office Phone
                                    </span>
                                    <asp:TextBox ID="txtBoPhone" runat="server" ClientIDMode="Static" CssClass="form-control" MaxLength="100" Text='<%#Bind("BoxOfficePhone") %>' />
                                </div>
                            </div>
                            <div class="form-group col-xs-6" style="">
                                <div class="input-group">
                                    <span class="input-group-addon">
                                        Box Office Ext
                                    </span>
                                    <asp:TextBox ID="txtBoExt" runat="server" ClientIDMode="Static" CssClass="form-control" MaxLength="100" Text='<%#Bind("BoxOfficePhoneExt") %>' />
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    Box Office Notes
                                </span>
                                <asp:TextBox ID="txtBoNotes" runat="server" ClientIDMode="Static" CssClass="form-control" MaxLength="500" Text='<%#Bind("BoxOfficeNotes") %>' />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-group col-xs-6" style="padding-left:0;">
                                <div class="input-group">
                                    <span class="input-group-addon">
                                        Main Office Phone
                                    </span>
                                    <asp:TextBox ID="txtMainPhone" runat="server" ClientIDMode="Static" CssClass="form-control" MaxLength="100" Text='<%#Bind("MainPhone") %>' />
                                </div>
                            </div>
                            <div class="form-group col-xs-6" style="">
                                <div class="input-group">
                                    <span class="input-group-addon">
                                        Main Office Ext
                                    </span>
                                    <asp:TextBox ID="txtMainPhoneExt" runat="server" ClientIDMode="Static" CssClass="form-control" MaxLength="100" Text='<%#Bind("MainPhoneExt") %>' />
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    Notes (fax)
                                </span>
                                <asp:TextBox ID="txtNotes" runat="server" ClientIDMode="Static" CssClass="form-control" MaxLength="500" Text='<%#Bind("Notes") %>' />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" ValidationGroup="srceditor" 
                                CommandName="Update" Text="Update" CssClass="btn btn-primary btn-command" />
                            <asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" 
                                Text="Cancel" CssClass="btn btn-primary btn-command" />
                            <asp:LinkButton Id="DeleteButton" CssClass="btn btn-danger" runat="server" CommandName="Delete" 
                                Text="Delete" CommandArgument='<%#Eval("Id") %>' ToolTip="Delete" CausesValidation="false"
                                OnClientClick='return confirm("Are you sure you want to delete this Venue?")' >&nbsp;<span class="glyphicon glyphicon-trash"></span></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </EditItemTemplate>
            <InsertItemTemplate>
                <div class="well-behind">
                    <div class="well well-outfront fade fade-slow in">
                        <h3>Create New Venue</h3>
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
                                    Belongs To
                                </span>
                                <asp:RadioButtonList ID="rdoPrincipal" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal" RepeatLayout="Table" 
                                    CssClass="form-control radio-horiz" 
                                    OnDataBinding="rdoPrincipal_DataBinding" 
                                    OnDataBound="listControl_DataBound" />
                            </div>
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
                                CommandName="Insert" Text="Create a new venue from Name" CssClass="btn btn-success btn-command" />
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
                            Belongs To
                        </span>
                        <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("vcPrincipal") %>' />
                    </div>
                </div>
                <div class="form-group">
                    <div class="input-group">
                        <span class="input-group-addon">
                            (<%# Eval("Id") %>) Name
                        </span>
                        <asp:TextBox ID="NameTextBox" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("Name") %>' />
                        <span class="input-group-btn">
                            <asp:LinkButton Id="EditButton" ToolTip="Edit" CssClass="btn btn-primary" runat="server" CommandName="Edit" 
                                Text="Edit" CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />                    
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
                        <asp:TextBox ID="txtWebsite" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("WebsiteUrl") %>' />
                        <span class="input-group-btn">
                            <asp:HyperLink ID="btnWebsiteUrl" runat="server" Target="_blank" CssClass="btn btn-primary btn-test" Text="Test" />
                        </span>
                    </div>
                </div>
                <div class="form-group">
                    <div class="form-group col-xs-6" style="padding-left:0;">
                        <div class="input-group">
                            <span class="input-group-addon">
                                Latitude
                            </span>
                            <asp:TextBox ID="txtLatitude" runat="server" ClientIDMode="Static" CssClass="form-control" ReadOnly="true" Text='<%#Eval("Latitude") %>' />
                        </div>
                    </div>
                    <div class="form-group col-xs-6" style="padding-right:0;">
                        <div class="input-group">
                            <span class="input-group-addon">
                                Longitude
                            </span>
                            <asp:TextBox ID="txtLongitude" runat="server" ClientIDMode="Static" CssClass="form-control" ReadOnly="true" Text='<%#Eval("Longitude") %>' />
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="form-group col-xs-6" style="padding-left:0;">
                        <div class="input-group">
                            <span class="input-group-addon">
                                Capacity
                            </span>
                            <asp:TextBox ID="txtCapacity" runat="server" ClientIDMode="Static" CssClass="form-control" ReadOnly="true" Text='<%#Eval("iCapacity") %>' />
                        </div>
                    </div>
                    <div class="form-group col-xs-6">
                        &nbsp;
                    </div>
                </div>
                <div class="form-group">
                    <div class="input-group">
                        <span class="input-group-btn input-group-addon-btn dropup">
                            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
                                <span class="glyphicon glyphicon-info-sign"></span> Full Address</button>
                            <ul class="dropdown-menu" role="menu">
                                <li>This should be street, city, state and zip, all on one line.</li>
                            </ul>
	                    </span>
                        <asp:TextBox ID="txtShortAddress" runat="server" ClientIDMode="Static" CssClass="form-control" ReadOnly="true" Text='<%#Eval("ShortAddress") %>' />
                    </div>
                </div>
                <div class="form-group">
                    <div class="input-group">
                        <span class="input-group-addon">
                            Street Address
                        </span>
                        <asp:TextBox ID="txtStreetAddress" runat="server" ClientIDMode="Static" CssClass="form-control" ReadOnly="true" Text='<%#Eval("Address") %>' />
                    </div>
                </div>
                <div class="form-group">
                    <div class="form-group col-xs-6" style="padding-left:0;">
                        <div class="input-group">
                            <span class="input-group-addon">
                                City
                            </span>
                            <asp:TextBox ID="txtCity" runat="server" ClientIDMode="Static" CssClass="form-control" ReadOnly="true" Text='<%#Eval("City") %>' />
                        </div>
                    </div>
                    <div class="form-group col-xs-3" style="">
                        <div class="input-group">
                            <span class="input-group-addon">
                                State
                            </span>
                            <asp:TextBox ID="txtState" runat="server" ClientIDMode="Static" CssClass="form-control" ReadOnly="true" Text='<%#Eval("State") %>' />
                        </div>
                    </div>
                    <div class="form-group col-xs-3" style="padding-right:0;">
                        <div class="input-group">
                            <span class="input-group-addon">
                                Zip
                            </span>
                            <asp:TextBox ID="txtZipCode" runat="server" ClientIDMode="Static" CssClass="form-control" ReadOnly="true" Text='<%#Eval("ZipCode") %>' />
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="form-group col-xs-6" style="padding-left:0;">
                        <div class="input-group">
                            <span class="input-group-addon">
                                Box Office Phone
                            </span>
                            <asp:TextBox ID="txtBoPhone" runat="server" ClientIDMode="Static" CssClass="form-control" ReadOnly="true" Text='<%#Eval("BoxOfficePhone") %>' />
                        </div>
                    </div>
                    <div class="form-group col-xs-6" style="">
                        <div class="input-group">
                            <span class="input-group-addon">
                                Box Office Ext
                            </span>
                            <asp:TextBox ID="txtBoExt" runat="server" ClientIDMode="Static" CssClass="form-control" ReadOnly="true" Text='<%#Eval("BoxOfficePhoneExt") %>' />
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="input-group">
                        <span class="input-group-addon">
                            Box Office Notes
                        </span>
                        <asp:TextBox ID="txtBoNotes" runat="server" ClientIDMode="Static" CssClass="form-control" ReadOnly="true" Text='<%#Eval("BoxOfficeNotes") %>' />
                    </div>
                </div>

                <div class="form-group">
                    <div class="form-group col-xs-6" style="padding-left:0;">
                        <div class="input-group">
                            <span class="input-group-addon">
                                Main Office Phone
                            </span>
                            <asp:TextBox ID="txtMainPhone" runat="server" ClientIDMode="Static" CssClass="form-control" ReadOnly="true" Text='<%#Eval("MainPhone") %>' />
                        </div>
                    </div>
                    <div class="form-group col-xs-6" style="">
                        <div class="input-group">
                            <span class="input-group-addon">
                                Main Office Ext
                            </span>
                            <asp:TextBox ID="txtMainPhoneExt" runat="server" ClientIDMode="Static" CssClass="form-control" ReadOnly="true" Text='<%#Eval("MainPhoneExt") %>' />
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="input-group">
                        <span class="input-group-addon">
                            Notes (fax)
                        </span>
                        <asp:TextBox ID="txtNotes" runat="server" ClientIDMode="Static" CssClass="form-control" ReadOnly="true" Text='<%#Eval("Notes") %>' />
                    </div>
                </div>
                <%} %>
            </ItemTemplate>
        </asp:FormView>
        <asp:Panel ID="PanelHidden" runat="server" Height="0">
            <input type="hidden" id="hidSelectedValue" name="hidSelectedValue" runat="server" />
            <asp:LinkButton ID="btnLoad" runat="server" Text="Load/Create" CssClass="btnhid" CausesValidation="false" OnClick="btnLoad_Click" />
        </asp:Panel>
    </div>
</div>
<asp:SqlDataSource ID="SqlDetails" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" EnableCaching="false"
    DeleteCommand="SELECT 0" 
    InsertCommand="INSERT INTO [Venue] ([ApplicationId], [Name], [vcPrincipal]) VALUES (@appId, @Name, @vcPrincipal); SELECT @NewId = @@IDENTITY; "
    SelectCommand="SELECT [Id], [Name], [DisplayName], [WebsiteUrl], [iCapacity], [Latitude], [Longitude], [ShortAddress], [Address], [City], [State], [ZipCode], [BoxOfficePhone], [BoxOfficePhoneExt], [BoxOfficeNotes], [MainPhone], [MainPhoneExt], [Notes], [vcPrincipal] FROM [Venue] WHERE ([ApplicationId] = @appId AND [Id] = @Id); "
    UpdateCommand="UPDATE [Venue] SET [Name] = @Name, [DisplayName] = @DisplayName, [WebsiteUrl] = @WebsiteUrl, [iCapacity] = @iCapacity, [Latitude] = @Latitude, [Longitude] = @Longitude, [ShortAddress] = @ShortAddress, [Address] = @Address, [City] = @City, [State] = @State, [ZipCode] = @ZipCode, [BoxOfficePhone] = @BoxOfficePhone, [BoxOfficePhoneExt] = @BoxOfficePhoneExt, [BoxOfficeNotes] = @BoxOfficeNotes, [MainPhone] = @MainPhone, [MainPhoneExt] = @MainPhoneExt, [Notes] = @Notes WHERE [Id] = @Id "
    OnInserting="SqlDetails_Inserting"
    OnInserted="SqlDetails_Inserted" OnSelecting="SqlDetails_Selecting" >
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Name" Type="String" />
        <asp:Parameter Name="DisplayName" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="WebsiteUrl" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="iCapacity" Type="Int32" />
        <asp:Parameter Name="Latitude" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="Longitude" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="ShortAddress" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="Address" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="City" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="State" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="ZipCode" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="BoxOfficePhone" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="BoxOfficePhoneExt" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="BoxOfficeNotes" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="MainPhone" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="MainPhoneExt" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="Notes" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:CookieParameter ConvertEmptyStringToNull="false" CookieName="vnid" DefaultValue="0" Name="Id" Type="int32" />
    </SelectParameters>
    <InsertParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:Parameter Name="NewId" Direction="output" DefaultValue="567" Type="Int32" />
        <asp:Parameter Name="Name" Type="String" />
        <asp:Parameter Name="vcPrincipal" Type="String" />
    </InsertParameters>
</asp:SqlDataSource>