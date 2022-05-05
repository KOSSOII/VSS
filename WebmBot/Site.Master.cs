using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace WebmBot
{
    public partial class SiteMaster : MasterPage
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebmDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
           if(Page.User.Identity.IsAuthenticated)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "sessionStorage.setItem('status', 'loggedIn');", true);
            }
           if(!IsPostBack)
            {
                string ipadrr = GetIPAddress();
                SqlCommand cmd = new SqlCommand($"UPDATE IpLog SET [IP]='{ipadrr}',[DateTime]='{DateTime.Now}' WHERE [IP]='{ipadrr}' IF @@ROWCOUNT = 0 INSERT INTO IpLog(IP, DateTime) VALUES('{ipadrr}', '{DateTime.Now}')", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            this.DataBind();
        }

        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
    }

}