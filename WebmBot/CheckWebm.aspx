<%@ Page Title="Проверка файлов" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CheckWebm.aspx.cs" Inherits="WebmBot.WebForm1" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server" style="min-height:600px;align-content:center;text-align:center">

<style>
        
        body{
            min-height:800px;
            width:100vh;
            min-width:1000px
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
    <asp:UpdatePanel runat="server" ID="UP"><ContentTemplate>
        <div style="min-height:600px;align-content:center;text-align:center;position:absolute; width:100%">
        <div runat="server" id="WebmContent" style="display:flex; justify-content:center">
        <asp:Table ID="tbl_fundtype" runat="server" style="width:180vh"  />
        </div>
        <br />
        <div style="margin-bottom:50px;">
        <asp:LoginView runat="server" ID="LOGIN">
            <RoleGroups>
                <asp:RoleGroup Roles="Admin">
                    <ContentTemplate>
                        <asp:Button runat="server" ID="ConfrimBtn" Text="Подтвердить перенос" OnClick="ConfrimBtn_Click" class="btn btn-primary"  />
                        <asp:Button runat="server" ID="ReRead" Text="Перечитать пак" OnClick="ReRead_Click" class="btn btn-primary"  />
                        <asp:Button runat="server" ID="Remove" Text="Удалить" OnClick="Remove_Click" class="btn btn-primary" />
                        <asp:Button runat="server" ID="RemoveAll" Text="Удалить всё" OnClick="RemoveAll_Click" class="btn btn-primary" />
                    </ContentTemplate>
                </asp:RoleGroup>
            </RoleGroups>
        <LoggedInTemplate>

        </LoggedInTemplate>
       </asp:LoginView>
       </div>

    </div>

        </ContentTemplate></asp:UpdatePanel>
</asp:Content>
