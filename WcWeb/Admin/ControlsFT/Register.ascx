<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Register.ascx.cs" Inherits="wctMain.Admin.ControlsFT.Register" EnableViewState="false" ViewStateMode="Disabled" %>

<%@ Register Src="UserProf.ascx" TagName="UserProfile" TagPrefix="wc" %>

<div id="register">    
    <%if (CreateUserWizard1.ActiveStep.ID == "createuser")
      {%>
    <asp:Panel ID="existing" runat="server">
        <div class="section-header">Existing Accounts</div>
        <div class="registration main-inner">
            <asp:Login ID="Login1" runat="server" VisibleWhenLoggedIn="false"
                OnLoggedIn="Login1_LoggedIn" OnLoggingIn="Login1_LoggingIn"
                OnPreRender="Login1_PreRender" OnLoginError="Login1_LoginError">
                <LayoutTemplate>
                    <table border="0" cellpadding="4" cellspacing="0" width="100%">
                        <tr>
                            <th>
                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server"
                                ControlToValidate="UserName" ErrorMessage="Email address is required."
                                ToolTip="Email address is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                Email Address
                            </th>
                            <td >
                                <asp:TextBox ID="UserName" runat="server" TabIndex="1" MaxLength="256" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server"
                                    ControlToValidate="Password" ErrorMessage="Password is required."
                                    ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                Password
                            </th>
                            <td>
                                <asp:TextBox ID="Password" runat="server" TabIndex="2" TextMode="Password" Width="300px"></asp:TextBox>

                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2" >
                                <span class="label label-danger"><asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal></span>
                            </td>
                        </tr>
                    </table>

                    <div class="interface-control">
                        <asp:LinkButton ID="Submit" runat="server" TabIndex="3" CommandName="Login" CssClass="btn btn-lg btn-primary" meta:resourcekey="SubmitResource1">Login</asp:LinkButton>
                        <a href="/Admin/ControlsFT/PasswordRecovery.aspx" class="btn btn-danger">forgot password?</a>
                        <asp:HyperLink ID="linkManageEmail" runat="server" NavigateUrl="/MailerManage.aspx" Text="manage email?" CssClass="btn btn-warning" />
                        <asp:LinkButton ID="linkLogout" runat="server" Text="logout" CausesValidation="false" CssClass="btn btn-success" OnClick="linkLogout_Click" />
                    </div>
                </LayoutTemplate>
            </asp:Login>
        </div>
    </asp:Panel>
    <%} %>

    <asp:Literal ID="updateProfile" runat="server" EnableViewState="False"></asp:Literal>
             
    <asp:ValidationSummary ValidationGroup="CreateUserWizard1" ID="ValidationSummary1" CssClass="validationsummary" runat="server" 
        ShowMessageBox="True" ShowSummary="False" />
                 
    <asp:CreateUserWizard runat="server" ID="CreateUserWizard1" AutoGeneratePassword="False" HyperLinkStyle-HorizontalAlign="Left" 
        ContinueDestinationPageUrl="/Default.aspx" FinishDestinationPageUrl="/Default.aspx" 
        OnFinishButtonClick="CreateUserWizard1_FinishButtonClick" OnCreatedUser="CreateUserWizard1_CreatedUser" 
        DuplicateUserNameErrorMessage="The e-mail address that you entered is already in use. If you have an existing account, please login at the top of the page." 
        CreateUserButtonType="Link" CreateUserButtonStyle-CssClass="btn btn-lg btn-primary btn-create" OnCreatingUser="CreateUserWizard1_CreatingUser" 
        OnSendMailError="CreateUserWizard1_SendMailError" OnSendingMail="CreateUserWizard1_SendingMail"
        FinishCompleteButtonType="Link" FinishCompleteButtonStyle-CssClass="btn btn-lg btn-primary btn-create" >

        <HyperLinkStyle HorizontalAlign="Left"></HyperLinkStyle>
        <WizardSteps>
            <asp:CreateUserWizardStep ID="createuser" runat="server">
                <ContentTemplate>
                    <div class="section-header">New Accounts</div>
                    <div class="registration main-inner">
                        <table border="0" cellpadding="4" cellspacing="0">
                            <tr>
                                <th>
                              <asp:RequiredFieldValidator ID="valRequireUserName" CssClass="validator" runat="server" ControlToValidate="UserName"
                                  SetFocusOnError="true" Display="Static" ErrorMessage="Email address is required." ToolTip="Email address is required."
                                  ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator runat="server" ID="valEmailPattern" CssClass="validator" Display="Dynamic"
                                        SetFocusOnError="true" ValidationGroup="CreateUserWizard1" ControlToValidate="UserName"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="The email address you specified is invalid.">*</asp:RegularExpressionValidator>
                                    Email Address
                                </th>
                                <td>
                                    <asp:TextBox TabIndex="4" runat="server" ID="UserName" Width="300px" /></td>
                                <td rowspan="99" valign="top">
                                    <div class="cookiewarning">
                                        <div class="title">Problems logging in?</div>
                                        <div class="warning">
                                            Please be sure you have javascript and cookies enabled on your browser. Also ensure you have the most up-to-date version of your browser. 
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                 <asp:RequiredFieldValidator ID="valRequireEmail" CssClass="validator" runat="server" ControlToValidate="Email" SetFocusOnError="true" Display="Static"
                                     ErrorMessage="Confirmation of email address is required." ToolTip="Email address is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareEmail" CssClass="validator" runat="server" ControlToCompare="UserName" ControlToValidate="Email"
                                        SetFocusOnError="true" Display="dynamic" ErrorMessage="Confirm Email address does not match Email address"
                                        ToolTip="Confirm Email address does not match Email address" ValidationGroup="CreateUserWizard1">*</asp:CompareValidator>
                                    Confirm Email
                                </th>
                                <td>
                                    <asp:TextBox TabIndex="5" runat="server" ID="Email" Width="300px" Text='<%# Email %>' /></td>
                            </tr>
                            <tr>
                                <th>
                                 <asp:RequiredFieldValidator ID="valRequirePassword" CssClass="validator" runat="server" ControlToValidate="Password" SetFocusOnError="true"
                                     Display="Static" ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="valPasswordLength" CssClass="validator" runat="server" ControlToValidate="Password"
                                        SetFocusOnError="true" Display="Static"
                                        ValidationExpression="[\@\*\-_\w!]{5,20}"
                                        ErrorMessage="* Valid password chars are letters (A-Z, a-z), numbers (0-9), exclamation point (!), underscore(_), at symbol(@) and asterick (*). Passwords must be at least 5 chars and cannot exceed 20 chars."
                                        ToolTip="* Valid password chars are letters (A-Z, a-z), numbers (0-9), exclamation point (!), underscore(_), at symbol(@) and asterick (*). Passwords must be at least 5 chars and cannot exceed 20 chars."
                                        ValidationGroup="CreateUserWizard1">*</asp:RegularExpressionValidator>
                                    Password
                                </th>
                                <td>
                                    <asp:TextBox runat="server" TabIndex="6" ID="Password" TextMode="Password" Width="300px" /></td>
                            </tr>
                            <tr>
                                <th>
                                 <asp:RequiredFieldValidator ID="valRequireConfirmPassword" CssClass="validator" runat="server" ControlToValidate="ConfirmPassword"
                                     SetFocusOnError="true" Display="Static" ErrorMessage="Confirm Password is required." ToolTip="Confirm Password is required."
                                     ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="valComparePasswords" CssClass="validator" runat="server" ControlToCompare="Password" SetFocusOnError="true"
                                        ControlToValidate="ConfirmPassword" Display="Static" ErrorMessage="The Password and Confirmation Password must match."
                                        ValidationGroup="CreateUserWizard1">*</asp:CompareValidator>
                                    Confirm Password
                                </th>
                                <td>
                                    <asp:TextBox TabIndex="7" runat="server" ID="ConfirmPassword" TextMode="Password" Width="300px" /></td>
                            </tr>

                            <tr>
                                <th>
                                <asp:CompareValidator ID="reqReqQuestion" runat="server" ControlToValidate="Question" Operator="GreaterThan"
                                    ValidationGroup="CreateUserWizard1" ValueToCompare="0" ErrorMessage="Please select a security question.">*</asp:CompareValidator>
                                    Security Question
                                </th>
                                <td>
                                    <asp:DropDownList ID="Question" runat="server" TabIndex="8" AppendDataBoundItems="true" Width="300px">
                                        <asp:ListItem Text="-- Select Security Question --" />
                                    </asp:DropDownList>
                                </td>

                            </tr>
                            <tr>
                                <th>
                                 <asp:RequiredFieldValidator ID="valRequireAnswer" CssClass="validator" runat="server" ControlToValidate="Answer"
                                     SetFocusOnError="true" Display="Static" ErrorMessage="Security answer is required." ToolTip="Security answer is required."
                                     ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                    Security Answer
                                </th>
                                <td class="input">
                                    <asp:TextBox TabIndex="9" runat="server" ID="Answer" MaxLength="200" Width="300px" /></td>
                            </tr>
                        </table>
                        <div class="update" style="padding: 8px 8px 16px 8px;">
                            <asp:Label ID="ErrorMessage" SkinID="FeedbackKO" CssClass="label label-danger" runat="server"
                                EnableViewState="False" Font-Bold="True" Font-Size="14px"></asp:Label>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:CreateUserWizardStep>
            <asp:WizardStep ID="profile" runat="server" Title="Set preferences">
                <div class="section-header">Set-up your profile</div>
                <div class="registration main-inner">
                    <wc:UserProfile ID="UserProfile1" runat="server" />
                </div>
            </asp:WizardStep>
            <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
                <CustomNavigationTemplate>
                    ---
                </CustomNavigationTemplate>
            </asp:CompleteWizardStep>
        </WizardSteps>
        <MailDefinition BodyFileName="" />
    </asp:CreateUserWizard>
</div>
