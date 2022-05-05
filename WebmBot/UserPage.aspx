<%@ Page Title="User Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserPage.aspx.cs" Inherits="WebmBot.WebForm7" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <div style="display:flex;justify-content:center;height:100vh">
        <asp:LoginView ID="LV2" runat="server">
                    <AnonymousTemplate>
                        <a class="btn btn-primary" style="    height: 35px;
    margin-top: 120px;" href="/loginn">Войти в аккаун</a>
                    </AnonymousTemplate>
        </asp:LoginView>
    <div style="display: inline-grid;height: 415px;width: 310px;flex: 1;">
        <figure id="Figure" runat="server" style="z-index: 1;top: 8px;border-radius: 300px;width: 300px;height: 300px;margin: 0;padding: 0; border: 2px solid; border-color:blue; overflow:hidden; margin-right: 10px;">
            <asp:Image ID="Avatar" style="height:100%;width:100%;object-fit:cover;" runat="server"/>
       </figure>              
                <asp:LoginView runat="server" ID="LoginIn">
            <LoggedInTemplate>
             <br />
        <asp:FileUpload ID="AvatarFileLoad" accept="image/*" multiple="false" runat="server" />
        <br />
        <asp:Button ID="LoadAvatarBtn" runat="server" OnClick="LoadAvatarBtn_Click" Text="Загрузить" CssClass="btn btn-primary" />
        <br />
                <div style="width:300px;    display: inline-grid;">
                    <br />Для того чтобы пользоваться всеми функциями сайта необходимо пройти верификацию.<br />
                   Чтобы пройти верификацию станьте участником сервера(Указан в вкладке <a href="/Contact">Связь</a>)<br />
                    <br />И дайте доступк к вашей информации(Другие пользователи не будут видеть вашу верефикацию по умолчанию статус скрыт)<br />
                 <%--  <a class="btn btn-primary" style="height: 35px;margin-top: 10px;" href="https://discordapp.com/api/oauth2/authorize?client_id=503556379810856961&redirect_uri=https%3A%2F%2Fwebm.kansan.ga%2Fapi%2FDiscordAUNT%2FVG%2F&response_type=code&scope=identify%20guilds">Верефикация</a>--%>
                </div>
            </LoggedInTemplate>
        </asp:LoginView>
        </div>
        
        </div>
</asp:Content>
