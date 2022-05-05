using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebmBot
{
    public partial class WebForm6 : System.Web.UI.Page
    {

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!Page.IsPostBack)
        //    {
        //        if (Page.User.Identity.IsAuthenticated)
        //        {
        //            init();
        //        }
        //        else
        //        {
        //            WebmConten.InnerHtml = "<div style=\"\"><h2 style=\"position:fixed; left:10px;bottom:500px;\">Эй долбоёб!</h2></br><img style=\"position:fixed; left:165px; bottom:25px; width:200px; heinght:500px\" src=\"Img/Naga.png\"/> <h2 style=\"position:fixed;left:355px;bottom:330px;\">Пошел нахуй!</h2></div><div style=\"justify-content: center; top:30%; position:absolute\"><h3 style=\"\"><font size=\"25\">401.7 – Access denied! Тебе сюда нельзя!</font></h3></div>";

        //        }
        //    }
        //}

        //protected void init()
        //{
        //    string volume = "1";
        //    HttpCookie myCookie = Request.Cookies["WebmVolumeValue"];
        //    if (myCookie != null)
        //    {
        //        volume = myCookie.Value;
        //    }
        //    else
        //    {
        //        myCookie = new HttpCookie("WebmVolumeValue");
        //        volume = "1";
        //        // Set the cookie value.
        //        myCookie.Value = volume;
        //        // Set the cookie expiration date.
        //        myCookie.Expires = DateTime.Now.AddYears(50); // For a cookie to effectively never expire
        //                                                      // Add the cookie.
        //        Response.Cookies.Add(myCookie);
        //    }
        //    DataSet DMSM = new DataSet();
        //    SqlConnection connM = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebmDB"].ConnectionString);
        //    DMSM.Clear();
        //    SqlDataAdapter adapterM = new SqlDataAdapter($"SELECT * FROM PackTable WHERE Loc='TEMP'", conn);
        //    adapterM.Fill(DMSM, "TempPack");
        //    if (DMSM.Tables["TempPack"].Rows.Count > 0)
        //    {
        //        WebmConten.InnerHtml = $"<video id = \"WebmPlayer\" autoplay loop controls onloaDMStart=\"this.volume = {volume}\" width=\"960\" height=\"540\" ><source src = \"/{DMSM.Tables["TempPack"].Rows[0]["Path"].ToString().Replace("F:\\", "").Replace("\\", "/")}\" type = \"video/{Path.GetExtension(DMSM.Tables["TempPack"].Rows[0]["Path"].ToString()).Replace(".", "")}\"/></video>";
        //        WebmID.Value = DMSM.Tables["TempPack"].Rows[0]["Id"].ToString();
        //        if (Page.User.Identity.IsAuthenticated)
        //        {

        //            var lable = LV.FindControl("WebmIdLable") as Label;
        //            var filenameLable = LV.FindControl("Username") as Label;
        //            filenameLable.Text = Path.GetFileName(DMSM.Tables["TempPack"].Rows[0]["Loader"].ToString());
        //            lable.Text = DMSM.Tables["TempPack"].Rows[0]["Id"].ToString();
        //        }
        //    }
        //    else
        //    {
        //        WebmConten.InnerHtml = "<h2>No download videos!</h2>";
        //    }
        //}
        //protected void ToFapButton_Click(object sender, EventArgs e)
        //{
        //    string id = WebmID.Value.ToString();
        //    string newPath = @"F:\webm\FromSite\Pack" + Guid.NewGuid().ToString() + Path.GetExtension(DMS.Tables["TempPack"].Rows.Find(id)["Path"].ToString());
        //    File.Move(DMS.Tables["TempPack"].Rows.Find(id)["Path"].ToString(), newPath);
        //    SqlCommand cmd = new SqlCommand($"UPDATE PackTable SET TAG='FAP',Path='{newPath}' WHERE Id='{id}'", conn);
        //    conn.Open();
        //    cmd.ExecuteNonQuery();
        //    conn.Close();
        //    init();
        //}

        //protected void RemoveFromBaseButton_Click(object sender, EventArgs e)
        //{
        //    string id = WebmID.Value.ToString();
        //    File.Delete(DMS.Tables["TempPack"].Rows.Find(id)["Path"].ToString());
        //    SqlCommand cmd = new SqlCommand($"UPDATE PackTable SET TAG='REMOVED' WHERE Id='{id}'", conn);
        //    conn.Open();
        //    cmd.ExecuteNonQuery();
        //    conn.Close();
        //    init();
        //}


        //protected void SkipReport_Click(object sender, EventArgs e)
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "alert('Test')", true);
        //    string id = WebmID.Value.ToString();
        //    string newPath = @"F:\webm\FromSite\Pack" + Guid.NewGuid().ToString() + Path.GetExtension(DMS.Tables["TempPack"].Rows.Find(id)["Path"].ToString());
        //    File.Move(DMS.Tables["TempPack"].Rows.Find(id)["Path"].ToString(), newPath);
        //    SqlCommand cmd = new SqlCommand($"UPDATE PackTable SET TAG='Untag',Path='{newPath}' WHERE Id='{id}'", conn);
        //    conn.Open();
        //    cmd.ExecuteNonQuery();
        //    conn.Close();
        //    init();
        //}
    }
}