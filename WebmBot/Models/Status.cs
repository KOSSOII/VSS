using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebmBot.Models
{
    public class Status
    {
      
        public string authToken { get; set; }
        public string Temperature { get; set; }
        public string Hydro { get; set; }
        public string Server_dor { get; set; }
        public string BPR_dor { get; set; }
        public string Server_sclad_dor { get; set; }
        public string Sclad_dor { get; set; }
        public string Server_sclad_dor_two { get; set; }
        public string BPR_dor_cf { get; set; }
        public string Server_sclad_dor2_two { get; set; }

    }
}