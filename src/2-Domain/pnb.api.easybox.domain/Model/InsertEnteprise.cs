using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace pnb.api.easybox.domain.Model
{
    public class InsertEnteprise
    {
        public string? userNameEp { get; set; }
        public string? documentEp { get; set; }
        public string? addressEp { get; set; }
        public string? zipEp { get; set; }
        public string? emailEp { get; set; }
        public string? countryEp { get; set; }
        public string? numberPhoneEp { get; set; }
        public string? passwordEp { get; set; }
        public string? webhookUrlEp { get; set; }     
        public string? hookTeamsEp { get; set; }
        public string? userNameUserProfile { get; set; }
        public string? emailUserUserProfile { get; set; }
        public string? profileTypeUserProfile { get; set; }
        public string? passwordUserProfile { get; set; }
        public string? numberPhoneUserProfile { get; set; }
        public string? hookTeamsUserProfile { get; set; }
        public bool? isAdministratorUserProfile { get; set; }
        public bool? isActiveUserProfile { get; set; }
        public string? permissionUserProfile { get; set; }
        public DateTime dateCreation { get; set; }
    }
}
