using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web;
using System.Web.UI;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http.Formatting;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Microsoft.AspNet.Identity;
using System.Web.Security;

namespace WebmBot.Controllers
{

    public class DiscordAUNTController : ApiController
    {
        DataSet DS = new DataSet();
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebmDB"].ConnectionString);

        public class GuildInfo
        {
            public bool owner { get; set; }
            public string permissions { get; set; }
            public string icon { get; set; }
            public ulong id { get; set; }
            public string name { get; set; }
        }
        public class DiscordUser
        {
            public string username { get; set; }
            public string locale { get; set; }
            public bool mfa_enabled { get; set; }
            public string flags { get; set; }
            public string avatar { get; set; }
            public string discriminator { get; set; }
            public ulong id { get; set; }
        }
        [HttpGet]
        public HttpResponseMessage VG([FromUri] string code)
        {
            string client_id = "503556379810856961";
            string client_sceret = "AUuZwOOv-svELYg3OO10GGsshP4yIJL0";
            string redirect_url = "https://webm.kansan.ga/api/DiscordAUNT/VG/";
            /*Get Access Token from authorization code by making http post request*/

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://discordapp.com/api/oauth2/token");
            webRequest.Method = "POST";
            string parameters = "client_id=" + client_id + "&client_secret=" + client_sceret + "&grant_type=authorization_code&code=" + code + "&redirect_uri=" + redirect_url + "";
            byte[] byteArray = Encoding.UTF8.GetBytes(parameters);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = byteArray.Length;
            Stream postStream = webRequest.GetRequestStream();
            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Close();
            WebResponse response = webRequest.GetResponse();
            postStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(postStream);
            string responseFromServer = reader.ReadToEnd();

            string tokenInfo = responseFromServer.Split(',')[0].Split(':')[1];
            string access_token = tokenInfo.Trim().Substring(1, tokenInfo.Length - 3);

            HttpWebRequest webRequest1 = (HttpWebRequest)WebRequest.Create("https://discordapp.com/api/users/@me");
            webRequest1.Method = "Get";
            webRequest1.ContentLength = 0;
            webRequest1.Headers.Add("Authorization", "Bearer " + access_token);
            webRequest1.ContentType = "application/x-www-form-urlencoded";

            string apiResponse1 = "";
            using (HttpWebResponse response1 = webRequest1.GetResponse() as HttpWebResponse)
            {
                StreamReader reader1 = new StreamReader(response1.GetResponseStream());
                apiResponse1 = reader1.ReadToEnd();
            }

            HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create("https://discordapp.com/api/users/@me/guilds");
            webRequest2.Method = "Get";
            webRequest2.ContentLength = 0;
            webRequest2.Headers.Add("Authorization", "Bearer " + access_token);
            webRequest2.ContentType = "application/x-www-form-urlencoded";

            string apiResponse2 = "";
            using (HttpWebResponse response2 = webRequest2.GetResponse() as HttpWebResponse)
            {
                StreamReader reader2 = new StreamReader(response2.GetResponseStream());
                apiResponse2 = reader2.ReadToEnd();
            }

            DiscordUser VUser = JsonConvert.DeserializeObject<DiscordUser>(apiResponse1);
            GuildInfo[] VUserGuilds = JsonConvert.DeserializeObject<GuildInfo[]>(apiResponse2);
            bool inguld = false;
            ulong BotGuild = 505892998400180234;
            foreach (GuildInfo guild in VUserGuilds)
            {
                if(guild.id==BotGuild)
                {
                    inguld = true;
                }
            }
            try
            {
                string siteUsername = User.Identity.Name.ToLower();
                SqlCommand cmd = new SqlCommand($@"UPDATE SDiscord SET [UserName]='{siteUsername}', [DiscordUserName]=@UDName, [DiscordUserID]=@DID, [InGuld]='{inguld.ToString()}',[RawUI]=@RUI, [RawGI]=@RGI WHERE [UserName]='{siteUsername}' OR [DiscordUserID]=@DID IF @@ROWCOUNT = 0 INSERT INTO SDiscord ([UserName],[DiscordUserName],[DiscordUserID],[InGuld],[RawUI],[RawGI]) VALUES ('{siteUsername}',@UDName,@DID,'{inguld.ToString()}',@RUI,@RGI)", conn);
                cmd.Parameters.Add(new SqlParameter("UDName", VUser.username));
                cmd.Parameters.Add(new SqlParameter("DID", VUser.id.ToString()));
                cmd.Parameters.Add(new SqlParameter("RUI", apiResponse1));
                cmd.Parameters.Add(new SqlParameter("RGI", apiResponse2));

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch(Exception ex)
            {
               // return ex.Message;
            }

            Roles.AddUserToRole(User.Identity.Name, "Trusted User");

            var responseRedirect = Request.CreateResponse(HttpStatusCode.Moved);
            responseRedirect.Headers.Location = new Uri("https://webm.kansan.ga/UserPage");
            return responseRedirect;

        }

    }
}
