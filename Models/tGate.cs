#nullable disable
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mYPMS.Models
{
    public partial class tGate
    {
        [Key]
        [DisplayName("شناسه")]
        public Guid xId { get; set; }
        [StringLength(30)]
        [DisplayName("نام")]
        public string xName { get; set; }
        [DisplayName("جهت تردد")]
        public int? xDirection { get; set; }
        [DisplayName("آدرس ویدئو ورودی")]
        public string xVideoInUrl { get; set; }
        [DisplayName("آدرس ویدئو خروجی")]
        public string xVideoOutUrl { get; set; }
        [DisplayName("آدرس تصویر ورودی")]
        public string xImageInUrl { get; set; }
        [DisplayName("آدرس تصویر خروجی")]
        public string xImageOutUrl { get; set; }
        [DisplayName("آدرس راهبند ورودی")]
        public string xBarrierInUrl { get; set; }
        [DisplayName("آدرس راهبند خروجی")]
        public string xBarrierOutUrl { get; set; }
        [DisplayName("توضیحات")]
        public string xDescription { get; set; }
        [DisplayName("شناسه پارکینگ")]
        public Guid? xParkingId { get; set; }
        [ForeignKey("xParkingId")]
        public virtual tParking xParking { get; set; }
        //public virtual List<tTraffic> xEntryGates { get; }
        //public virtual List<tTraffic> xDepartureGates { get; }
    }
}