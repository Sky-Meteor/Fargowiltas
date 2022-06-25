using Terraria;
using Terraria.ID;

namespace Fargowiltas.Items.Summons.Deviantt
{
    public class HemoclawCrab : BaseSummon
    {
        public override int NPCType => NPCID.GoblinShark;

        public override string NPCName => FargoUtils.IsChinese() ? "血浆哥布林鲨鱼" : "Hemogoblin Shark";

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Hemoclaw Crab");
            Tooltip.SetDefault("Summons Hemogoblin Shark" +
                               "\nOnly usable during Blood Moon");
        }

        public override bool CanUseItem(Player player)
        {
            return !Main.dayTime && Main.bloodMoon;
        }
    }
}