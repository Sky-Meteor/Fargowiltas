using Terraria;
using Terraria.ID;

namespace Fargowiltas.Items.Summons.Deviantt
{
    public class WormSnack : BaseSummon
    {
        public override int NPCType => Main.hardMode ? NPCID.DiggerHead : NPCID.GiantWormHead;

        public override string NPCName => FargoUtils.IsChinese() ? (Main.hardMode ? "挖掘怪" : "巨型蠕虫") : (Main.hardMode ? "Digger" : "Giant Worm");

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Worm Snack");
            Tooltip.SetDefault(@"Summons Giant Worm in pre-Hardmode
Summons Digger in Hardmode
Only usable underground");
        }

        public override bool CanUseItem(Player player)
        {
            return player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight || player.ZoneUnderworldHeight;
        }
    }
}