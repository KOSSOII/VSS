using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.IO;
namespace WebmBot
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        DataSet DS = new DataSet();
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebmDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Page.User.IsInRole("Admin"))
                {

                    init();
                }
                else
                {
                    WebmConten.InnerHtml = "<div style=\"\"><h2 style=\"position:fixed; left:10px;bottom:500px;\">Эй долбоёб!</h2></br><img style=\"position:fixed; left:165px; bottom:25px; width:200px; heinght:500px\" src=\"Img/Naga.png\"/> <h2 style=\"position:fixed;left:355px;bottom:330px;\">Пошел нахуй!</h2></div><div style=\"justify-content: center; top:30%; position:absolute\"><h3 style=\"\"><font size=\"25\">401.7 – Access denied! Тебе сюда нельзя!</font></h3></div>";

                }
            }
        }

        protected void init()
        {
            string volume = "1";
            HttpCookie myCookie = Request.Cookies["WebmVolumeValue"];
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
            SqlConnection connM = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebmDB"].ConnectionString);
            DS.Clear();
            SqlDataAdapter adapterM = new SqlDataAdapter($"SELECT * FROM WebmReport WHERE Watched <> 'True'", conn);
            adapterM.Fill(DS, "ReportedWebm");
            if (DS.Tables["ReportedWebm"].Rows.Count > 0)
            {
                DataSet DMS = new DataSet();
                DMS.Clear();
                adapterM = new SqlDataAdapter($"SELECT * FROM PackTable WHERE Id='{DS.Tables["ReportedWebm"].Rows[0]["WebmId"].ToString()}'", conn);
                adapterM.Fill(DMS, "TempPack");
                WebmConten.InnerHtml = $"<video id = \"WebmPlayer\" autoplay loop controls onloaDMStart=\"this.volume = {volume}\" width=\"960\" height=\"540\" ><source src = \"/{DMS.Tables["TempPack"].Rows[0]["Path"].ToString().Replace("H:\\", "").Replace("\\", "/")}\" type = \"video/{Path.GetExtension(DMS.Tables["TempPack"].Rows[0]["Path"].ToString()).Replace(".", "")}\"/></video>";
                WebmID.Value = DMS.Tables["TempPack"].Rows[0]["Id"].ToString();
                ReportId.Value = DS.Tables["ReportedWebm"].Rows[0]["id"].ToString();
                if (Page.User.Identity.IsAuthenticated)
                {

                    var lable = LV.FindControl("WebmIdLable") as Label;
                    var filenameLable = LV.FindControl("WebmName") as Label;
                    filenameLable.Text = Path.GetFileName(DMS.Tables["TempPack"].Rows[0]["Path"].ToString());
                    lable.Text = DMS.Tables["TempPack"].Rows[0]["Id"].ToString();
                    var ReportText = LV.FindControl("ReportText") as Label;
                    ReportText.Text = DS.Tables["ReportedWebm"].Rows[0]["ReportText"].ToString();
                    var ReportId = LV.FindControl("ReportIdInBase") as Label;
                    ReportId.Text = DS.Tables["ReportedWebm"].Rows[0]["WebmId"].ToString();
                   
                }
            }
            else
            {
                WebmConten.InnerHtml = "<h2>No Reported videos!</h2>";
            }
        }
        protected void ToFapButton_Click(object sender, EventArgs e)
        {
            string id = WebmID.Value.ToString();
            SqlCommand cmd = new SqlCommand($"UPDATE PackTable SET TAG='FAP' WHERE Id='{id}'", conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            string rid = ReportId.Value.ToString();
            cmd = new SqlCommand($"UPDATE WebmReport SET Watched='True' WHERE id='{rid}'", conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            init();
        }

        protected void RemoveFromBaseButton_Click(object sender, EventArgs e)
        {
            string id = WebmID.Value.ToString();
            SqlCommand cmd = new SqlCommand($"UPDATE PackTable SET TAG='REMOVED' WHERE Id='{id}'", conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            string rid = ReportId.Value.ToString();
            cmd = new SqlCommand($"UPDATE WebmReport SET Watched='True' WHERE id='{rid}'", conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            init();
        }


        protected void SkipReport_Click(object sender, EventArgs e)
        {
            string rid = ReportId.Value.ToString();
            SqlCommand cmd = new SqlCommand($"UPDATE WebmReport SET Watched='True' WHERE id='{rid}'", conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            init();
        }

    }
}