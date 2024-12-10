using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFA.Storage
{
    public class ForumEntity
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;

        [InverseProperty(nameof(TopicEntity.Forum))]
        public ICollection<TopicEntity> Topics { get; set; }
    }
}