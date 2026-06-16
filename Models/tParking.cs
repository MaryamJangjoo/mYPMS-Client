#nullable disable
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mYPMS.Models
{
    public partial class tParking
    {
        [Key]
        [DisplayName("شناسه")]
        public Guid xId { get; set; }
        [StringLength(10)]
        [DisplayName("کد")]
        public string xCode { get; set; }
        [StringLength(50)]
        [DisplayName("عنوان")]
        public string xTitle { get; set; }
        [StringLength(200)]
        [DisplayName("آدرس")]
        public string xAddress { get; set; }
        [StringLength(20)]
        [DisplayName("تلفن")]
        public string xPhone { get; set; }
        [StringLength(50)]
        [DisplayName("مختصات")]
        public string xCordination { get; set; }
        [DisplayName("ظرفیت")]
        public int? xCapacity { get; set; }
        [DisplayName("رزور")]
        public int? xReservedCapacity { get; set; }
        [StringLength(50)]
        [DisplayName("کلمه عبور")]
        public string xPassword { get; set; }
        [DisplayName("پیمانکار")]
        public string xContractor { get; set; }
        [DisplayName("توضیحات")]
        public string xDescription { get; set; }
        [DisplayName("کد وضعیت")]
        public int? xStatusCode { get; set; }
        [DisplayName("تنظیمات")]
        public string xSettings { get; set; }

        public Guid? xTariffId { get; set; }
        [ForeignKey("xTariffId")]
        public virtual tTariff xTariff { get; set; }
        [DisplayName("دروازه ها")]
        public virtual List<tGate> xGates { get; }
        //public virtual List<tTraffic> xTraffics { get; }
    }
}