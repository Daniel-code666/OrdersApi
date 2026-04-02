using System.ComponentModel.DataAnnotations.Schema;

namespace Order.Domain.Entitites.Base
{
    public class AuditTable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public DateTime ModificationDate { get; set; }
    }
}
