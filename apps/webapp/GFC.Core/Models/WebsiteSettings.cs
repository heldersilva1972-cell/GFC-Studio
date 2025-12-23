// [NEW]
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class WebsiteSettings
    {
        [Key]
        public int Id { get; set; }

        public string ClubPhone { get; set; }

        public string ClubAddress { get; set; }

        public bool MasterEmailKillSwitch { get; set; }

        public decimal MemberRate { get; set; }

        public decimal NonMemberRate { get; set; }
    }
}
