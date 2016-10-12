<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ShowDetails.ascx.cs" Inherits="wctMain.Admin._shows._controls.ShowDetails" %>
<%@ Register src="/Admin/_customControls/WctCkEditor.ascx" tagname="WctCkEditor" tagprefix="uc2" %>
<%@ Register assembly="WctControls" namespace="WctControls.WebControls" tagprefix="cc1" %>

<div id="showdetails" class="show-mgmt">    
    <div class="panel-actions">
        <asp:LinkButton ID="btnUpdate" runat="server" CausesValidation="false" CommandName="Update" Text="Save" 
            OnClick="btnUpdate_Click" CssClass="btn btn-lg btn-primary btn-command" />
        <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn btn-lg btn-primary btn-command" 
            CommandName="Cancel" Text="Cancel" OnClick="btnCancel_Click" />                    
        <asp:LinkButton ID="btnChangeShowName" runat="server" Text="Sync Name" CssClass="btn btn-lg btn-primary btn-command" 
            OnClick="btnChangeShowName_Click"
            OnClientClick="return confirm('This will update the show name to reflect the current information. Are you sure you want to continue?');" />
        <a data-toggle="modal" class="btn btn-lg btn-primary btn-command showcopy-modal-launcher" href="#" >Copy Show</a>                              
        <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" CssClass="btn btn-lg btn-danger btn-command" 
            CommandName="Delete" Text="Delete" OnClick="btnDelete_Click" />
        <a data-toggle="modal" class="btn btn-lg btn-success btn-command showdisplay-modal-launcher" href="#" >Preview Panel</a>
        <a target="_blank" class="btn btn-lg btn-success btn-command" 
            href='http://<%= Wcss._Config._DomainName%>/<%= Atx.CurrentShowRecord.FirstShowDate.ConfiguredUrl %>/?adm=t' >Preview Window</a>
        <%if(Atx.IsSuperSession(this.Page.User)) {%>
            <small><%= Atx.CurrentShowRecord.Id.ToString()%> / <%= Atx.CurrentShowRecord.FirstShowDate.Id.ToString() %></small>
        <%} %>
    </div>
    <div class="panel-body">
        <asp:FormView ID="FormView1" Width="100%" runat="server" DataKeyNames="Id" DefaultMode="Edit"
            OnDataBinding="FormView1_DataBinding" 
            OnDataBound="FormView1_DataBound" 
            OnItemUpdating="FormView1_ItemUpdating" 
            OnItemDeleting="FormView1_ItemDeleting"  
            OnModeChanging="FormView1_ModeChanging" >
            <EditItemTemplate>
                <cc1:ErrorDisplayLabel ID="ErrorDisplayLabel1" runat="server" />

                <div class="form-group">
                    <div class="input-group">
                        <span class="input-group-addon">
                            ID <%#Eval("Id")%>
                        </span>
                        <input type="text" readonly="true" size="100%" class="form-control" 
                            value='http://<%= Wcss._PrincipalBase.Host_IntendedForShow(this.Request, Atx.CurrentShowRecord) %>/<%#Eval("FirstShowDate.ConfiguredUrl") %>' />
                    </div>
                </div>
                <div class="form-group">
                    <div class="form-group col-xs-6" style="padding-left:0;">
                        <cc1:BootstrapDateTimePicker ID="txtDateOfShow" Label="Show Date" Date='<%#Bind("FirstShowDate.DtDateOfShow") %>' 
                            DateCompareEmpty="min" IsRequired="false" CssClass="showeditor-dtpicker" runat="server" />
                    </div>
                    <div class="form-group col-xs-6" style="padding-right:0;">
                        <div class="input-group">
                            <span class="input-group-addon">
                                Active
                            </span>
                            <asp:CheckBox id="chkActive" checked='<%#Bind("IsActive")%>' CssClass="<%#getActiveClass() %>" runat="server" />
                            <span class="input-group-addon input-group-info input-group-info-after">
                                Not active removes user listings
                            </span>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="form-group col-xs-6" style="padding-left:0;">                                    
                        <cc1:BootstrapDateTimePicker ID="txtShowTime" Label="Show Time" OnDataBinding="txtShowTime_DataBinding"
                            FormatString='<%# WctControls.WebControls.Bootstrap.DateTimePicker.Time_FormatString %>' 
                            DateCompareEmpty="min" IsRequired="false" CssClass="showeditor-showtime-dtpicker" runat="server" />
                    </div>
                    <div class="form-group col-xs-6" style="padding-right:0;">
                        <div class="input-group">
                            <span class="input-group-btn input-group-addon-btn dropup">
                                <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
                                    <span class="glyphicon glyphicon-info-sign"></span> Late Show</button>
                                <ul class="dropdown-menu" role="menu">
                                    <li>Changes the sorting date for show ordering.</li>
                                    <li>This essentially adds 24 hours to the show time to make shows at midnight or later order correctly.</li>
                                    <li>This is a matter of preference as some people like to have the date of midnight be from the previous day.</li>
                                </ul>
	                        </span>
                            <asp:CheckBox id="chkLate" CssClass="form-control" runat="server" Text="" />
                            <span class="input-group-addon input-group-info input-group-info-after">
                                <%#Eval("FirstShowDate.DateOfShow_ToSortBy") %>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="form-group col-xs-6" style="padding-left:0;">
                        <cc1:BootstrapDateTimePicker ID="txtDateAnnounce" Label="Announce" Date='<%#Bind("AnnounceDate") %>' 
                            DateCompareEmpty="min" IsRequired="false" CssClass="showeditor-dtpicker" runat="server" />
                    </div>
                    <div class="form-group col-xs-6" style="padding-right:0;">
                        <div class="input-group">
                            <span class="input-group-addon">
                                Ages
                            </span>
                            <asp:DropDownList ID="ddlAges" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </div>

                <div class="clearfix"></div>
                <div class="form-group">
                    <div class="input-group just-announced">
	                    <span class="input-group-addon" style="letter-spacing:-1px;">
                            Just Annc'd Widget
	                    </span>
                        <asp:RadioButtonList ID="rdoJust" runat="server" AutoPostBack="false" RepeatDirection="Horizontal" 
                            RepeatLayout="Table" CssClass="form-control radio-horiz" OnDataBinding="rdoJust_DataBinding"
                            OnDataBound="rdoJust_DataBound" >
                        </asp:RadioButtonList>
                        <span class="input-group-addon input-group-info input-group-info-after">
                            normal will follow algorithm - by announce or creation date
                        </span>
                    </div>
                </div>


                <div class="form-group">
                    <div class="form-group col-xs-6" style="padding-left:0;">
                        <cc1:BootstrapDateTimePicker ID="txtDateOnsale" Label="On Sale" Date='<%#Bind("DateOnSale") %>' 
                            DateCompareEmpty="min" IsRequired="false" CssClass="showeditor-dtpicker" runat="server" />
                    </div>
                    <div class="form-group col-xs-6" style="padding-right:0;">
                        <div class="input-group">
                            <span class="input-group-addon">
                                Status
                            </span>
                            <asp:DropDownList ID="ddlSaleStatus" runat="server" CssClass="form-control" 
                                OnDataBinding="ddlSaleStatus_DataBinding" OnDataBound="ddlSaleStatus_OnDataBound" />
                        </div>
                    </div>                                
                </div>
                <div class="clearfix"></div>
                <div class="form-group">
                    <div class="input-group">
	                    <span class="input-group-addon">
                            Pricing
	                    </span>
                        <asp:TextBox ID="txtPricing" MaxLength="500" runat="server" 
                            CssClass="form-control" Text='<%#Bind("firstShowDate.PricingText") %>' />
                    </div>
                </div>
                <div class="form-group">	
                    <div class="input-group">
                        <span class="input-group-btn input-group-addon-btn dropup dropup-multi">
                            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
                                <span class="glyphicon glyphicon-info-sign"></span> Menu Billing</button>
                            <ul class="dropdown-menu" role="menu">
                                <li>Enter a custom value here if you need to override the auto-generated value for menu listings.</li>
                                <li>It will not change the display of the show info page.</li>
                                <li>If you would like to change this for the show, use the acts admin page within the show.</li>
                            </ul>
	                    </span>
                        <asp:TextBox ID="txtBilling" MaxLength="300" runat="server" CssClass="form-control" Text='<%#Bind("firstShowDate.MenuBilling") %>' />
                    </div>
                </div>              
                <div class="form-group">	
                    <div class="input-group">
	                    <span class="input-group-btn input-group-addon-btn dropup dropup-multi">
                            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
                                <span class="glyphicon glyphicon-info-sign"></span> Ticket Url</button>
                            <ul class="dropdown-menu" role="menu">
                                <li><code>ex: http://foxtheatre.frontgatetickets.com/choose.php?a=1&lid=44423&eid=51253</code></li>
                            </ul>
	                    </span>
                        <asp:TextBox ID="txtUrl" MaxLength="500" runat="server" CssClass="form-control" Text='' 
                            OnTextChanged="txtUrl_TextChanged" />  
                        <span class="input-group-btn">
                            <asp:HyperLink ID="linkTestWebsite" runat="server" Target="_blank" CssClass="btn btn-primary" 
                                NavigateUrl="" OnDataBinding="linkTestWebsite_DataBinding" Text="test"></asp:HyperLink>
                        </span>
                    </div>
                </div>
                <div class="form-group">	
                    <%if (Atx.CurrentShowRecord.VcPrincipal == Wcss._Enums.Principal.chq.ToString())
                      { %>
                    <div class="alert alert-danger">Use FB Event for Details Page Url for Chautauqua shows!!!</div>
                    <%} %>
                    <div class="input-group">
	                    <span class="input-group-btn input-group-addon-btn dropup dropup-multi">
                            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
                                <span class="glyphicon glyphicon-info-sign"></span> FB Event</button>
                            <ul class="dropdown-menu" role="menu">
                                <li>Cut and copy from facebook.</li>
                                <li>Be sure to leave out any extraneous information after the trailing slash of the event id.</li>
                                <li><code>ex: https://www.facebook.com/events/1448127192119722/</code></li>
                            </ul>
	                    </span>
                        <asp:TextBox ID="txtFacebookEventUrl" MaxLength="500" runat="server" CssClass="form-control" 
                            Text='<%= FacebookEventUrl %>' OnTextChanged="txtFacebookEventUrl_TextChanged" />  
                        <span class="input-group-btn">
                            <asp:HyperLink ID="linkTestFacebookEvent" runat="server" Target="_blank" CssClass="btn btn-primary" 
                                NavigateUrl="" OnDataBinding="linkTestFacebookEvent_DataBinding" Text="test"></asp:HyperLink>
                        </span>
                    </div>
                </div>  
                <div class="form-group">                                    
                    <div class="input-group">
                        <span class="input-group-btn input-group-addon-btn dropup dropup-multi">
                            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
                                    <span class="glyphicon glyphicon-info-sign"></span> Show Alert</button>
                            <ul class="dropdown-menu" role="menu">
                                <li>Displays an alert status for the show.</li>
                                <li>Shows in the header as well as the left hand menu(fox).</li>
                            </ul>
	                    </span>
                        <asp:TextBox ID="txtAlert" MaxLength="500" runat="server" CssClass="form-control" Text='<%#Eval("ShowAlert") %>' />
                    </div>
                </div>
                <div class="form-group">
                    <div class="input-group">
                        <span class="input-group-btn input-group-addon-btn dropup dropup-multi">
                            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
                                <span class="glyphicon glyphicon-info-sign"></span> Title</button>
                            <ul class="dropdown-menu" role="menu">
                                <li>Shows only on the main event and read more popup.</li>
                                <li>Does not show on mobile or in side menu.</li>
                            </ul>
	                    </span>
                        <asp:TextBox ID="txtTitle" MaxLength="300" runat="server" CssClass="form-control" Text='<%#Eval("ShowTitle") %>' />
                    </div>
                </div>
                <div class="form-group">
                    <div class="input-group">
	                    <span class="input-group-addon">
                            Display Notes
	                    </span>
                        <asp:TextBox ID="txtDisplayNotes" MaxLength="1000" Height="120" TextMode="multiline" CssClass="form-control" runat="server" Text='<%#Eval("DisplayNotes") %>' />
                    </div>
                </div>
            </EditItemTemplate>
        </asp:FormView>
    </div> <!-- end primary panel-->
    <div class="panel-footer">
        <asp:LinkButton ID="btnFormUpdate" runat="server" CausesValidation="false" CommandName="Update" Text="Save" 
            OnClick="btnUpdate_Click" CssClass="btn btn-lg btn-primary btn-command" />
        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CssClass="btn btn-lg btn-primary btn-command" 
            CommandName="Cancel" Text="Cancel" OnClick="btnCancel_Click" />
        <a data-toggle="modal" class="btn btn-lg btn-success btn-command showdisplay-modal-launcher" href="#" >Preview Show</a>
        <a target="_blank" class="btn btn-lg btn-success btn-command" href='http://<%= Wcss._Config._DomainName%>/<%= Atx.CurrentShowRecord.FirstShowDate.ConfiguredUrl %>/?adm=t' >Preview In New Tab</a>
    </div>
</div>