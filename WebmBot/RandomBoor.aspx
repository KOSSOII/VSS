<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RandomBoor.aspx.cs" Inherits="WebmBot.WebForm11" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UP" runat="server">
        <ContentTemplate>
    
        </ContentTemplate>
    </asp:UpdatePanel>
    <center>
    <div style="display:inline-grid;">
    <asp:TextBox runat="server" ID="TagsFU" placeholder="Тэги с Gelbooru.com" class="form-control"></asp:TextBox>
    <asp:Button runat="server" ID="GetB" Text="Получить 10 картинок" class="btn btn-primary" OnClick="GetB_Click" />
    <asp:Button runat="server" ID="ImagePWB" Text="Следующая картинка" class="btn btn-primary" OnClick="ImagePWB_Click" />
    <asp:Label ID="TextL" runat="server"></asp:Label>
    <asp:Image ID="ImagePW" runat="server" style="height:720px" />
        </div>
        </center>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LoginContent" runat="server">
    
</asp:Content>
