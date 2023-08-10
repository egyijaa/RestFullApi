using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestFullApi.Models
{
    public class News
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Title { get; set; }
        public String? NewsContent { get; set; }
        public int CategoryId { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        //[ForeignKey("CategoryId")]
        //public Category categoryId { get; set; }

        //[ForeignKey("CreatedBy")]
        //public Users UsersCreated { get; set; }

        //[ForeignKey("UpdatedBy")]
        //public Users UsersUpdated { get; set; }
    }
}
