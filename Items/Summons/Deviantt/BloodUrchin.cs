using Terraria;
using Terraria.ID;

namespace Fargowiltas.Items.Summons.Deviantt
{
    public class BloodUrchin : BaseSummon
    {
        public override int NPCType => NPCID.BloodEelHead;

        public override string NPCName => FargoUtils.IsChinese() ? "血鳗鱼" : "Blood Eel";

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Blood Urchin");
            Tooltip.SetDefault("Summons Blood Eel" +
                               "\nOnly usable during Blood Moon");
        }

        public override bool CanUseItem(Player player)
        {
            return !Main.dayTime && Main.bloodMoon;
        }
    }
}