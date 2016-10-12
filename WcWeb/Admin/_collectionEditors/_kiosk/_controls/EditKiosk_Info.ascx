<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditKiosk_Info.ascx.cs" Inherits="wctMain.Admin._collectionEditors._kiosk._controls.EditKiosk_Info" %>
<%@ Register assembly="WctControls" namespace="WctControls.WebControls" tagprefix="cc1" %>

<div class="vd-data-entry">
    <br />
    <cc1:ErrorDisplayLabel ID="ErrorDisplayLabel1" runat="server" />

    <asp:FormView id="formEnt" runat="server" ClientIDMode="Static" EnableViewState="true" DefaultMode="Edit" 
        OnDataBinding="formEntity_DataBinding"  
        OnDataBound="formEntity_DataBound" 
        OnItemCommand="formEntity_ItemCommand"
        OnItemUpdating="formEntity_ItemUpdating"
        OnModeChanging="formEntity_ModeChanging" >
        <EmptyDataTemplate>
            There is no Kiosk selection.
        </EmptyDataTemplate>
        <EditItemTemplate>
            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-addon alert-danger">
                        Belongs To
                    </span>
                    <asp:CheckBoxList ID="chkPrincipal" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" CssClass="form-control checkbox-control" OnDataBinding="chkPrincipal_DataBinding" />
                </div>
            </div>     
            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-addon alert-danger">
                        Name
                    </span>
                    <asp:TextBox ID="txtName" runat="server" ClientIDMode="Static" CssClass="form-control form-control-name" Text='<%#Bind("Name") %>' />
                </div>
            </div>
            <div class="form-group">
                <ul class="list-group" style="margin-bottom:0;">
                    <li class="list-group-item list-group-item-info"><span class="glyphicon glyphicon-info-sign"></span> Timeouts for calendars should be set to 30000.</li></ul>
            </div>
            <div class="form-group">
                <div class="form-group col-xs-6" style="padding-left:0;">
                    <div class="input-group">
                        <span class="input-group-addon">
                            Active
                        </span>
                        <asp:CheckBox ID="chkActive" runat="server" ClientIDMode="Static" CssClass="form-control" TextAlign="Right"  Checked='<%#Bind("IsActive") %>' />
                    </div>
                </div>
                <div class="form-group col-xs-6" style="padding-right:0;">
                    <div class="input-group">
                        <span class="input-group-addon">
                            Timeout
                        </span>
                        <asp:TextBox ID="txtTimeout" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("Timeout") %>' />
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="form-group col-xs-6" style="padding-left:0;">
                    <cc1:BootstrapDateTimePicker ID="txtDateStart" Label="Start" Date='<%#Bind("DateStart") %>' DateCompareEmpty="min" CssClass="kiosk-dtpicker" runat="server" />
                </div>
                <div class="form-group col-xs-6" style="padding-right:0;">
                    <cc1:BootstrapDateTimePicker ID="txtDateEnd" Label="End" Date='<%#Bind("DateEnd") %>' DateCompareEmpty="max" CssClass="kiosk-dtpicker" runat="server" />
                </div>
            </div>
            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-addon">
                        Venue
                    </span>
                    <asp:TextBox ID="txtEventVenue" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("EventVenue") %>' />
                </div>
            </div>
            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-addon">
                        Date
                    </span>
                    <asp:TextBox ID="txtEventDate" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("EventDate") %>' />
                </div>
            </div>
            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-addon">
                        Title
                    </span>
                    <asp:TextBox ID="txtEventTitle" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("EventTitle") %>' />
                </div>
            </div>
            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-addon">
                        Headline
                    </span>
                    <asp:TextBox ID="txtEventHeads" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("EventHeads") %>' />
                </div>
            </div>
            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-addon">
                        Openers
                    </span>
                    <asp:TextBox ID="TextBox1" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("EventOpeners") %>' />
                </div>
            </div>

            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-addon">
                        Desc
                    </span>
                    <asp:TextBox ID="txtEventDescription" runat="server" ClientIDMode="Static" TextMode="MultiLine" Rows="3" CssClass="form-control" Text='<%#Bind("EventDescription") %>' />
                </div>
            </div>
            
        </EditItemTemplate>
    </asp:FormView>
</div>