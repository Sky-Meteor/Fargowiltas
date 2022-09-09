using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using static Terraria.ModLoader.ModContent;
using Fargowiltas.Items.Summons.SwarmSummons;
using Fargowiltas.Items.Misc;
using Fargowiltas.Items.Summons.Mutant;
using Fargowiltas.Projectiles;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Fargowiltas.ShoppingBiomes;

namespace Fargowiltas.NPCs
{
    [AutoloadHead]
    public class Mutant : ModNPC
    {
        private static bool prehardmodeShop;
        private static bool hardmodeShop;
        private static int shopNum = 1;

        internal bool spawned;
        private bool canSayDefeatQuote = true;
        private int defeatQuoteTimer = 900;

        //public override bool Autoload(ref string name)
        //{
        //    name = "Mutant";
        //    return true;// Mod.Properties.Autoload;
        //}

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mutant");

            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 90;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Velocity = -1f,
                Direction = -1
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness.SetBiomeAffection<SkyBiome>(AffectionLevel.Love);
            NPC.Happiness.SetBiomeAffection<ForestBiome>(AffectionLevel.Like);
            NPC.Happiness.SetBiomeAffection<HallowBiome>(AffectionLevel.Dislike);

            NPC.Happiness.SetNPCAffection<Abominationn>(AffectionLevel.Love);
            NPC.Happiness.SetNPCAffection<Deviantt>(AffectionLevel.Like);
            NPC.Happiness.SetNPCAffection<LumberJack>(AffectionLevel.Dislike);

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
                new FlavorTextBestiaryInfoElement("Mods.Fargowiltas.Bestiary.Mutant")
            });
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 10;
            NPC.defense = NPC.downedMoonlord ? 50 : 15;
            NPC.lifeMax = NPC.downedMoonlord ? 5000 : 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            AnimationType = NPCID.Guide;

            if (GetInstance<FargoConfig>().CatchNPCs)
            {
                Main.npcCatchable[NPC.type] = true;
            //    NPC.catchItem = (short)Mod.ItemType("Mutant");
            }

            NPC.buffImmune[BuffID.Suffocation] = true;

            if (Fargowiltas.ModLoaded["FargowiltasSouls"] && (bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedMutant"))
            {
                NPC.lifeMax = 77000;
                NPC.defense = 360;
            }
        }

        public override bool CanGoToStatue(bool toKingStatue) => true;

        public override void AI()
        {
            NPC.breath = 200;
            if (defeatQuoteTimer > 0)
                defeatQuoteTimer--;
            else
                canSayDefeatQuote = false;

            if (!spawned)
            {
                spawned = true;
                if (Fargowiltas.ModLoaded["FargowiltasSouls"] && (bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedMutant"))
                {
                    NPC.lifeMax = 77000;
                    NPC.life = NPC.lifeMax;
                    NPC.defense = 360;
                }
            }
        }

        public override bool CanTownNPCSpawn(int numTownnpcs, int money)
        {
            if (Fargowiltas.ModLoaded["FargowiltasSouls"] && (bool)ModLoader.GetMod("FargowiltasSouls").Call("MutantAlive"))
            {
                return false;
            }

            return GetInstance<FargoConfig>().Mutant && FargoWorld.DownedBools["boss"] && !FargoGlobalNPC.AnyBossAlive();
        }

        public override List<string> SetNPCNameList()
        {
            string[] names = { "Flacken", "Dorf", "Bingo", "Hans", "Fargo", "Grim", "Mike", "Fargu", "Terrance", "Catty N. Pem", "Tom", "Weirdus", "Polly" };

            return new List<string>(names);
        }

        public override string GetChat()
        {
            if (NPC.homeless && canSayDefeatQuote && Fargowiltas.ModLoaded["FargowiltasSouls"] && (bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedMutant"))
            {
                canSayDefeatQuote = false;

                if ((bool)ModLoader.GetMod("FargowiltasSouls").Call("EternityMode"))
                    return Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.DefeatE");
                else
                    return Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.Defeat");
            }

            if (Fargowiltas.ModLoaded["FargowiltasSouls"] && Main.rand.NextBool(4))
            {
                if ((bool)ModLoader.GetMod("FargowiltasSouls").Call("MutantArmor"))
                    return Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.TrueMutant");
            }

            if (Fargowiltas.ModLoaded["FargowiltasSouls"] && Main.rand.NextBool(4))
            {
                if ((bool)ModLoader.GetMod("FargowiltasSouls").Call("MutantArmor"))
                    return "Nice armor you have, very realistic skin. As a matter of fact, mind if I borrow it? Your skin, that is.";
            }

            List<string> dialogue = new List<string>
            {
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.1"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.2"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.3"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.4"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.5"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.6"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.7"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.8"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.9"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.10"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.11"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.12"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.13"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.14"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.15"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.16"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.17"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.18"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.19"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.20"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.21"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.22"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.23"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.24"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.25"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.26"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.27"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.28"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.29"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.30"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.31"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.32"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.33"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.34"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.35"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.36"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.37"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.38"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.39"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.40"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.41"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.42"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.43"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.44"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.45")
            };

            if (Fargowiltas.ModLoaded["FargowiltasSouls"])
            {
                dialogue.AddWithCondition(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.ml"), NPC.downedMoonlord);

                if ((bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedMutant"))
                {
                    dialogue.Add(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.Downed"));
                }
                else if ((bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedFishronEX") || (bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedAbom"))
                {
                    dialogue.Add(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.DownedAbom"));
                }
            }
            else
            {
                dialogue.Add(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.NoSouls"));
            }

            //dialogue.AddWithCondition("Why would you do this.", Fargowiltas.ModLoaded["CalamityMod"]);
            //dialogue.AddWithCondition("I feel a great imbalance in this world.", Fargowiltas.ModLoaded["CalamityMod"] && Fargowiltas.ModLoaded["ThoriumMod"]);
            //dialogue.AddWithCondition("A great choice, shame about that first desert boss thing though.", Fargowiltas.ModLoaded["ThoriumMod"]);
            dialogue.AddWithCondition(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.Pumpkin"), Main.pumpkinMoon);
            dialogue.AddWithCondition(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.Frost"), Main.snowMoon);
            dialogue.AddWithCondition(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.Slime"), Main.slimeRain);
            dialogue.AddWithCondition(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.Blood1"), Main.bloodMoon);
            dialogue.AddWithCondition(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.Blood2"), Main.bloodMoon);
            dialogue.AddWithCondition(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.Night"), !Main.dayTime);

            int partyGirl = NPC.FindFirstNPC(NPCID.PartyGirl);
            if (BirthdayParty.PartyIsUp)
            {
                if (partyGirl >= 0)
                {
                    dialogue.Add(Main.npc[partyGirl].GivenName + Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.PartyGirl"));
                }
                
                dialogue.Add(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.NoPartyGirl"));
            }

            int nurse = NPC.FindFirstNPC(NPCID.Nurse);
            if (nurse >= 0)
            {
                dialogue.Add(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.Nurse1") + Main.npc[nurse].GivenName + Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.Nurse2"));
            }

            int witchDoctor = NPC.FindFirstNPC(NPCID.WitchDoctor);
            if (witchDoctor >= 0)
            {
                dialogue.Add(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.WitchDoctor1") + Main.npc[witchDoctor].GivenName + Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.WitchDoctor2"));
            }

            int dryad = NPC.FindFirstNPC(NPCID.Dryad);
            if (dryad >= 0)
            {
                dialogue.Add(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.Dryad1") + Main.npc[dryad].GivenName + Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.Dryad2"));
            }

            int stylist = NPC.FindFirstNPC(NPCID.Stylist);
            if (stylist >= 0)
            {
                dialogue.Add(Main.npc[stylist].GivenName + Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.Stylist"));
            }

            int truffle = NPC.FindFirstNPC(NPCID.Truffle);
            if (truffle >= 0)
            {
                dialogue.Add(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.Truffle"));
            }

            int tax = NPC.FindFirstNPC(NPCID.TaxCollector);
            if (tax >= 0)
            {
                dialogue.Add(Main.npc[tax].GivenName + Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.TaxCollector"));
            }

            int guide = NPC.FindFirstNPC(NPCID.Guide);
            if (guide >= 0)
            {
                dialogue.Add(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.Guide1") + Main.npc[guide].GivenName + Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.Guide2"));
            }

            int cyborg = NPC.FindFirstNPC(NPCID.Cyborg);
            if (truffle >= 0 && witchDoctor >= 0 && cyborg >= 0 && Main.rand.NextBool(52))
            {
                dialogue.Add(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.Cyborg1") + Main.npc[witchDoctor].GivenName + Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.Cyborg2")
                + Main.npc[truffle].GivenName + Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.Cyborg3") + Main.npc[cyborg].GivenName + Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.Cyborg4"));
            }

            if (partyGirl >= 0)
            {
                dialogue.Add(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.PartyGirl2") + Main.npc[partyGirl].GivenName + Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.PartyGirl3"));
            }

            int demoman = NPC.FindFirstNPC(NPCID.Demolitionist);
            if (demoman >= 0)
            {
                dialogue.Add(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.Demolitionist1") + Main.npc[demoman].GivenName + Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.Demolitionist2"));
            }

            int tavernkeep = NPC.FindFirstNPC(NPCID.DD2Bartender);
            if (tavernkeep >= 0)
            {
                dialogue.Add(Main.npc[tavernkeep].GivenName + Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.DD2Bartender"));
            }

            int dyeTrader = NPC.FindFirstNPC(NPCID.DyeTrader);
            if (dyeTrader >= 0)
            {
                dialogue.Add(Main.npc[dyeTrader].GivenName + Language.GetTextValue("Mods.Fargowiltas.Dialogues.Mutant.DyeTrader"));
            }

            return Main.rand.Next(dialogue);
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            switch (shopNum)
            {
                case 1:
                    button = FargoUtils.IsChinese() ? "困难模式前" : "Pre Hardmode";
                    break;

                case 2:
                    button = FargoUtils.IsChinese() ? "困难模式" : "Hardmode";
                    break;

                default:
                    button = FargoUtils.IsChinese() ? "月亮领主后" : "Post Moon Lord";
                    break;
            }

            if (Main.hardMode)
            {
                button2 = FargoUtils.IsChinese() ? "切换商店" : "Cycle Shop";
            }

            if (NPC.downedMoonlord)
            {
                if (shopNum >= 4)
                {
                    shopNum = 1;
                }
            }
            else
            {
                if (shopNum >= 3)
                {
                    shopNum = 1;
                }
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                shop = true;

                switch (shopNum)
                {
                    case 1:
                        prehardmodeShop = true;
                        hardmodeShop = false;
                        break;
                    case 2:
                        hardmodeShop = true;
                        prehardmodeShop = false;
                        break;
                    default:
                        prehardmodeShop = false;
                        hardmodeShop = false;
                        break;
                }
            }
            else if (!firstButton && Main.hardMode)
            {
                shopNum++;
            }
        }

        public static void AddItem(bool check, int itemType, int price, ref Chest shop, ref int nextSlot)
        {
            if (!check || shop is null)
            {
                return;
            }

            shop.item[nextSlot].SetDefaults(itemType);
            shop.item[nextSlot].shopCustomPrice = price > 0 ? price : shop.item[nextSlot].value;

            // Lowered prices with discount card and pact
            if (Fargowiltas.ModLoaded["FargowiltasSouls"])
            {
                float modifier = 1f;
                if ((bool)ModLoader.GetMod("FargowiltasSouls").Call("MutantDiscountCard"))
                {
                    modifier -= 0.2f;
                }

                if ((bool)ModLoader.GetMod("FargowiltasSouls").Call("MutantPact"))
                {
                    modifier -= 0.3f;
                }

                shop.item[nextSlot].shopCustomPrice = (int)(shop.item[nextSlot].shopCustomPrice * modifier);
            }

            nextSlot++;
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            AddItem(Main.expertMode, ModContent.ItemType<Overloader>(), 400000, ref shop, ref nextSlot);

            if (prehardmodeShop)
            {
                AddItem(true, ModContent.ItemType<ModeToggle>(), -1, ref shop, ref nextSlot);

                if (Fargowiltas.ModLoaded["FargowiltasSouls"] && TryFind("FargowiltasSouls", "Masochist", out ModItem masochist))
                {
                    AddItem(true, masochist.Type, 10000, ref shop, ref nextSlot); // mutants gift, dam meme namer
                }

                foreach (MutantSummonInfo summon in Fargowiltas.summonTracker.SortedSummons)
                {
                    //phm
                    if (summon.progression <= MutantSummonTracker.WallOfFlesh)
                    {
                        AddItem(summon.downed(), summon.itemId, summon.price, ref shop, ref nextSlot);
                    }
                }
            }
            else if (hardmodeShop)
            {
                foreach (MutantSummonInfo summon in Fargowiltas.summonTracker.SortedSummons)
                {
                    //hm
                    if (summon.progression > MutantSummonTracker.WallOfFlesh && summon.progression <= MutantSummonTracker.Moonlord)
                    {
                        AddItem(summon.downed(), summon.itemId, summon.price, ref shop, ref nextSlot);
                    }
                }
            }
            else
            {
                foreach (MutantSummonInfo summon in Fargowiltas.summonTracker.SortedSummons)
                {
                    //post ml
                    if (summon.progression > MutantSummonTracker.Moonlord)
                    {
                        AddItem(summon.downed(), summon.itemId, summon.price, ref shop, ref nextSlot);
                    }
                }

                AddItem(true, ModContent.ItemType<AncientSeal>(), 100000000, ref shop, ref nextSlot);
            }
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            if (Fargowiltas.ModLoaded["FargowiltasSouls"] && (bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedMutant"))
            {
                damage = 700;
                knockback = 7f;
            }
            else if (NPC.downedMoonlord)
            {
                damage = 250;
                knockback = 6f;
            }
            else if (Main.hardMode)
            {
                damage = 60;
                knockback = 5f;
            }
            else
            {
                damage = 20;
                knockback = 4f;
            }
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            if (NPC.downedMoonlord)
            {
                cooldown = 1;
            }
            else if (Main.hardMode)
            {
                cooldown = 20;
                randExtraCooldown = 25;
            }
            else
            {
                cooldown = 30;
                randExtraCooldown = 30;
            }
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            if (Fargowiltas.ModLoaded["FargowiltasSouls"] && (bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedMutant") && TryFind("FargowiltasSouls", "MutantSpearThrownFriendly", out ModProjectile penetrator))
            {
                projType = penetrator.Type;
            }
            else if (NPC.downedMoonlord)
            {
                projType = ProjectileType<PhantasmalEyeProjectile>();
            }
            else if (Main.hardMode)
            {
                projType = ProjectileType<MechEyeProjectile>();
            }
            else
            {
                projType = ProjectileType<EyeProjectile>();
            }

            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            if (Fargowiltas.ModLoaded["FargowiltasSouls"] && (bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedMutant"))
            {
                multiplier = 25f;
                randomOffset = 0f;
            }
            else
            {
                multiplier = 12f;
                randomOffset = 2f;
            }
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
                    Gore.NewGore(NPC.GetSource_Death(), pos, NPC.velocity, ModContent.Find<ModGore>("Fargowiltas/MutantGore3").Type);

                    pos = NPC.position + new Vector2(Main.rand.Next(NPC.width - 8), Main.rand.Next(NPC.height / 2));
                    Gore.NewGore(NPC.GetSource_Death(), pos, NPC.velocity, ModContent.Find<ModGore>("Fargowiltas/MutantGore2").Type);

                    pos = NPC.position + new Vector2(Main.rand.Next(NPC.width - 8), Main.rand.Next(NPC.height / 2));
                    Gore.NewGore(NPC.GetSource_Death(), pos, NPC.velocity, ModContent.Find<ModGore>("Fargowiltas/MutantGore1").Type);
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
