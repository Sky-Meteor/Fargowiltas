using Fargowiltas.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Items.Summons.SwarmSummons
{
    public class OverloadDarkMage : SwarmSummonBase
    {
        public OverloadDarkMage() : base(NPCID.DD2DarkMageT1, FargoUtils.IsChinese() ? "你感觉你身在图书馆！" : "You feel like you're in a library!", 50, "ForbiddenTome")
        {
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Really Forbidden Tome");
            Tooltip.SetDefault("Summons several Dark Mages\nOnly Treasure Bags will be dropped");
        }

        public override bool CanUseItem(Player player)
        {
            return !Fargowiltas.SwarmActive;
        }
    }
}
