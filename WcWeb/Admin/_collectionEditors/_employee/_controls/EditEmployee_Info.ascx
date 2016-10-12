<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditEmployee_Info.ascx.cs" Inherits="wctMain.Admin._collectionEditors._employee._controls.EditEmployee_Info" %>
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
            There is no Employee selection.
        </EmptyDataTemplate>
        <EditItemTemplate>
            <div class="form-group">
                <div class="input-group">                    
                    <ul class="list-group" style="margin-bottom:0;">
                        <li class="list-group-item list-group-item-info"><span class="glyphicon glyphicon-info-sign"></span> Z2 employees do not use Names - enter placeholder data if names not applicable.</li>
                    </ul>
                </div>
            </div>
            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-addon alert-danger">
                        Belongs To
                    </span>
                    <asp:CheckBoxList ID="chkPrincipal" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="form-control checkbox-control" OnDataBinding="chkPrincipal_DataBinding" />
                </div>
            </div>            
            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-addon alert-danger">
                        First Name
                    </span>
                    <asp:TextBox ID="txtFirstName" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("FirstName") %>' />
                </div>
            </div>            
            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-addon alert-danger">
                        Last Name
                    </span>
                    <asp:TextBox ID="txtLastName" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("LastName") %>' />
                </div>
            </div>
            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-addon alert-danger">
                        Title
                    </span>
                    <asp:TextBox ID="txtTitle" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("Title") %>' />
                </div>
            </div>
            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-addon alert-danger">
                        Email
                    </span>
                    <asp:TextBox ID="txtEmail" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("EmailAddress") %>' />
                </div>
            </div>
            <div class="form-group">
                <div class="form-group col-xs-6" style="padding-left:0;">
                    <div class="input-group">
                        <span class="input-group-addon">
                            Active
                        </span>
                        <asp:CheckBox ID="chkActive" runat="server" ClientIDMode="Static" CssClass="form-control" TextAlign="Right"  Checked='<%#Bind("IsListInDirectory") %>' />
                    </div>
                </div>
                <div class="form-group col-xs-6" style="padding-right:0;">
                    &nbsp;
                </div>
            </div>
        </EditItemTemplate>
    </asp:FormView>
</div>