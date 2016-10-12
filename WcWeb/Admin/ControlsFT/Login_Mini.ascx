<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Login_Mini.ascx.cs" EnableViewState="false" Inherits="wctMain.Admin.ControlsFT.Login_Mini" %>
<div id="loginsmall">
    <%if (this.Page.ToString().ToLower() != "asp.register_aspx")
      {%>
      <div class="logcontainer">
        <div class="logininfo">
              <%if (this.Page.User.Identity.IsAuthenticated)
              {%>
               <div class="title"><%=this.Page.User.Identity.Name %></div>
               <%} %>               
           </div>
           <div class="functions">
           <%if (this.Page.User.Identity.IsAuthenticated)
          {%>
               <div ><a href="/Admin/ControlsFT/EditProfile.aspx" id="linkEdit" name="linkEdit">edit profile</a></div>
               <div ><asp:LinkButton ID="linkLogout" runat="server" Text="logout"  CausesValidation="false"
                       onclick="linkLogout_Click" /></div>
          <%}else{%>
            <div><asp:LinkButton ID="linkLogin" CausesValidation="false" runat="server" onclick="linkRegister_Click" CssClass="loginsmall"><span><b>login</b></span></asp:LinkButton></div>
            <div><a href="/Admin/ControlsFT/PasswordRecovery.aspx" id="linkRecover" name="linkRecover">forgot password</a></div>
            <div><asp:LinkButton ID="linkCreate" CausesValidation="false" runat="server" Text="create account" onclick="linkRegister_Click" /></div>
         <%}%>
         </div>
     </div>
     <% }%>
</div>