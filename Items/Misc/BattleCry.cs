using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Items.Misc
{
    public class BattleCry : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Battle Cry");
            Tooltip.SetDefault("Left click to toggle 10x increased spawn rates" +
                               "\nRight click to toggle 10x decreased spawn rates");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 38;
            Item.value = Item.sellPrice(0, 0, 2);
            Item.rare = ItemRarityID.Pink;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
        }

        public override bool AltFunctionUse(Player player) => true;

        void ToggleCry(bool isBattle, string playerName, ref bool cry)
        {
            cry = !cry;
            string cryToggled = isBattle ? "Battle" : "Calming";
            string toggle = cry ? "activated" : "deactivated";
            string punctuation = isBattle ? "!" : ".";
            Color color = isBattle ? new Color(255, 0, 0) : new Color(0, 255, 255);
            FargoUtils.PrintText($"{cryToggled} Cry {toggle} for {playerName}{punctuation}", color);
        }

        public override bool? UseItem(Player player)
        {
            FargoPlayer modPlayer = player.GetFargoPlayer();
            if (player.altFunctionUse == 2)
            {
                if (modPlayer.BattleCry)
                    ToggleCry(true, player.name, ref modPlayer.BattleCry);

                ToggleCry(false, player.name, ref modPlayer.CalmingCry);
            }
            else
            {
                if (modPlayer.CalmingCry)
                    ToggleCry(false, player.name, ref modPlayer.CalmingCry);

                ToggleCry(true, player.name, ref modPlayer.BattleCry);
            }

            if (!Main.dedServ)
                SoundEngine.PlaySound(new SoundStyle("Fargowiltas/Sounds/Horn"), player.Center);

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BattlePotion, 15)
                .AddIngredient(ItemID.WaterCandle, 5)
                .AddIngredient(ItemID.CalmingPotion, 15)
                .AddIngredient(ItemID.PeaceCandle, 5)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}