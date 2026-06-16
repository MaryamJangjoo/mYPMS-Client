#nullable disable
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace mYPMS.Models
{
    public partial class tSetting
    {
        [Key]
        [DisplayName("شناسه")]
        public Guid xId { get; set; }
        [StringLength(50)]
        [DisplayName("نام")]
        public string xName { get; set; }
        [DisplayName("مقدار")]
        public string xValue { get; set; }
        [DisplayName("رابطه")]
        public Guid? xRelationship { get; set; }
    }
}