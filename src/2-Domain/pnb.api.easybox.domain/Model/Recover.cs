
namespace pnb.api.easybox.domain.Model
{
    public class Recover
    {
        public int  idUserProfile { get; set; }
        public string?  email { get; set; }
        public string?  defaultKey { get; set; }
        public string?  shortQuestion { get; set; }
        public string?  shortanswer { get; set; }
        public string?  codeForPhone { get; set; }
        public string?  codeForEmail { get; set; }
        public DateTime dateCreation { get; set; }
    }
}
