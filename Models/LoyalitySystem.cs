using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyLine.Models
{

    public enum Tier
    {
        Silver,
        Gold,
        Platinum,
        Diamond
    }

    public class LoyalitySystem
    {
        [Key]
        public int Loyality_Id_PK { get; set; }

        [ForeignKey(nameof(User))]
        public string User_Id_FK { get; set; } = string.Empty;

        public int Points { get; set; }
        public Tier TierLevel { get; set; }

        public ApplicationUser? User { get; set; }
    }
}
