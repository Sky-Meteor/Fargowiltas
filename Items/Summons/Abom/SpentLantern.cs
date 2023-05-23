using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace Fargowiltas.Items.Summons.Abom
{
    public class SpentLantern : MatsuriLantern
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Spent Lantern");
            // Tooltip.SetDefault("Deactivates Matsuri Lantern effect");

            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.consumable = false;
        }

        public override bool CanUseItem(Player player) => FargoWorld.Matsuri;

        public override bool? UseItem(Player player)
        {
            FargoWorld.Matsuri = false;
            FargoUtils.PrintText(Language.GetTextValue("Mods.Fargowiltas.MessageInfo.SpentLantern"), new Color(175, 75, 255));
            
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData);

            Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, player.position);

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<MatsuriLantern>())
                .AddCondition(Condition.NearWater)
                .Register();
        }
    }
}