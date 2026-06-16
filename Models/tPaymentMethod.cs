#nullable disable
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace mYPMS.Models
{
    public partial class tPaymentMethod
    {
        [Key]
        [DisplayName("شناسه")]
        public Guid xId { get; set; }
        [StringLength(50)]
        [DisplayName("عنوان")]
        public string xPaymentMethod { get; set; }
        //public virtual List<tTraffic> xTraffics { get; }
    }
}