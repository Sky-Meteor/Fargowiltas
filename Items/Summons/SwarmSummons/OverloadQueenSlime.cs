using Fargowiltas.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Items.Summons.SwarmSummons
{
    public class OverloadQueenSlime : SwarmSummonBase
    {
        public OverloadQueenSlime() : base(NPCID.QueenSlimeBoss, FargoUtils.IsChinese() ? "欢迎来到名副其实的史莱姆雨！" : "Welcome to the truer slime rain!", 25, "JellyCrystal")
        {
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swarm Crystal");
            Tooltip.SetDefault("Summons several Queen Slimes\nOnly Treasure Bags will be dropped");
        }

        public override bool CanUseItem(Player player)
        {
            return !Fargowiltas.SwarmActive;
        }
    }
}