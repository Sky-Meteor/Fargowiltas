using System.Collections.Generic;
using System.Linq;
using Fargowiltas.Items.Tiles;
using Fargowiltas.Items.Vanity;
using Fargowiltas.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Fargowiltas.NPCs
{
    [AutoloadHead]
    public class LumberJack : ModNPC
    {
        private bool dayOver;
        private bool nightOver;

        //public override bool Autoload(ref string name)
        //{
        //    name = "LumberJack";
        //    return mod.Properties.Autoload;
        //}

        public override ITownNPCProfile TownNPCProfile()
        {
            return new LumberJackProfile();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("LumberJack");

            Main.npcFrameCount[NPC.type] = 25;

            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 90;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;
            NPCID.Sets.HatOffsetY[NPC.type] = 2;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Velocity = -1f,
                Direction = -1
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness.SetBiomeAffection<ForestBiome>(AffectionLevel.Love);

            NPC.Happiness.SetNPCAffection<Squirrel>(AffectionLevel.Like);
            NPC.Happiness.SetNPCAffection(NPCID.Dryad, AffectionLevel.Dislike);
            NPC.Happiness.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Hate);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("Mods.Fargowiltas.Bestiary.LumberJack")
            });
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 40;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            AnimationType = NPCID.Guide;

            //if (GetInstance<FargoConfig>().CatchNPCs)
            //{
            //    Main.npcCatchable[NPC.type] = true;
            //    NPC.catchItem = (short)mod.ItemType("LumberJack");
            //}
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            return GetInstance<FargoConfig>().Lumber && FargoWorld.DownedBools.TryGetValue("lumberjack", out bool down) && down;
        }

        public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;

        public override void AI()
        {
            if (!Main.dayTime)
            {
                nightOver = true;
            }

            if (Main.dayTime)
            {
                dayOver = true;
            }
        }

        public override List<string> SetNPCNameList()
        {
            string[] names = { "Griff", "Jack", "Bruce", "Larry", "Will", "Jerry", "Liam", "Stan", "Lee", "Woody", "Leif", "Paul" };

            return new List<string>(names);
        }

        public override string GetChat()
        {
            List<string> dialogue = new List<string>
            {
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.1"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.2"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.3"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.4"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.5"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.6"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.7"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.8"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.9"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.10"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.11"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.12"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.13"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.14"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.15"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.16"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.17"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.18"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.19"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.20")
            };

            int nurse = NPC.FindFirstNPC(NPCID.Nurse);
            if (nurse >= 0)
            {
                dialogue.Add(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.Nurse", Main.npc[nurse].GivenName));
            }

            Player player = Main.LocalPlayer;

            if (player.HeldItem.type == ItemID.LucyTheAxe)
            {
                dialogue.Add(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.LucyTheAxe"));
            }

            return Main.rand.Next(dialogue);
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
            button2 = Language.GetTextValue("Mods.Fargowiltas.UI.TreeTreasures");
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            Player player = Main.LocalPlayer;

            if (firstButton)
            {
                shop = true;
                return;
            }

            if (dayOver && nightOver)
            {
                string quote = "";
                int itemType;

                if (player.ZoneDesert && !player.ZoneBeach)
                {
                    quote = Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.Desert");
                    itemType = Main.rand.Next(new int[] { ItemID.Scorpion, ItemID.BlackScorpion });
                    player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType, 5);
                    player.QuickSpawnItem(player.GetSource_OpenItem(ItemID.Cactus), ItemID.Cactus, 100);
                }
                else if (player.ZoneJungle)
                {
                    quote = Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.Jungle");
                    itemType = Main.rand.Next(new int[] { ItemID.Buggy, ItemID.Sluggy, ItemID.Grubby, ItemID.Frog });
                    player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType, 5);
                    itemType = Main.rand.Next(new int[] { ItemID.Mango, ItemID.Pineapple });
                    player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType, 5);
                    player.QuickSpawnItem(player.GetSource_OpenItem(ItemID.RichMahogany), ItemID.RichMahogany, 50);
                }
                else if (player.ZoneHallow)
                {
                    quote = Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.Hallow");
                    for (int i = 0; i < 5; i++)
                    {
                        itemType = Main.rand.Next(new int[] { ItemID.LightningBug, ItemID.FairyCritterBlue, ItemID.FairyCritterGreen, ItemID.FairyCritterPink });
                        player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType);
                    }
                    itemType = Main.rand.Next(new int[] { ItemID.Starfruit, ItemID.Dragonfruit });
                    player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType, 5);
                    player.QuickSpawnItem(player.GetSource_OpenItem(ItemID.Pearlwood), ItemID.Pearlwood, 50);

                    //add prismatic lacewing if post plantera
                }
                else if (player.ZoneGlowshroom && Main.hardMode)
                {
                    quote = Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.Glowshroom");
                    itemType = Main.rand.Next(new int[] { ItemID.GlowingSnail, ItemID.TruffleWorm });
                    player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType, 5);
                    player.QuickSpawnItem(player.GetSource_OpenItem(ItemID.GlowingMushroom), ItemID.GlowingMushroom, 50);
                    //add mushroom grass seeds

                }
                else if (player.ZoneCorrupt || player.ZoneCrimson)
                {
                    quote = Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.EvilBiome");
                    for (int i = 0; i < 5; i++)
                    {
                        itemType = Main.rand.Next(new int[] { ItemID.Elderberry, ItemID.BlackCurrant, ItemID.BloodOrange, ItemID.Rambutan });
                        player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType);
                    }
                }
                else if (player.ZoneSnow)
                {
                    //penguin
                    quote = Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.Snow");
                    itemType = Main.rand.Next(new int[] { ItemID.Cherry, ItemID.Plum });
                    player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType, 5);
                    player.QuickSpawnItem(player.GetSource_OpenItem(ItemID.BorealWood), ItemID.BorealWood, 50);
                }
                else if (player.ZoneBeach)
                {
                    quote = Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.Beach");
                    itemType = Main.rand.Next(new int[] { ItemID.Coconut, ItemID.Banana });
                    player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType, 5);
                    player.QuickSpawnItem(player.GetSource_OpenItem(ItemID.Seagull), ItemID.Seagull, 5);
                    player.QuickSpawnItem(player.GetSource_OpenItem(ItemID.PalmWood), ItemID.PalmWood, 50);
                }
                else if (player.ZoneUnderworldHeight)
                {
                    quote = Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.Underworld");
                    for (int i = 0; i < 5; i++)
                    {
                        itemType = Main.rand.Next(new int[] { ItemID.HellButterfly, ItemID.MagmaSnail, ItemID.Lavafly });
                        player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType);
                    }
                }
                else if (player.ZoneRockLayerHeight || player.ZoneDirtLayerHeight)
                {
					if (Main.rand.NextBool(2))
					{
						quote = Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.Gem");

						for (int i = 0; i < 5; i++)
						{
							itemType = Main.rand.Next(new int[] { ItemID.Diamond, ItemID.Ruby, ItemID.Amethyst, ItemID.Emerald, ItemID.Sapphire, ItemID.Topaz, ItemID.Amber });
							player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType, 3);
							
							itemType = Main.rand.Next(new int[] { ItemID.GemSquirrelDiamond, ItemID.GemSquirrelAmber, ItemID.GemSquirrelAmethyst, ItemID.GemSquirrelEmerald, ItemID.GemSquirrelRuby, ItemID.GemSquirrelSapphire, ItemID.GemSquirrelTopaz, ItemID.GemBunnyAmber, ItemID.GemBunnyAmethyst, ItemID.GemBunnyDiamond, ItemID.GemBunnyEmerald, ItemID.GemBunnyRuby, ItemID.GemBunnySapphire, ItemID.GemBunnyTopaz });
							player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType, 1);
						}
					}
					else
					{
						quote = Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.Mouse");
						itemType = ItemID.Mouse;
						player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType, 5);
					}
                }
                //purity, most common option likely
                else// if (player.position.Y > Main.worldSurface)
                {
                    if (Main.dayTime)
                    {
						if (Main.WindyEnoughForKiteDrops && Main.rand.NextBool(2)) //ladybug
						{
							quote = Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.Ladybug");
							itemType = ItemID.LadyBug;
							player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType, 5);
						}
                        else if (Main.rand.NextBool(3)) //butterfly
                        {
                            quote = Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.Butterfly");
							for (int i = 0; i < 5; i++)
                            {
								itemType = Main.rand.Next(new int[] { ItemID.JuliaButterfly, ItemID.MonarchButterfly, ItemID.PurpleEmperorButterfly, ItemID.RedAdmiralButterfly, ItemID.SulphurButterfly, ItemID.TreeNymphButterfly, ItemID.UlyssesButterfly, ItemID.ZebraSwallowtailButterfly });
								player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType);
							}
                        }
                        else if (Main.rand.NextBool(20))
                        {
                            quote = Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.EucaluptusSap");
                            player.QuickSpawnItem(player.GetSource_OpenItem(ItemID.EucaluptusSap), ItemID.EucaluptusSap);
                        }
                        else
                        {
                            quote = Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.DayTime");
							for (int i = 0; i < 5; i++)
                            {
								itemType = Main.rand.Next(new int[] { ItemID.Grasshopper, ItemID.Squirrel, ItemID.SquirrelRed, ItemID.Bird, ItemID.BlueJay, ItemID.Cardinal });
								player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType);
							}
                        }
                    }
                    else
                    {
                        quote = Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.NightTime");
                        player.QuickSpawnItem(player.GetSource_OpenItem(ItemID.Firefly), ItemID.Firefly);
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        itemType = Main.rand.Next(new int[] { ItemID.Lemon, ItemID.Peach, ItemID.Apricot, ItemID.Grapefruit });
                        player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType);
                    }
                    player.QuickSpawnItem(player.GetSource_OpenItem(ItemID.Wood), ItemID.Wood, 50);
                }

                Main.npcChatText = quote;
                dayOver = false;
                nightOver = false;
            }
            else
            {
                Main.npcChatText = Language.GetTextValue("Mods.Fargowiltas.Dialogues.Lumber.Rest");
            }
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            shop.item[nextSlot].SetDefaults(ItemID.WoodPlatform);
            shop.item[nextSlot].shopCustomPrice = 5;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.Wood);
            shop.item[nextSlot].shopCustomPrice = 10;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.BorealWood);
            shop.item[nextSlot].shopCustomPrice = 10;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.RichMahogany);
            shop.item[nextSlot].shopCustomPrice = 15;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.PalmWood);
            shop.item[nextSlot].shopCustomPrice = 15;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.Ebonwood);
            shop.item[nextSlot].shopCustomPrice = 15;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.Shadewood);
            shop.item[nextSlot].shopCustomPrice = 15;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.Pearlwood);
            shop.item[nextSlot].shopCustomPrice = 20;
            nextSlot++;

            if (NPC.downedHalloweenKing)
            {
                shop.item[nextSlot].SetDefaults(ItemID.SpookyWood);
                shop.item[nextSlot].shopCustomPrice = 50;
                nextSlot++;
            }


            shop.item[nextSlot].SetDefaults(ItemID.Cactus);
            shop.item[nextSlot].shopCustomPrice = 10;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.BambooBlock);
            shop.item[nextSlot].shopCustomPrice = 10;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.LivingWoodWand);
            shop.item[nextSlot].shopCustomPrice = 10000;
            nextSlot++;



            shop.item[nextSlot].SetDefaults(ModContent.ItemType<LumberjackMask>());
            shop.item[nextSlot].shopCustomPrice = 10000;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<LumberjackBody>());
            shop.item[nextSlot].shopCustomPrice = 10000;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<LumberjackPants>());
            shop.item[nextSlot].shopCustomPrice = 10000;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Weapons.LumberJaxe>());
            shop.item[nextSlot].shopCustomPrice = 10000;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.SharpeningStation);
            shop.item[nextSlot].shopCustomPrice = 100000;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<WoodenToken>());
            shop.item[nextSlot].shopCustomPrice = 10000;
            nextSlot++;
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 30;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<LumberJaxe>();
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
        }

        public override void OnKill()
        {
            FargoWorld.DownedBools["lumberjack"] = true;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 8; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, 2.5f * hitDirection, -2.5f, Scale: 0.8f);
                }

                if (!Main.dedServ)
                {
                    Vector2 pos = NPC.position + new Vector2(Main.rand.Next(NPC.width - 8), Main.rand.Next(NPC.height / 2));
                    Gore.NewGore(NPC.GetSource_Death(), pos, NPC.velocity, ModContent.Find<ModGore>("Fargowiltas/LumberGore3").Type);

                    pos = NPC.position + new Vector2(Main.rand.Next(NPC.width - 8), Main.rand.Next(NPC.height / 2));
                    Gore.NewGore(NPC.GetSource_Death(), pos, NPC.velocity, ModContent.Find<ModGore>("Fargowiltas/LumberGore2").Type);

                    pos = NPC.position + new Vector2(Main.rand.Next(NPC.width - 8), Main.rand.Next(NPC.height / 2));
                    Gore.NewGore(NPC.GetSource_Death(), pos, NPC.velocity, ModContent.Find<ModGore>("Fargowiltas/LumberGore1").Type);
                }
            }
            else
            {
                for (int k = 0; k < damage / NPC.lifeMax * 50.0; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, hitDirection, -1f, Scale: 0.6f);
                }
            }
        }
    }

    public class LumberJackProfile : ITownNPCProfile
    {
        public int RollVariation() => 0;
        public string GetNameForVariant(NPC npc) => npc.getNewNPCName();

        public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
        {
            if (npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn)
                return ModContent.Request<Texture2D>("Fargowiltas/NPCs/LumberJack");

            if (npc.altTexture == 1)
                return ModContent.Request<Texture2D>("Fargowiltas/NPCs/LumberJack_Party");

            return ModContent.Request<Texture2D>("Fargowiltas/NPCs/LumberJack");
        }

        public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot("Fargowiltas/NPCs/LumberJack_Head");
    }
}
