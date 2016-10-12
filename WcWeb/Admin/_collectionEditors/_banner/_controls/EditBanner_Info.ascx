<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditBanner_Info.ascx.cs" Inherits="wctMain.Admin._collectionEditors._banner._controls.EditBanner_Info" %>
<%@ Register assembly="WctControls" namespace="WctControls.WebControls" tagprefix="cc1" %>

<div class="vd-data-entry">
    <br />
    <cc1:ErrorDisplayLabel ID="ErrorDisplayLabel1" runat="server" />

    <asp:FormView id="formEnt" runat="server" ClientIDMode="Static" EnableViewState="true" DefaultMode="Edit" 
        OnDataBinding="formEntity_DataBinding"  
        OnDataBound="formEntity_DataBound" 
        OnItemUpdating="formEntity_ItemUpdating"
        OnModeChanging="formEntity_ModeChanging" >
        <EmptyDataTemplate>
            There is no Banner selection.
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
                <div class="input-group">
                    <span class="input-group-addon alert-danger">
                        Display Text
                    </span>
                    <asp:TextBox ID="txtDisplayText" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("DisplayText") %>' />
                </div>
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
                            DisplayTime
                        </span>
                        <asp:TextBox ID="txtBannerTimeout" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("BannerTimeout") %>' />
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="form-group col-xs-6" style="padding-left:0;">
                    <cc1:BootstrapDateTimePicker ID="txtDateStart" Label="Start" Date='<%#Bind("DateStart") %>' DateCompareEmpty="min" CssClass="banner-dtpicker" runat="server" />
                </div>
                <div class="form-group col-xs-6" style="padding-right:0;">
                    <cc1:BootstrapDateTimePicker ID="txtDateEnd" Label="End" Date='<%#Bind("DateEnd") %>' DateCompareEmpty="max" CssClass="banner-dtpicker" runat="server" />
                </div>
            </div>
            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-addon">
                        Additional Text
                    </span>
                    <asp:TextBox ID="txtAdditionalText" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("AdditionalText") %>' />
                </div>
            </div>
            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-btn input-group-addon-btn">
                        <a id="insertShowSelection" href="#ddlShowList" rel="#txtBannerClickUrl" class="btn">Insert Selection</a>
                    </span>
                    <asp:DropDownList ID="ddlShowList" ClientIDMode="Static" runat="server" OnDataBinding="ddlShowList_DataBinding" CssClass="form-control" />
                </div>
            </div>
            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-addon">
                        Click Url
                    </span>
                    <asp:TextBox ID="txtBannerClickUrl" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("BannerClickUrl") %>' />
                </div>
            </div>
        </EditItemTemplate>
    </asp:FormView>
</div>