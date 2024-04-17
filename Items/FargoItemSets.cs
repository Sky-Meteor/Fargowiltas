using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Items
{
    public class FargoItemSets : ModSystem
    {
        public static bool[] MechanicalAccessory;
        public static bool[] InfoAccessory;
        public override void PostSetupContent()
        {
            SetFactory itemFactory = new(ContentSamples.ItemsByType.Count);

            MechanicalAccessory = itemFactory.CreateBoolSet(false, ItemID.MechanicalLens, ItemID.WireKite, ItemID.Ruler, ItemID.LaserRuler, ItemID.PaintSprayer, 
            ItemID.ArchitectGizmoPack, ItemID.HandOfCreation, ItemID.ActuationAccessory, ItemID.EncumberingStone,
            ItemID.DontHurtCrittersBook, ItemID.DontHurtComboBook, ItemID.DontHurtNatureBook, ItemID.LucyTheAxe);

            InfoAccessory = itemFactory.CreateBoolSet(false, ItemID.CopperWatch, ItemID.TinWatch, ItemID.SilverWatch, ItemID.TungstenWatch, ItemID.GoldWatch, ItemID.PlatinumWatch,
                ItemID.Compass, ItemID.DepthMeter, ItemID.GPS, ItemID.PDA, ItemID.CellPhone, 5358, 5359, 5360,
                5361, ItemID.GoblinTech, ItemID.DPSMeter, ItemID.MetalDetector, ItemID.Stopwatch, ItemID.LifeformAnalyzer,
                ItemID.FishermansGuide, ItemID.WeatherRadio, ItemID.Sextant, ItemID.Radar, ItemID.TallyCounter);
        }
    }
}
