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
using System.Diagnostics;
using NReco.VideoInfo;

namespace WebmBot
{
    public partial class WebForm5 : System.Web.UI.Page
    {
        DataSet DMS = new DataSet();
        DataSet DMSM = new DataSet();
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
                    WebmConten.InnerHtml = "<div style=\"\"><h2 style=\"position:absolute; left:10px;bottom:500px;\">Эй долбоёб!</h2></br><img style=\"position:absolute; left:165px; z-index:-1; bottom:-85px; width:200px; heinght:500px\" src=\"Img/Naga.png\"/> <h2 style=\"position:absolute;left:355px;bottom:330px;\">Пошел нахуй!</h2></div><div style=\"justify-content: center; top:30%; position:absolute\"><h3 style=\"\"><font size=\"25\">401.7 – Access denied! Тебе сюда нельзя!</font></h3></div>";

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
            DMSM.Clear();
            SqlDataAdapter adapterM = new SqlDataAdapter($"SELECT * FROM PackTable WHERE Loc='TEMP'", conn);
            adapterM.Fill(DMSM, "TempPack");
            if (DMSM.Tables["TempPack"].Rows.Count > 0)
            {
                WebmConten.InnerHtml = $"<video id = \"WebmPlayer\" autoplay loop controls onloaDMStart=\"this.volume = {volume}\" width=\"960\" height=\"540\" ><source src = \"/{DMSM.Tables["TempPack"].Rows[0]["Path"].ToString().Replace("H:\\", "").Replace("\\", "/")}\" type = \"video/{Path.GetExtension(DMSM.Tables["TempPack"].Rows[0]["Path"].ToString()).Replace(".", "")}\"/></video>";
                WebmID.Value = DMSM.Tables["TempPack"].Rows[0]["Id"].ToString();
                if (Page.User.Identity.IsAuthenticated)
                {

                    var lable = LV.FindControl("WebmIdLable") as Label;
                    var filenameLable = LV.FindControl("Username") as Label;
                    filenameLable.Text = DMSM.Tables["TempPack"].Rows[0]["Loader"].ToString();
                    lable.Text = DMSM.Tables["TempPack"].Rows[0]["Id"].ToString();
                }
            }
            else
            {
                WebmConten.InnerHtml = "<h2>No download videos!</h2>";
            }
        }

        protected void ToFapButton_Click(object sender, EventArgs e)
        {
            string id = WebmID.Value.ToString();
            DataSet DSA = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter($"SELECT * FROM PackTable WHERE Id='{id}'", conn);
            DSA.Clear();
            da.Fill(DSA, "ActionTable");
            string newPath = @"H:\webm\FromSite\Pack\" + Path.GetFileName(DSA.Tables["ActionTable"].Rows[0]["Path"].ToString());
            File.Move(DSA.Tables["ActionTable"].Rows[0]["Path"].ToString(), newPath);
            SqlCommand cmd = new SqlCommand($"UPDATE PackTable SET Loc='Pack',Path='{newPath}',TAG='FAP' WHERE Id='{id}'", conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            init();
        }

        protected void RemoveFromBaseButton_Click(object sender, EventArgs e)
        {
            string id = WebmID.Value.ToString();
            DataSet DSA = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter($"SELECT * FROM PackTable WHERE Id='{id}'", conn);
            DSA.Clear();
            da.Fill(DSA, "ActionTable");
            File.Delete(DSA.Tables["ActionTable"].Rows[0]["Path"].ToString());
            SqlCommand cmd = new SqlCommand($"UPDATE PackTable SET Loc='REMOVED',Path='REMOVED',TAG='REMOVED' WHERE Id='{id}'", conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            init();
        }


        protected void SkipReport_Click(object sender, EventArgs e)
        {
            
            string id = WebmID.Value.ToString();
            DataSet DSA = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter($"SELECT * FROM PackTable WHERE Id='{id}'", conn);
            DSA.Clear();
            da.Fill(DSA, "ActionTable");
            string newPath = @"H:\webm\FromSite\Pack\" + Path.GetFileName(DSA.Tables["ActionTable"].Rows[0]["Path"].ToString());
            File.Move(DSA.Tables["ActionTable"].Rows[0]["Path"].ToString(), newPath);
            SqlCommand cmd = new SqlCommand($"UPDATE PackTable SET Loc='Pack',Path='{newPath}',TAG='Untag' WHERE Id='{id}'", conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            Process ffmpeg;
            string video = newPath; 
            string thumb = @"H:\WebmBotSite\WebmBot\Previews\" + Path.GetFileNameWithoutExtension(video) + ".jpg";
            ffmpeg = new Process();
            string agrstring = "/C ffmpeg  -i \"" + video + "\" -ss 00:00:00.500 -map 0:0  -vframes 1 -f image2 -vcodec mjpeg \"" + thumb + "\" -y";
            ffmpeg.StartInfo.Arguments = agrstring;
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "console.log('"+ agrstring + "')", true);
            ffmpeg.StartInfo.FileName = "cmd.exe";
            ffmpeg.StartInfo.UseShellExecute = false;
            ffmpeg.StartInfo.CreateNoWindow = false;
            ffmpeg.Start();
            if (!File.Exists(thumb))
            {
                agrstring = "/C ffmpeg -i \"" + video + "\" -ss 00:00:00.500 -map 0:1 -vframes 1 -f image2 -vcodec mjpeg \"" + thumb + "\" -y";
                ffmpeg.StartInfo.Arguments = agrstring;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "console.log('"+ agrstring + "')", true);
                ffmpeg.StartInfo.FileName = "cmd.exe";
                ffmpeg.StartInfo.UseShellExecute = false;
                ffmpeg.StartInfo.CreateNoWindow = false;
                ffmpeg.Start();
                ffmpeg.WaitForExit();
            }
            string Dur = "NaN";
            try
            {
                var ffProbe = new FFProbe();
                var videoInfo = ffProbe.GetMediaInfo(video);
                Dur = videoInfo.Duration.ToString();
                if (Dur.Contains("."))
                {
                    Dur = Dur.Substring(0, Dur.LastIndexOf("."));
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "alert('"+ ex.Message + "')", true);
            }
            TextBox tb = (TextBox)LV.FindControl("tags");
            cmd = new SqlCommand($"UPDATE PackTable SET PWpath='{thumb}',VUTAG=@TGP,TimeDur='{Dur}' WHERE Id={id}", conn);
            cmd.Parameters.Add(new SqlParameter("TGP", tb.Text));
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            init();
            
        }
    }
}