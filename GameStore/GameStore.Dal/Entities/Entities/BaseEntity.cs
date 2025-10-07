using System.ComponentModel.DataAnnotations;

namespace GameStore.Dal.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public bool IsDeleted { get; set; }
    }
}
