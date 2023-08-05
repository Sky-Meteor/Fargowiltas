using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Items.Misc
{
	public class DeathFruit : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Death Fruit");
			// Tooltip.SetDefault("Permanently decreases maximum life by 20\nEffects may not be reversible for characters that have used modded life-increasing items");
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.maxStack = 99;
			Item.rare = 1;
			Item.useStyle = 4;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.consumable = true;

			Item.UseSound = SoundID.Item27;
		}

		public override bool ConsumeItem (Player player)
		{
			if (player.statLifeMax <= 20)
				return false;

			return true;
		}

		public override bool? UseItem(Player player)
		{
			if (player.itemAnimation > 0 && player.statLifeMax > 20 && player.itemTime == 0)
			{
				player.statLifeMax -= 20;
				player.statLifeMax2 -= 20;
				player.statLife -= 20;
				if (Main.myPlayer == player.whoAmI)
				{
					player.HealEffect(-20, true);
				}
			}

			return true;
		}
	}
}
