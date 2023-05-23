using Fargowiltas.Items.Tiles;
using Fargowiltas.Projectiles.Explosives;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Items.Explosives
{
    public class OmniBridgifier : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Omni-Bridgifier");
            // Tooltip.SetDefault("Can be reused infinitely\nUpgrades the platform you are standing on to use Omnistations\nDoes NOT build a platform!");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 32;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.value = Item.buyPrice(0, 0, 3);
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<OmniBridgifierProj>();
            Item.shootSpeed = 5f;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = player.Bottom;
            position.Y += 8;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float ai0 = -1;
            if (player.inventory.Any(i => !i.IsAir && i.type == ModContent.ItemType<Omnistation>()))
                ai0 = 0;
            if (player.inventory.Any(i => !i.IsAir && i.type == ModContent.ItemType<Omnistation2>()))
                ai0 = ai0 == 0 ? Main.rand.Next(2) : 1; //if have both omnis, pick one randomly
            if (ai0 == -1)
                ai0 = Main.rand.Next(2); //if have neither omni, pick one randomly
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, ai0);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<InstaBridge>())
                .AddTile(ModContent.TileType<OmnistationSheet>())
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<InstaBridge>())
                .AddTile(ModContent.TileType<OmnistationSheet2>())
                .Register();
        }
    }
}