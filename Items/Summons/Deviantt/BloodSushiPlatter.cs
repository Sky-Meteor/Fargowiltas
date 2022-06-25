using Terraria;
using Terraria.ID;

namespace Fargowiltas.Items.Summons.Deviantt
{
    public class BloodSushiPlatter : BaseSummon
    {
        public override int NPCType => NPCID.BloodNautilus;

        public override string NPCName => FargoUtils.IsChinese() ? "恐惧鹦鹉螺" : "Dreadnautilus";

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Blood Sushi Platter");
            Tooltip.SetDefault("Summons Dreadnautilus" +
                               "\nOnly usable during Blood Moon");
        }

        public override bool CanUseItem(Player player)
        {
            return !Main.dayTime && Main.bloodMoon;
        }
    }
}