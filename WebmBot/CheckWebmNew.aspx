<%@ Page Title="Checkfile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CheckWebmNew.aspx.cs" Inherits="WebmBot.WebForm5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<asp:UpdatePanel runat="server" ID="UP">
            <ContentTemplate>
                <div id="WebmConten" runat="server" style="margin-top: 10px; display: flex;
    justify-content: center;margin-bottom: 15px">
                </div>
                <div style="display: flex; justify-content: center">
                   
                </div>
                <asp:LoginView runat="server" ID="LV">
                    <RoleGroups><asp:RoleGroup Roles="Admin">
                        <ContentTemplate>
                                                    <div class="clear" style="display: flex; justify-content: center; margin-top: 50px;">
                            <div class="clear" style="border: 1px solid black; padding: 5px 5px 5px 5px">
                                File ID: 
                                <asp:Label runat="server" ID="WebmIdLable" Text="InitText"></asp:Label><br />
                                Username or Id: 
                                <asp:Label runat="server" ID="Username" Text="InitText"></asp:Label><br />
                            </div>
                            <asp:Button runat="server" ID="SkipReport" Text="Accept File" OnClick="SkipReport_Click" AutoPostBack="false" Style="margin-right: 10px; margin-left: 10px" class="btn btn-primary" />
                            <asp:Button runat="server" ID="ToFapButton" Text="Send To FAP" OnClick="ToFapButton_Click" AutoPostBack="false" Style="margin-right: 10px; margin-left: 10px" class="btn btn-primary" />
                            <asp:Button runat="server" ID="RemoveFromBaseButton" Text="Remove FROM Base" OnClick="RemoveFromBaseButton_Click" AutoPostBack="false" Style="margin-right: 10px; margin-left: 10px" class="btn btn-primary" />
                                                    </div>
                            <div id="tt" runat="server" style="display: flex;
    justify-content: center;">
                                <asp:TextBox CssClass="form-control" style="margin-top:35px; max-width:50%; width:50%" id="tags" placeholder="TAGS" runat="server"></asp:TextBox>

                            </div>
                                                        <br />
                            <div style="height:100vh"></div>
                        </ContentTemplate>
                                </asp:RoleGroup>
                    </RoleGroups>
                    <LoggedInTemplate>

                    </LoggedInTemplate>
                </asp:LoginView>
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
                TAGS
                <asp:HiddenField runat="server" ID="WebmID" />
                <asp:HiddenField runat="server" ID="ReportId" />
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
