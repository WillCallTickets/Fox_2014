<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditFaq_Info.ascx.cs" Inherits="wctMain.Admin._collectionEditors._faq._controls.EditFaq_Info" %>
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
            There is no Faq selection.
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
                        Category
                    </span>
                    <asp:RadioButtonList ID="rdoCategory" runat="server" ClientIDMode="Static" CssClass="form-control radio-horiz" 
                        RepeatLayout="Table" RepeatDirection="Horizontal"
                        OnDataBinding="rdoCategory_DataBinding"
                        OnDataBound="rdoCategory_DataBound">
                    </asp:RadioButtonList>
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
            <div class="clearfix"></div>

            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-addon alert-danger">
                        Question
                    </span>
                    <asp:TextBox ID="txtName" runat="server" ClientIDMode="Static" TextMode="MultiLine" MaxLength="896" Height="100px" CssClass="form-control form-control-name" Text='<%#Bind("Question") %>' />
                </div>
            </div>
            </div>
            
            
        </EditItemTemplate>
    </asp:FormView>
</div>