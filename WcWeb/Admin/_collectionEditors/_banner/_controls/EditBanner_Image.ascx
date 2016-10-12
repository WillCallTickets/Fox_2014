<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditBanner_Image.ascx.cs" Inherits="wctMain.Admin._collectionEditors._banner._controls.EditBanner_Image" %>
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
                    <span class="input-group-addon">
                        Name
                    </span>
                    <asp:TextBox ID="txtName" runat="server" ReadOnly="true" ClientIDMode="Static" CssClass="form-control form-control-name" Text='<%#Eval("Name") %>' />
                </div>
            </div>            
            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-addon">
                        Image
                    </span>
                    <asp:TextBox ID="txtBannerUrl" runat="server" ReadOnly="true" ClientIDMode="Static" CssClass="form-control" Text='<%#Eval("BannerUrl") %>' />
                </div>
            </div>
            <div class="form-group">
                <div class="input-group">                    
                    <ul class="list-group" style="margin-bottom:0;">
                        <li class="list-group-item list-group-item-info"><span class="glyphicon glyphicon-info-sign"></span> Valid image types are gif, jpg, jpeg and png.</li>
                        <li class="list-group-item list-group-item-info"><span class="glyphicon glyphicon-info-sign"></span> <%=Wcss._Config._BannerDimensionText %></li>
                    </ul>
                </div>
            </div>
            <div class="form-group">
                <div class="input-group" style="width:100%;">
                    <div class="input-group-btn input-group-addon-btn input-group-dropdown">
                        <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">Import <span class="caret"></span></button>
                        <ul class="dropdown-menu">            
                            <li><asp:FileUpload ID="basicUploadify" ClientIDMode="Static" runat="server" /></li>
                        </ul>
                    </div>
                    <div class="form-control" style="height:100px;">
                        <asp:Literal ID="litImageEditBox" runat="server" EnableViewState="false" />                         
                    </div>
                </div>
            </div>

            <asp:HiddenField ID="hdnEntityType" runat="server" ClientIDMode="Static" Value='<%#Wcss.SalePromotion.Schema.TableName %>' />
            <asp:HiddenField ID="hdnEntityId" runat="server" ClientIDMode="Static" Value='<%#Eval("Id")%>' />

        </EditItemTemplate>
    </asp:FormView>
</div>
