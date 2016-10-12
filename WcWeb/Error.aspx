<%@ Page Language="C#" MasterPageFile="/View/Masters/Master_Main.master" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="wctMain.Error" Title="Error" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="errorpage-container">
        <br />
        <div class="sectiontitle">Unexpected error occurred!</div>
        <br />
        <p></p>
        <asp:Label Visible="false" runat="server" CssClass="alert alert-danger" ID="lbl404" Text="The requested page or resource was not found." />
        <asp:Label Visible="false" runat="server" CssClass="alert alert-danger" ID="lbl408" Text="The request timed out. This may be caused by a too high traffic. Please try again later." />
        <asp:Label Visible="false" runat="server" CssClass="alert alert-danger" ID="lbl505" Text="The server encountered an unexpected condition which prevented it from fulfilling the request. Please try again later." />
        <asp:Label runat="server" ID="lblError" Visible="false" CssClass="alert alert-danger"
            Text="There was some problems processing your request. An e-mail with details about this error has been sent to the administrator." />
        <br />
        <br />
        <p></p>
        If you would like to contact the webmaster to report the problem with more details, 
	please use the
        <asp:HyperLink runat="server" ID="lnkContact" Text="Contact Us" CssClass="link-foxt" NavigateUrl="/Contact.aspx" />
        page.
	<br />
        <br />
    </div>
</asp:Content> 

<asp:Content ID="Content1" ContentPlaceHolderID="WidgetContent" runat="server">
    
</asp:Content>

