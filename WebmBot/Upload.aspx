<%@ Page Title="Загрузчик" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="WebmBot._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div style="height:100vh;">
    <div style="height:320px;width:550px;position:absolute;float:left; bottom:-50px; left:25px; z-index:-1; display:block">
    <label style="float:right;bottom:185px; left:185px; position:absolute">Ты сдохнешь от <font color="red">рака</font><br /> если будешь заливать плохие шебмки.</label>
    <img src="Img/Shi.png" style="max-width:100%;max-height:100%;float:left"/>
        </div>
    <div style="align-content:center;text-align:center;width:100%;height:85vh">
        <style>
            .FU
            {

            }
        </style>
        <br />
        <p>Загрузка файлов в бота.</p>
        <br />
        <p>Файл не должен иметь размер больше 8mb и за один раз нельзя загружать больше 10 файлов!</p>
        <br />
        <div runat="server" id="LogDiv"></div>
            <asp:Label runat="server" ID="StatusLabel" Text="" />
        <br />
            <asp:FileUpload ID="FileUploadControl" Multiple="Multiple" AllowMultiple="true" runat="server" CssClass="FU"  style="padding-bottom:20px; padding-top:20px" />
        <br />
         <p>Если хотите чтобы бот меншонил вас когда залитая вами вебм будет запощена напишите в поле ниже свой ник в дискорде а лучще Id и тогда он всегда сможет вас найти. Или оставьте пустым чтобы остаться анонимом.</p>
            <asp:TextBox ID="Nickname" runat="server" placeholder="Nickname или 262684444815523842" style="width:100%; margin-bottom:20px;margin-top:10px;" class="form-control"></asp:TextBox>
        <br />
        <br />
            <asp:Button runat="server" ID="UploadButton" Text="Upload" OnClick="UploadButton_Click" class="btn btn-primary" />
        <br />
            <div runat="server" id="Statistic" style="padding-top:250px"></div>
    </div>
    </div>
</asp:Content>
