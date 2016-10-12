<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditFaq_Content.ascx.cs" Inherits="wctMain.Admin._collectionEditors._faq._controls.EditFaq_Content" %>
<%@ Register assembly="WctControls" namespace="WctControls.WebControls" tagprefix="cc1" %>
<%@ Register src="/Admin/_customControls/WctCkEditor.ascx" tagname="WctCkEditor" tagprefix="uc2" %>

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
                    <uc2:WctCkEditor ID="WctCkEditor1" runat="server" Width="750px" Height="600" Rows="10" Text='<%#Eval("Answer") %>' />
                </div>
            </div>
            
        </EditItemTemplate>
    </asp:FormView>
</div>