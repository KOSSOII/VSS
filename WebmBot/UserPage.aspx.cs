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
using System.Text.RegularExpressions;
using System.Text;

namespace WebmBot
{
    public static class Extension
    {
        static Extension()
        {
            ImageTypes = new Dictionary<string, string>();
            ImageTypes.Add("FFD8", "jpg");
            ImageTypes.Add("424D", "bmp");
            ImageTypes.Add("474946", "gif");
            ImageTypes.Add("89504E470D0A1A0A", "png");
        }

        /// <summary>
        ///     <para> Registers a hexadecimal value used for a given image type </para>
        ///     <param name="imageType"> The type of image, example: "png" </param>
        ///     <param name="uniqueHeaderAsHex"> The type of image, example: "89504E470D0A1A0A" </param>
        /// </summary>
        public static void RegisterImageHeaderSignature(string imageType, string uniqueHeaderAsHex)
        {
            Regex validator = new Regex(@"^[A-F0-9]+$", RegexOptions.CultureInvariant);

            uniqueHeaderAsHex = uniqueHeaderAsHex.Replace(" ", "");

            if (string.IsNullOrWhiteSpace(imageType)) throw new ArgumentNullException("imageType");
            if (string.IsNullOrWhiteSpace(uniqueHeaderAsHex)) throw new ArgumentNullException("uniqueHeaderAsHex");
            if (uniqueHeaderAsHex.Length % 2 != 0) throw new ArgumentException("Hexadecimal value is invalid");
            if (!validator.IsMatch(uniqueHeaderAsHex)) throw new ArgumentException("Hexadecimal value is invalid");

            ImageTypes.Add(uniqueHeaderAsHex, imageType);
        }

        private static Dictionary<string, string> ImageTypes;

        public static bool IsImage(this Stream stream)
        {
            string imageType;
            return stream.IsImage(out imageType);
        }

        public static bool IsImage(this Stream stream, out string imageType)
        {
            stream.Seek(0, SeekOrigin.Begin);
            StringBuilder builder = new StringBuilder();
            int largestByteHeader = ImageTypes.Max(img => img.Value.Length);

            for (int i = 0; i < largestByteHeader; i++)
            {
                string bit = stream.ReadByte().ToString("X2");
                builder.Append(bit);

                string builtHex = builder.ToString();
                bool isImage = ImageTypes.Keys.Any(img => img == builtHex);
                if (isImage)
                {
                    imageType = ImageTypes[builder.ToString()];
                    return true;
                }
            }
            imageType = null;
            return false;
        }
    }
    public partial class WebForm7 : System.Web.UI.Page
    {
        DataSet DS = new DataSet();
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebmDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            AvatarLoad();
            
        }
        protected void AvatarLoad()
        {
            if (Page.User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(Page.User.Identity.Name))
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
                    string AvatarUrl = "/Img/Avatars/" + avaname;
                    Avatar.ImageUrl = AvatarUrl;
                }
            }
            else
            {
                Avatar.Visible = false;
                Figure.Visible = false;
            }

        }

        protected void LoadAvatarBtn_Click(object sender, EventArgs e)
        {
            DownloadAvatar();
        }
        protected void DownloadAvatar()
        {
            
            HttpFileCollection uploadedAvatar = Request.Files;
            HttpPostedFile file = uploadedAvatar[0];
            if (WebmBot.Extension.IsImage(file.InputStream))
            {
                string formant = Path.GetExtension(file.FileName);
                string filename = Page.User.Identity.Name.ToLower() + "Avatar";
                string path = $@"{AppDomain.CurrentDomain.BaseDirectory}\Img\Avatars\" + filename + formant;
                file.SaveAs(path);
                FileInfo fi = new FileInfo(path);
                string sqlstring = $"UPDATE UserAvatars SET AvatarName='{filename + formant}' WHERE UserName='{Page.User.Identity.Name.ToLower()}'  IF @@ROWCOUNT = 0 INSERT INTO UserAvatars ([UserName],[AvatarName]) VALUES('{Page.User.Identity.Name.ToLower()}','{filename + formant}')";
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlstring, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            else
            {
                //ClientScript.RegisterStartupScript(this.GetType(),Guid.NewGuid().ToString(), );
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "alert('Файл не является изображением');", true);
            }

        }
    }
}