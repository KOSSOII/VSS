using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Security.Cryptography;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace WebmBot
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebmDB"].ConnectionString);
        SqlCommand cmd;
        DataSet DS = new DataSet();
        public static List<string> toCopyList = new List<string>();
        public static List<string> ToRemoveList = new List<string>();
        public static Dictionary<string, string> Tag = new Dictionary<string, string>();
        protected void Page_Load(object sender, EventArgs e)
        {

           
            if (Page.User.IsInRole("Admin"))
                {
                    InitDir();
                }
                else
                {
                    WebmContent.InnerHtml = "<div style=\"\"><h2 style=\"position:fixed; left:10px;bottom:500px;\">Эй долбоёб!</h2></br><img style=\"position:fixed; left:165px; bottom:25px; width:200px; heinght:500px\" src=\"Img/Naga.png\"/> <h2 style=\"position:fixed;left:355px;bottom:330px;\">Пошел нахуй!</h2></div><div style=\"justify-content: center; top:30%; position:absolute\"><h3 style=\"\"><font size=\"25\">401.7 – Access denied! Тебе сюда нельзя!</font></h3></div>";
            }
            
            
        }
        protected void InitDir()
        {
            DS.Clear();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM PackTable WHERE Loc='TEMP'", conn);
            adapter.Fill(DS, "TEMP");

            foreach (DataRow row in DS.Tables["TEMP"].Rows)
            {
                TableRow thead = new TableRow();
                TableCell td = new TableCell();
                CheckBox chk = new CheckBox();
                chk.ID = Path.GetFileNameWithoutExtension(row["Path"].ToString());
                chk.Text = Path.GetFileName(row["Path"].ToString());
                chk.AutoPostBack = true;
                chk.CheckedChanged += new EventHandler(Chk_Fund_CheckedChange);
                td.Controls.Add(chk);
                if (Page.User.Identity.IsAuthenticated)
                    td.Visible = true;
                else
                    td.Visible = false;
                thead.Cells.Add(td);

                td = new TableCell();
                chk = new CheckBox();
                chk.ID = Path.GetFileNameWithoutExtension(row["Path"].ToString()) + "FAP";
                chk.Text = "Is FAP";
                chk.AutoPostBack = true;
                chk.CheckedChanged += new EventHandler(FAP_fund_CheckedChange);
                td.Controls.Add(chk);
                if (Page.User.Identity.IsAuthenticated)
                    td.Visible = true;
                else
                    td.Visible = false;
                thead.Cells.Add(td);


                td = new TableCell();
                td.Text = $"MN: {row["Loader"].ToString()}";
                if (Page.User.Identity.IsAuthenticated)
                    td.Visible = true;
                else
                    td.Visible = false;
                thead.Cells.Add(td);            
                td = new TableCell();
                td.Text = $" <br/><br/><video width=\"320\" height=\"240\" controls><source src = \"{Page.ResolveUrl("~/OnCheck/" + Path.GetFileName(row["Path"].ToString()))}\" type = \"video/{Path.GetExtension(row["Path"].ToString()).Replace(".","")}\"/></video><br/> <asp:CheckBox ID=\"" + Path.GetFileNameWithoutExtension(row["Path"].ToString()) + "\" runat=\"server\" Text=\"" + Path.GetFileName(row["Path"].ToString()) + "\" /><br/>";
                thead.Cells.Add(td);

                td = new TableCell();
                chk = new CheckBox();
                chk.ID = Path.GetFileNameWithoutExtension(row["Path"].ToString()) + "REMOVE";
                chk.Text = "Remove";
                chk.AutoPostBack = true;
                chk.CheckedChanged += new EventHandler(Remove_CheckedChange);
                td.Controls.Add(chk);
                if (Page.User.Identity.IsAuthenticated)
                    td.Visible = true;
                else
                    td.Visible = false;
                thead.Cells.Add(td);

                tbl_fundtype.Rows.Add(thead);
            }

        }

        private void Chk_Fund_CheckedChange(object sender, EventArgs e)
        {
            foreach (DataRow row in DS.Tables["TEMP"].Rows)
            {
                var chk = tbl_fundtype.FindControl(Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(row["Path"].ToString()))) as CheckBox;
                if (chk.Checked)
                {
                    if (!toCopyList.Contains(row["Path"].ToString()))
                        toCopyList.Add(row["Path"].ToString());
                }
                else
                {
                    if (toCopyList.Contains(row["Path"].ToString()))
                        toCopyList.Remove(row["Path"].ToString());
                }
            }
        }


        private void FAP_fund_CheckedChange(object sender, EventArgs e)
        {
            foreach (DataRow row in DS.Tables["TEMP"].Rows)
            {
                var chk = tbl_fundtype.FindControl(Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(row["Path"].ToString())) + "FAP") as CheckBox;
                if (chk.Checked)
                {
                    if(!Tag.ContainsKey(row["Path"].ToString()))
                    {
                        Tag.Add(row["Path"].ToString(), "FAP");
                    }
                    else
                    {
                        Tag[row["Path"].ToString()]="FAP";
                    }
                }
                else
                {
                    if (!Tag.ContainsKey(row["Path"].ToString()))
                    {
                        Tag.Add(row["Path"].ToString(), "UNTAG");
                    }
                    else
                    {
                        Tag[row["Path"].ToString()] = "Untag";
                    }

                }
            }
        }

        private void Remove_CheckedChange(object sender, EventArgs e)
        {
            foreach (DataRow row in DS.Tables["TEMP"].Rows)
            {
                
                var chk = tbl_fundtype.FindControl(Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(row["Path"].ToString())) + "REMOVE") as CheckBox;
                if (chk.Checked)
                {
                    if (!ToRemoveList.Contains(row["Path"].ToString()))
                        ToRemoveList.Add(row["Path"].ToString());
                }
                else
                {
                    if (ToRemoveList.Contains(row["Path"].ToString()))
                        ToRemoveList.Remove(row["Path"].ToString());
                }
            }
        }


        protected void ConfrimBtn_Click(object sender, EventArgs e)
        {
            foreach (string filename in toCopyList)
            {
               
                if (File.Exists(filename))
                {
                    string newFilePath = @"H:\webm\FromSite\Pack\" + Path.GetFileName(filename);
                    if (!Tag.ContainsKey(filename))
                    {                       
                        Tag.Add(filename,"Untag");
                    }
                    if(Tag[filename]=="FAP")
                    {
                        newFilePath = @"H:\webm\FromSite\FAP\" + Path.GetFileName(filename);
                    }
                    File.Copy(filename, newFilePath);
                    string sqlstring = $"UPDATE PackTable Set Path='{newFilePath.Replace("'", "''")}',Loc='Pack',TAG='{Tag[filename]}' WHERE Path='{filename.Replace("'", "''")}'";
                    conn.Open();
                    cmd = new SqlCommand(sqlstring, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    File.Delete(filename);
                }
                else
                {
                    Tag.Remove(filename);
                }
            }
            toCopyList.Clear();
            Tag.Clear();
            Page.Response.Redirect(Page.Request.Url.ToString(), true);

        }
    
    protected void ReRead_Click(object sender, EventArgs e)
        {
            long Mass = 0;
            string[] filePaths = Directory.GetFiles(@"H:\webm", "*.webm",
                             SearchOption.TopDirectoryOnly);
            string[] filePathsMP4 = Directory.GetFiles(@"H:\webm", "*.mp4",
                             SearchOption.TopDirectoryOnly);


            List<string> webmPathList = new List<string>();
            List<string> mp4PathList = new List<string>();
            Dictionary<string, string> PathHashList = new Dictionary<string, string>();
            Dictionary<string, long> PathMass = new Dictionary<string, long>();
            foreach (string filename in filePaths)
            {
                FileInfo info = new FileInfo(filename);
                if (info.Length <= 8388608)
                {
                    webmPathList.Add(filename);
                    Mass += info.Length;
                    PathMass.Add(filename, info.Length);
                }

            }
            foreach (string filename in filePathsMP4)
            {
                FileInfo info = new FileInfo(filename);
                if (info.Length <= 8388608)
                {
                    mp4PathList.Add(filename);
                    Mass += info.Length;
                    PathMass.Add(filename, info.Length);
                }
            }
            string[] SitefilePaths = Directory.GetFiles(@"H:\webm\FromSite\Pack", "*.webm");
            string[] SitefilePathsMP4 = Directory.GetFiles(@"H:\webm\FromSite\Pack", "*.mp4");
            foreach (string filename in SitefilePaths)
            {
                FileInfo info = new FileInfo(filename);
                if (info.Length <= 8388608)
                {
                    webmPathList.Add(filename);
                    Mass += info.Length;
                    PathMass.Add(filename, info.Length);
                }

            }
            foreach (string filename in SitefilePathsMP4)
            {
                FileInfo info = new FileInfo(filename);
                if (info.Length <= 8388608)
                {
                    mp4PathList.Add(filename);
                    Mass += info.Length;
                    PathMass.Add(filename, info.Length);
                }
            }

            string sqlstring = "TRUNCATE TABLE PackTable";
            conn.Open();
            SqlCommand cmd = new SqlCommand(sqlstring, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            int mathes = 0;
            int id = 0;
            foreach (string filePath in webmPathList)
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    Stream str = fs as Stream;
                    var FileHash = BitConverter.ToString(SHA256.Create().ComputeHash(str));
                    if (!PathHashList.ContainsKey(FileHash))
                    {
                        PathHashList.Add(FileHash, filePath);
                        sqlstring = $"INSERT INTO PackTable (Path, Hash, Loader, TAG,Loc,FileSize) VALUES('{filePath.Replace("'","''")}','{FileHash}','Site','Untag','Pack','{PathMass[filePath].ToString()}');";
                        conn.Open();
                        cmd = new SqlCommand(sqlstring, conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        id++;
                    }
                    else
                    {
                        mathes++;
                    }
                }

            }
            foreach (string filePath in mp4PathList)
            {

                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    Stream str = fs as Stream;
                    var FileHash = BitConverter.ToString(SHA256.Create().ComputeHash(str));
                    if (!PathHashList.ContainsKey(FileHash))
                    {
                        PathHashList.Add(FileHash, filePath);
                        sqlstring = $"INSERT INTO PackTableReRead (Path, Hash, Loader, TAG,Loc,FileSize) VALUES('{filePath.Replace("'", "''")}','{FileHash}','Site','Untag','Pack','{PathMass[filePath].ToString()}');";
                        conn.Open();
                        cmd = new SqlCommand(sqlstring, conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        id++;
                    }
                    else
                    {
                        mathes++;
                    }

                }
            }

        }

        protected void Remove_Click(object sender, EventArgs e)
        {
            foreach (string filename in ToRemoveList)
            {

                string sqlstring = $"UPDATE PackTable SET Loc='Deni' WHERE Path='{filename}';";
                conn.Open();
                cmd = new SqlCommand(sqlstring, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
            }
            ToRemoveList.Clear();
        }

        protected void RemoveAll_Click(object sender, EventArgs e)
        {
            string sqlstring = $"UPDATE PackTable SET Loc='Deni' WHERE Loc='TEMP';";
            conn.Open();
            cmd = new SqlCommand(sqlstring, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            System.IO.DirectoryInfo dInfo = new System.IO.DirectoryInfo(@"H:\webm\FromSite\OnCheck");
            foreach (System.IO.FileInfo file in dInfo.GetFiles())
                file.Delete();

            toCopyList.Clear();
            Tag.Clear();
            ToRemoveList.Clear();
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }
    }
}