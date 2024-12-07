using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFA.Storage
{
    public class TopicEntity
    {
        [Key]
        public Guid Id { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        public Guid ForumId { get; set; }
        public Guid AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; }

        [ForeignKey(nameof(ForumId))]
        public ForumEntity Forum { get; set; }

        [InverseProperty(nameof(Comment.Topic))]
        public ICollection<Comment> Comments { get; set; }
    }
}