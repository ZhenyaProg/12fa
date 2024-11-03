using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFA.Storage
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(20)]
        public string Login { get; set; } = string.Empty;

        [InverseProperty(nameof(TopicEntity.Author))]
        public ICollection<TopicEntity> Topics { get; set; }

        [InverseProperty(nameof(Comment.Author))]
        public ICollection<Comment> Comments { get; set; }
    }
}