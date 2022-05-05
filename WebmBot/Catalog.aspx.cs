using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Configuration;
using System.Web.Services;

namespace WebmBot
{
    public partial class WebForm10 : System.Web.UI.Page
    {
        static int CurrentWebmId = 0;
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebmDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CatalogLoad();
            }
        }
        protected void CatalogLoad()
        {
            if (Page.User.IsInRole("Catalog") || Page.User.IsInRole("Admin"))
            {
                DataSet DSA = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter($"SELECT * FROM PackTable WHERE Loc='Pack' AND TAG<>'FAP' AND TAG<>'REMOVED'", conn);
                DSA.Clear();
                da.Fill(DSA, "Pack");
                string catalogHTML = "<table>";
                for (int i = 0; i < DSA.Tables["Pack"].Rows.Count - 3501;)
                {
                    catalogHTML += "<tr>";
                    for (int j = 0; j <= 3; j++)
                    {
                        if (DSA.Tables["Pack"].Rows.Count > 0 && i <= DSA.Tables["Pack"].Rows.Count - 1)
                        {
                            catalogHTML += $"<td><div class=\"PW\"><h5>({DSA.Tables["Pack"].Rows[i]["TimeDur"].ToString()},{Convert.ToInt32(DSA.Tables["Pack"].Rows[i]["FileSize"].ToString()) / 1024}KB)</h5><img data-toggle=\"modal\" onclick=\"\" data-target=\"#VideoModal\" data-tags=\" {DSA.Tables["Pack"].Rows[i]["VUTAG"].ToString()} \" data-whatever=\"{ DSA.Tables["Pack"].Rows[i]["Path"].ToString().Replace("H:\\", "").Replace("\\", "/")} \" data-vtype=\"video/{Path.GetExtension(DSA.Tables["Pack"].Rows[0]["Path"].ToString())}\"style=\"height:140px;border-width:1px;border-style: dashed; cursor: pointer; \" src=\"/{DSA.Tables["Pack"].Rows[i]["PWpath"].ToString().Replace("H:\\", "").Replace("WebmBotSite\\WebmBot\\", "").Replace("\\", "/")}\" alt=\"Webm\"></div></td>";
                            i++;
                        }
                    }
                    catalogHTML += "</tr>";
                }
                catalogHTML += "</table>";


                HttpCookie myCookie = Request.Cookies["WebmVolumeValue"];
                string volume = "1";
                if (myCookie != null)
                {
                    volume = myCookie.Value;
                }
                else
                {
                    myCookie = new HttpCookie("WebmVolumeValue");
                    volume = "1";
                    // Set the cookie value.
                    myCookie.Value = volume;
                    // Set the cookie expiration date.
                    myCookie.Expires = DateTime.Now.AddYears(50); // For a cookie to effectively never expire
                                                                  // Add the cookie.
                    Response.Cookies.Add(myCookie);
                }
                if (Page.User.IsInRole("Catalog") || Page.User.IsInRole("Admin"))
                    MainCatalog.InnerHtml = catalogHTML;
            }
            else
            {
                MainCatalog.InnerHtml= "<div style=\"\"><h2 style=\"position:absolute; left:10px;bottom:500px;\">Эй долбоёб!</h2></br><img style=\"position:absolute; left:165px; z-index:-1; bottom:-85px; width:200px; heinght:500px\" src=\"Img/Naga.png\"/> <h2 style=\"position:absolute;left:355px;bottom:330px;\">Пошел нахуй!</h2></div><div style=\"justify-content: center; top:30%; position:absolute\"><h3 style=\"\"><font size=\"25\">401.7 – Access denied! Тебе сюда нельзя!</font></h3></div>";
                msercher.InnerHtml = "";
            }
        }
        protected void Search_click(object sender, EventArgs e)
            {
            if (Page.User.IsInRole("Catalog") || Page.User.IsInRole("Admin")) { 
                if (string.IsNullOrEmpty(Searcher.Text))
                {
                    CatalogLoad();
                    return;
                }
                DataSet DSA = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter($"SELECT * FROM PackTable WHERE Loc='Pack' AND TAG<>'FAP' AND TAG<>'REMOVED' AND VUTAG like @VTAG", conn);
                da.SelectCommand.Parameters.AddWithValue("VTAG", "%" + Searcher.Text + "%");
                DSA.Clear();
                da.Fill(DSA, "Pack");
                string catalogHTML = "<table>";
                for (int i = 0; i <= DSA.Tables["Pack"].Rows.Count - 1;)
                {
                    catalogHTML += "<tr>";
                    for (int j = 0; j <= 3; j++)
                    {
                        if (DSA.Tables["Pack"].Rows.Count > 0 && i <= DSA.Tables["Pack"].Rows.Count - 1)
                        {
                            catalogHTML += $"<td><div class=\"PW\"><h5>({DSA.Tables["Pack"].Rows[i]["TimeDur"].ToString()},{DSA.Tables["Pack"].Rows[i]["FileSize"].ToString()}KB)</h5><img data-toggle=\"modal\" onclick=\"\" data-target=\"#VideoModal\" data-tags=\" {DSA.Tables["Pack"].Rows[i]["VUTAG"].ToString()} \" data-whatever=\"{ DSA.Tables["Pack"].Rows[i]["Path"].ToString().Replace("H:\\", "").Replace("\\", "/")} \" data-vtype=\"video/{Path.GetExtension(DSA.Tables["Pack"].Rows[0]["Path"].ToString())}\"style=\"height:140px;border-width:1px;border-style: dashed; cursor: pointer; \" src=\"/{DSA.Tables["Pack"].Rows[i]["PWpath"].ToString().Replace("H:\\", "").Replace("WebmBotSite\\WebmBot\\", "").Replace("\\", "/")}\" alt=\"Webm\"></div></td>";
                            i++;
                        }

                    }
                    catalogHTML += "</tr>";
                }
                catalogHTML += "</table>";


                HttpCookie myCookie = Request.Cookies["WebmVolumeValue"];
                string volume = "1";
                if (myCookie != null)
                {
                    volume = myCookie.Value;
                }
                else
                {
                    myCookie = new HttpCookie("WebmVolumeValue");
                    volume = "1";
                    // Set the cookie value.
                    myCookie.Value = volume;
                    // Set the cookie expiration date.
                    myCookie.Expires = DateTime.Now.AddYears(50); // For a cookie to effectively never expire
                                                                  // Add the cookie.
                    Response.Cookies.Add(myCookie);
                }

                MainCatalog.InnerHtml = catalogHTML;
            }
            else
            {
                MainCatalog.InnerHtml = "<div style=\"\"><h2 style=\"position:absolute; left:10px;bottom:500px;\">Эй долбоёб!</h2></br><img style=\"position:absolute; left:165px; z-index:-1; bottom:-85px; width:200px; heinght:500px\" src=\"Img/Naga.png\"/> <h2 style=\"position:absolute;left:355px;bottom:330px;\">Пошел нахуй!</h2></div><div style=\"justify-content: center; top:30%; position:absolute\"><h3 style=\"\"><font size=\"25\">401.7 – Access denied! Тебе сюда нельзя!</font></h3></div>";
            }
        }   

        }
    }