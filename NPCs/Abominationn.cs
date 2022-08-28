using System.Collections.Generic;
using Fargowiltas.Items.Summons.Deviantt;
using Fargowiltas.Items.Summons.Abom;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using Fargowiltas.Items.Vanity;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Fargowiltas.ShoppingBiomes;

namespace Fargowiltas.NPCs
{
    [AutoloadHead]
    public class Abominationn : ModNPC
    {
        private bool canSayDefeatQuote = true;
        private int defeatQuoteTimer = 900;

        //public override bool Autoload(ref string name)
        //{
        //    name = "Abominationn";
        //    return mod.Properties.Autoload;
        //}

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abominationn");

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

            NPC.Happiness.SetBiomeAffection<SkyBiome>(AffectionLevel.Love);
            NPC.Happiness.SetBiomeAffection<OceanBiome>(AffectionLevel.Like);
            NPC.Happiness.SetBiomeAffection<DungeonBiome>(AffectionLevel.Dislike);

            NPC.Happiness.SetNPCAffection<Mutant>(AffectionLevel.Love);
            NPC.Happiness.SetNPCAffection<Deviantt>(AffectionLevel.Like);
            NPC.Happiness.SetNPCAffection(NPCID.Nurse, AffectionLevel.Hate);

            NPCID.Sets.DebuffImmunitySets.Add(NPC.type, new Terraria.DataStructures.NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[]
                {
                    BuffID.Suffocation
                }
            });
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("Mods.Fargowiltas.Bestiary.Abominationn")
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
            NPC.defense = NPC.downedMoonlord ? 50 : 15;
            NPC.lifeMax = NPC.downedMoonlord ? 5000 : 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            AnimationType = NPCID.Guide;

            //if (GetInstance<FargoConfig>().CatchNPCs)
            //{
            //    Main.npcCatchable[NPC.type] = true;
            //    NPC.catchItem = (short)mod.ItemType("Abominationn");
            //}
                
            NPC.buffImmune[BuffID.Suffocation] = true;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            if (Fargowiltas.ModLoaded["FargowiltasSouls"] && ((bool)ModLoader.GetMod("FargowiltasSouls").Call("MutantAlive") || (bool)ModLoader.GetMod("FargowiltasSouls").Call("AbomAlive")))
            {
                return false;
            }
            return GetInstance<FargoConfig>().Abom && NPC.downedGoblins && !FargoGlobalNPC.AnyBossAlive();
        }

        public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;

        public override void AI()
        {
            NPC.breath = 200;
            if (defeatQuoteTimer > 0)
                defeatQuoteTimer--;
            else
                canSayDefeatQuote = false;
        }

        public override List<string> SetNPCNameList()
        {
            string[] names = { "Wilta", "Jack", "Harley", "Reaper", "Stevenn", "Doof", "Baroo", "Fergus", "Entev", "Catastrophe", "Bardo", "Betson" };

            return new List<string>(names);
        }

        public override string GetChat()
        {
            if (NPC.homeless && canSayDefeatQuote && Fargowiltas.ModLoaded["FargowiltasSouls"] && (bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedAbom"))
            {
                canSayDefeatQuote = false;
                return Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.Defeat");
            }

            List<string> dialogue = new List<string>
            {
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.1") + (!Main.hardMode ? Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.1phm") : Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.1hm")),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.2"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.3"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.4"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.5"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.6"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.7"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.8"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.9"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.10"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.11"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.12"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.13"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.14"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.15"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.16"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.17"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.18"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.19"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.20"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.21"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.22"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.23"),
            };

            if (Main.LocalPlayer.ZoneGraveyard)
            {
                dialogue.Add(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.Graveyard"));
            }

            int mechanic = NPC.FindFirstNPC(NPCID.Mechanic);
            if (mechanic != -1)
            {
                dialogue.Add(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.Mechanic1") + Main.npc[mechanic].GivenName + Language.GetTextValue("Mods.Fargowiltas.Dialogues.Abom.Mechanic2"));
            }

            return Main.rand.Next(dialogue);
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
            button2 = FargoUtils.IsChinese() ? "取消事件" : "Cancel Event";
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                shop = true;
            }
            else
            {
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    var netMessage = Mod.GetPacket();
                    netMessage.Write((byte)6);
                    netMessage.Send();
                }

                if (Fargowiltas.IsEventOccurring)
                {
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        var netMessage = Mod.GetPacket();
                        netMessage.Write((byte)2);
                        netMessage.Send();
                    }

                    Main.npcChatText = FargoUtils.IsChinese() ? (Fargowiltas.TryClearEvents() ? "吼吼嘿哈，事件结束了！" : $"我现在感觉不到，{FargoWorld.AbomClearCD / 60}秒之后再来吧。")
                                                : (Fargowiltas.TryClearEvents() ? "Hocus pocus, the event is over" : $"I'm not feeling it right now, come back in {FargoWorld.AbomClearCD / 60} seconds.");
                }
                else
                {
                    Main.npcChatText = FargoUtils.IsChinese() ? "我认为现在没有事件。" : "I don't think there's an event right now.";
                }
            }
        }

        //public static void AddModItem(bool condition, string modName, string itemName, int price, ref Chest shop, ref int nextSlot)
        //{
        //    if (condition)
        //    {
        //        shop.item[nextSlot].SetDefaults(ModLoader.GetMod(modName).ItemType(itemName));
        //        shop.item[nextSlot].shopCustomPrice = price;
        //        nextSlot++;
        //    }
        //}

        public static void AddItem(bool check, int item, int price, ref Chest shop, ref int nextSlot)
        {
            if (check)
            {
                shop.item[nextSlot].SetDefaults(item);
                shop.item[nextSlot].shopCustomPrice = price;
                nextSlot++;
            }
        }

        //public static void AddItem(bool check, string mod, string item, int price, ref Chest shop, ref int nextSlot)
        //{
        //    if (!check || shop is null)
        //    {
        //        return;
        //    }

        //    shop.item[nextSlot].SetDefaults(ModLoader.GetMod(mod).ItemType(item));
        //    shop.item[nextSlot].shopCustomPrice = price;

        //    nextSlot++;
        //}

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            // Events
            AddItem(true, ItemType<PartyCone>(), 10000, ref shop, ref nextSlot);
            AddItem(true, ItemType<WeatherBalloon>(), 20000, ref shop, ref nextSlot);
            AddItem(true, ItemType<Anemometer>(), 30000, ref shop, ref nextSlot);
            AddItem(true, ItemType<ForbiddenScarab>(), 30000, ref shop, ref nextSlot);
            AddItem(true, ItemType<SlimyBarometer>(), Item.buyPrice(0, 4), ref shop, ref nextSlot);
            AddItem(true, ItemID.BloodMoonStarter, Item.buyPrice(0, 5), ref shop, ref nextSlot);
            AddItem(true, ItemID.GoblinBattleStandard, Item.buyPrice(0, 6), ref shop, ref nextSlot);
            
            AddItem(FargoWorld.DownedBools["boss"], ItemType<MatsuriLantern>(), Item.buyPrice(10), ref shop, ref nextSlot);
            
            AddItem(Main.hardMode, ItemID.SnowGlobe, Item.buyPrice(0, 15), ref shop, ref nextSlot);
            AddItem(NPC.downedPirates, ItemID.PirateMap, Item.buyPrice(0, 20), ref shop, ref nextSlot);
            AddItem(NPC.downedPirates && FargoWorld.DownedBools["flyingDutchman"], ItemType<PlunderedBooty>(), Item.buyPrice(0, 15), ref shop, ref nextSlot);
            AddItem(NPC.downedMechBossAny, ItemID.SolarTablet, Item.buyPrice(0, 20), ref shop, ref nextSlot);
            AddItem(FargoWorld.DownedBools["darkMage"] || NPC.downedMechBossAny, ItemType<ForbiddenTome>(), Item.buyPrice(0, 5), ref shop, ref nextSlot);
            AddItem(FargoWorld.DownedBools["ogre"] || NPC.downedGolemBoss, ItemType<BatteredClub>(), Item.buyPrice(0, 15), ref shop, ref nextSlot);
            AddItem(FargoWorld.DownedBools["betsy"], ItemType<BetsyEgg>(), Item.buyPrice(0, 40), ref shop, ref nextSlot);

            AddItem(NPC.downedHalloweenKing, ItemID.PumpkinMoonMedallion, Item.buyPrice(0, 50), ref shop, ref nextSlot);
            AddItem(FargoWorld.DownedBools["headlessHorseman"], ItemType<HeadofMan>(), Item.buyPrice(0, 20), ref shop, ref nextSlot);
            AddItem(NPC.downedHalloweenTree, ItemType<SpookyBranch>(), Item.buyPrice(0, 20), ref shop, ref nextSlot);
            AddItem(NPC.downedHalloweenKing, ItemType<SuspiciousLookingScythe>(), Item.buyPrice(0, 30), ref shop, ref nextSlot);
            
            AddItem(NPC.downedChristmasIceQueen, ItemID.NaughtyPresent, Item.buyPrice(0, 50), ref shop, ref nextSlot);
            AddItem(NPC.downedChristmasTree, ItemType<FestiveOrnament>(), Item.buyPrice(0, 20), ref shop, ref nextSlot);
            AddItem(NPC.downedChristmasSantank, ItemType<NaughtyList>(), Item.buyPrice(0, 20), ref shop, ref nextSlot);
            AddItem(NPC.downedChristmasIceQueen, ItemType<IceKingsRemains>(), Item.buyPrice(0, 30), ref shop, ref nextSlot);

            AddItem(NPC.downedGolemBoss, ItemType<RunawayProbe>(), Item.buyPrice(0, 50), ref shop, ref nextSlot);
            AddItem(NPC.downedMartians, ItemType<MartianMemoryStick>(), Item.buyPrice(0, 30), ref shop, ref nextSlot);
            

            AddItem(NPC.downedTowers, ItemType<PillarSummon>(), Item.buyPrice(0, 75), ref shop, ref nextSlot);

            //foreach (MutantSummonInfo summon in Fargowiltas.summonTracker.EventSummons)
            //{
            //    AddItem(summon.downed(), summon.modSource, summon.itemName, summon.price, ref shop, ref nextSlot);
            //}

            AddItem(NPC.downedTowers, ItemType<AbominationnScythe>(), Item.buyPrice(0, 5), ref shop, ref nextSlot);
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = NPC.downedMoonlord ? 150 : 20;
            knockback = NPC.downedMoonlord ? 10f : 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = NPC.downedMoonlord ? 1 : 30;
            if (!NPC.downedMoonlord)
            {
                randExtraCooldown = 30;
            }
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = NPC.downedMoonlord ? ProjectileType<Projectiles.DeathScythe>() : ProjectileID.DeathSickle;
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
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
                    Gore.NewGore(NPC.GetSource_Death(), pos, NPC.velocity, ModContent.Find<ModGore>("Fargowiltas/AbomGore3").Type);

                    pos = NPC.position + new Vector2(Main.rand.Next(NPC.width - 8), Main.rand.Next(NPC.height / 2));
                    Gore.NewGore(NPC.GetSource_Death(), pos, NPC.velocity, ModContent.Find<ModGore>("Fargowiltas/AbomGore2").Type);

                    pos = NPC.position + new Vector2(Main.rand.Next(NPC.width - 8), Main.rand.Next(NPC.height / 2));
                    Gore.NewGore(NPC.GetSource_Death(), pos, NPC.velocity, ModContent.Find<ModGore>("Fargowiltas/AbomGore1").Type);
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
}
