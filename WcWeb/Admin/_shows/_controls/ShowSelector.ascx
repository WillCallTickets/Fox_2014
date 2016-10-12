<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ShowSelector.ascx.cs" Inherits="wctMain.Admin._shows._controls.ShowSelector" %>
<%@ Register assembly="WctControls" namespace="WctControls.WebControls" tagprefix="cc1" %>

<div id="showselector">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="width:60%;">
                <asp:DropDownList ID="ddlShow" runat="server" AutoPostBack="True" EnableViewState="true" 
                    DataSourceID="SqlShowList" DataTextField="ShowName" DataValueField="Id" 
                    OnSelectedIndexChanged="ddlShow_SelectedIndexChanged" 
                    OnDataBound="ddlShow_DataBound" CssClass="form-control" />
            </td>
            <td style="width:20%;">
                <%//FormatString='<%# WctControls.WebControls.Bootstrap.DateTimePicker.Date_FormatString %>
                <cc1:BootstrapDateTimePicker ID="txtShowListStartDate" Label="Start" Date='<%#Atx.CurrentShowListStartDate %>' 
                    FormatString='<%# WctControls.WebControls.Bootstrap.DateTimePicker.Date_FormatString %>'
                    DateCompareEmpty="min" CssClass="showselector-dtpicker" runat="server" OnTextChanged="txtShowListStartDate_TextChanged" AutoPostBack="true" />
                    
            </td>
            <td style="width:20%;" class="sel-typeahead">
                <asp:TextBox ID="search_show" runat="server" ClientIDMode="Static" CssClass="form-control typeahead" Width="100%" AutoCompleteType="None" />
            </td>
        </tr>
        <tr>
            <td>
                <span class="btn-group btn-group-justified btn-bold">
                    <asp:Repeater ID="rptContext" runat="server" OnDataBinding="rptContext_DataBinding" OnItemDataBound="rptContext_ItemDataBound" 
                        OnItemCommand="rptContext_ItemCommand">
                        <ItemTemplate><asp:LinkButton ID="btnContext" runat="server" CausesValidation="false" CommandName="" Text="" CssClass="" /></ItemTemplate>
                    </asp:Repeater>
                </span>
            </td>
            <td colspan="2">
                <span class="btn-group btn-group-justified btn-bold btn-group-principal" >
                    <asp:Repeater ID="rptPrincipal" runat="server" 
                        OnDataBinding="rptPrincipal_DataBinding"
                        OnItemCommand="rptPrincipal_ItemCommand">
                        <ItemTemplate>
                            <asp:LinkButton ID="linkNav" runat="server" CommandName="select" CausesValidation="false" OnClientClick="return true;" 
                                Text="<%#Container.DataItem.ToString() %>" CommandArgument="<%#Container.DataItem.ToString() %>"
                                cssClass='<%# (Container.DataItem.ToString() == Atx.CurrentEditPrincipal.ToString()) ? "btn btn-primary" : "btn btn-default" %>'
                                    />
                        </ItemTemplate>
                    </asp:Repeater>
                </span>
            </td>
        </tr>
    </table>
</div>
<asp:SqlDataSource ID="SqlShowList" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"    
    SelectCommand=" " 
        EnableViewState="True" onselecting="SqlShowList_Selecting">
    <SelectParameters>
        <asp:Parameter Name="startDate" DbType="DateTime" />
        <asp:Parameter Name="Principal" DbType="String" />
    </SelectParameters>
</asp:SqlDataSource>