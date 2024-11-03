using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFA.Storage
{
    public class ForumEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        [InverseProperty(nameof(TopicEntity.Forum))]
        public ICollection<TopicEntity> Topics { get; set; }
    }
}