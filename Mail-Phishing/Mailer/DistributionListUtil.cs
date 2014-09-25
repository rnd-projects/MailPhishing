using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using outlook = Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Interop.Outlook;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.DirectoryServices;


namespace Mail_Phishing.Mailer
{
    public class DistributionListUtil
    {

        private outlook.Application OlApp = (outlook.Application)Marshal.GetActiveObject("Outlook.Application");

        /// <summary>
        /// Extract whats in the Distribution List by showing the outlook Global Address List Search
        /// </summary>
        /// <returns></returns>
        public List<string> GetDistributionListMembers()
        {
            List<string> emails = new List<string>();

            outlook.SelectNamesDialog snd = OlApp.Application.Session.GetSelectNamesDialog();
            outlook.AddressLists addrLists = OlApp.Application.Session.AddressLists;

            //foreach (outlook.AddressList addrList in addrLists)
            //{
            //    if (addrList.Name == "All Groups")
            //    {
            //        snd.InitialAddressList = addrList;
            //        break;
            //    }
            //}
            snd.NumberOfRecipientSelectors = outlook.OlRecipientSelectors.olShowTo;
            snd.ToLabel = "To";
            snd.ShowOnlyInitialAddressList = true;
            snd.AllowMultipleSelection = false;
            snd.Display();

            if (snd.Recipients.Count > 0)
            {
                outlook.AddressEntry addrEntry = snd.Recipients[1].AddressEntry;

                if (addrEntry.AddressEntryUserType == outlook.OlAddressEntryUserType.olExchangeDistributionListAddressEntry ||
                    addrEntry.AddressEntryUserType == outlook.OlAddressEntryUserType.olOutlookDistributionListAddressEntry ||
                    addrEntry.AddressEntryUserType == outlook.OlAddressEntryUserType.olExchangeAgentAddressEntry)
                {
                    outlook.ExchangeDistributionList exchDL = addrEntry.GetExchangeDistributionList();
                    outlook.AddressEntries addrEntries = exchDL.GetExchangeDistributionListMembers();

                    if (addrEntries != null)

                        foreach (outlook.AddressEntry exchDLMember in addrEntries)
                        {
                            string contact = exchDLMember.GetExchangeUser().PrimarySmtpAddress;

                            if (contact != null) { emails.Add(contact); }
                        }
                }
            }

            return emails;

        }

        public void GetCurrentUserMembership()
        {
            outlook.AddressEntry currentUser = OlApp.Application.Session.CurrentUser.AddressEntry;

            if (currentUser.Type == "EX")
            {
                outlook.ExchangeUser exchUser = currentUser.GetExchangeUser();
                if (exchUser != null)
                {
                    outlook.AddressEntries addrEntries = exchUser.GetMemberOfList();
                    if (addrEntries != null)
                    {
                        foreach (outlook.AddressEntry addrEntry in addrEntries)
                        {
                            Debug.WriteLine(addrEntry.Name);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the List of A dynamic distribution Group in Key Value Pairs 
        /// the key is the CN
        /// the value is the filter for the membership of that group
        /// </summary>
        /// <returns>Dictionary of strings </returns>
        public List<DistributionList> GetDynamicDistributionLists()
        {
            List<DistributionList> distrubutionLists = new List<DistributionList>();

            using (var group = new DirectoryEntry("GC://dc=ccg,dc=local"))
            {

                using (var searchRoot = new DirectoryEntry("GC://10.1.0.230/dc=ccg,dc=local"))
                using (var searcher = new DirectorySearcher(searchRoot, "(ObjectClass=msExchDynamicDistributionList)"))
                using (var results = searcher.FindAll())
                {
                    foreach (SearchResult result in results)
                    {
                        if (result.Properties.Contains("cn") && result.Properties.Contains("msExchDynamicDLFilter"))
                        {
                            DistributionList dl = new DistributionList();

                            dl.DType = DLT.DDL;
                            dl.CN = result.Properties["cn"][0].ToString();
                            dl.FILORDN = result.Properties["msExchDynamicDLFilter"][0].ToString();

                            distrubutionLists.Add(dl);
                        }
                    }
                }

            }

            return distrubutionLists;

        }

        /// <summary>
        /// Get the List of A distribution Group in Key Value Pairs
        /// the key is the CN
        /// the value is the DN of the Group
        /// </summary>
        /// <returns>Dictionary of strings</returns>
        public List<DistributionList> GetDistributionLists()
        {
            List<DistributionList> distrubutionLists = new List<DistributionList>();

            using (var group = new DirectoryEntry("GC://dc=ccg,dc=local"))
            {

                using (var searchRoot = new DirectoryEntry("GC://10.1.0.230/dc=ccg,dc=local"))
                using (var searcher = new DirectorySearcher(searchRoot, "(&(objectCategory=group)(!groupType:1.2.840.113556.1.4.803:=2147483648))"))
                using (var results = searcher.FindAll())
                {
                    foreach (SearchResult result in results)
                    {
                        if (result.Properties.Contains("cn") && result.Properties.Contains("distinguishedName"))
                        {
                            DistributionList dl = new DistributionList();

                            dl.DType = DLT.DL;
                            dl.CN = result.Properties["cn"][0].ToString();
                            dl.FILORDN = result.Properties["distinguishedName"][0].ToString();

                            distrubutionLists.Add(dl);
                        }
                    }
                }

            }

            return distrubutionLists;
        }

        /// <summary>
        /// List Distribution List Members
        /// </summary>
        /// <param name="dn"> The distinguishedName of the Group </param>
        /// <returns>List of Email Addresses</returns>
        public List<string> listDLMembers(string dn)
        {
            List<string> addresses = new List<string>();

            using (var group = new DirectoryEntry("GC://dc=ccg,dc=local"))
            {

                using (var searchRoot = new DirectoryEntry("GC://10.1.0.230/dc=ccg,dc=local"))
                using (var searcher = new DirectorySearcher(searchRoot, "(&(objectCategory=person)(|(objectClass=contact)(objectClass=user))(memberOf=" + dn + "))"))
                using (var results = searcher.FindAll())
                {
                    foreach (SearchResult result in results)
                    {
                        if (result.Properties.Contains("mail"))
                            addresses.Add(result.Properties["mail"][0].ToString());
                    }
                }
            }
            return addresses;
        }

        /// <summary>
        /// List Dynamic Distribution List Members
        /// </summary>
        /// <param name="filter"> Filter to be used in the LDAP Query</param>
        /// <returns>List Of Email Addresses</returns>
        public List<string> listDDLMembers(string filter)
        {
            // The filter is the value of the previous dictionary

            List<string> addresses = new List<string>();

            using (var group = new DirectoryEntry("GC://dc=ccg,dc=local"))
            {

                using (var searchRoot = new DirectoryEntry("GC://10.1.0.230/dc=ccg,dc=local"))
                using (var searcher = new DirectorySearcher(searchRoot, filter))
                using (var results = searcher.FindAll())
                {
                    foreach (SearchResult result in results)
                    {
                        if (result.Properties.Contains("mail"))
                            addresses.Add(result.Properties["mail"][0].ToString());
                    }
                }
            }
            return addresses;
        }
    }

}
