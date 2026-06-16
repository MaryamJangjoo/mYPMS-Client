#nullable disable
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mYPMS.Models
{
    public partial class tTraffic
    {
        [Key]
        [DisplayName("شناسه")]
        public Guid xId { get; set; }
        [DisplayName("شناسه پارکینگ")]
        public Guid? xParkingId { get; set; }
        [ForeignKey("xParkingId")]
        public virtual tParking xParking { get; set; }
        [StringLength(20)]
        [DisplayName("شناسه کارت")]
        public string xTagId { get; set; }
        [Column(TypeName = "datetime2")]
        [DisplayName("زمان ورود")]
        public DateTime? xEntryDateTime { get; set; }
        [Column(TypeName = "datetime2")]
        [DisplayName("زمان خروج")]
        public DateTime? xDepartureDateTime { get; set; }
        [DisplayName("اپراتور ورود")]
        public string xEntryOperator { get; set; }
        [DisplayName("اپراتور خروج")]
        public string xDepartureOperator { get; set; }
        [StringLength(20)]
        [DisplayName("پلاک ورود")]
        public string xLicencePlateEn { get; set; }
        [StringLength(20)]
        [DisplayName("پلاک خروج")]
        public string xLicencePlateEx { get; set; }
        [DisplayName("ویژگی ها")]
        public string xProperties { get; set; }
        [DisplayName("کد وضعیت")]
        public int? xStatusCode { get; set; }
        [DisplayName("دروازه ورودی")]
        public Guid? xEntryGateId { get; set; }
        [ForeignKey("xEntryGateId")]
        public virtual tGate xEntryGate { get; set; }
        [DisplayName("دروازه خروجی")]
        public Guid? xDepartureGateId { get; set; }
        [ForeignKey("xDepartureGateId")]
        public virtual tGate xDepartureGate { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        [DisplayName("مبلغ")]
        public decimal? xPaid { get; set; }
        [DisplayName("شیوه پرداخت")]
        public Guid? xPaidMethodId { get; set; }
        [ForeignKey("xPaidMethodId")]
        public virtual tPaymentMethod xPaidMethod { get; set; }
        [DisplayName("تعرفه")]
        public Guid? xTariffId { get; set; }
        [ForeignKey("xTariffId")]
        public virtual tTariff xTariff { get; set; }

    }
}