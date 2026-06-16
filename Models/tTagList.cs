#nullable disable
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mYPMS.Models
{
    public partial class tTagList
    {
        [Key]
        [StringLength(20)]
        [DisplayName("شناسه کارت")]
        public string xId { get; set; }
        [Column(TypeName = "datetime2")]
        [DisplayName("تاریخ انقضاء")]
        public DateTime? xExpirationDate { get; set; }
        [DisplayName("توضیحات")]
        public string xComment { get; set; }
        [DisplayName("گروه تعرفه")]
        public Guid? xTagGroupId { get; set; }
        [ForeignKey("xTagGroupId")]
        public virtual tTagGroup xTagGroup { get; set; }
    }
}