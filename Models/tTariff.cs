#nullable disable
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace mYPMS.Models
{
    public partial class tTariff
    {
        [Key]
        [DisplayName("شناسه")]
        public Guid xId { get; set; }
        [StringLength(50)]
        [DisplayName("عنوان")]
        public string xTitle { get; set; }
        [DisplayName("ورودی تا یک ساعت")]
        public int? xEntryPrice { get; set; }
        [DisplayName("هر ساعت روزانه")]
        public int? xDayHourPrice { get; set; }
        [DisplayName("هر ساعت شبانه")]
        public int? xNightHourPrice { get; set; }
        [DisplayName("هر 24 ساعت")]
        public int? xWholeDayPrice { get; set; }
        [DisplayName("شروع ساعت شبانه")]
        public int? xNightStartHour { get; set; }
        [DisplayName("پایان ساعت شبانه")]
        public int? xNightEndHour { get; set; }
        [DisplayName("توضیحات")]
        public string xDescription { get; set; }
        //public virtual List<tParking> xParkings { get; }
        //public virtual List<tTraffic> xTraffics { get; }
    }
}
