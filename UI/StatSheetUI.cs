using Fargowiltas.Items.Misc;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace Fargowiltas.UI
{
    public class StatSheetUI : UIState
    {
        public const int BackWidth = 600;
        public const int BackHeight = 25 * HowManyPerColumn + 26 + 4; //row height * stat rows + search bar + padding
        public const int HowManyPerColumn = 14;
        public const int HowManyColumns = 2;

        public int LineCounter;
        public int ColumnCounter;

        public UISearchBar SearchBar;
        public UIDragablePanel BackPanel;
        public UIPanel InnerPanel;

        public override void OnInitialize()
        {
            Vector2 offset = new Vector2(Main.screenWidth / 2 - BackWidth * 0.75f, Main.screenHeight / 2 - BackHeight * 0.75f);

            BackPanel = new UIDragablePanel();
            BackPanel.Left.Set(offset.X, 0f);
            BackPanel.Top.Set(offset.Y, 0f);
            BackPanel.Width.Set(BackWidth, 0f);
            BackPanel.Height.Set(BackHeight, 0f);
            BackPanel.PaddingLeft = BackPanel.PaddingRight = BackPanel.PaddingTop = BackPanel.PaddingBottom = 0;
            BackPanel.BackgroundColor = new Color(29, 33, 70) * 0.7f;
            Append(BackPanel);

            SearchBar = new UISearchBar(BackWidth - 8, 26);
            SearchBar.Left.Set(4, 0f);
            SearchBar.Top.Set(6, 0f); // 6 so padding lines up
            BackPanel.Append(SearchBar);

            InnerPanel = new UIPanel();
            InnerPanel.Left.Set(6, 0f);
            InnerPanel.Top.Set(6 + 28, 0f); // 28 for search bar
            InnerPanel.Width.Set(BackWidth - 12, 0f);
            InnerPanel.Height.Set(BackHeight - 12 - 28, 0);
            InnerPanel.PaddingLeft = InnerPanel.PaddingRight = InnerPanel.PaddingTop = InnerPanel.PaddingBottom = 0;
            InnerPanel.BackgroundColor = new Color(73, 94, 171) * 0.9f;
            BackPanel.Append(InnerPanel);

            base.OnInitialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Main.GameUpdateCount % (!SearchBar.IsEmpty ? 2 : 4) == 0) // 15 times a second, or 30 times a second if searchbar has text
            {
                RebuildStatList();
            }
        }

        public void RebuildStatList()
        {
            Player player = Main.LocalPlayer;
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            InnerPanel.RemoveAllChildren();
            ColumnCounter = LineCounter = 0;

            double Damage(DamageClass damageClass) => Math.Round(player.GetTotalDamage(damageClass).Additive * player.GetTotalDamage(damageClass).Multiplicative * 100 - 100);
            int Crit(DamageClass damageClass) => (int)player.GetTotalCritChance(damageClass);

            AddStat("MeleeDamage", Damage(DamageClass.Melee).ToString(), ItemID.CopperBroadsword);
            AddStat("MeleeCritical", Crit(DamageClass.Melee), ItemID.CopperBroadsword);
            AddStat("MeleeSpeed", (int)(1f / player.GetAttackSpeed(DamageClass.Melee) * 100), ItemID.CopperBroadsword);
            AddStat("RangedDamage", Damage(DamageClass.Ranged), ItemID.CopperBow);
            AddStat("RangedCritical", Crit(DamageClass.Ranged), ItemID.CopperBow);
            AddStat("MagicDamage", Damage(DamageClass.Magic), ItemID.WandofSparking);
            AddStat("MagicCritical", Crit(DamageClass.Magic), ItemID.WandofSparking);
            AddStat("ManaCostReduction", Math.Round((1.0 - player.manaCost) * 100), ItemID.WandofSparking);
            AddStat("SummonDamage", Damage(DamageClass.Summon), ItemID.SlimeStaff);
            if (Fargowiltas.ModLoaded["FargowiltasSouls"])
                AddStat("SummonCritical", (int)ModLoader.GetMod("FargowiltasSouls").Call("GetSummonCrit"), ItemID.SlimeStaff);
            else
                AddStat("");
            AddStat("MaxMinions", player.maxMinions, ItemID.SlimeStaff);
            AddStat("MaxSentries", player.maxTurrets, ItemID.SlimeStaff);

            AddStat("ArmorPenetration", player.GetArmorPenetration(DamageClass.Generic), ItemID.SharkToothNecklace);
            AddStat("Aggro", player.aggro, ItemID.FleshKnuckles);


            AddStat("Life", player.statLifeMax2, ItemID.LifeCrystal);
            AddStat("LifeRegen", player.lifeRegen / 2, ItemID.BandofRegeneration);
            AddStat("Mana", player.statManaMax2, ItemID.ManaCrystal);
            AddStat("ManaRegen", player.manaRegen / 2, ItemID.ManaCrystal);
            AddStat("Defense", player.statDefense, ItemID.CobaltShield);
            AddStat("DamageReduction", Math.Round(player.endurance * 100), ItemID.WormScarf);
            AddStat("Luck", Math.Round(player.luck, 2), ItemID.Torch);
            AddStat("FishingQuests", player.anglerQuestsFinished, ItemID.AnglerEarring);
            AddStat("BattleCry", modPlayer.BattleCry ? $"[c/ff0000:{StatSheetText("Battle")}]" : (modPlayer.CalmingCry ? $"[c/00ffff:${StatSheetText("Calming")}]" : StatSheetText("None")), ModContent.ItemType<BattleCry>());
            AddStat("MaxSpeed", (int)((player.accRunSpeed + player.maxRunSpeed) / 2f * player.moveSpeed * 6), ItemID.HermesBoots);

            string RenderWingStat(double stat) => stat <= 0 ? StatSheetText("???") : stat.ToString();
            AddStat("WingTime", player.wingTimeMax / 60 > 60 || player.empressBrooch ? StatSheetText("WingTimeInfinite") : StatSheetText("WingTimeSec", RenderWingStat(Math.Round(player.wingTimeMax / 60.0, 2))), ItemID.AngelWings);
            AddStat("WingMaxSpeed", RenderWingStat(Math.Round(modPlayer.StatSheetWingSpeed * 32 / 6.25)), ItemID.AngelWings);
            AddStat("WindAscentModifier", RenderWingStat(Math.Round(modPlayer.StatSheetMaxAscentMultiplier * 100)), ItemID.AngelWings);
            AddStat("WingCanHover", modPlayer.CanHover == null ? StatSheetText("???") : StatSheetText(modPlayer.CanHover.ToString()), ItemID.AngelWings);
        }

        public void AddStat(string key, object value = null, int item = -1)
        {
            int left = 8 + ColumnCounter * ((BackWidth - 8) / HowManyColumns);
            int top = 8 + LineCounter * (23); // I don't know why but 23 works perfectly
            if (++LineCounter == HowManyPerColumn)
            {
                ColumnCounter++;
                LineCounter = 0;
            }
            string text = "";
            if (key != "")
                text = Language.GetTextValue($"Mods.Fargowiltas.UI.StatSheet.{key}", value);
            UIText ui = new UIText(item > -1 ? $"[i:{item}] {text}" : text);
            ui.Left.Set(left, 0f);
            ui.Top.Set(top, 0f);

            string[] words = text.Split(' ');
            if (!SearchBar.IsEmpty)
            {
                if (words.Any(s => s.StartsWith(SearchBar.Input, StringComparison.OrdinalIgnoreCase)))
                {
                    float fade = MathHelper.Lerp(0.1f, 0.9f, (float)(Math.Sin(Main.GameUpdateCount / 10f) + 1f) / 2f);
                    Color color = Color.Lerp(Color.Yellow, Color.Goldenrod, fade);
                    ui.TextColor = color;
                }
                else
                    // Gray out text when filtered by search
                    ui.TextColor = Color.Gray * 1.5f;
            }

            InnerPanel.Append(ui);
        }

        private static string StatSheetText(string key, object value = null) => Language.GetTextValue($"Mods.Fargowiltas.UI.StatSheet.{key}", value);
        /*public void SetPositionToPoint(Point point)
        {
            BackPanel.Left.Set(point.X, 0f);
            BackPanel.Top.Set(point.Y, 0f);
        }

        public Point GetPositinAsPoint() => new Point((int)BackPanel.Left.Pixels, (int)BackPanel.Top.Pixels);*/
    }
}