using System;
using System.Collections.Generic;
using System.Text;
using Models.Core;
using Models.PMF.Organs;
using System.Xml.Serialization;
using Models.PMF.Interfaces;
using Models.Soils.Arbitrator;
using Models.Interfaces;
using APSIM.Shared.Utilities;


namespace Models.PMF
{
   /* /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [ViewName("UserInterface.Views.GridView")]
    [PresenterName("UserInterface.Presenters.PropertyPresenter")]
    [ValidParent(ParentType = typeof(Plant))]
    public class SimpleArbitrator : Model, IUptake
    {

        /// <summary>Gets and Sets NO3 Supply</summary>
        /// <value>NO3 supplies from each soil layer</value>
        [XmlIgnore]
        public double[] PotentialNO3NUptake { get; set; }

        /// <summary>Gets and Sets NO3 Supply</summary>
        /// <value>NO3 supplies from each soil layer</value>
        [XmlIgnore]
        public double TotalPotentialNO3Uptake
        {
            get
            {
                if (PotentialNO3NUptake != null)
                    return MathUtilities.Sum(PotentialNO3NUptake);
                else
                    return 0;
            }
        }

        /// <summary>Gets and Sets NH4 Supply</summary>
        /// <value>NH4 supplies from each soil layer</value>
        [XmlIgnore]
        public double[] PotentialNH4NUptake { get; set; }


        #region Class Members
        // Input paramaters

        /// <summary>The top level plant object in the Plant Modelling Framework</summary>
        [Link]
        public Plant Plant = null;

        /// <summary>APSIMs clock model</summary>
        [Link]
        public Clock Clock = null;

        /// <summary>Gets the dm supply.</summary>
        /// <value>Supply of DM from photosynthesising organs</value>
        [XmlIgnore]
        public double DMSupply
        {
            get
            {
                if (Plant.IsAlive)
                {
                    if (Plant.Phenology != null)
                    {
                        if (Plant.Phenology.Emerged == true)
                            return 20; //Fix
                        else return 0;
                    }
                    else
                        return 0; //Fix
                }
                else
                    return 0.0;
            }
        }


        private IArbitration[] Organs;

        /// <summary>Called when crop is ending</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="data">The <see cref="EventArgs"/> instance containing the event data.</param>
        [EventSubscribe("PlantSowing")]
        private void OnPlantSowing(object sender, SowPlant2Type data)
        {
            if (data.Plant == Plant)
            {
                List<IArbitration> organsToArbitrate = new List<IArbitration>();

                foreach (IOrgan organ in Plant.Organs)
                    //if (!(organ is Root))
                    {
                        if (organ is IArbitration)
                            organsToArbitrate.Add(organ as IArbitration);
                    }
                Organs = organsToArbitrate.ToArray();
            }
        }

        #endregion

        /// <summary>Calculates how much N the roots will take up in the absence of competition</summary>
        /// <param name="soilstate">The state of the soil.</param>
        public void DoNUptakeDemandCalculations(SoilState soilstate)
        {
  
        }
        /// <summary>Allocates the NUptake that the soil arbitrator has returned</summary>
        /// <param name="AllocatedNO3Nuptake">AllocatedNO3Nuptake</param>
        /// <param name="AllocatedNH4Nuptake">AllocatedNH4Nuptake</param>
        public void DoNUptakeAllocations(double[] AllocatedNO3Nuptake, double[] AllocatedNH4Nuptake)  //Fixme Needs to take N allocation from soil arbitrator
        {
            ////Calculation the proportion of potential N uptake from each organ
            //double[] proportion = new double[Organs.Length];
            //for (int i = 0; i < Organs.Length; i++)
            //{
            //    proportion[i] = N.UptakeSupply[i] / N.TotalUptakeSupply;
            //}

            ////Reset actual uptakes to each organ based on uptake allocated by soil arbitrator and the organs proportion of potential uptake
            //for (int i = 0; i < Organs.Length; i++)
            //{   //Allocation of n involves resetting UptakeSupply from the potential value calculated on DoNUptakeDemandCalculations to an actual value based on what soil arbitrator allocated
            //    N.UptakeSupply[i] = MathUtilities.Sum(AllocatedNO3Nuptake) * kgha2gsm * proportion[i];
            //    N.UptakeSupply[i] += MathUtilities.Sum(AllocatedNH4Nuptake) * kgha2gsm * proportion[i];
            //}

            ////Recalculate totals based on actual supply
            //N.TotalUptakeSupply = MathUtilities.Sum(N.UptakeSupply);
            //N.TotalPlantSupply = N.TotalReallocationSupply + N.TotalUptakeSupply + N.TotalFixationSupply + N.TotalRetranslocationSupply;

            ////Allocate N that the SoilArbitrator has allocated the plant to each organ
            //DoUptake(Organs, N, NArbitrationOption);
        }

        #region IUptake interface
        /// <summary>
        /// Calculate the potential sw uptake for today
        /// </summary>
        public List<ZoneWaterAndN> GetSWUptakes(SoilState soilstate)
        {
            if (Plant.IsAlive)
            {
                // Model can only handle one root zone at present
                ZoneWaterAndN MyZone = new ZoneWaterAndN();
                Zone ParentZone = Apsim.Parent(this, typeof(Zone)) as Zone;
                foreach (ZoneWaterAndN Z in soilstate.Zones)
                    if (Z.Name == ParentZone.Name)
                        MyZone = Z;

                double Supply = 0;
                double Demand = 0;
                double[] supply = null;
                foreach (IArbitration o in Organs)
                {
                    double[] organSupply = o.WaterSupply(soilstate.Zones);
                    if (organSupply != null)
                    {
                        supply = organSupply;
                        Supply += MathUtilities.Sum(organSupply);
                    }
                    
                    Demand += o.WaterDemand;
                }
                WaterSupply = Supply;

                double FractionUsed = 0;
                if (Supply > 0)
                    FractionUsed = Math.Min(1.0, Demand / Supply);

                // Just send uptake from my zone
                ZoneWaterAndN uptake = new ZoneWaterAndN();
                uptake.Name = MyZone.Name;
                uptake.Water = MathUtilities.Multiply_Value(supply, FractionUsed);
                uptake.NO3N = new double[uptake.Water.Length];
                uptake.NH4N = new double[uptake.Water.Length];

                List<ZoneWaterAndN> zones = new List<ZoneWaterAndN>();
                zones.Add(uptake);
                return zones;
            }
            else
                return null;
        }
        /// <summary>
        /// 
        /// </summary>
        public double WaterSupply { get; set; }
         
        /// <summary>
        /// Set the sw uptake for today
        /// </summary>
        public void SetSWUptake(List<ZoneWaterAndN> zones)
        {
            // Model can only handle one root zone at present
            ZoneWaterAndN MyZone = new ZoneWaterAndN();
            Zone ParentZone = Apsim.Parent(this, typeof(Zone)) as Zone;
            foreach (ZoneWaterAndN Z in zones)
                if (Z.Name == ParentZone.Name)
                    MyZone = Z;

            double[] uptake = MyZone.Water;
            double Supply = MathUtilities.Sum(uptake);
            double Demand = 0;
            foreach (IArbitration o in Organs)
                Demand += o.WaterDemand;

            double fraction = 1;
            if (Demand > 0)
                fraction = Math.Min(1.0, Supply / Demand);

            foreach (IArbitration o in Organs)
                if (o.WaterDemand > 0)
                    o.WaterAllocation = fraction * o.WaterDemand;

            Plant.Root.DoWaterUptake(uptake);
        }

        /// <summary>
        /// Calculate the potential sw uptake for today. Should return null if crop is not in the ground.
        /// </summary>
        public List<Soils.Arbitrator.ZoneWaterAndN> GetNUptakes(SoilState soilstate)
        {
            if (Plant.IsAlive)
            {
                // Model can only handle one root zone at present
                ZoneWaterAndN MyZone = new ZoneWaterAndN();
                Zone ParentZone = Apsim.Parent(this, typeof(Zone)) as Zone;
                foreach (ZoneWaterAndN Z in soilstate.Zones)
                    if (Z.Name == ParentZone.Name)
                        MyZone = Z;

                ZoneWaterAndN UptakeDemands = new ZoneWaterAndN();
                if (Plant.Phenology != null)
                {
                    if (Plant.Phenology.Emerged == true)
                    {
                        DoNUptakeDemandCalculations(soilstate);
                        PotentialNO3NUptake = new double[MyZone.NO3N.Length];
                        PotentialNH4NUptake = new double[MyZone.NH4N.Length];
                        //Pack results into uptake structure
                        UptakeDemands.NO3N = PotentialNO3NUptake;
                        UptakeDemands.NH4N = PotentialNH4NUptake;
                    }
                    else //Uptakes are zero
                    {
                        UptakeDemands.NO3N = new double[MyZone.NO3N.Length];
                        for (int i = 0; i < UptakeDemands.NO3N.Length; i++) { UptakeDemands.NO3N[i] = 0; }
                        UptakeDemands.NH4N = new double[MyZone.NH4N.Length];
                        for (int i = 0; i < UptakeDemands.NH4N.Length; i++) { UptakeDemands.NH4N[i] = 0; }
                    }
                }
                else
                {
                    DoNUptakeDemandCalculations(soilstate);

                    //Pack results into uptake structure
                    UptakeDemands.NO3N = PotentialNO3NUptake;
                    UptakeDemands.NH4N = PotentialNH4NUptake;
                }

                UptakeDemands.Name = MyZone.Name;
                UptakeDemands.Water = new double[UptakeDemands.NO3N.Length];

                List<ZoneWaterAndN> zones = new List<ZoneWaterAndN>();
                zones.Add(UptakeDemands);
                return zones;
            }
            else
                return null;
        }
        /// <summary>
        /// Set the sw uptake for today
        /// </summary>
        public void SetNUptake(List<ZoneWaterAndN> zones)
        {
            if (Plant.IsAlive)
            {
                // Model can only handle one root zone at present
                ZoneWaterAndN MyZone = new ZoneWaterAndN();
                Zone ParentZone = Apsim.Parent(this, typeof(Zone)) as Zone;
                foreach (ZoneWaterAndN Z in zones)
                    if (Z.Name == ParentZone.Name)
                        MyZone = Z;

                if (Plant.Phenology != null)
                {
                    if (Plant.Phenology.Emerged == true)
                    {
                        double[] AllocatedNO3Nuptake = MyZone.NO3N;
                        double[] AllocatedNH4Nuptake = MyZone.NH4N;
                        DoNUptakeAllocations(AllocatedNO3Nuptake, AllocatedNH4Nuptake); //Fixme, needs to send allocations to arbitrator
                        Plant.Root.DoNitrogenUptake(AllocatedNO3Nuptake, AllocatedNH4Nuptake);
                    }
                }
                else
                {
                    double[] AllocatedNO3Nuptake = MyZone.NO3N;
                    double[] AllocatedNH4Nuptake = MyZone.NH4N;
                    DoNUptakeAllocations(AllocatedNO3Nuptake, AllocatedNH4Nuptake); //Fixme, needs to send allocations to arbitrator
                    Plant.Root.DoNitrogenUptake(AllocatedNO3Nuptake, AllocatedNH4Nuptake);
                }
            }
        }
        #endregion
        //Get demand from demand functions
        /// <summary>Does the water limited dm allocations.  Water constaints to growth are accounted for in the calculation of DM supply
        /// and does initial N calculations to work out how much N uptake is required to pass to SoilArbitrator</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        [EventSubscribe("DoPotentialPlantPartioning")]
        private void OnDoPotentialPlantPartioning(object sender, EventArgs e)
        {
            if (Plant.IsAlive)
            {
                foreach (IOrgan Organ in Organs)
                {

                }
            }
        }

        //Get supplys (including fixation, retrans, realloc)

        //Balance Retrans

        //Set weights 
    }*/
}
