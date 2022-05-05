<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="loginn.aspx.cs" Inherits="WebmBot.WebForm2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server" >
    <style>
                
        body{
            width:100%;
            margin:0px 0px 0px 0px;
            align-content:center;
            align-items:center;
            align-self:center;
            text-align:center;
        }
#tbl_fundtype {
    font-family: "Trebuchet MS", Arial, Helvetica, sans-serif;
    border-collapse: collapse;
    width: 100%;
}

td, th {
    border: 1px solid #ddd;
    padding: 8px;
}

tr:nth-child(even){background-color: #414a4c;}


th {
    padding-top: 12px;
    padding-bottom: 12px;
    text-align: left;
    background-color: #4CAF50;
    color: white;
}
    </style>

    <div style="display:flex; justify-content:center; width:100%;     height: 100vh; margin-top:200px">
    <asp:Login ID="Login" runat="server" TextBoxStyle-CssClass="form-control" LoginButtonStyle-CssClass="btn btn-primary"></asp:Login>
        </div>
</asp:Content>
