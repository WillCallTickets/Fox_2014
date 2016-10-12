<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditKiosk_Arrangement.ascx.cs" Inherits="wctMain.Admin._collectionEditors._kiosk._controls.EditKiosk_Arrangement" %>
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
                    <span class="input-group-addon">
                        Name
                    </span>
                    <asp:TextBox ID="txtName" runat="server" ReadOnly="true" ClientIDMode="Static" CssClass="form-control form-control-name" Text='<%#Bind("Name") %>' />
                </div>
            </div>
            <div class="form-group">
                <div class="input-group">
                    <div class="input-group-btn input-group-addon-btn input-group-dropdown">
                        <button type="button" class="btn btn-default btn-no-background dropdown-toggle" data-toggle="dropdown">Text Position <span class="caret"></span></button>
                        <asp:RadioButtonList ID="rdoKioskPosition" runat="server" ClientIDMode="Static" RepeatLayout="OrderedList" 
                            AutoPostBack="true" cssclass="dropdown-menu padded-menu"
                            OnSelectedIndexChanged="rdoKioskPosition_SelectedIndexChanged" EnableViewState="true">
                            <asp:ListItem Text="Top Left" Value="kiosk-postl"></asp:ListItem>
                            <asp:ListItem Text="Bottom Left" Value="kiosk-posbl"></asp:ListItem>
                            <asp:ListItem Text="Top Right" Value="kiosk-postr"></asp:ListItem>
                            <asp:ListItem Text="Bottom Right" Value="kiosk-posbr"></asp:ListItem>
                            <asp:ListItem Text="Top Center" Value="kiosk-postc"></asp:ListItem>
                            <asp:ListItem Text="Middle Center" Value="kiosk-posmc"></asp:ListItem>
                            <asp:ListItem Text="Bottom Center" Value="kiosk-posbc"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="input-group-btn input-group-addon-btn input-group-dropdown">
                        <button type="button" class="btn btn-default btn-no-background dropdown-toggle" data-toggle="dropdown">Text Color <span class="caret"></span></button>
                        <asp:RadioButtonList ID="rdoTextColor" runat="server" ClientIDMode="Static" RepeatLayout="OrderedList" 
                            AutoPostBack="true" CssClass="dropdown-menu padded-menu" 
                            OnSelectedIndexChanged="rdoTextColor_SelectedIndexChanged" EnableViewState="true">
                            <asp:ListItem Text="Light Text" Value="kisok-lighttext"></asp:ListItem>
                            <asp:ListItem Text="Dark Text" Value="kiosk-darktext"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <asp:TextBox ID="TextBox1" runat="server" ReadOnly="true" ClientIDMode="Static" CssClass="form-control" Text="" />
                </div>
            </div>
            <div class="form-group">
                <div class="kiosk-viewport kiosk-admin-viewport">
                    <asp:Literal ID="litViewportImage" runat="server" EnableViewState="false" />
                    <asp:Literal ID="litViewportText" runat="server" EnableViewState="false" />
                </div>
            </div>
            <asp:HiddenField ID="hdnEntityType" runat="server" ClientIDMode="Static" Value='<%#Wcss.Kiosk.Schema.TableName %>' />
            <asp:HiddenField ID="hdnDisplayWidth" runat="server" ClientIDMode="Static" Value='<%#displayWidth.ToString()%>' />
        </EditItemTemplate>
    </asp:FormView>
</div>