<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Editor_Video.ascx.cs" Inherits="wctMain.Admin._editors._controls.Editor_Video" %>
<%@ Register assembly="WctControls" namespace="WctControls.WebControls" tagprefix="cc2" %>

<div id="videoeditor" class="show-mgmt">
    <div class="panel-body">
        <h1>You Tube Playlist instructions</h1>
        <h4>Tied into Google Plus pages - if this link does not work, head to plus.google.com and search fox FoxTheatre</h4>
        <a target="_blank" href="https://plus.google.com/+TheFoxTheatreBoulder/">https://plus.google.com/+TheFoxTheatreBoulder/</a>
        <br />
        <h4>To edit Google Plus settings:</h4>
        <a target="_blank" href="https://plus.google.com/b/112617896826195842215/pages/settings/plus">https://plus.google.com/b/112617896826195842215/pages/settings/plus</a>
        <br />
        <h4>To add/edit Google Plus managers:</h4>
        <a target="_blank" href="https://plus.google.com/b/112617896826195842215/pages/settings/admin">https://plus.google.com/b/112617896826195842215/pages/settings/admin</a>
        <br />
        <br /><br />

        <h4>Go to</h4> 
        <a target="_blank" href="https://youtube.com">https://youtube.com</a>
        <br />
        <ul>
            <li>Sign in to your google account</li>
            <li>You will need to switch your account to The Fox Theatre.</li>
            <li>Click on you avatar in the top right hand corner.</li>
            <li>Click on switch account and select The Fox Theatre.</li>
        </ul>
        <br />
        <h4>To edit the playlist</h4>
        <ul>
            <li>On the left hand side, click playlists</li>
            <li>Clicking on the thumbnail video will play all the videos</li>
            <li>Clicking on the title under the video will take you to where you can edit the playlist</li>
            <li>After you click on the title there is a button available on the upper right (below the header)</li>
            <li>As of 2/15/2014 (still valid 10/11/2014): PLemkhiE8QQpv6i_wyVpxmQZN7VAn0lruf</li>
        </ul>
        <br />
        <div class="form-group" style="width:700px;">
            <div class="input-group">
                <span class="input-group-addon">
                    Playlist Id
                </span>
                <asp:TextBox ID="txtPlaylistId" MaxLength="100" runat="server" CssClass="form-control" Text='<%# Wcss._Config._YouTubePlaylist %>'></asp:TextBox>
                <span class="input-group-btn">
                    <asp:Button ID="btnUpdate" runat="server" CausesValidation="false" CommandName="Update" Text="Save" Width="80px" 
                        OnClick="btnUpdate_Click" CssClass="btn btn-primary" />
                    <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn btn-primary" Width="80px"
                        CommandName="Cancel" Text="Cancel" OnClick="btnCancel_Click" />
                </span>
            </div>
        </div>
        <br />
        <section id="widgetpanel">
    
            <section id="videoplayer" style="width:400px;margin-top:15px;">
                <div class="section-header">
                    Featured Artist Videos
                </div>
                <div id="ytplayer" class="iframe-wrapper main-inner">
                    <iframe id="Iframe2" type="text/html" width="400" class="img-rounded"                
                        src="https://www.youtube.com/embed/?listType=playlist&list=<%=Wcss._Config._YouTubePlaylist %>&fs=1&color=white&theme=dark&modestbranding=1&autohide=1&wmode=transparent" 
                        frameborder="0" allowfullscreen></iframe>
                </div>    
            </section>
        </section>
    </div>
</div>