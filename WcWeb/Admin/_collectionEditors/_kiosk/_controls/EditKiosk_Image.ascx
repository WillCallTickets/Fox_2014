<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditKiosk_Image.ascx.cs" Inherits="wctMain.Admin._collectionEditors._kiosk._controls.EditKiosk_Image" %>
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
                    <span class="input-group-addon">
                        Image
                    </span>
                    <asp:TextBox ID="txtDisplayUrl" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Eval("DisplayUrl") %>' />
                </div>
            </div>
            <div class="form-group">
                <div class="input-group">
                    <div class="input-group-btn input-group-addon-btn input-group-dropdown">
                        <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">Import <span class="caret"></span></button>
                        <ul class="dropdown-menu">
                            <li><asp:FileUpload ID="imageuploadify" ClientIDMode="Static" runat="server" /></li>
                            <li><asp:LinkButton ID="btnImportFromSelection" runat="server" ClientIDMode="Static" CommandName="Import" Text="Import Selected" /></li>
                        </ul>
                    </div>
                    <asp:dropdownlist id="ddlShowImageImport" runat="server" ClientIDMode="Static" EnableViewState="false" CssClass="form-control"
                        DataSourceID="SqlShowList" DataTextField="ShowName" DataValueField="ShowId">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <ul class="list-group" style="margin-bottom:0;">
                    <li class="list-group-item list-group-item-info"><span class="glyphicon glyphicon-info-sign"></span> Valid image types are gif, jpg, jpeg and png.</li>
                    <li class="list-group-item list-group-item-info"><span class="glyphicon glyphicon-info-sign"></span> The optimal size is 1080w and 600h (1.8 w/h ratio).</li>
                    <li class="list-group-item list-group-item-info"><span class="glyphicon glyphicon-info-sign"></span> Calendars should be resized to 1080w and 600h prior to import.</li>
                </ul>
            </div>
            <div class="form-group">
                <div class="input-group">
                    <div class="input-group-btn input-group-addon-btn input-group-dropdown">
                        <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">Action <span class="caret"></span></button>
                        <ul class="dropdown-menu">
                            <li><asp:LinkButton ID="performcrop" runat="server" ClientIDMode="Static" CommandName="Crop" Text="Crop" /></li>
                            <li><asp:LinkButton ID="performrotate" runat="server" ClientIDMode="Static" CommandName="Rotate" Text="Rotate 90&deg;" /></li>
                        </ul>
                    </div>
                    <div class="form-control" style="height:600px;">
                        <asp:Literal ID="litImageEditBox" runat="server" EnableViewState="false" />                        
                    </div>
                </div>
            </div>  

            <asp:HiddenField ID="hdnEntityType" runat="server" ClientIDMode="Static" Value='<%#Wcss.Kiosk.Schema.TableName %>' />
            <asp:HiddenField ID="hdnDisplayWidth" runat="server" ClientIDMode="Static" Value='<%#displayWidth.ToString()%>' />
            <asp:HiddenField ID="hdnCropRatio" runat="server" ClientIDMode="Static" Value='1.6' />

            <input type="hidden" id="x1" name="x1" />
            <input type="hidden" id="y1" name="y1" />
            <input type="hidden" id="x2" name="x2" />
            <input type="hidden" id="y2" name="y2" />
            <input type="hidden" id="w1" name="w1" />
            <input type="hidden" id="h1" name="h1" />

        </EditItemTemplate>
    </asp:FormView>
</div>

<asp:SqlDataSource ID="SqlShowList" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="SELECT 0"
    onselecting="SqlShowList_Selecting">
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:Parameter Name="startDate" Type="DateTime" />
    </SelectParameters>    
</asp:SqlDataSource>