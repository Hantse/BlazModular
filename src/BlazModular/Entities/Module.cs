using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazModular.Entities
{
    [Table("Modules")]
    public class Module
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string PackageId { get; set; }

        [Required]
        public string ActivatedVersion { get; set; }

        [Required]
        public bool Activated { get; set; } = true;

        [Required]
        public byte[] RawAssembly { get; set; }
    }
}
