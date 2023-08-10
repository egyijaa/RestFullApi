using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestFullApi.Models
{
    public class CustomPages
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public String? CustomUrl { get; set; }
        public String? PageContent { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        //[ForeignKey("CreatedBy")]
        //public Users UsersCreated { get; set; }

        //[ForeignKey("UpdatedBy")]
        //public Users UsersUpdated { get; set; }
    }
}
