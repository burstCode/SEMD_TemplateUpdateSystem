using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TemplatesWebsite.Models
{
    public class Template
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Template_Id { get; set; }
        public string Template_Filename { get; set; }
        public DateTime Template_LastUpdated { get; set; }
        public string Template_Version { get; set; }
        public byte[] Template_Content { get; set; }
    }
}
