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
using System.Web.Security;
using System.Diagnostics;


namespace WebmBot
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        DataSet DS = new DataSet();
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebmDB"].ConnectionString);
        public static string RegistredUsername;
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie cookieReq = Request.Cookies["UUID"];

            if (cookieReq == null)
            {
                HttpCookie UUIDCookie = new HttpCookie("UUID");

                UUIDCookie.Value = Guid.NewGuid().ToString();
                UUIDCookie.Expires = DateTime.Now.AddYears(50);
                Response.Cookies.Add(UUIDCookie);
            }
            if (!Page.IsPostBack)
            {
                GetNameAndAvatar();
                loadWebm();
                likeLoad();
                commentsLoad();
            }
            if(Page.User.IsInRole("Admin"))
            {
                Tagger.Visible = true;
            }
            if(Page.User.IsInRole("Tagger"))
            {
                Tagger.Visible = true;
            }
            
        }

        protected void GetNameAndAvatar()
        {
            if(Page.User.Identity.IsAuthenticated)
            {
                SqlDataAdapter da = new SqlDataAdapter($"SELECT * FROM UserAvatars WHERE UserName='" + Page.User.Identity.Name.ToLower() + "'", conn);
                DataTable AvatarInfo = new DataTable();
                da.Fill(AvatarInfo);
                string avaname = "anonAva.jpg";
                if (AvatarInfo.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(AvatarInfo.Rows[0]["AvatarName"].ToString()))
                    {
                        avaname = AvatarInfo.Rows[0]["AvatarName"].ToString();
                    }
                }
                da = new SqlDataAdapter($"SELECT * FROM aspnet_Users WHERE LoweredUserName='" + Page.User.Identity.Name.ToLower() + "'", conn);
                DataTable userNameTable = new DataTable();
                da.Fill(userNameTable);
                string username = userNameTable.Rows[0]["UserName"].ToString();
                RegistredUsername = username;
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), $"var UserAvatar = \"{avaname}\"; var GetedUserName =\"{username}\";", true);
                ChatAva.Src = @"\Img\Avatars\" + avaname;
            }
            else
            {
                string avaname = "anonAva.jpg";
                string username = "NonameAnoname";
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), $"var UserAvatar = \"{avaname}\"; var GetedUserName =\"{username}\";", true);
                ChatAva.Src = @"\Img\Avatars\" + avaname;
            }
        }
        protected void commentsLoad()
        {
            SqlDataAdapter da = new SqlDataAdapter($"SELECT * FROM CommentTable WHERE webmid='{WebmID.Value.ToString()}'", conn);// WHERE webmid='{WebmID.Value.ToString()}'
            DataTable comments = new DataTable();
            da.Fill(comments);
            CommentsTogle.Value = "Комментарии " + comments.Rows.Count;
            for(int i = comments.Rows.Count-1;i>0;i--)
            {
                DataRow comment = comments.Rows[i];
                da = new SqlDataAdapter($"SELECT * FROM UserAvatars WHERE UserName='" + comment["Author"].ToString().ToLower() + "'", conn);
                DataTable AvatarInfo = new DataTable();
                da.Fill(AvatarInfo);
                string avaname = AvatarInfo.Rows[0]["AvatarName"].ToString();


                string htmlComment = "<div class=\"CommentHeader\" style=\"display:flex; margin-bottom:10px\">" +
                                    "<figure style = \"z-index: 1;top: 8px;border-radius: 30px;width: 30px;height: 30px;margin: 0;padding: 0; border: 2px solid; overflow:hidden; margin-right: 10px;\" >" +
                                    $"<img style=\"height:100%;width:100%;object-fit:cover;\" src=\"/Img/Avatars/{avaname}\">" + "</figure>" +
                                    "<div class=\"CommentHeaderText\" style=\"display:flex;align-items:center;height:30px;\">" +
                                    $"<font style = \"color: rebeccapurple;font-weight: bold;margin-right: 7px;\">{comment["Author"].ToString()}</font>" +
                                    $"<font style=\"color:aquamarine;font-size: smaller;font-style: italic;font-family: initial;\">{comment["Dtime"].ToString()}</font></div></div>";
                                    //"<div class=\"CommentText\">"+ comment ["Comment"] + "</div><hr>";
                System.Web.UI.HtmlControls.HtmlGenericControl commentDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                commentDiv.Style.Add(HtmlTextWriterStyle.MarginTop, "10px");
                commentDiv.InnerHtml = htmlComment;
                System.Web.UI.HtmlControls.HtmlGenericControl commentText = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                commentText.Style.Add("overflow-wrap", "break-word");
                commentText.InnerText = comment["Comment"].ToString();
                System.Web.UI.HtmlControls.HtmlGenericControl commentLine = new System.Web.UI.HtmlControls.HtmlGenericControl("hr");
                commentDiv.Controls.Add(commentText);
                commentDiv.Controls.Add(commentLine);
                Comments.Controls.Add(commentDiv);
                

            }
        }

        protected void likeLoad()
        {

            HttpCookie cookieReq = Request.Cookies["UUID"];
            string UUID = "Nan";
            if (Page.User.Identity.IsAuthenticated)
            {
                UUID = Page.User.Identity.Name.ToLower();
            }
            else
            {
                UUID = cookieReq.Value.ToString();
            }
            string id = WebmID.Value.ToString();
            SqlDataAdapter da = new SqlDataAdapter($"SELECT * FROM LikesFromUsers WHERE UUIDN=@UUID AND WebmId='{id}' AND LORD='L'", conn);
            da.SelectCommand.Parameters.Add(new SqlParameter("UUID", UUID));
            DataSet LDS = new DataSet();
            da.Fill(LDS, "Likes");
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), $"console.log('Like load: Found  {LDS.Tables["Likes"].Rows.Count} ikes.');", true);
            if (LDS.Tables["Likes"].Rows.Count > 0)
            {
                LikeButton.Attributes["style"] = "background-color: #008bc1; margin: 10px;  min-width:120px";
            }
            else
            {
                LikeButton.Attributes["style"] = "background-color:  #5a7d44; margin: 10px;  min-width:120px";
            }
            SqlDataAdapter daa = new SqlDataAdapter($"SELECT * FROM LikesFromUsers WHERE UUIDN=@UUID AND WebmId='{id}' AND LORD='D'", conn);
            daa.SelectCommand.Parameters.Add(new SqlParameter("UUID", UUID));
            DataSet LDSS = new DataSet();
            daa.Fill(LDSS, "Dislikes");
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), $"console.log('Dislike load: Found  {LDSS.Tables["Dislikes"].Rows.Count} dislikes.');", true);
            if (LDSS.Tables["Dislikes"].Rows.Count > 0)
            {
                DislikeButton.Attributes["style"] = "background-color:  #008bc1; margin: 10px;  min-width:120px";
            }
            else
            {
                DislikeButton.Attributes["style"] = "background-color: #a41637; margin: 10px;  min-width:120px";
            }
        }

        protected void rand_Click(object sender, EventArgs e)
        {
            loadWebm();
            likeLoad();
            commentsLoad();
            UP.Update();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "if(togleStatus==1){togleComments(document.getElementById('CommentsHolder'), 0);}", true);
        }
        protected void loadWebm()
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

            DS.Clear();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM PackTable WHERE Loc='Pack' AND TAG<>'FAP' AND TAG<>'REMOVED'", conn);
            adapter.Fill(DS, "Webm");
            Random rnd = new Random();
            int rndIndex = rnd.Next(0, DS.Tables["Webm"].Rows.Count);
            
            string htmlvideo = $"<video id = \"WebmPlayer\" autoplay loop controls onloadstart=\"this.volume = {volume}\" width=\"960\" height=\"540\" style=\"margin-top: 10px; margin-bottom: 15px; \" ><source src = \"/{DS.Tables["Webm"].Rows[rndIndex]["Path"].ToString().Replace("H:\\", "").Replace("\\", "/")}\" type = \"video/{Path.GetExtension(DS.Tables["Webm"].Rows[rndIndex]["Path"].ToString()).Replace(".", "")}\"/></video>";
            WebmConten.InnerHtml = htmlvideo;
            WebmID.Value = DS.Tables["Webm"].Rows[rndIndex]["Id"].ToString();
            if (Page.User.IsInRole("Trusted User"))
            {
                WebmIdFoUser.Text = "WebmID: "+WebmID.Value;
            }
            LikeButton.Text= "Нравится " + DS.Tables["Webm"].Rows[rndIndex]["Likes"].ToString();
            DislikeButton.Text = "Не нравится " + DS.Tables["Webm"].Rows[rndIndex]["Dislikes"].ToString();
            string[] vtags = DS.Tables["Webm"].Rows[rndIndex]["VUTAG"].ToString().Split(',');
            string formatedTags = "<p style =\"margin-right:5px;padding-left:15px;padding-right:15px;height:24px;font-size:18px;color:chocolate;\">Tэги:</p>";
            int tagsadd = 0;
            if (vtags.Length > 0)
            {
                foreach (string tag in vtags)
                {
                    if (!string.IsNullOrEmpty(tag)&&tag!=" ")
                    {
                        formatedTags += "<p style =\"margin-right: 5px;border: 1px solid #cccccc2e!important;border-radius:16px;box-sizing: inherit;background-color:#4691e8;padding-left:15px;padding-right:15px;height:24px;\">" + tag + "</p>";
                        tagsadd++;
                    }
                }
                if (tagsadd > 0)
                {
                    
                    tagdiv.InnerHtml = formatedTags;
                }
                else
                {
                    tagdiv.InnerHtml = "";
                }
            }
            if (!string.IsNullOrEmpty(DS.Tables["Webm"].Rows[rndIndex]["Loader"].ToString()) && !string.IsNullOrWhiteSpace(DS.Tables["Webm"].Rows[rndIndex]["Loader"].ToString())&& DS.Tables["Webm"].Rows[rndIndex]["Loader"].ToString()!="Site")
            {
                if (Page.User.IsInRole("Trusted User"))
                {
                    LoaderName.Text = "Загрузил: " + DS.Tables["Webm"].Rows[rndIndex]["Loader"].ToString();
                }
            }
            else
            {
                LoaderName.Text = "";
            }
            if (Page.User.IsInRole("Admin"))
            {
                var lable = LV.FindControl("WebmIdLable") as Label;
                var filenameLable = LV.FindControl("WebmName") as Label;
                filenameLable.Text = Path.GetFileName(DS.Tables["Webm"].Rows[rndIndex]["Path"].ToString());
                lable.Text = DS.Tables["Webm"].Rows[rndIndex]["Id"].ToString();
            }
            TagsAddBox.Text = string.Empty;

        }
        protected void SendReport_Click(object sender, EventArgs e)
        {         
            string id = WebmID.Value.ToString();
            SqlCommand cmd = new SqlCommand($"SELECT COUNT(*) from WebmReport WHERE WebmId= '{id}' AND Watched <> 'True';", conn);
            conn.Open();
            int ActiveReportCount = (int)cmd.ExecuteScalar();
            conn.Close();
            if (ActiveReportCount == 0)
            {
                cmd = new SqlCommand($"INSERT INTO WebmReport (WebmId, ReportText,Watched) VALUES('{id}', @ReportText,'False')", conn);
                cmd.Parameters.Add(new SqlParameter("ReportText", ReportText.Text.ToString()));
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                ReportText.Text = string.Empty;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "alert('Репорт принят. Спасибо.');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "alert('На эту вебм уже поступил репорт. Спасибо.');", true);
            }
        }

        protected void ToFapButton_Click(object sender, EventArgs e)
        {
            string id = WebmID.Value.ToString();
            SqlCommand cmd = new SqlCommand($"UPDATE PackTable SET TAG='FAP' WHERE Id='{id}'", conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        protected void RemoveFromBaseButton_Click(object sender, EventArgs e)
        {
            string id = WebmID.Value.ToString();
            SqlCommand cmd = new SqlCommand($"UPDATE PackTable SET TAG='REMOVED' WHERE Id='{id}'", conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        protected void LikeButton_Click(object sender, EventArgs e)
        {
        //Stopwatch wath = new Stopwatch();
           // wath.Start();
          
            string UUID = "Nan";
            string id = WebmID.Value.ToString();
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), $"console.log('=====================START FOR {id}=====================');", true);
            HttpCookie cookieReq = Request.Cookies["UUID"];
            if (Page.User.Identity.IsAuthenticated)
            {
                UUID = Page.User.Identity.Name.ToLower();
            }
            else
            {
                UUID = cookieReq.Value.ToString();
            }
           // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), $"console.log('Step 1, time: {wath.ElapsedMilliseconds} ms.');", true);
            SqlDataAdapter da = new SqlDataAdapter($"SELECT * FROM LikesFromUsers WHERE UUIDN=@UUID AND WebmId='{id}' AND LORD='L'", conn);    
            da.SelectCommand.Parameters.Add(new SqlParameter("UUID",UUID));
            DataSet LDS = new DataSet();
            da.Fill(LDS, "Likes");
           // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), $"console.log('Step 2, time: {wath.ElapsedMilliseconds} ms.');", true);
            if (LDS.Tables["Likes"].Rows.Count > 0)
            {
                SqlCommand cmd = new SqlCommand($"UPDATE LikesFromUsers SET LORD='N' WHERE UUIDN=@UUID AND WebmId='{id}' AND LORD='L'", conn);
                cmd.Parameters.Add(new SqlParameter("UUID", UUID));
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
               // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), $"console.log('Step 3, time: {wath.ElapsedMilliseconds} ms.');", true);
                cmd = new SqlCommand($"UPDATE PackTable SET Likes = Likes-1 WHERE Id='{id}'", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), $"console.log('Step 4, time: {wath.ElapsedMilliseconds} ms.');", true);
                cmd = new SqlCommand($"SELECT Likes FROM PackTable WHERE Id='{id}'", conn);
                conn.Open();
                int likes = (int)cmd.ExecuteScalar();
                conn.Close();
                LikeButton.Text = "Нравится " + likes.ToString();
                likeLoad();
               // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), $"console.log('Step 5, time: {wath.ElapsedMilliseconds} ms.');", true);
            }
            else
            {
                SqlDataAdapter daa = new SqlDataAdapter($"SELECT * FROM LikesFromUsers WHERE UUIDN=@UUID AND WebmId='{id}' AND LORD='D'", conn);
                daa.SelectCommand.Parameters.Add(new SqlParameter("UUID", UUID));
                DataSet LDSS = new DataSet();
                daa.Fill(LDSS, "Dislikes");
              //  ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), $"console.log('Step 3, time: {wath.ElapsedMilliseconds} ms.');", true);
                if (LDSS.Tables["Dislikes"].Rows.Count > 0)
                {
                    SqlCommand cmd = new SqlCommand($"UPDATE LikesFromUsers SET LORD='N' WHERE UUIDN=@UUID AND WebmId='{id}' AND LORD='D'", conn);
                    cmd.Parameters.Add(new SqlParameter("UUID", UUID));
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
//                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), $"console.log('Step 4, time: {wath.ElapsedMilliseconds} ms.');", true);
                    cmd = new SqlCommand($"UPDATE PackTable SET Dislikes = Dislikes-1 WHERE Id='{id}'", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
//                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), $"console.log('Step 5, time: {wath.ElapsedMilliseconds} ms.');", true);
                    cmd = new SqlCommand($"SELECT Dislikes FROM PackTable WHERE Id='{id}'", conn);
                    conn.Open();
                    int Dislikes = (int)cmd.ExecuteScalar();
                    conn.Close();
//                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), $"console.log('Step 6, time: {wath.ElapsedMilliseconds} ms.');", true);
                    DislikeButton.Text = "Нравится " + Dislikes.ToString();
                    cmd = new SqlCommand($"UPDATE LikesFromUsers SET LORD='L' WHERE UUIDN=@UUID AND WebmId='{id}' IF @@ROWCOUNT = 0 INSERT INTO LikesFromUsers (WebmId,UUIDN,LORD) VALUES ('{id}',@UUID,'L')", conn);
                    cmd.Parameters.Add(new SqlParameter("UUID", UUID));
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
//                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), $"console.log('Step 7, time: {wath.ElapsedMilliseconds} ms.');", true);
                    cmd = new SqlCommand($"UPDATE PackTable SET Likes = Likes+1 WHERE Id='{id}'", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
//                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), $"console.log('Step 8, time: {wath.ElapsedMilliseconds} ms.');", true);
                    cmd = new SqlCommand($"SELECT Likes FROM PackTable WHERE Id='{id}'", conn);
                    conn.Open();
                    int likes = (int)cmd.ExecuteScalar();
                    conn.Close();
                    LikeButton.Text = "Нравится " + likes.ToString();
                    likeLoad();
//                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), $"console.log('Step 9, time: {wath.ElapsedMilliseconds} ms.');", true);
                }
                else
                {
                    SqlCommand cmd = new SqlCommand($"UPDATE LikesFromUsers SET LORD='L' WHERE UUIDN=@UUID AND WebmId='{id}' IF @@ROWCOUNT = 0 INSERT INTO LikesFromUsers (WebmId,UUIDN,LORD) VALUES ('{id}',@UUID,'L')", conn);
                    cmd.Parameters.Add(new SqlParameter("UUID", UUID));
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
//                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), $"console.log('Step 4, time: {wath.ElapsedMilliseconds} ms.');", true);
                    cmd = new SqlCommand($"UPDATE PackTable SET Likes = Likes+1 WHERE Id='{id}'", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
//                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), $"console.log('Step 5, time: {wath.ElapsedMilliseconds} ms.');", true);
                    cmd = new SqlCommand($"SELECT Likes FROM PackTable WHERE Id='{id}'", conn);
                    conn.Open();
                    int likes = (int)cmd.ExecuteScalar();
                    conn.Close();
//                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), $"console.log('Step 6, time: {wath.ElapsedMilliseconds} ms.');", true);
                    LikeButton.Text = "Нравится " + likes.ToString();
                    likeLoad();
//                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), $"console.log('Step 7, time: {wath.ElapsedMilliseconds} ms.');", true);
                    
                }

            }
//ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), $"console.log('Stop time {wath.ElapsedMilliseconds} ms.');", true);
 //           ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), $"console.log('=====================END FOR {id}=====================');", true);
//            wath.Stop();
        }

        protected void DislikeButton_Click(object sender, EventArgs e)
        {
            string UUID = "Nan";
            HttpCookie cookieReq = Request.Cookies["UUID"];
            if (Page.User.Identity.IsAuthenticated)
            {
                UUID = Page.User.Identity.Name.ToLower();
            }
            else
            {
                UUID = cookieReq.Value.ToString();
            }
            string id = WebmID.Value.ToString();
            SqlDataAdapter da = new SqlDataAdapter($"SELECT * FROM LikesFromUsers WHERE UUIDN=@UUID AND WebmId='{id}' AND LORD='D'", conn);
            da.SelectCommand.Parameters.Add(new SqlParameter("UUID", UUID));
            DataSet LDS = new DataSet();
            da.Fill(LDS, "Dislikes");
            if (LDS.Tables["Dislikes"].Rows.Count > 0)
            {
                SqlCommand cmd = new SqlCommand($"UPDATE LikesFromUsers SET LORD='N' WHERE UUIDN=@UUID AND WebmId='{id}' AND LORD='D'", conn);
                cmd.Parameters.Add(new SqlParameter("UUID", UUID));
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                cmd = new SqlCommand($"UPDATE PackTable SET Dislikes = Dislikes-1 WHERE Id='{id}'", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                cmd = new SqlCommand($"SELECT Dislikes FROM PackTable WHERE Id='{id}'", conn);
                conn.Open();
                int likes = (int)cmd.ExecuteScalar();
                conn.Close();
                DislikeButton.Text = "Не нравится " + likes.ToString();
                likeLoad();
            }
            else
            {
                SqlDataAdapter daa = new SqlDataAdapter($"SELECT * FROM LikesFromUsers WHERE UUIDN=@UUID AND WebmId='{id}' AND LORD='L'", conn);
                daa.SelectCommand.Parameters.Add(new SqlParameter("UUID", UUID));
                DataSet LDSS = new DataSet();
                daa.Fill(LDSS, "Likes");
                if (LDSS.Tables["Likes"].Rows.Count > 0)
                {
                    SqlCommand cmd = new SqlCommand($"UPDATE LikesFromUsers SET LORD='N' WHERE UUIDN=@UUID AND WebmId='{id}' AND LORD='L'", conn);
                    cmd.Parameters.Add(new SqlParameter("UUID", UUID));
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    cmd = new SqlCommand($"UPDATE PackTable SET Likes = Likes-1 WHERE Id='{id}'", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    cmd = new SqlCommand($"SELECT Likes FROM PackTable WHERE Id='{id}'", conn);
                    conn.Open();
                    int likes = (int)cmd.ExecuteScalar();
                    conn.Close();
                    LikeButton.Text = "Нравится " + likes.ToString();
                    cmd = new SqlCommand($"UPDATE LikesFromUsers SET LORD='D' WHERE UUIDN=@UUID AND WebmId='{id}' IF @@ROWCOUNT = 0 INSERT INTO LikesFromUsers (WebmId,UUIDN,LORD) VALUES ('{id}',@UUID,'D')", conn);
                    cmd.Parameters.Add(new SqlParameter("UUID", UUID));
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    cmd = new SqlCommand($"UPDATE PackTable SET Dislikes = Dislikes+1 WHERE Id='{id}'", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    cmd = new SqlCommand($"SELECT Dislikes FROM PackTable WHERE Id='{id}'", conn);
                    conn.Open();
                    int Dislike = (int)cmd.ExecuteScalar();
                    conn.Close();
                    DislikeButton.Text = "Не нравится " + Dislike.ToString();
                    likeLoad();

                }
                else
                {
                    SqlCommand cmd = new SqlCommand($"UPDATE LikesFromUsers SET LORD='D' WHERE UUIDN=@UUID AND WebmId='{id}' IF @@ROWCOUNT = 0 INSERT INTO LikesFromUsers (WebmId,UUIDN,LORD) VALUES ('{id}',@UUID,'D')", conn);
                    cmd.Parameters.Add(new SqlParameter("UUID", UUID));
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    cmd = new SqlCommand($"UPDATE PackTable SET Dislikes = Dislikes+1 WHERE Id='{id}'", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    cmd = new SqlCommand($"SELECT Dislikes FROM PackTable WHERE Id='{id}'", conn);
                    conn.Open();
                    int likes = (int)cmd.ExecuteScalar();
                    conn.Close();
                    DislikeButton.Text = "Не нравится " + likes.ToString();
                    likeLoad();
                }
            }
        }

        protected void SendComment_Click(object sender, EventArgs e)
        {
            if (Page.User.Identity.IsAuthenticated)
            {
                string id = WebmID.Value.ToString();
                SqlCommand cmd = new SqlCommand($"INSERT INTO CommentTable (webmid, Comment,Author,Dtime) VALUES('{id}', @CommentText,'{RegistredUsername}','{DateTime.Now}')", conn);
                cmd.Parameters.Add(new SqlParameter("CommentText", CommentText.Text.ToString()));
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                CommentText.Text = "";
                commentsLoad();
                UP.Update();
                
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "togleComments(document.getElementById('CommentsHolder'), 0);", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "alert('Только зарегестрированные пользователи могут оставлять комментарии. Войдите или зарегестрируйтесь.')", true);
            }
        }

        protected void TagWebmB_Click(object sender, EventArgs e)
        {

            string id = WebmID.Value.ToString();
            SqlCommand cmd = new SqlCommand($"INSERT INTO tagsaddlog (webmid,tags,adder) VALUES('{id}',@tagsToAdd,'{Page.User.Identity.Name}')", conn);
            cmd.Parameters.Add(new SqlParameter("tagsToAdd", TagsAddBox.Text.ToString()));
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            cmd = new SqlCommand($"UPDATE PackTable SET VUTAG=VUTAG+','+@TagsToAdd WHERE id={Convert.ToInt32(id)}", conn);
            cmd.Parameters.Add(new SqlParameter("TagsToAdd", TagsAddBox.Text.ToString()));
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), " document.getElementById('ty').innerHTML = 'Тэги приняты! Спасибо!';", true);
            string[] vtagsadd = TagsAddBox.Text.ToString().Split(',');
            string TagsTemp = "";
            if(string.IsNullOrEmpty(tagdiv.InnerHtml))
            {
                 TagsTemp = "<p style =\"margin-right:5px;padding-left:15px;padding-right:15px;height:24px;font-size:18px;color:chocolate;\">Tэги:</p>";
            }
            else
            {
                 TagsTemp = tagdiv.InnerHtml;
            }
            foreach (string tag in vtagsadd)
            {
                if (!string.IsNullOrEmpty(tag) && tag != " ")
                {
                    TagsTemp += "<p style =\"margin-right: 5px;border: 1px solid #cccccc2e!important;border-radius:16px;box-sizing: inherit;background-color:#4691e8;padding-left:15px;padding-right:15px;height:24px;\">" + tag + "</p>";
                }
            }
            tagdiv.InnerHtml = TagsTemp;
            TagsAddBox.Text = string.Empty;
            UP.Update();

        }

        protected void LV_ViewChanged(object sender, EventArgs e)
        {

        }

        protected void RNDNOTAG_Click(object sender, EventArgs e)
        {
            loadWebmNOTAG();
            likeLoad();
            commentsLoad();
            UP.Update();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "if(togleStatus==1){togleComments(document.getElementById('CommentsHolder'), 0);}", true);
        }
        protected void loadWebmNOTAG()
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

            DS.Clear();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM PackTable WHERE Loc='Pack' AND TAG<>'FAP' AND TAG<>'REMOVED' AND VUTAG=' '", conn);
            adapter.Fill(DS, "Webm");
            Random rnd = new Random();
            int rndIndex = rnd.Next(0, DS.Tables["Webm"].Rows.Count);

            string htmlvideo = $"<video id = \"WebmPlayer\" autoplay loop controls onloadstart=\"this.volume = {volume}\" width=\"960\" height=\"540\" style=\"margin-top: 10px; margin-bottom: 15px; \" ><source src = \"/{DS.Tables["Webm"].Rows[rndIndex]["Path"].ToString().Replace("H:\\", "").Replace("\\", "/")}\" type = \"video/{Path.GetExtension(DS.Tables["Webm"].Rows[rndIndex]["Path"].ToString()).Replace(".", "")}\"/></video>";
            WebmConten.InnerHtml = htmlvideo;
            WebmID.Value = DS.Tables["Webm"].Rows[rndIndex]["Id"].ToString();
            if (Page.User.IsInRole("Trusted User"))
            {
                WebmIdFoUser.Text = "WebmID: " + WebmID.Value;
            }
            LikeButton.Text = "Нравится " + DS.Tables["Webm"].Rows[rndIndex]["Likes"].ToString();
            DislikeButton.Text = "Не нравится " + DS.Tables["Webm"].Rows[rndIndex]["Dislikes"].ToString();
            string[] vtags = DS.Tables["Webm"].Rows[rndIndex]["VUTAG"].ToString().Split(',');
            string formatedTags = "<p style =\"margin-right:5px;padding-left:15px;padding-right:15px;height:24px;font-size:18px;color:chocolate;\">Tэги:</p>";
            int tagsadd = 0;
            if (vtags.Length > 0)
            {
                foreach (string tag in vtags)
                {
                    if (!string.IsNullOrEmpty(tag) && tag != " ")
                    {
                        formatedTags += "<p style =\"margin-right: 5px;border: 1px solid #cccccc2e!important;border-radius:16px;box-sizing: inherit;background-color:#4691e8;padding-left:15px;padding-right:15px;height:24px;\">" + tag + "</p>";
                        tagsadd++;
                    }
                }
                if (tagsadd > 0)
                {

                    tagdiv.InnerHtml = formatedTags;
                }
                else
                {
                    tagdiv.InnerHtml = "";
                }
            }
            if (!string.IsNullOrEmpty(DS.Tables["Webm"].Rows[rndIndex]["Loader"].ToString()) && !string.IsNullOrWhiteSpace(DS.Tables["Webm"].Rows[rndIndex]["Loader"].ToString()) && DS.Tables["Webm"].Rows[rndIndex]["Loader"].ToString() != "Site")
            {
                if (Page.User.IsInRole("Trusted User"))
                {
                    LoaderName.Text = "Загрузил: " + DS.Tables["Webm"].Rows[rndIndex]["Loader"].ToString();
                }
            }
            else
            {
                LoaderName.Text = "";
            }
            if (Page.User.IsInRole("Admin"))
            {
                var lable = LV.FindControl("WebmIdLable") as Label;
                var filenameLable = LV.FindControl("WebmName") as Label;
                filenameLable.Text = Path.GetFileName(DS.Tables["Webm"].Rows[rndIndex]["Path"].ToString());
                lable.Text = DS.Tables["Webm"].Rows[rndIndex]["Id"].ToString();
            }
            TagsAddBox.Text = string.Empty;

        }
    }
}