using Fargowiltas.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Items.Summons.SwarmSummons
{
    public class OverloadFish : SwarmSummonBase
    {
        public OverloadFish() : base(NPCID.DukeFishron, FargoUtils.IsChinese() ? "海洋被凶猛的猪们搅的天翻地覆！" : "The ocean swells with ferocious pigs!", 25, "TruffleWorm2")
        {
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Truffle Worm Clump");
            Tooltip.SetDefault("Summons several Duke Fishrons\nOnly Treasure Bags will be dropped");
        }

        public override bool CanUseItem(Player player)
        {
            return !Fargowiltas.SwarmActive;
        }
    }
}