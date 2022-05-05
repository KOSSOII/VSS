<%@ Page Title="Catalog" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Catalog.aspx.cs" Inherits="WebmBot.WebForm10" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        p{
            cursor: pointer;
        }
        .PW{
            margin-bottom:25px;
            margin-left:25px;
            margin-right:25px;
            width: 333px;
            max-width:340px;
                      display: flex;
          flex-flow: column nowrap;
          align-items: center;
        }
        h5{
            font-size: 14px;
    text-align: center;
        margin-bottom: 1px;
        }
        p
        {
                border: 1px solid #cccccc2e!important;
                    border-radius: 16px;
                    box-sizing: inherit;
                    background-color: darkslategrey;
                    margin: 5px;
                    padding: 2px;
                padding-left: 15px;
            padding-right: 15px;
                height: 24px;
        }
        p:hover { 
  background-color: #586dbad1;
}
    </style>
    <script>
        $(document).on('show.bs.modal', '#VideoModal', function (event) {
            //alert(f);
            var PW = $(event.relatedTarget) 
            var VideoLink = PW.data('whatever') 
            var VideoType = PW.data('vtype')
            var VideoTags = PW.data('tags')
            $("#WebmConten").find("#WebmPlayer").attr("src", VideoLink)
            $("#WebmConten").find("#WebmPlayer").attr("type", VideoType)
            //var modal = $(this)
            //modal.find('.modal-title').text('New message to ' + recipient)
            //modal.find('.modal-body input').val(recipient)
            var Tagarray = VideoTags.split(',');
            console.log(Tagarray);
            var formatedTags = "";
            for (var i = 0; i < Tagarray.length; i++)
            {
                if (Tagarray[i].trim() != "" || Tagarray[i].trim() != " ") {
                    formatedTags += "<p onclick=\"document.getElementById('Searcher').value='" + Tagarray[i].trim() + "';$('#Search').trigger('click');\">" + Tagarray[i] + "</p>";
                }
            }
            $(formatedTags).appendTo('#tags');
            $("#WebmPlayer")[0].play();

        });
        $(document).on('hide.bs.modal', '#VideoModal', function (event) {
            //alert(f);
            $("#WebmPlayer")[0].pause();
            jQuery('#tags').html('');
        });
    </script>
    <div id="msercher" runat="server" style="margin-top: 35px;
    display: flex;
    justify-content: center;">
    <asp:TextBox ID="Searcher" runat="server" style="width: 90%;max-width:90%;" Visible="true" ClientIDMode="Static" CssClass="form-control" placeholder="TAGS"></asp:TextBox>
       
        <asp:Button ID="Search" runat="server" Text="Search" class="btn btn-primary" ClientIDMode="Static" OnClick="Search_click" />
        </div>
    <div id="MainCatalog" style="display: flex;
    justify-content: center;" runat="server"></div>
    <div id="VideoModal" class="modal fade" role="dialog">
        <div class="modal-dialog" style="    display: grid;
    justify-content: center;">
            <div id="AllModal" style="    background-color: #2f4f4f6b;
    width: 100%;
    height: auto;
        display: inherit;

    justify-content: center;">
            <div id="WebmConten">
                <video id = "WebmPlayer" loop controls onloadstart="this.volume = volume" width="auto" height="auto" style="margin:10px;max-width:1280px;"><source src = "/webm/testo.webm" type = "video/webm"/></video>
            </div>
            <div id="tags" style="display:flex">

            </div>
                </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LoginContent" runat="server">
</asp:Content>
