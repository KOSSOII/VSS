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
using Newtonsoft.Json;
using WebmBot.Models;
namespace WebmBot.Controllers
{


    public class WebmAPIController : ApiController
    {
        DataSet DS = new DataSet();
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebmDB"].ConnectionString);
        [HttpGet]
        public HttpResponseMessage GetRandomVideo()
        {
 
            DS.Clear();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM PackTable WHERE Loc='Pack' AND TAG<>'FAP' AND TAG<>'REMOVED'", conn);
            adapter.Fill(DS, "Webm");
            Random rnd = new Random();
            int rndIndex = rnd.Next(0, DS.Tables["Webm"].Rows.Count);
            string url = "https://webm.kansan.ga/" + DS.Tables["Webm"].Rows[rndIndex]["Path"].ToString().Replace("H:\\", "").Replace("\\", "/");
            url = Uri.EscapeUriString(url).ToString();
            //.Replace(System.IO.Path.GetFileName(DS.Tables["Webm"].Rows[rndIndex]["Path"].ToString()), "")+ System.Web.HttpUtility.UrlEncode(System.IO.Path.GetFileName(DS.Tables["Webm"].Rows[rndIndex]["Path"].ToString()).ToString()); 
            XmlDocument vid = new XmlDocument();
            XmlDeclaration xmlDeclaration = vid.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = vid.DocumentElement;
            vid.InsertBefore(xmlDeclaration, root);
            XmlElement XMLURL = vid.CreateElement(string.Empty, "URL", string.Empty);
            XmlText XmlURLText = vid.CreateTextNode(url);
            XMLURL.AppendChild(XmlURLText);
            vid.AppendChild(XMLURL);
            return new HttpResponseMessage() { Content = new StringContent(vid.OuterXml, Encoding.UTF8, "application/xml") }; 

        }

        public HttpResponseMessage GetRandomVideoJson()
        {

            DS.Clear();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM PackTable WHERE Loc='Pack' AND TAG<>'FAP' AND TAG<>'REMOVED'", conn);
            adapter.Fill(DS, "Webm");
            Random rnd = new Random();
            int rndIndex = rnd.Next(0, DS.Tables["Webm"].Rows.Count);
            string url = "https://webm.kansan.ga/" + DS.Tables["Webm"].Rows[rndIndex]["Path"].ToString().Replace("H:\\", "").Replace("\\", "/");
            string[] vid = new string[2];
            vid[0] = url;
            vid[1] = "№"+rndIndex;
            url = Uri.EscapeUriString(url).ToString();
            var json = JsonConvert.SerializeObject(vid);
            return new HttpResponseMessage() { Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json") };

        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] Status model)
        {
            string AuthToken = model.authToken;
            string Temperature = model.Temperature;
            string Hydro = model.Hydro;
            string Server_dor = model.Server_dor;
            string BPR_dor = model.BPR_dor;
            string Server_sclad_dor = model.Server_sclad_dor;
            string Sclad_dor = model.Sclad_dor;
            string Server_sclad_dor_two = model.Server_sclad_dor_two;
            string BPR_dor_cf = model.BPR_dor_cf;
            string Server_sclad_dor2_two = model.Server_sclad_dor2_two;

            return new HttpResponseMessage() { Content = new StringContent($"Data Read: Token is: {AuthToken}. Data:\"Temperature:{Temperature}, Hydro:{Hydro}", Encoding.UTF8, "application/json") }; ;
        }


        public HttpResponseMessage GetRandomVideoJsonTags(string tags)
        {

            DS.Clear();

            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM PackTable WHERE Loc='Pack' AND TAG<>'FAP' AND TAG<>'REMOVED' AND VUTAG LIKE @TAGS", conn);
            adapter.SelectCommand.Parameters.AddWithValue("TAGS", "%" + tags + "%");
            adapter.Fill(DS, "Webm");
            Random rnd = new Random();
            int rndIndex = rnd.Next(0, DS.Tables["Webm"].Rows.Count);
            string url = "https://webm.kansan.ga/" + DS.Tables["Webm"].Rows[rndIndex]["Path"].ToString().Replace("H:\\", "").Replace("\\", "/");
            string[] vid = new string[3];
            vid[0] = url;
            vid[1] = "№" + rndIndex;
            vid[2] = "TAGS:" + DS.Tables["Webm"].Rows[rndIndex]["VUTAG"].ToString();
            url = Uri.EscapeUriString(url).ToString();
            var json = JsonConvert.SerializeObject(vid);
            return new HttpResponseMessage() { Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json") };

        }
    }


}
