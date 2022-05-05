<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm6.aspx.cs" Inherits="WebmBot.WebForm6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat ="server" ID="UP2"><ContentTemplate>
    <asp:Button runat="server" ID="SkipReport" Text="Accept File" OnClick="SkipReport_Click" AutoPostBack="false" Style="margin-right: 10px; margin-left: 10px" class="btn btn-primary" />
        </ContentTemplate></asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LoginContent" runat="server">
</asp:Content>
