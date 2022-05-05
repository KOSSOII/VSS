using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebmBot
{
    
    public partial class WebForm11 : System.Web.UI.Page
    {
        string URLString = "http://gelbooru.com/index.php?page=dapi&s=post&q=index&limit=0&tags=";
        public static string[] urlmass = new string[100];
        public static int gc = 1;
        protected void Page_Load(object sender, EventArgs e)
        {


        }
        protected void GetB_Click(object sender, EventArgs e)
        {
            URLString = URLString + TagsFU.Text;
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(URLString);
            XmlNodeList elemList = xDoc.GetElementsByTagName("posts");
            string count = "";
            for (int i = 0; i < elemList.Count; i++)
            {
                count = elemList[i].Attributes["count"].Value;
            }
            if(Convert.ToInt32(count)> 20000)
            {
                count = "20000";
            }
            for (int ii = 0; ii < 10; ii++)
            {
                Random rnd = new Random();
                int rndget = rnd.Next(0, Convert.ToInt32(count));
                string borugetrandurl = "http://gelbooru.com/index.php?page=dapi&s=post&q=index&limit=1&tags=" + TagsFU.Text + "&pid=" + rndget;
                xDoc.Load(borugetrandurl);
                elemList = xDoc.GetElementsByTagName("post");
                string imageurl = "";
                for (int i = 0; i < elemList.Count; i++)
                {
                    imageurl = elemList[i].Attributes["file_url"].Value;    
                }
                urlmass[ii] = imageurl.Replace("https","http");

            }
            ImagePW.ImageUrl = urlmass[0];
            TextL.Text = "Картинка 1 из 10";



        }
        protected void ImagePWB_Click(object sender, EventArgs e)
        {
            ImagePW.ImageUrl = urlmass[gc];
            if (gc < 10) { 
                gc++;
            }
            else {
                gc = 0;
                TextL.Text = "Картинка 1 из 10";
            }
            TextL.Text = "Картинка " + gc+ " из 10";


        }
    }
}
