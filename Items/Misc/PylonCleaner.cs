using Fargowiltas.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Items.Misc
{
	public class PylonCleaner : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Pylon Cleaner");
			// Tooltip.SetDefault("Purifies the area around every pylon\nWill use Blue Solution on Hallow Pylon and Green Solution on others");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.maxStack = 99;
			Item.consumable = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 20;
			Item.useTime = 20;
		}

		public override bool? UseItem(Player player)
		{
			if (player.itemAnimation > 0 && player.itemTime == 0 && player.whoAmI == Main.myPlayer)
			{
				//iterate to every pylon, drop a purity renewal
				foreach (TeleportPylonInfo pylonInfo in Main.PylonSystem.Pylons)
                {
				  	Vector2 pos = pylonInfo.PositionInTiles.ToWorldCoordinates();
					
					int projType = ModContent.ProjectileType<PurityNukeProj>();
					if (pylonInfo.TypeOfPylon == TeleportPylonType.Hallow)
						projType = ModContent.ProjectileType<HallowNukeProj>();

					int p = Projectile.NewProjectile(player.GetSource_ItemUse(Item), pos, Vector2.Zero, projType, 0, 0f, Main.myPlayer);
					if (p != Main.maxProjectiles)
						Main.projectile[p].timeLeft = 2;

					if (pylonInfo.TypeOfPylon == TeleportPylonType.GlowingMushroom)
                    {
						projType = ModContent.ProjectileType<MushroomNukeProj>();
						p = Projectile.NewProjectile(player.GetSource_ItemUse(Item), pos, Vector2.Zero, projType, 0, 0f, Main.myPlayer);
						if (p != Main.maxProjectiles)
							Main.projectile[p].timeLeft = 6;
					}
				}
			}

			return true;
		}

        public override void AddRecipes()
        {
			CreateRecipe()
				.AddIngredient(ItemID.PurificationPowder)
				.AddTile(TileID.MythrilAnvil)
				.Register();
        }
    }
}
