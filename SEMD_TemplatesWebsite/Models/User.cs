using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TemplatesWebsite.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int User_Id { get; set; }
        public string User_Email { get; set; }
        public string User_Name { get; set; }
        public string User_Password { get; set; }
    }
}
