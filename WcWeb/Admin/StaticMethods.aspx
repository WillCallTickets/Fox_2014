<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StaticMethods.aspx.cs" MasterPageFile="/View/Masters/TemplateAdmin.master" 
Inherits="wctMain.Admin.StaticMethods" Title="Admin - Static Methods" %>

    <asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:HiddenField id="hidEarlyDescriptor" runat="server" value="Masq_080802_EarlyShow" />
    <asp:HiddenField id="hidLateDescriptor" runat="server" value="Masq_080802_LateShow" />    
    <asp:HiddenField id="hidRefundServiceFees" runat="server" value="true" />
    
    <asp:Button ID="btnTest" Enabled="true" Height="24px" runat="server" visible="false"
                    Text="Test" OnClick="btnTest_Click" />
    
    <table border="1" cellspacing="3" cellpadding="3">
        <tr>
            <th>
                Rebuild Show Thumbs
            </th>
            <td>
                <asp:Button ID="Button3" Enabled="true" Height="24px" runat="server" Text="Rebuild Show Thumbs"
                    OnClick="btnRebuildShowThumbs_Click" />
            </td>
            <td>
                <asp:Literal ID="Literal4" runat="server" />
            </td>
        </tr>
        <tr>
            <th>
                Send test email
            </th>
            <td>
                <asp:Button ID="Button2" Enabled="true" Height="24px" runat="server" Text="Send Test Mail"
                    OnClick="btnSendTestMail_Click" />
            </td>
            <td>
                <asp:Literal ID="Literal3" runat="server" />
            </td>
        </tr>
        
        <tr>
            <th>
                Init facebbok values
            </th>
            <td>
                <asp:Button ID="btnFB" Enabled="true" Height="24px" runat="server" Text="Init FB"
                    OnClick="btnFB_Click" />
            </td>
            <td>
                <asp:Literal ID="Literal2" runat="server" />
            </td>
        </tr>
        <tr>
            <th>
                Record the images to the past shows
            </th>
            <td>
                <asp:Button ID="btnPast" Enabled="true" Height="24px" runat="server" Text="Record the images to the past shows"
                    OnClick="btnPast_Click" OnClientClick="return confirm('Are you sure?');" />
            </td>
            <td>
                <asp:Literal ID="Literal1" runat="server" />
            </td>
        </tr>
       
        <tr>
            <th>Clean Up an act</th>
            <td>
                <asp:TextBox ID="txtAct" runat="server" />
                <asp:Button ID="btnCleanAct" Enabled="false" Height="24px" runat="server" 
                    Text="Clean Act Image" OnClick="btnCleanAct_Click" 
                    OnClientClick="return confirm('Are you sure?');" />
            </td>
            <td><asp:Literal ID="litCleanAct" runat="server" /></td>
        </tr>
        <tr>
            <th>Check Pass</th>
            <td>
                <asp:Button ID="Button1" Enabled="false" Height="24px" runat="server" Text="Check Pass" OnClick="btnPass_Click" OnClientClick="return confirm('Are you sure?');" />
            </td>
            <td><asp:Literal ID="pass" runat="server" /></td>
        </tr>
        
        <tr>
            <th>Rename Shows</th>
            <td>
                <asp:Button ID="btnShowName" Enabled="false" Height="24px" runat="server" Text="Redo Show Name" OnClick="btnRename_Click" OnClientClick="return confirm('Are you sure?');" />
            </td>
            <td>&nbsp;</td>
        </tr>
       
        
    </table>
    
</asp:Content>