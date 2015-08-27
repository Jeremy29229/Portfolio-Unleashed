using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMEditingCollaborators
    {
        public List<VMContribution> Collaborators { get; set; }
        public List<VMQuickContact> QuickContacts { get; set; }

        public VMEditingCollaborators(IEnumerable<Contribution> contributions, IEnumerable<User> quickContacts)
        {
            Collaborators = new List<VMContribution>();
            if (contributions != null && contributions.Count() > 0)
            {
                foreach (Contribution c in contributions)
                {
                    Collaborators.Add(new VMContribution(c));
                }
            }

            QuickContacts = new List<VMQuickContact>();
            if (quickContacts != null && quickContacts.Count() > 0)
            {
                foreach (User u in quickContacts)
                {
                    QuickContacts.Add(new VMQuickContact(u));
                }
            }
        }
        public VMEditingCollaborators()
        {
            Collaborators = new List<VMContribution>();
            QuickContacts = new List<VMQuickContact>();
        }
    }
}