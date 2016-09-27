using System;
using System.Collections.Generic;
using System.Text;
using Models.Core;
using Models.PMF.Interfaces;
using APSIM.Shared.Utilities;

namespace Models.PMF.Organs
{
    /// <summary>
    /// A root model for SWIM
    /// </summary>
    [Serializable]
    public class RootSWIM : BaseOrgan, BelowGround
    {
        /// <summary>The uptake</summary>
        private double[] Uptake = null;
        /// <summary>The RLV</summary>
        public double[] rlv = null;


        /// <summary>Gets or sets the water uptake.</summary>
        /// <value>The water uptake.</value>
        [Units("mm")]
        public double WaterUptake
        {
            get { return -MathUtilities.Sum(Uptake); }
        }


        /// <summary>Called when [water uptakes calculated].</summary>
        /// <param name="Uptakes">The uptakes.</param>
        [EventSubscribe("WaterUptakesCalculated")]
        private void OnWaterUptakesCalculated(WaterUptakesCalculatedType Uptakes)
        {
            for (int i = 0; i != Uptakes.Uptakes.Length; i++)
            {
                if (Uptakes.Uptakes[i].Name == Plant.Name)
                    Uptake = Uptakes.Uptakes[i].Amount;
            }
        }

        /// <summary>Called when [simulation commencing].</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        [EventSubscribe("Commencing")]
        private void OnSimulationCommencing(object sender, EventArgs e)
        {
            Clear();
        }

        /// <summary>Called when crop is ending</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="data">The <see cref="EventArgs"/> instance containing the event data.</param>
        [EventSubscribe("PlantSowing")]
        private void OnPlantSowing(object sender, SowPlant2Type data)
        {
            if (data.Plant == Plant)
                Clear();
        }

        /// <summary>Called when crop is ending</summary>
        public override void DoPlantEnding()
        {
            Clear();
        }

    }
}
