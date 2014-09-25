using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mail_Phishing.Mailer
{
    // <summary>
    /// This enum will hold the type of a Distrinution List 
    /// DL for Normal Distribution List 
    /// DLL Dynamic Distribution List
    /// </summary>
    public enum DLT { DL = 1, DDL = 2 }

    /// <summary>
    /// This Class will Hold all the information about DL or DDL 
    /// and based on the DLT it will call the disignated function to return the list of email address
    /// </summary>
    public class DistributionList
    {
        public DLT DType { get; set; }
        public string CN { get; set; }
        public string FILORDN { get; set; }

        public string DTypeDescription {
            get { 
                return (DType.Equals(DLT.DL) ? "Static" : "Dynamic");
            }
        }
    }
}
