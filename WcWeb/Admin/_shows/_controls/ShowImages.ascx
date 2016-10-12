<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ShowImages.ascx.cs" Inherits="wctMain.Admin._shows._controls.ShowImages" %>
<%@ Register assembly="WctControls" namespace="WctControls.WebControls" tagprefix="cc1" %>

<div id="showimages" class="show-mgmt">    
    <div class="panel-actions">
        <asp:LinkButton ID="btnUpdate" runat="server" CausesValidation="false" CommandName="Update" Text="Save" Enabled="false"
            OnClick="btnUpdate_Click" CssClass="btn btn-lg btn-primary btn-command" />
        <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn btn-lg btn-primary btn-command" Enabled="false"
            CommandName="Cancel" Text="Cancel" OnClick="btnCancel_Click" />                    
        <asp:LinkButton ID="btnChangeShowName" runat="server" Text="Sync Name" CssClass="btn btn-lg btn-primary btn-command" Enabled="false" 
            OnClick="btnChangeShowName_Click"
            OnClientClick="return confirm('This will update the show name to reflect the current information. Are you sure you want to continue?');" />
        <a data-toggle="modal" class="btn btn-lg btn-primary btn-command showcopy-modal-launcher disabled" href="#" >Copy Show</a>                              
        <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" CssClass="btn btn-lg btn-danger btn-command" 
            CommandName="Delete" Enabled="false" Text="Delete" OnClick="btnDelete_Click" />
        <a data-toggle="modal" class="btn btn-lg btn-success btn-command showdisplay-modal-launcher" href="#" >Preview Panel</a>
        <a target="_blank" class="btn btn-lg btn-success btn-command" 
            href='http://<%= Wcss._Config._DomainName%>/<%= Atx.CurrentShowRecord.FirstShowDate.ConfiguredUrl %>/?adm=t' >Preview Window</a>            
        <%if(Atx.IsSuperSession(this.Page.User)) {%>
            <small><%= Atx.CurrentShowRecord.Id.ToString()%> / <%= Atx.CurrentShowRecord.FirstShowDate.Id.ToString() %></small>
        <%} %>
    </div>
    <div class="panel-body">
        <cc1:ErrorDisplayLabel ID="ErrorDisplayLabel1" runat="server" />

        <asp:FormView ID="frmImaging" runat="server" DataKeyNames="Id" DefaultMode="Edit"
            OnDataBinding="frmImaging_DataBinding" OnDataBound="frmImaging_DataBound" >
            <EditItemTemplate>
                <div class="form-group col-xs-8" style="padding-left:0;padding-top:0;">
                    <div class="form-group">
                        <div style="display:inline-block;position:relative;bottom:-3px;">
                            <asp:FileUpload ID="imageuploadify" ClientIDMode="Static" runat="server" CssClass="btn btn-lg" />
                        </div>
                            
                        <a id="performcrop" class="btn btn-lg btn-tab-command btn-primary" >Crop It</a>
                            
                        <a id="performcrop_ratio_square" class="btn btn-lg btn-tab-command btn-primary" >Square Crop</a>
                            
                        <a id="performrotate" class="btn btn-lg btn-tab-command btn-primary" >Rotate 90</a>
                    </div>
                    <div class="form-group">
                        <img id="imageeditbox" src='<%#Eval("ImageManager.OriginalUrl") %>' alt="" style="display:inline-block;width:<%#displayWidth.ToString()%>px;" />
                    </div>
                    <asp:HiddenField ID="hdnEntityType" runat="server" ClientIDMode="Static" Value='<%#Wcss.Show.Schema.TableName%>' />
                    <asp:HiddenField ID="hdnDisplayWidth" runat="server" ClientIDMode="Static" Value='<%#displayWidth.ToString()%>' />
                </div>
                <div class="form-group col-xs-4" style="padding-left:0;">
                    <ul>
                        <li class="list-group-item list-group-item-info">If no image is specified, it will default to the default bg image from the config (settings) (currently Flume)</li>
                        <li class="list-group-item list-group-item-info">The image default is centered. If the heads are getting cutoff, ie if the image needs to move down, uncheck the vertical(y) option. This, essentially, aligns the image to the top.</li>
                        <li class="list-group-item list-group-item-info">If the pic is too wide, uncheck the horizontal(x) option. This will align the image to the left hand side and is generally more subtle.</li>                                
                        <li class="list-group-item list-group-item-info">Width and heights under 1024 are subject to stretching. Landscape ratios > 1.5 are very wide. Portrait ratios < .7 are very tall</li>
                        <asp:Literal ID="litImageFromText" runat="server" />
                    </ul>
                    <div class="form-group">
                        <div class="input-group">
                            <span class="input-group-addon">
                                Center Vertical
                            </span>
                            <asp:CheckBox id="chkCtrY" checked='<%#Eval("Centered_Y")%>' runat="server" AutoPostBack="true" OnCheckedChanged="CheckCtr_CheckChanged"
                                CssClass="form-control" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="input-group">
                            <span class="input-group-addon">
                                Center Horiz
                            </span>
                            <asp:CheckBox id="chkCtrX" checked='<%#Eval("Centered_X")%>' runat="server" AutoPostBack="true" OnCheckedChanged="CheckCtr_CheckChanged" 
                                CssClass="form-control" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-lg btn-primary" Text="Clear Image" CausesValidation="false" 
                            OnClientClick='return confirm("Are you sure you want to delete this image? This will only delete an image assigned to the show. It will not affect an image linked from an act.")' 
                            OnClick="btnClear_Click" />
                    </div>
                </div>
            </EditItemTemplate>
        </asp:FormView>
    </div>

    <div class="panel-footer">
        <div class="form-group">
            &nbsp;
        </div>
    </div>
</div>

<input type="hidden" id="x1" name="x1" />
<input type="hidden" id="y1" name="y1" />
<input type="hidden" id="x2" name="x2" />
<input type="hidden" id="y2" name="y2" />
<input type="hidden" id="w1" name="w1" />
<input type="hidden" id="h1" name="h1" />