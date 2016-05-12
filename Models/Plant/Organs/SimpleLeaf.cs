using System;
using System.Collections.Generic;
using System.Text;

using Models.Core;
using Models.PMF.Functions;
using Models.PMF.Functions.SupplyFunctions;
using System.Xml.Serialization;
using Models.PMF.Interfaces;
using Models.Interfaces;
using Models.PMF.Phen;
using APSIM.Shared.Utilities;

namespace Models.PMF.Organs
{
    /// <summary>
    /// A simple leaf organ
    /// </summary>
    /// \retval LAI Leaf area index for green leaf (\f$\text{LAI}_{g}\f$, \f$m^2 m^{-2}\f$)
    /// \retval LAIDead Leaf area index for dead leaf  (\f$\text{LAI}_{d}\f$, \f$m^2 m^{-2}\f$)
    /// \retval LAITotal Total LAI including live and dead parts (\f$m^2 m^{-2}\f$)
    ///     \f[
    /// /// LAI = \text{LAI}_{g} + \text{LAI}_{d}
    ///     \f]
    /// \retval CoverGreen Cover for green leaf (\f$C_g\f$, unitless). The value of CoverFunction is returned 
    ///     if "CoverFunction" exists in the model. \f$C_g\f$ is calculated according to
    ///     extinction coefficient of green leaf (\f$k_{g}\f$) 
    ///     if "ExtinctionCoefficientFunction" exists in the model.
    ///     \f[
    /// /// C_g = 1-\exp(-k_{g} * \text{LAI}_{g})
    ///     \f]
    ///     where, \f$k\f$ is the extinction coefficient which calculates by "ExtinctionCoefficientFunction"
    /// \retval CoverDead Cover for dead leaf (\f$C_d\f$, unitless). \f$C_d\f$ is calculated according to 
    ///     extinction coefficient of dead leaf (\f$k_{d}\f$). 
    ///     \f[
    /// /// C_d = 1-\exp(-k_{d} * \text{LAI}_{d})
    ///     \f]
    /// <remarks>
    /// </remarks>
    [Serializable]
    [ViewName("UserInterface.Views.GridView")]
    [PresenterName("UserInterface.Presenters.PropertyPresenter")]
    public class SimpleLeaf : GenericOrgan, AboveGround, ICanopy, ILeaf
    {
        #region Leaf Interface
        /// <summary>
        /// 
        /// </summary>
        public bool CohortsInitialised { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double PlantAppearedLeafNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="proprtionRemoved"></param>
        public void DoThin(double proprtionRemoved) { }
        #endregion

        #region Canopy interface

        /// <summary>Gets the canopy. Should return null if no canopy present.</summary>
        public string CanopyType { get { return Plant.CropType; } }

        /// <summary>Albedo.</summary>
        [Description("Albedo")]
        public double Albedo { get; set; }

        /// <summary>Gets or sets the gsmax.</summary>
        [Description("GSMAX")]
        public double Gsmax { get; set; }

        /// <summary>Gets or sets the R50.</summary>
        [Description("R50")]
        public double R50 { get; set; }

        /// <summary>Gets the LAI</summary>
        [Units("m^2/m^2")]
        public double LAI { get; set; }

        /// <summary>Gets the LAI live + dead (m^2/m^2)</summary>
        public double LAITotal { get { return LAI + LAIDead; } }

        /// <summary>Gets the cover green.</summary>
        [Units("0-1")]
        public double CoverGreen
        {
            get
            {
                //if (CoverFunction == null)
                //    return 1.0 - Math.Exp((-1 * ExtinctionCoefficientFunction.Value) * LAI);
                //return Math.Min(Math.Max(CoverFunction.Value, 0), 1);

                if (Plant.IsAlive)
                {
                    if (CoverFunction == null)
                        return 1.0 - Math.Exp((-1 * ExtinctionCoefficientFunction.Value) * LAI);
                    return Math.Min(Math.Max(CoverFunction.Value, 0), 1);
                }
                else
                {
                    return 0;
                }

            }
        }

        /// <summary>Gets the cover total.</summary>
        [Units("0-1")]
        public double CoverTotal
        {
            get { return 1.0 - (1 - CoverGreen) * (1 - CoverDead); }
        }

        /// <summary>Gets or sets the height.</summary>
        [Units("mm")]
        public double Height { get; set; }
        /// <summary>Gets the depth.</summary>
        [Units("mm")]
        public double Depth { get { return Height; } }//  Fixme.  This needs to be replaced with something that give sensible numbers for tree crops

        /// <summary>Gets or sets the FRGR.</summary>
        [Units("mm")]
        public double FRGR { get; set; }

        /// <summary>Sets the potential evapotranspiration. Set by MICROCLIMATE.</summary>
        public double PotentialEP { get; set; }

        /// <summary>Sets the light profile. Set by MICROCLIMATE.</summary>
        public CanopyEnergyBalanceInterceptionlayerType[] LightProfile { get; set; }
        #endregion

        #region Parameters
        /// <summary>The FRGR function</summary>
        [Link]
        IFunction FRGRFunction = null;   // VPD effect on Growth Interpolation Set
        ///// <summary>The dm demand function</summary>
        //[Link]
        //IFunction DMDemandFunction = null;
        /// <summary>The cover function</summary>
        [Link(IsOptional = true)]
        IFunction CoverFunction = null;
        /// <summary>The nitrogen demand switch</summary>
        [Link(IsOptional = true)]
        IFunction NitrogenDemandSwitch = null;

        /// <summary>The lai function</summary>
        [Link(IsOptional = true)]
        IFunction LAIFunction = null;
        /// <summary>The extinction coefficient function</summary>
        [Link(IsOptional = true)]
        IFunction ExtinctionCoefficientFunction = null;
        /// <summary>The photosynthesis</summary>
        [Link(IsOptional = true)]
        IFunction Photosynthesis = null;
        /// <summary>The height function</summary>
        [Link(IsOptional = true)]
        IFunction HeightFunction = null;
        /// <summary>The lai dead function</summary>
        [Link(IsOptional = true)]
        IFunction LaiDeadFunction = null;
        /// <summary>The structural fraction</summary>
        [Link(IsOptional = true)]
        IFunction StructuralFraction = null;

        /// <summary>The structure</summary>
        [Link(IsOptional = true)]
        public Structure Structure = null;
        /// <summary>The phenology</summary>
        [Link(IsOptional = true)]
        public Phenology Phenology = null;
        /// <summary>TE Function</summary>
        [Link(IsOptional = true)]
        IFunction TranspirationEfficiencyCoefficient = null;
        /// <summary></summary>
        [Link(IsOptional = true)]
        IFunction SVPFrac = null;

        #endregion

        #region States and variables
        /// <summary>The _ water allocation</summary>
        private double _WaterAllocation;
        /// <summary>The biomass yesterday</summary>
        public double BiomassYesterday = 0;
        /// <summary>The _ structural fraction</summary>
        private double _StructuralFraction = 1;

        /// <summary>Gets or sets the ep.</summary>
        /// <value>The ep.</value>
        private double EP { get; set; }
        /// <summary>Gets or sets the k.</summary>
        /// <value>The k.</value>
        public double K { get; set; }                      // Extinction Coefficient (Green)
        /// <summary>Gets or sets the k dead.</summary>
        /// <value>The k dead.</value>
        public double KDead { get; set; }                  // Extinction Coefficient (Dead)
        /// <summary>Gets or sets the delta biomass.</summary>
        /// <value>The delta biomass.</value>
        public double DeltaBiomass { get; set; }
        /// <summary>Gets or sets the water demand.</summary>
        /// <value>The water demand.</value>
        [Units("mm")]
        public override double WaterDemand
        {
            get
            {
                if (SVPFrac != null && TranspirationEfficiencyCoefficient != null)
                {
                    return Photosynthesis.Value / (TranspirationEfficiencyCoefficient.Value / VPD / 0.001);
                }
                return PotentialEP;
            }
            //set
            //{
            //    Plant.PotentialEP = value;
            //}
        }
        /// <summary>Gets the transpiration.</summary>
        /// <value>The transpiration.</value>
        public double Transpiration { get { return EP; } }

        /// <summary>Gets the fw.</summary>
        /// <value>The fw.</value>
        public double Fw
        {
            get
            {
                double F = 0;
                if (WaterDemand > 0)
                    F = EP / WaterDemand;
                else
                    F = 1;
                return F;
            }
        }
        /// <summary>Gets the function.</summary>
        /// <value>The function.</value>
        public double Fn
        {
            get
            {
                double MaxNContent = Live.Wt * MaximumNConc.Value;
                return Live.N / MaxNContent;
            }
        }

        /// <summary>Gets or sets the lai dead.</summary>
        /// <value>The lai dead.</value>
        public double LAIDead { get; set; }

        /// <summary>Gets VPD.</summary>
        [Units("")]
        public double VPD
        {
            get
            {
                if (SVPFrac != null)
                {
                    double svpMax = MetUtilities.svp(MetData.MaxT) * 0.1;
                    double svpMin = MetUtilities.svp(MetData.MinT) * 0.1;
                    return Math.Max(SVPFrac.Value * (svpMax - svpMin), 0.01);
                }
                else
                {
                    return 0;
                }
            }
        }


        /// <summary>Gets the cover dead.</summary>
        /// <value>The cover dead.</value>
        public double CoverDead
        {
            get { return 1.0 - Math.Exp(-KDead * LAIDead); }
        }
        /// <summary>Gets the RAD int tot.</summary>
        /// <value>The RAD int tot.</value>
        [Units("MJ/m^2/day")]
        [Description("This is the intercepted radiation value that is passed to the RUE class to calculate DM supply")]
        public double RadIntTot
        {
            get
            {
                return CoverGreen * MetData.Radn;
            }
        }
        #endregion

        #region Arbitrator Methods
        /// <summary>Gets or sets the water allocation.</summary>
        /// <value>The water allocation.</value>
        public override double WaterAllocation
        {
            get { return _WaterAllocation; }
            set
            {
                _WaterAllocation = value;
                EP += _WaterAllocation;
            }
        }
        /// <summary>Gets or sets the dm demand.</summary>
        /// <value>The dm demand.</value>
        public override BiomassPoolType DMDemand
        {
            get
            {
                double Demand = 0;
                if (DMDemandFunction != null)
                    Demand = DMDemandFunction.Value;
                else
                    Demand = 1;
                if (Math.Round(Demand, 8) < 0)
                    throw new Exception(this.Name + " organ is returning a negative DM demand.  Check your parameterisation");
                return new BiomassPoolType { Structural = Demand };
            }
        }
        /// <summary>Gets or sets the dm supply.</summary>
        /// <value>The dm supply.</value>
        public override BiomassSupplyType DMSupply
        {
            get
            {
                if (Math.Round(Photosynthesis.Value, 8) < 0)
                    throw new Exception(this.Name + " organ is returning a negative DM supply.  Check your parameterisation");
                return new BiomassSupplyType
                {
                    Fixation = DMSupplyFixationFunction == null ? Photosynthesis.Value : DMSupplyFixationFunction.Value,
                    Retranslocation = DMSupplyReTranslocationFunction == null ? 0 : DMSupplyReTranslocationFunction.Value,
                    Reallocation = DMSupplyReAllocationFunction == null ? 0 : DMSupplyReAllocationFunction.Value
                };
            }
        }
        /// <summary>Sets the dm allocation.</summary>
        /// <value>The dm allocation.</value>
        public override BiomassAllocationType DMAllocation
        {
            set
            {
                Live.StructuralWt += value.Structural;
            }
        }
        /// <summary>Gets or sets the n demand.</summary>
        /// <value>The n demand.</value>
        public override BiomassPoolType NDemand
        {
            get
            {
                double StructuralDemand = 0;
                double NDeficit = 0;
                if (NitrogenDemandSwitch == null)
                    NDeficit = 0;
                if (NitrogenDemandSwitch != null)
                {
                    if (NitrogenDemandSwitch.Value == 0)
                        NDeficit = 0;
                }

                if (MaximumNConc == null)
                    NDeficit = 0;
                else
                {
                    double DMDemandTot = DMDemand.Structural + DMDemand.NonStructural + DMDemand.Metabolic;
                    StructuralDemand = MaximumNConc.Value * DMDemandTot * _StructuralFraction;
                    NDeficit = Math.Max(0.0, MaximumNConc.Value * (Live.Wt + DMDemandTot) - Live.N) - StructuralDemand;
                }
                if (Math.Round(StructuralDemand, 8) < 0)
                    throw new Exception(this.Name + " organ is returning a negative structural N Demand.  Check your parameterisation");
                if (Math.Round(NDeficit, 8) < 0)
                    throw new Exception(this.Name + " organ is returning a negative Non structural N Demand.  Check your parameterisation");
                return new BiomassPoolType { Structural = StructuralDemand, NonStructural = NDeficit };
            }
        }

        /// <summary>Sets the n allocation.</summary>
        /// <value>The n allocation.</value>
        /// <exception cref="System.Exception">
        /// Invalid allocation of N
        /// or
        /// N allocated to Leaf left over after allocation
        /// or
        /// UnKnown Leaf N allocation problem
        /// </exception>
        public override BiomassAllocationType NAllocation
        {
            set
            {
                // Allocation
                if (value.Structural > 0)
                {
                    Live.StructuralN += value.Structural;
                }
                if (value.NonStructural > 0)
                    Live.NonStructuralN += value.NonStructural;

            }
        }

        #endregion

        #region Events

        /// <summary>Called when [simulation commencing].</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        [EventSubscribe("Commencing")]
        private new void OnSimulationCommencing(object sender, EventArgs e)
        {
            Clear();
        }

        /// <summary>Called when [do daily initialisation].</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        [EventSubscribe("DoDailyInitialisation")]
        private void OnDoDailyInitialisation(object sender, EventArgs e)
        {
            if (Phenology != null)
                if (Phenology.OnDayOf("Emergence"))
                {
                    if (Structure != null)
                        Structure.MainStemNodeNo = 1.0;
                }

            EP = 0;
        }
        #endregion

        #region Component Process Functions

        /// <summary>Called when crop is ending</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="data">The <see cref="EventArgs"/> instance containing the event data.</param>
        [EventSubscribe("PlantSowing")]
        private new void OnPlantSowing(object sender, SowPlant2Type data)
        {
            if (data.Plant == Plant)
            {
                Clear();

                if (StructuralFraction != null)
                    _StructuralFraction = StructuralFraction.Value;
            }
        }

        /// <summary>Clears this instance.</summary>
        protected override void Clear()
        {
            base.Clear();
            Height = 0;
            LAI = 0;
        }
        #endregion

        #region Top Level time step functions
        /// <summary>Event from sequencer telling us to do our potential growth.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        [EventSubscribe("DoPotentialPlantGrowth")]
        private new void OnDoPotentialPlantGrowth(object sender, EventArgs e)
        {
            base.OnDoPotentialPlantGrowth(sender, e);
            if (Plant.IsEmerged)
            {
                FRGR = FRGRFunction.Value;
                if (CoverFunction == null & ExtinctionCoefficientFunction == null)
                {
                    throw new Exception("\"CoverFunction\" or \"ExtinctionCoefficientFunction\" should be defined in " + this.Name);
                }
                if (CoverFunction != null)
                    LAI = (Math.Log(1 - CoverGreen) / (ExtinctionCoefficientFunction.Value * -1));
                if (LAIFunction != null)
                    LAI = LAIFunction.Value;

                Height = HeightFunction.Value;

                if (LaiDeadFunction != null)
                    LAIDead = LaiDeadFunction.Value;
                else
                    LAIDead = 0;

                /*/Set N Demand
                double StructuralDemand = 0;
                double NDeficit = 0;
                if (NitrogenDemandSwitch == null)
                    NDeficit = 0;
                if (NitrogenDemandSwitch != null)
                {
                    if (NitrogenDemandSwitch.Value == 0)
                        NDeficit = 0;
                }

                if (NConc == null)
                    NDeficit = 0;
                else
                {
                    StructuralDemand = NConc.Value * DeltaBiomass * _StructuralFraction;
                    NDeficit = Math.Max(0.0, NConc.Value * (Live.Wt + DMDemand.Structural + DMDemand.NonStructural + DMDemand.Metabolic) - Live.N) - StructuralDemand;
                } //return new BiomassPoolType { Structural = StructuralDemand, NonStructural = NDeficit };
                NDemand = new BiomassPoolType();           
                NDemand.Structural = StructuralDemand;
                NDemand.NonStructural = NDeficit;*/
            }
        }

        #endregion

        /// <summary>Writes documentation for this function by adding to the list of documentation tags.</summary>
        /// <param name="tags">The list of tags to add to.</param>
        /// <param name="headingLevel">The level (e.g. H2) of the headings.</param>
        /// <param name="indent">The level of indentation 1, 2, 3 etc.</param>
        public override void Document(List<AutoDocumentation.ITag> tags, int headingLevel, int indent)
        {
            // add a heading.
            Name = this.Name;
            tags.Add(new AutoDocumentation.Heading(Name, headingLevel));

            tags.Add(new AutoDocumentation.Paragraph(Name + " is parameterised using a simple leaf organ type which provides the core functions of intercepting radiation, providing a photosynthesis supply and a transpiration demand.  It is parameterised as follows.", indent));

            // write memos.
            foreach (IModel memo in Apsim.Children(this, typeof(Memo)))
                memo.Document(tags, -1, indent);

            // Describe biomass production
            tags.Add(new AutoDocumentation.Heading("Dry Matter Supply", headingLevel + 1));  //FIXME, this will need to be changed to photoysnthesis rather that potential Biomass
            if (Photosynthesis != null)
                tags.Add(new AutoDocumentation.Paragraph("DryMatter Fixation Supply (Photosynthesis) provided to the Organ Arbitrator (for partitioning between organs) is calculated each day as the product of a unstressed potential and a series of stress factors.", indent));
            foreach (IModel child in Apsim.Children(this, typeof(IModel)))
            {
                if (child.Name == "Photosynthesis")
                    child.Document(tags, headingLevel + 5, indent + 1);
            }

            tags.Add(new AutoDocumentation.Paragraph("DM is not retranslocated out of " + this.Name + " ", indent));

            tags.Add(new AutoDocumentation.Heading("Dry Matter Demands", headingLevel + 1));
            if (StructuralFraction != null)
                tags.Add(new AutoDocumentation.Paragraph("Of the organs total DM demand " + StructuralFraction.Value * 100 + "% is structural demand and " + (100 - StructuralFraction.Value * 100) + "is non-structural demand", indent));
            else
                tags.Add(new AutoDocumentation.Paragraph("100% of the DM demanded from this organ is structural", indent));

            if (DMDemandFunction != null)
            {
                tags.Add(new AutoDocumentation.Paragraph("The daily DM demand from this organ is calculated using", indent));
                foreach (IModel child in Apsim.Children(this, typeof(IModel)))
                {
                    if (child.Name == "DMDemandFunction")
                        child.Document(tags, headingLevel + 5, indent + 1);
                }
            }

            tags.Add(new AutoDocumentation.Heading("Nitrogen Demands", headingLevel + 1));
            tags.Add(new AutoDocumentation.Paragraph("The daily structural N demand from " + this.Name + " is the product of Total DM demand and a Nitrogen concentration of " + MaximumNConc.Value * 100 + "%", indent));
            if (NitrogenDemandSwitch != null)
            {
                tags.Add(new AutoDocumentation.Paragraph("The Nitrogen demand swith is a multiplier applied to nitrogen demand so it can be turned off at certain phases.  For the " + Name + " Organ it is set as:", indent));
                foreach (IModel child in Apsim.Children(this, typeof(IModel)))
                {
                    if (child.Name == "NitrogenDemandSwitch")
                        child.Document(tags, headingLevel + 5, indent);
                }
            }

            tags.Add(new AutoDocumentation.Heading("Nitrogen Supplies", headingLevel + 1));
            tags.Add(new AutoDocumentation.Paragraph("N is not reallocated from " + this.Name + " ", indent));
            tags.Add(new AutoDocumentation.Paragraph("Non-structural N in " + this.Name + "  is not available for re-translocation to other organs", indent));

            tags.Add(new AutoDocumentation.Heading("Biomass Senescece and Detachment", headingLevel + 1));
            tags.Add(new AutoDocumentation.Paragraph("No senescence occurs from " + Name, indent));
            tags.Add(new AutoDocumentation.Paragraph("No Detachment occurs from " + Name, indent));

            tags.Add(new AutoDocumentation.Heading("Canopy", headingLevel + 1));
            if (CoverFunction != null)
            {
                tags.Add(new AutoDocumentation.Paragraph("The Green cover (proportion of ground cover comprising green leaf) and Leaf area index (LAI, the area of leaf per unit area of ground) estimations are calculated using a CoverFunction as folows" + " ", indent));
                foreach (IModel child in Apsim.Children(this, typeof(IModel)))
                {
                    if (child.Name == "CoverFunction")
                        child.Document(tags, headingLevel + 5, indent + 1);
                }
                tags.Add(new AutoDocumentation.Paragraph("Then LAI is calculated using an inverted Beer Lamberts equation with the estimated Cover value:"
                    + " <b>LAI = Log(1 - Cover) / (ExtinctionCoefficient * -1));", indent));
                tags.Add(new AutoDocumentation.Paragraph("Where ExtinctionCoefficient has a value of " + ExtinctionCoefficientFunction.Value, indent + 1));
            }
            if (LAIFunction != null)
            {
                tags.Add(new AutoDocumentation.Paragraph("First the leaf area index is calculated as:", indent));
                foreach (IModel child in Apsim.Children(this, typeof(IModel)))
                {
                    if (child.Name == "LAIFunction")
                        child.Document(tags, headingLevel + 5, indent + 1);
                }
                tags.Add(new AutoDocumentation.Paragraph("Then the cover produced by the canopy is calculated using LAI and the Beer Lamberts equation:" + this.Name
                    + " <b>Cover = 1.0 - e<sup>((-1 * ExtinctionCoefficient) * LAI);", indent));
                tags.Add(new AutoDocumentation.Paragraph("Where ExtinctionCoefficient has a value of " + ExtinctionCoefficientFunction.Value, indent + 1));
            }
            tags.Add(new AutoDocumentation.Paragraph("The canopies values of Cover and LAI are passed to the MicroClimate module which uses the Penman Monteith equation to calculate potential evapotranspiration for each canopy and passes the value back to the crop", indent));
            tags.Add(new AutoDocumentation.Paragraph("The effect of growth rate on transpiration is captured using the Fractional Growth Rate (FRGR) function which is parameterised as a function of temperature for the simple leaf", indent));
            foreach (IModel child in Apsim.Children(this, typeof(IModel)))
            {
                if (child.Name == "FRGRFunction")
                    child.Document(tags, headingLevel + 1, indent + 1);
            }
            // write Other functions.
            bool NonStandardFunctions = false;
            foreach (IModel child in Apsim.Children(this, typeof(IModel)))
            {
                if (((child.Name != "StructuralFraction")
                   | (child.Name != "DMDemandFunction")
                   | (child.Name != "NConc")
                   | (child.Name != "Photosynthesis")
                   | (child.Name != "Photosynthesis")
                   | (child.Name != "NReallocationFactor")
                   | (child.Name != "NRetranslocationFactor")
                   | (child.Name != "DMRetranslocationFactor")
                   | (child.Name != "SenescenceRateFunction")
                   | (child.Name != "DetachmentRateFunctionFunction")
                   | (child.Name != "LAIFunction")
                   | (child.Name != "CoverFunction")
                   | (child.Name != "ExtinctionCoefficientFunction")
                   | (child.Name != "Live")
                   | (child.Name != "Dead")
                   | (child.Name != "FRGRFunction")
                   | (child.Name != "NitrogenDemandSwitch"))
                   && (child.GetType() != typeof(Memo)))
                {
                    NonStandardFunctions = true;
                }
            }

            if (NonStandardFunctions)
            {
                tags.Add(new AutoDocumentation.Heading("Other functionality", headingLevel + 1));
                tags.Add(new AutoDocumentation.Paragraph("In addition to the core functionality and parameterisation described above, the " + this.Name + " organ has additional functions used to provide paramters for core functions and create additional functionality", indent));
                foreach (IModel child in Apsim.Children(this, typeof(IModel)))
                {
                    if ((child.Name == "StructuralFraction")
                       | (child.Name == "DMDemandFunction")
                       | (child.Name == "NConc")
                       | (child.Name == "Photosynthesis")
                       | (child.Name == "Photosynthesis")
                       | (child.Name == "NReallocationFactor")
                       | (child.Name == "NRetranslocationFactor")
                       | (child.Name == "DMRetranslocationFactor")
                       | (child.Name == "SenescenceRateFunction")
                       | (child.Name == "DetachmentRateFunctionFunction")
                       | (child.Name == "LAIFunction")
                       | (child.Name == "CoverFunction")
                       | (child.Name == "ExtinctionCoefficientFunction")
                       | (child.Name == "Live")
                       | (child.Name == "Dead")
                       | (child.Name == "FRGRFunction")
                       | (child.Name == "NitrogenDemandSwitch")
                       | (child.GetType() == typeof(Memo)))
                    {//Already documented 
                    }
                    else
                    {
                        //tags.Add(new AutoDocumentation.Heading(child.Name, headingLevel + 2));
                        child.Document(tags, headingLevel + 2, indent + 1);
                    }
                }
            }
        }
    }
}
