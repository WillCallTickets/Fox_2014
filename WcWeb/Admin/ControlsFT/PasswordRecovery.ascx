<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PasswordRecovery.ascx.cs" Inherits="wctMain.Admin.ControlsFT.PasswordRecovery" %>

<div id="recoverpass">
    <div class="section-header">Recover your password</div>
    <div class="recovery main-inner">
        <div class="validationsection">
            <asp:ValidationSummary runat="server" ID="valSummary" ValidationGroup="PasswordRecovery1" CssClass="validationsummary"
                HeaderText="Please correct the following errors:" ShowSummary="true" />
        </div>

        <asp:PasswordRecovery ID="PasswordRecovery1" runat="server"
            OnSendingMail="PasswordRecovery1_SendingMail" OnSendMailError="PasswordRecovery1_SendMailError"
            OnVerifyingUser="PasswordRecovery1_VerifyUser">
            <QuestionTemplate>
                
                <table border="0" cellspacing="0" cellpadding="4">
                    <tr>
                        <td colspan="3">
                            <h1><asp:Literal ID="UserName" runat="server"></asp:Literal></h1>
                        </td>
                    </tr>
                    <tr>
                        <th>Your Security Question</th>
                        <td colspan="2">
                            <asp:Literal ID="Question" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="AnswerLabel" runat="server" AssociatedControlID="Answer">Please Provide Your Answer</asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="Answer" runat="server" Width="250px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="AnswerRequired" runat="server"
                                ControlToValidate="Answer" ErrorMessage="Answer is required."
                                ToolTip="Answer is required." ValidationGroup="ctl00$PasswordRecovery1">*</asp:RequiredFieldValidator>
                        </td>
                        <td colspan="2">
                            <asp:LinkButton ID="SubmitButton" runat="server" CommandName="Submit"
                                ValidationGroup="PasswordRecovery1" CssClass="btn btn-lg btn-primary">Submit</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <span class="label label-danger"><asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal></span>
                        </td>
                    </tr>
                </table>
            </QuestionTemplate>
            <SuccessTextStyle Font-Bold="true" ForeColor="Green" />
            <MailDefinition BodyFileName="" />
            <UserNameTemplate>                
                <table border="0" cellpadding="4" cellspacing="0">
                    <tr>
                        <th>
                            <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Your Email Address:</asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="UserName" runat="server" Width="250px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server"
                                ControlToValidate="UserName" ErrorMessage="Email address is required."
                                ToolTip="Email address is required." ValidationGroup="ctl00$PasswordRecovery1">*</asp:RequiredFieldValidator>
                        </td>
                        <td colspan="2">
                            <asp:LinkButton ID="SubmitButton" runat="server" CommandName="Submit"
                                ValidationGroup="PasswordRecovery1" CssClass="btn btn-lg btn-primary">Submit</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <span class="label label-danger"><asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal></span>
                        </td>
                    </tr>
                </table>
            </UserNameTemplate>
        </asp:PasswordRecovery>
    </div>
</div>
