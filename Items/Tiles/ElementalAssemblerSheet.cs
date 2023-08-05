using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Fargowiltas.Items.Tiles
{
    public class ElementalAssemblerSheet : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.Width = 4;
            Main.tileNoAttach[Type] = true;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Elemental Assembler");
            AddMapEntry(new Color(200, 200, 200), name);
            TileID.Sets.DisableSmartCursor[Type] = true;
            //counts as
            AdjTiles = new int[] { TileID.Hellforge, TileID.Furnaces, TileID.AlchemyTable, TileID.TinkerersWorkbench, TileID.ImbuingStation, TileID.DyeVat, TileID.LivingLoom, TileID.GlassKiln, TileID.IceMachine, TileID.HoneyDispenser, TileID.SkyMill, TileID.Solidifier, TileID.BoneWelder, TileID.Bottles, TileID.DemonAltar, TileID.Tombstones };
            
            TileID.Sets.CountsAsHoneySource[Type] = true;
            TileID.Sets.CountsAsLavaSource[Type] = true;

            AnimationFrameHeight = 54;

        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 1f;
            g = 1f;
            b = 1f;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<ElementalAssembler>());
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter >= 8) //replace with duration of frame in ticks
            {
                frameCounter = 0;
                frame++;
                frame %= 12;
            }

            if ((frame >= 1 && frame <= 4))// || frame == 6 || frame == 8)
            {
                Main.tileLighted[Type] = true;
            }
            else
            {
                Main.tileLighted[Type] = false;
            }
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (Main.LocalPlayer.Distance(new Vector2(i * 16 + 8, j * 16 + 8)) < 16 * 5)
            {
                Main.LocalPlayer.GetModPlayer<FargoPlayer>().ElementalAssemblerNearby = 6;
            }
        }
    }
}