using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
namespace WebmBot
{
    public partial class _Default : Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebmDB"].ConnectionString);
        SqlCommand cmd;
        public static string[] hashList;
        public static string[] TempHashList;
        public static string log = "";
        protected void Page_Load(object sender, EventArgs e)
        {           
            cmd = new SqlCommand("SELECT COUNT(*) FROM PackTable WHERE TAG<>'REMOVED' AND PATH<>'REMOVED'", conn);
            conn.Open();
            int rowCount = (int)cmd.ExecuteScalar();
            conn.Close();
            cmd = new SqlCommand("SELECT SUM(CAST(FileSize as BIGINT)) FROM PackTable WHERE TAG<>'REMOVED' AND PATH<>'REMOVED'", conn);
            conn.Open();
            long packSize = (long)cmd.ExecuteScalar();
            conn.Close();
            cmd = new SqlCommand("SELECT COUNT(*) FROM PackTable WHERE Loc='TEMP'", conn);
            conn.Open();
            int rowCountTemp = (int)cmd.ExecuteScalar();
            conn.Close();
            Statistic.InnerHtml = $"Количество файлов в паке: {rowCount} <br/>Вес пака: {((packSize / 1024f) / 1024f )} мб <br/> Количество файлов на проверке: { rowCountTemp}";
            LogDiv.InnerHtml = log;
        }
        protected string GetFillesList(Stream fs)
        {
            bool answer = true;
            var FileHash = BitConverter.ToString(SHA256.Create().ComputeHash(fs));
            string sereachstring = $"SELECT * FROM PackTable WHERE Hash='{FileHash}';";
            cmd = new SqlCommand(sereachstring, conn);
            conn.Open();
            var result = cmd.ExecuteScalar();
            answer = result != null ? (int)result > 0 : false;
            conn.Close();
            if(answer)
            {
                sereachstring = $"SELECT * FROM PackTable WHERE Hash='{FileHash}' AND Loc='Deni';";
                cmd = new SqlCommand(sereachstring, conn);
                conn.Open();
                result = cmd.ExecuteScalar();
                answer = result != null ? (int)result > 0 : false;
                conn.Close();
                if (answer)
                {
                    return "-1";
                }
                else
                {
                    return "0";
                }
            }
            else
            {
                return FileHash;
            }

        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (FileUploadControl.HasFile)
            {
                try
                {
                    DirectoryInfo UploadDirInfo = new DirectoryInfo(@"H:\webm\FromSite\OnCheck\");
                    FileInfo[] files = UploadDirInfo.GetFiles();
                    if (files.Count() >= 100)
                    {
                        StatusLabel.Text = "Файлов на проверке больше 100! Загрузка файлов закрыта!";
                        return;
                    }
                    LogDiv.InnerHtml = string.Empty;
                    HttpFileCollection uploadedFiles = Request.Files;
                    if(uploadedFiles.Count>10)
                    {
                        StatusLabel.Text += $"<font color=\"red\">Слишком много файлов для одной загрузки!</font><br/>";
                        return;
                    }
                    for(int i = 0;i < uploadedFiles.Count;i++)
                    {
                        HttpPostedFile file = uploadedFiles[i];
                        string filename = Path.GetFileName(file.FileName);
                        if (file.ContentType == "video/webm" || file.ContentType == "video/mp4")
                        {

                            if (file.ContentLength < 8388100)
                            {
                                string FileHash = GetFillesList(file.InputStream);
                                if (FileHash=="0"|| FileHash == "-1")
                                {
                                    if (FileHash == "0")
                                    {
                                        StatusLabel.Text += $"<font color=\"red\">Файл {filename} уже есть в паке!</font><br/>";
                                    }
                                    if(FileHash == "-1")
                                    {
                                        StatusLabel.Text += $"<font color=\"red\">Файл {filename} Запрещён к загрузке!</font><br/>";
                                    }
                                
                                    
                                }
                                else
                                {

                                    string tag = "Untag";
                                    string oldfilename = Path.GetFileName(file.FileName);
                                    string formant = Path.GetExtension(file.FileName);
                                    filename = Guid.NewGuid().ToString();
                                    string path = @"H:\webm\FromSite\OnCheck\" + filename+formant;
                                    file.SaveAs(path);
                                    FileInfo fi = new FileInfo(path);
                                    string sqlstring = $"INSERT INTO PackTable (Path, Hash, Loader, TAG,Loc,FileSize) VALUES('{path.Replace("'", "''")}','{FileHash}','{Nickname.Text.Replace("'", "''")}','{tag}','TEMP','{fi.Length}');";
                                    conn.Open();
                                    cmd = new SqlCommand(sqlstring, conn);
                                    cmd.ExecuteNonQuery();
                                    conn.Close();
                                    StatusLabel.Text += $"<font color=\"green\">Файл {oldfilename} загружен!</font><br/>";
                                }                                      
                            }
                            else
                                StatusLabel.Text += $"<font color=\"red\">Файл {filename} больше 8388100 байт!</font><br/>";
                        }
                        else
                        {
                            StatusLabel.Text += $"<font color=\"red\">Формат файла {filename} не подерживается! Можно загрузить только видео в формате webm или mp4!</font><br/>";
                        }


                    }
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = "Нельзя загрузить файл! Причина: " + ex.Message;
                }
             
            }
        }
    }
}