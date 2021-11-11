using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models.Entity
{
    [Table("Articles")]
    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public string ArticlePicture { get; set; }

        [Required]
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        [ForeignKey("Author")]
        public int AuthorId { get; set; }

        [Required]
        public User Author { get; set; }
    }
}
