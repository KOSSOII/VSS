<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Zal.aspx.cs" Inherits="WebmBot.WebForm9" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        #my-video-stream::cue {
    background-color: transparent;
    color: #FFF;
    font-size: 32px;
    font-family: "Lobster";
}
    </style>
    <div style="    display: flex;
    justify-content: center;
    margin-bottom: 150px;"> 
   <video id="my-video-stream" controls width="1280" height="720" preload="metadata">
   <source src="/ZF/Jojo.mp4" type="video/mp4">
   <track label="English" kind="subtitles" srclang="en" src="/ZF/subs/Jojo_ENG_Subtitles02.vtt">
   <track label="Русский" kind="subtitles" srclang="ru" src="/ZF/subs/Jojo_RUS_Subtitles01.vtt" default>
       <track label="Русский от Кары" kind="subtitles" srclang="ru" src="/ZF/subs/kara.vtt" default>
</video>
        </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LoginContent" runat="server">
</asp:Content>
