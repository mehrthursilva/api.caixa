
namespace pnb.api.easybox.domain.Model
{
    public class Login
    {
      public int idUser { get; set; }
      public string?  userName  { get; set; }
      public string?  document { get; set; }   
      public string?  address { get; set; }   
      public string?  zip { get; set; }   
      public string?  email { get; set; }   
      public string? country { get; set; }   
      public string?  numberPhone { get; set; }   
      public string?  password { get; set; }
      public string?  webhook { get; set; }
      public string? hookTeams { get; set; }
      public DateTime dateCreation { get; set; }
      public DateTime? lastLogin { get; set; }
    }
}
