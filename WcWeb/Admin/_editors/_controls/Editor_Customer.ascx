<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Editor_Customer.ascx.cs" Inherits="wctMain.Admin._editors._controls.Editor_Customer" %>

<div id="customereditor" class="show-mgmt">
    <asp:HiddenField ID="hdnCollectionTableName" runat="server" ClientIDMode="Static" Value="<%# Wcss.AspnetUser.Schema.TableName %>" />
    <div class="panel-spacer"></div>
    <div class="panel-body">
        <table  border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr class="cc-pager-row">
                    <td class="cc-principal">  
                        <div class="form-group">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    Email
                                </span>
                                <asp:TextBox ID="txtEmail" TabIndex="1" MaxLength="256" Width="300px" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    Last Name
                                </span>
                                <asp:TextBox ID="txtLastName" TabIndex="2" MaxLength="256" Width="300px" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    Birthday Month
                                </span>
                                <asp:DropDownList ID="ddlBdMonth" TabIndex="5" Width="300px" runat="server" CssClass="form-control">
                                    <asp:ListItem Selected="True" Text="-- Please Select a Month --" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="January" Value="1" />
                                    <asp:ListItem Text="February" Value="2" />
                                    <asp:ListItem Text="March" Value="3" />
                                    <asp:ListItem Text="April" Value="4" />
                                    <asp:ListItem Text="May" Value="5" />
                                    <asp:ListItem Text="June" Value="6" />
                                    <asp:ListItem Text="July" Value="7" />
                                    <asp:ListItem Text="August" Value="8" />
                                    <asp:ListItem Text="September" Value="9" />
                                    <asp:ListItem Text="October" Value="10" />
                                    <asp:ListItem Text="November" Value="11" />
                                    <asp:ListItem Text="December" Value="12" />
                                </asp:DropDownList>
                            </div>
                        </div>  
                    </td>
                </tr>
                <tr>
                    <td class="cc-status">
                        <br />
                        <asp:LinkButton ID="btnSearch" CssClass="btn btn-primary" CommandName="Search" runat="server" Text="Search"
                            OnClick="btnSearch_Click" CausesValidation="false" />                                
                        <%if(Page.User.IsInRole("Super") || Page.User.IsInRole("Administrator")){ %>
                        <asp:LinkButton ID="btnAdmins" CssClass="btn btn-primary btn-tab-command-left" runat="server" Text="Admin List" OnClick="btnAdmins_Click" CausesValidation="false" />
                        <%} %>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="panel-footer">
        <asp:Repeater ID="rptResults" runat="server" OnItemDataBound="rptResults_ItemDataBound" OnItemCreated="rptResults_ItemCreated" >
            <HeaderTemplate>
            <asp:Label ID="lblCriteria" CssClass="customer-criteria" runat="server"></asp:Label>            
            <ul class="list-unstyled"></HeaderTemplate>
            <ItemTemplate>
                <li class="vdquery-row vdquery-item form-inline vdquery-row-select"><asp:Literal ID="litItem" runat="server" /></li>
            </ItemTemplate>
            <FooterTemplate>
            </ul>
        </FooterTemplate>
            </asp:Repeater>
    </div>
</div>