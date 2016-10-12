<%@ Control Language="C#" AutoEventWireup="false" CodeFile="FaqView.ascx.cs" Inherits="wctMain.View.FaqView" %>
<div class="displayer fade fade-slow faq-container static-container">
    <div class="section-header">FAQ</div>
    <form id="faqform" action="faq" runat="server" class="main-inner">
        <asp:ListView ID="ListView1" runat="server" DataKeyNames="Id" ItemPlaceholderID="ListViewContent"             
            ondatabinding="ListView1_DataBinding" 
            OnItemDataBound="ListView1_ItemDataBound" EnableViewState="false"
                >
            <EmptyDataTemplate>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <asp:Repeater ID="rptTabNav" runat="server" EnableViewState="false" OnItemDataBound="rptTabNav_ItemDataBound"><HeaderTemplate>
                <ul id="faqtabnav" class="nav nav-pills nav-justified"></HeaderTemplate>
                    <ItemTemplate>
                    <asp:Literal ID="litItem" runat="server" EnableViewState="false" /></ItemTemplate>
                    <FooterTemplate>
                </ul></FooterTemplate></asp:Repeater>

                <div class="accordion-container tab-content">
                    <asp:Panel ID="ListViewContent" runat="server" />
                </div>
            </LayoutTemplate>
            <ItemTemplate>
                <!-- create a tab pane for viewing active content -->
                <asp:Literal ID="litTabPaneStart" runat="server" EnableViewState="false" />
                    <asp:Repeater ID="rptFaqs" runat="server" EnableViewState="false" OnDataBinding="rptFaqs_DataBinding" OnItemDataBound="rptFaqs_ItemDataBound">
                        <HeaderTemplate>
                            <asp:Literal ID="litHeader" runat="server" EnableViewState="false" />                            
                        </HeaderTemplate>
                        <ItemTemplate>
                                <div class="panel panel-default faq-item">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <asp:Literal ID="litItemToggle" runat="server" EnableViewState="false" />                                             
                                                <%#Eval("Question") %>
                                            </a>
                                        </h4>
                                    </div>
                                    <asp:Literal ID="litItemContainer" runat="server" EnableViewState="false" />
                                        <div class="panel-body">
                                            <%#Eval("AnswerWithMungedMailto") %>
                                        </div>
                                    </div>
                                </div><!-- end of panel faq item -->
                        </ItemTemplate>
                        <FooterTemplate>
                            </div><!-- end of panel-group -->
                        </FooterTemplate>
                    </asp:Repeater>
                </div><!-- end tab pane start -->
            </ItemTemplate>
        </asp:ListView>
    </form>
</div> 


<script type="text/javascript">

    $(document).ready(function () {

        $('#faqtabnav LI A').click(function () {
            //this function lives in the index.js
            anchorClientLink(this);
        });

        registerMailto();
    });

</script>
