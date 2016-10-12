<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ShowWriteup.ascx.cs" Inherits="wctMain.Admin._shows._controls.ShowWriteup" %>
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
        <a data-toggle="modal" class="btn btn-lg btn-primary btn-command showcopy-modal-launcher disabled" href="#" >Copy Show</a>                              
        <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" CssClass="btn btn-lg btn-danger btn-command" 
            CommandName="Delete" Text="Delete" OnClick="btnDelete_Click" Enabled="false" />
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
            OnModeChanging="FormView1_ModeChanging" >
            <EditItemTemplate>
                <cc1:ErrorDisplayLabel ID="ErrorDisplayLabel1" runat="server" />                                                 
                <div class="form-group">
                    <div class="form-group col-xs-6" style="padding-left:0;">
                        <div class="input-group">
                            <div style="width:100%;height:600px;display:block;">
                                <uc2:WctCkEditor ID="WctCkEditor1" runat="server" Height="465" />
                                <div class="clearfix"></div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group col-xs-6">
                        &nbsp;
                    </div>
                </div>
            </EditItemTemplate>
        </asp:FormView>
    </div>
    <div class="panel-footer">
        <asp:LinkButton ID="btnFormUpdate" runat="server" CausesValidation="false" CommandName="Update" Text="Save" 
            OnClick="btnUpdate_Click" CssClass="btn btn-lg btn-primary btn-command" />
        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CssClass="btn btn-lg btn-primary btn-command" 
            CommandName="Cancel" Text="Cancel" OnClick="btnCancel_Click" />
        <a data-toggle="modal" class="btn btn-lg btn-success btn-command showdisplay-modal-launcher" href="#" >Preview Show</a>
        <a target="_blank" class="btn btn-lg btn-success btn-command" href='http://<%= Wcss._Config._DomainName%>/<%= Atx.CurrentShowRecord.FirstShowDate.ConfiguredUrl %>/?adm=t' >Preview In New Tab</a>
    </div>
</div>