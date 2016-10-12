<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditPost_Info.ascx.cs" Inherits="wctMain.Admin._collectionEditors._post._controls.EditPost_Info" %>
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
            There is no Post selection.
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
                    <span class="input-group-addon">
                        Slug
                    </span>
                    <asp:TextBox ID="txtSlug" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%#Bind("Slug") %>' />
                </div>
            </div>
            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-addon">
                        Desc
                    </span>
                    <asp:TextBox ID="txtDescription" runat="server" ClientIDMode="Static" TextMode="MultiLine" Rows="3" CssClass="form-control" Text='<%#Bind("Description") %>' />
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
                <div class="form-group col-xs-6" style="padding-right:0;">&nbsp;</div>
            </div>
            
        </EditItemTemplate>
    </asp:FormView>
</div>