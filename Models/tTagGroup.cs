#nullable disable
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mYPMS.Models
{
    public partial class tTagGroup
    {
        [Key]
        [DisplayName("شناسه")]
        public Guid xId { get; set; }
        [StringLength(50)]
        [DisplayName("نام")]
        public string xName { get; set; }
        [DisplayName("تعرفه")]
        public Guid? xTariffId { get; set; }
        [ForeignKey("xTariffId")]
        public virtual tTariff xTariff { get; set; }
        public virtual List<tTagList> xTagList { get; }
    }
}