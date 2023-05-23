using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Items.Summons.Mutant
{
    public class FleshyDoll : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fleshy Doll");
            /* Tooltip.SetDefault("Summons the Wall of Flesh" +
                               "\nMake sure you use it in the Underworld"); */
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 20;
            Item.value = Item.sellPrice(0, 0, 2);
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.position.Y / 16 > Main.maxTilesY - 200 && !NPC.AnyNPCs(NPCID.WallofFlesh);
        }

        public override bool? UseItem(Player player)
        {
            NPC.SpawnWOF(player.Center);
            SoundEngine.PlaySound(SoundID.Roar, player.position);

            return true;
        }

        public override void PostUpdate()
        {
            if (Item.lavaWet && !NPC.AnyNPCs(NPCID.WallofFlesh))
            {
                NPC.SpawnWOF(Item.position);
                Item.TurnToAir();
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
               .AddIngredient(ItemID.GuideVoodooDoll)
               .AddTile(TileID.WorkBenches)
               .Register();
        }
    }
}