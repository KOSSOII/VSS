<%@ Page Title="WebmPlay" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebmBot.WebForm3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.5.0/css/all.css" integrity="sha384-B4dIYHKNBt8Bc12p+WXckhzcICo0wtJAoU8YZTY5qE0Id1GSseTk6S+L3BlXeVIU" crossorigin="anonymous">
    <link href="Content/Custom.css" rel="stylesheet" />
    <div style="display: block; justify-content: center; display:block" class="layout">
                        <script> 
                </script>
        <div id ="telo" style="flex:1">
            <script>
                    var togleStatus = 0;

                    function togleComments(elements,mod) {
                        if (mod==1) {
  elements = elements.length ? elements : [elements];
  for (var index = 0; index < elements.length; index++) {
    elements[index].style.display = 'none';
  }
                            togleStatus = 0;
                            
                        }
                        else {1
       
                              elements = elements.length ? elements : [elements];
  for (var index = 0; index < elements.length; index++) {
    elements[index].style.display = 'block';
  }
                            togleStatus = 1;
                            
                        }

                    }
                </script>
        <asp:UpdatePanel runat="server" ID="UP" UpdateMode="Conditional" ChildrenAsTriggers="false">
            <ContentTemplate>
                <div style="display: block; justify-content: center; display: inline-flex;width: 100%;">
                <div id="WebmConten" runat="server" style="display:contents; width:100%">
                </div>
                    </div>
                <div style="display: flex; justify-content: center">
                    <asp:Button runat="server" ID="rand" Text="Random" OnClick="rand_Click" class="btn btn-primary" usesubmitbehavior="false"/>                  
                </div>
                 <div style="display:flex; margin-top:15px; justify-content: center; color:#b1c1c1">
                  <asp:Label runat="server" id="WebmIdFoUser" Text=""></asp:Label>
                </div>
                
                 <div style="display:flex; margin-bottom:15px; justify-content: center; color:#b1c1c1">
                 <asp:Label runat="server" id="LoaderName" Text=""></asp:Label>    
                 </div>
                      <asp:UpdatePanel runat="server" ID="LikePanel"><ContentTemplate>
                     <div style="display: flex; justify-content: center; color:#303a3a">
                     <asp:Button runat="server" ID="LikeButton" Text="Нравится" ValidationGroup="LikeValidation" OnClick="LikeButton_Click" AutoPostBack="false" style="margin:10px; background-color: #5a7d44; min-width:120px" class="btn btn-primary" usesubmitbehavior="false"/>
                     <asp:Button runat="server" ID="DislikeButton" Text="Не нравится" ValidationGroup="LikeValidation" OnClick="DislikeButton_Click" AutoPostBack="false" style="margin:10px;background-color: #a41637; min-width:120px" class="btn btn-primary" usesubmitbehavior="false"/>
                     </div>
                    </ContentTemplate>
                      </asp:UpdatePanel>
                 <div id="tagdiv" style="margin-top: 25px;display:flex;justify-content:center;" runat="server"></div>
                <asp:LoginView runat="server" ID="LV" OnViewChanged="LV_ViewChanged">
                    <LoggedInTemplate>
                        
                    </LoggedInTemplate>
                    <RoleGroups>
                        <asp:RoleGroup Roles="Tagger">
                            <ContentTemplate>
                                
                                </ContentTemplate>
                        </asp:RoleGroup>
                        <asp:RoleGroup Roles="Admin">
                            <ContentTemplate>
                                <div class="clear" style="display: flex; justify-content: center; margin-top: 50px;">
                                <div class="clear" style="border: 1px solid black; padding: 5px 5px 5px 5px">
                                FileID: 
                                <asp:Label runat="server" ID="WebmIdLable" Text="InitText"></asp:Label><br />
                                Название файла: 
                                <asp:Label runat="server" ID="WebmName" Text="InitText"></asp:Label><br />
                            </div>
                            <asp:Button runat="server" ID="ToFapButton" Text="Move To FAP" OnClick="ToFapButton_Click" AutoPostBack="false" Style="margin-right: 10px; margin-left: 10px" class="btn btn-primary" usesubmitbehavior="false" />
                            <asp:Button runat="server" ID="RemoveFromBaseButton" Text="Remove FROM Base" OnClick="RemoveFromBaseButton_Click" AutoPostBack="false" Style="margin-right: 10px; margin-left: 10px" class="btn btn-primary" usesubmitbehavior="false" />
                        </div>
                            </ContentTemplate>
                        </asp:RoleGroup>
                    </RoleGroups>
                </asp:LoginView>
                 <div style="display:none; justify-content: center; color:#303a3a">
                 <label id="ty" style="color:green;font-size:24px;" ></label>
                 </div>
                <div runat="server" id="Tagger" visible="false"  style="display:flex;justify-content:center;margin-top:50px;">
                                       <asp:Button runat="server" ID="NextNotAG" Text="Следующая без тэга." OnClick="RNDNOTAG_Click" AutoPostBack="false" style="margin:10px; background-color: #5a7d44; min-width:120px" class="btn btn-primary" usesubmitbehavior="false"/>
                                     <asp:RequiredFieldValidator runat="server" ValidationGroup='TagValidation' style="color:red;font-size:24px;" ControlToValidate="TagsAddBox" ErrorMessage="А Тэги?!"> А Тэги?!</asp:RequiredFieldValidator>
                                    <asp:TextBox runat="server" ID="TagsAddBox" placeholder="Anime,Monogatari,Shinobu Oshino Без пробела после запятой." style="margin-right: 25px;width:430px;max-width:430px;"  autocomplete="off" class="form-control"></asp:TextBox>
                                    <asp:Button runat="server" ID="TagWebmB" Text="Tag Video"  ValidationGroup="TagValidation" AutoPostBack="false" OnClick="TagWebmB_Click" class="btn btn-primary" usesubmitbehavior="false" />
                                </div>
                <script>
                    document.addEventListener("DOMContentLoaded", function (event) {

                        function setCookie(name, value, options) {
                            options = options || {};

                            var expires = options.expires;

                            if (typeof expires == "number" && expires) {
                                var d = new Date();
                                d.setTime(d.getTime() + expires * 1000);
                                expires = options.expires = d;
                            }
                            if (expires && expires.toUTCString) {
                                options.expires = expires.toUTCString();
                            }

                            value = encodeURIComponent(value);

                            var updatedCookie = name + "=" + value;

                            for (var propName in options) {
                                updatedCookie += "; " + propName;
                                var propValue = options[propName];
                                if (propValue !== true) {
                                    updatedCookie += "=" + propValue;
                                }
                            }

                            document.cookie = updatedCookie;
                        }

                        var vid = document.getElementById("WebmPlayer");
                        vid.onvolumechange = function () {
                            setCookie("WebmVolumeValue", vid.volume)

                        };
                    })
                    var prm = Sys.WebForms.PageRequestManager.getInstance();
                    prm.add_endRequest(function () {
                        function setCookie(name, value, options) {
                            options = options || {};

                            var expires = options.expires;

                            if (typeof expires == "number" && expires) {
                                var d = new Date();
                                d.setTime(d.getTime() + expires * 1000);
                                expires = options.expires = d;
                            }
                            if (expires && expires.toUTCString) {
                                options.expires = expires.toUTCString();
                            }

                            value = encodeURIComponent(value);

                            var updatedCookie = name + "=" + value;

                            for (var propName in options) {
                                updatedCookie += "; " + propName;
                                var propValue = options[propName];
                                if (propValue !== true) {
                                    updatedCookie += "=" + propValue;
                                }
                            }

                            document.cookie = updatedCookie;
                        }
                        var vid = document.getElementById("WebmPlayer");
                        vid.onvolumechange = function () {
                            setCookie("WebmVolumeValue", vid.volume)
                        };
                    });

                </script>
                <asp:HiddenField runat="server" ID="WebmID" />
                <div style="width:100%; justify-content:center; display:flex; margin-top:20px;">
                <input type="button" id="CommentsTogle" runat="server" class="btn btn-primary" onclick="togleComments(document.getElementById('CommentsHolder'),togleStatus);" CausesValidation="false" value="Комментарии" />
                </div>
                <div id="CommentsHolder" style="display:none">
                <div id="CommentsAll" style="width:100%;max-height:500px;display:grid;justify-content:center; margin-top:50px;">
                    <div id="Comments" runat="server" style="margin-left:auto;margin-right:auto;max-width:50%;min-width:500px;margin-top:10px; max-height:500px; overflow:auto">
                    </div>
                </div>
                 <div style="display:flex; margin-top:15px">
                 <asp:TextBox runat="server" ID="CommentText" TextMode="MultiLine" Rows="10" MaxLength="2" Wrap="true" placeholder="Комментарий" Style="max-width: 100%;margin-left: auto; width:20%;height: 80px;margin-top: 10px;" class="form-control" onkeypress="return this.value.length<=400"></asp:TextBox>
                 <asp:Button runat="server" ID="SendComment" OnClick="SendComment_Click" class="btn btn-primary" Text="Отправить" style="margin-right:auto;height:40px;margin-left:15px; margin-top:auto;margin-bottom:auto" usesubmitbehavior="false"/>
                 </div>
                </div>
             </ContentTemplate>
        </asp:UpdatePanel>
            </div>
       <div id="ReportDiv" style="width: min-content; width: -moz-min-content; position:relative; float: left; bottom: 0px; left: 0px; z-index: 5; display: block">
        <label style="float: right; bottom: -230px; left: 55px; position: relative">Эти видео вы загрузили <font color="red">сами</font>
            <br />
            если там что-то неприличное
            <br />
            отправь репорт за плюшки.</label>
             <div style="float: right; bottom: 0px; left: 265px; position: relative;">
            <div style="margin-bottom:10px;display: flex; justify-content: center; position: relative; bottom: -360px;">
                <asp:RequiredFieldValidator runat="server" ValidationGroup='ReportValidation' ControlToValidate="ReportText" ErrorMessage="Напиши текст!."> Напиши текст!</asp:RequiredFieldValidator>
            </div>
            <div style="display:flex; bottom:-360px; left:-60px;position:relative;z-index:3">
                <div style="margin-right:10px">
                    <asp:Button runat="server" ID="SendReport" Text="Send Report" OnClick="SendReport_Click" ValidationGroup="ReportValidation" AutoPostBack="false" class="btn btn-primary" usesubmitbehavior="false" />
                </div>                 
            <div>
              <asp:TextBox runat="server" autocomplete="off" ID="ReportText" class="form-control"></asp:TextBox>
             </div>
            </div>
        </div>
         <div style="float:left;left:-15px;position:relative;">
        <img src="Img/Kana.png"/>
        </div>
    </div>
       <div id ="ChatDiv" style="width:20%; position:fixed; bottom:30px; right:0px; display:inline-block;float:right; z-index:1; background-color:#1a2121; border-radius:25px">                             
      <div class="floating-chat enter" id="ChatButton" style="background-color:#786e81; bottom: 35px;" onclick="TogleChat()">
    <i class="fa fa-comments" aria-hidden="true" style=""></i></div>
  <div id="ChatBox" class="chat" style="resize:inherit">
  <div class="chat-title">
    <h1><input type="text" id="user" class="SetName" placeholder="Name" autocomplete="off" readonly="readonly" style=" font-size: large;" maxlength="16" size="16"/></h1>
    <h2 id="ChatConnectionState"></h2>
    <script>
        $userSet = document.getElementById('user')
        $userSet.value = GetedUserName;
    </script>
     <figure class="avatar">
     <img src="#" runat="server" style="height:100%;width:100%;object-fit:cover;" ID="ChatAva"/></figure>
  </div>
  <div id ="MessagesChat" class="messages" style="overflow:auto">
    <div class="messages-content"></div>
  </div>
  <div class="message-box">
      <textarea  id="message" class="message-input" placeholder="Type message..." onkeypress="handleKeyPress(event)" onkeyup="textAreaAdjust(this)" maxlength="255" style="font-size: 14px;height:17px;"  rows="20" cols="9" wrap="soft"></textarea>
      <asp:button runat="server" Text="Post video" OnClientClick="PostVideo(); return false;"  class="message-submit" usesubmitbehavior="false" />
    <%--<button type="submit" placeholder="this text will show in the textarea"> class="message-submit">Send</button>--%>
  </div>

</div>
        <script type="text/javascript">

            function textAreaAdjust(o) {
                var key = o.keyCode || o.which;
                if (key == 13 || key == 10) { o.preventDefault(); return false; }
                o.style.height = "17px";
                o.style.height = (1 + o.scrollHeight) + "px";
            }

            function TogleChat() {
                var chatdiv = document.getElementById('ChatBox');
                var cnopka = document.getElementById('ChatButton');
                if (chatdiv.style.display != 'none') {
                    chatdiv.style.display = 'none';
                    cnopka.style.backgroundColor = '#786e81';
                }
                else {
                    chatdiv.style.display = 'flex';
                    cnopka.style.backgroundColor = '#786e81';
                }
                
            }
            


            function arrayRemove(arr, value) {

   return arr.filter(function(ele){
       return ele != value;
   });

}
        var socket,
            $txt = document.getElementById('message'),
            $user = document.getElementById('user'),
            $messages = document.getElementById('MessagesChat');

        if (typeof (WebSocket) !== 'undefined') {
            socket = new WebSocket("wss://webm.kansan.ga/ChatHandler.ashx");
        } else {
            socket = new MozWebSocket("wss://webm.kansan.ga/ChatHandler.ashx");
            }
            var Reconecter;
            var constate = document.getElementById("ChatConnectionState"); 
            function connectionChek() {
                if (socket.readyState === 1) {
                    clearInterval(Reconecter);
                    constate.textContent = "Подключено"
                }
                else {
                                        constate.textContent = "Нет связи."
                }
            }
            setInterval(connectionChek, 10000);
            
            var delayPost = false;

            function SetDelayFNC() {
                delayPost = false;
            }
            function PostVideo() {
                
                    if ($user.value.trim() == "" || $user.value.trim().length == 0 || $user.value == null) {
                        alert("Поля не заполнены!")
                    }
                    else {
                        if (delayPost === false) {
                            var videoDivIn = document.getElementById('WebmPlayer');
                            var videoDiv = videoDivIn.cloneNode(true);
                            videoDiv.style.width = '250px';
                            videoDiv.style.height = '160px';
                            videoDiv.autoplay = false;
                            var wrap = document.createElement('div');
                            wrap.appendChild(videoDiv.cloneNode(true));
                            if (sessionStorage.getItem('status') != null) {
                                socket.send($user.value + ' ╫ ' + ' LOGINED ' + ' ╫ ' + ' <SSenTHTMLVIDE0> ' + ' ╫ ' + UserAvatar + ' ╫ ' + wrap.innerHTML + ' ╫ ' + $txt.value);
                            }
                            else {
                                socket.send($user.value + ' ╫ ' + ' ANON ' + ' ╫ ' + ' <SSenTHTMLVIDE0> ' + ' ╫ ' + UserAvatar + ' ╫ ' + wrap.innerHTML + ' ╫ ' + $txt.value);
                            }
                            $txt.value = '';
                            delayPost = true;
                            setTimeout(SetDelayFNC, 10000);
                        }
                        else {
                            alert("Не так быстро братишка! Откат.");
                        }
                    }
            }


            function parseMessages(msg) {
                
                var chatdiv = document.getElementById('ChatBox');
                 var cnopka = document.getElementById('ChatButton');
                if (chatdiv.style.display == 'none') {
                   cnopka.style.backgroundColor ='#1738dc';
                }
            var re = /\s*╫\s*/;
                var DataArray = msg.data.split(re);
                if (DataArray[2] == '<SSenTHTMLVIDE0>')
                {

                if (DataArray[1]=='LOGINED')
                {
                var $el0 = document.createElement('p3');
                var $el = document.createElement('div');
                $el.className="message new";
                    $el.innerHTML = "<figure class=\"avatar\"><img src=\"/Img/Avatars/" + DataArray[3]+"\" /></figure>";
                    $el0.textContent = DataArray[0];
                                    DataArray[0] += ":";
                    var result = arrayRemove(DataArray, DataArray[0]);
                    var result2 = arrayRemove(result, result[0]);
                    var result3 = arrayRemove(result2, result2[0]);
                    var result4 = arrayRemove(result3, result3[0]);
                    var htmlstring = result4[0];
                    var result5 = arrayRemove(result4, result4[0]);
                    var text = result5.join();
                    var $el2 = document.createElement('p');
                    $el2.textContent = text;

                    $videodiv = document.createElement('div');
                    $videodiv.className = "ChatVideo";
                    $videodiv.innerHTML = htmlstring;
                    $el.appendChild($el0);
                    $el.appendChild($videodiv);
                    $el.appendChild($el2);
                    $messages.appendChild($el);
                    var objDiv = document.getElementById("MessagesChat");
                    objDiv.scrollTop = objDiv.scrollHeight;
                    return;


            }
            else
            {
                                var $el0 = document.createElement('pu');
                var $el = document.createElement('div');
                $el.className="message new";
                    $el.innerHTML = "<figure class=\"avatar\"><img src=\"/Img/Avatars/" + DataArray[3] + "\" /></figure>";
                    $el0.textContent = DataArray[0];
                                    DataArray[0] += ":";
                    var result = arrayRemove(DataArray, DataArray[0]);
                    var result2 = arrayRemove(result, result[0]);
                    var result3 = arrayRemove(result2, result2[0]);
                    var result4 = arrayRemove(result3, result3[0]);
                    var htmlstring = result4[0];
                    var result5 = arrayRemove(result4, result4[0]);
                    var text = result5.join();
                    var $el2 = document.createElement('p');
                    $el2.textContent = text;

                    $videodiv = document.createElement('div');
                    $videodiv.className = "ChatVideo";
                    $videodiv.innerHTML = htmlstring;
                    $el.appendChild($el0);
                    $el.appendChild($videodiv);
                    $el.appendChild($el2);
                    $messages.appendChild($el);
                    var objDiv = document.getElementById("MessagesChat");
                    objDiv.scrollTop = objDiv.scrollHeight;
                    return;
                    }



                }
                
          
            if (DataArray[1]==='LOGINED')
            {
                var $el0 = document.createElement('p3');
                var $el = document.createElement('div');
                $el.className="message new";
                $el.innerHTML = "<figure class=\"avatar\"><img src=\"/Img/Avatars/" + DataArray[2] + "\" /></figure>";

            }
            else
            {
                var $el0 = document.createElement('pu');
                var $el = document.createElement('div');
                $el.className="message new";
                $el.innerHTML = "<figure class=\"avatar\"><img src=\"/Img/Avatars/" + DataArray[2] + "\" /></figure>";
            }
                
                $el0.textContent = DataArray[0];
                
                var $el2 = document.createElement('p');
                DataArray[0] += ":";
                var result = arrayRemove(DataArray, DataArray[0]);
                var result2 = arrayRemove(result, result[0]);
                var result3 = arrayRemove(result2, result2[0]);
                $el2.textContent = result3.join();
                $el.appendChild($el0);
            $el.appendChild($el2);
            $messages.appendChild($el);
            //console.log(DataArray);
            var objDiv = document.getElementById("MessagesChat");
            objDiv.scrollTop = objDiv.scrollHeight;
            }


            socket.onmessage = function (msg) { 
                if (msg.data != "") {
                    parseMessages(msg);
                }
            };

        socket.onclose = function (event) {
            Reconecter = setInterval(function () { start("wss://webm.kansan.ga/ChatHandler.ashx");constate.textContent="Нет связи. Переподключаемся..." }, 5000);
        };
            function handleKeyPress(e){
 var key=e.keyCode || e.which;
                if (key == 13 || key == 10) {

                    e.preventDefault();
                    if ($user.value.trim() == "" || $user.value.trim().length == 0 || $user.value == null || $txt.value.trim() == "" || $txt.value.trim().length == 0 || $txt.value == null)
                    {
                        alert("Поля не заполнены!")
                    }
                    else
                    {
                        if (sessionStorage.getItem('status') != null)
                        {
                            socket.send($user.value + ' ╫ ' + ' LOGINED ' + ' ╫ ' + UserAvatar +' ╫ ' + $txt.value);
                        }
                        else
                        {
                            socket.send($user.value + ' ╫ ' + ' ANON ' + ' ╫ ' + UserAvatar +' ╫ ' + $txt.value);
                        }
                    }
      
      $txt.value = '';
       var objDiv = document.getElementById("MessagesChat");
            objDiv.scrollTop = objDiv.scrollHeight;
                    return false;
                }
                
}

        </script>
         </div>
    </div>   
</asp:Content>
