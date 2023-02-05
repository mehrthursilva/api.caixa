
namespace pnb.api.easybox.domain.Model
{
    public class Users
    {
       public int idUserEntreprise { get; set; }
       public int idUserProfile { get; set; }
       public string? numberPhone { get; set; }
       public string? userName { get; set; }
       public string? email { get; set; }
       public string? profileType { get; set; }
       public string? password { get; set; }
       public DateTime dateCreation { get; set; }
        public string? hookTeams { get; set; }
        public bool? isAdministrator { get; set; }
        public bool? isActive { get; set; }
        public string? permission { get; set; }
        public DateTime? lastLogin { get; set; }
    }
}
