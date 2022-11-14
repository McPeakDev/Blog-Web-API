namespace BlogAPI.Models {
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    /// <summary>
    /// Defines the needed parameters to perform CRUD operations with a blog post.
    /// </summary>
    [Table("Post")]
    public class Post {
        /// <summary>
        /// The blog post's DB generated id.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// The blog post's title
        /// </summary>
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        /// <summary>
        /// The blog post's story
        /// </summary>
        [Required(ErrorMessage = "Body is required.")]
        public string Body { get; set; }
    }
}
