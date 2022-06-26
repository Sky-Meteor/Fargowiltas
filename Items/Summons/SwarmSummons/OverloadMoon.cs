using Fargowiltas.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Items.Summons.SwarmSummons
{
    public class OverloadMoon : SwarmSummonBase
    {
        public OverloadMoon() : base(NPCID.MoonLordCore, FargoUtils.IsChinese() ? "逼近死亡之风呢喃不已……" : "The wind whispers of death's approach!", 20, "CelestialSigil2")
        {
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Runic Inscription");
            Tooltip.SetDefault("Summons several Moon Lords\nOnly Treasure Bags will be dropped");
        }

        public override bool CanUseItem(Player player)
        {
            return !Fargowiltas.SwarmActive;
        }
    }
}