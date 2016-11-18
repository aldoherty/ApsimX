﻿using System;
using System.Xml.Serialization;
using Models.Core;

namespace Models
{
    /// <summary>
    /// The clock model
    /// </summary>
    [Serializable]
    [ViewName("UserInterface.Views.GridView")]
    [PresenterName("UserInterface.Presenters.PropertyPresenter")]
    [ValidParent(ParentType = typeof(Simulation))]
    public class Clock : Model, IClock
    {
        /// <summary>The arguments</summary>
        private EventArgs args = new EventArgs();

        // Links
        /// <summary>The summary</summary>
        [Link]
        private ISummary Summary = null;

        /// <summary>Gets or sets the start date.</summary>
        /// <value>The start date.</value>
        [Summary]
        [Description("The start date of the simulation")]
        public DateTime StartDate { get; set; }

        /// <summary>Gets or sets the end date.</summary>
        /// <value>The end date.</value>
        [Summary]
        [Description("The end date of the simulation")]
        public DateTime EndDate { get; set; }

        // Public events that we're going to publish.
        /// <summary>Occurs when [start of simulation].</summary>
        public event EventHandler StartOfSimulation;
        /// <summary>Occurs when [start of day].</summary>
        public event EventHandler StartOfDay;
        /// <summary>Occurs when [start of month].</summary>
        public event EventHandler StartOfMonth;
        /// <summary>Occurs when [start of year].</summary>
        public event EventHandler StartOfYear;
        /// <summary>Occurs when [start of week].</summary>
        public event EventHandler StartOfWeek;
        /// <summary>Occurs when [end of day].</summary>
        public event EventHandler EndOfDay;
        /// <summary>Occurs when [end of month].</summary>
        public event EventHandler EndOfMonth;
        /// <summary>Occurs when [end of year].</summary>
        public event EventHandler EndOfYear;
        /// <summary>Occurs when [end of week].</summary>
        public event EventHandler EndOfWeek;
        /// <summary>Occurs when [end of simulation].</summary>
        public event EventHandler EndOfSimulation;

        /// <summary>Occurs when [do weather].</summary>
        public event EventHandler DoWeather;
        /// <summary>Occurs when [do daily initialisation].</summary>
        public event EventHandler DoDailyInitialisation;
        /// <summary>Occurs when [do initial summary].</summary>
        public event EventHandler DoInitialSummary;
        /// <summary>Occurs when [do management].</summary>
        public event EventHandler DoManagement;
        /// <summary>Occurs when [do energy arbitration].</summary>
        public event EventHandler DoEnergyArbitration;                                //MicroClimate
        /// <summary>Occurs when [do soil water movement].</summary>
        public event EventHandler DoSoilWaterMovement;                                //Soil module
        //DoSoilTemperature will be here
        //DoSoilNutrientDynamics will be here
        /// <summary>Occurs when [do soil organic matter].</summary>
        public event EventHandler DoSoilOrganicMatter;                                 //SurfaceOM
        /// <summary>Occurs when [do surface organic matter decomposition].</summary>
        public event EventHandler DoSurfaceOrganicMatterDecomposition;                 //SurfaceOM
        /// <summary>Occurs when [do water arbitration].</summary>
        public event EventHandler DoWaterArbitration;                                  //Arbitrator
        /// <summary>Occurs when [do phenology].</summary>                             
        public event EventHandler DoPhenology;                                         // Plant 
        /// <summary>Occurs when [do potential plant growth].</summary>
        public event EventHandler DoPotentialPlantGrowth;                              //Refactor to DoWaterLimitedGrowth  Plant        
        /// <summary>Occurs when [do potential plant partioning].</summary>
        public event EventHandler DoPotentialPlantPartioning;                          // PMF OrganArbitrator.
        /// <summary>Occurs when [do nutrient arbitration].</summary>
        public event EventHandler DoNutrientArbitration;                               //Arbitrator
        /// <summary>Occurs when [do potential plant partioning].</summary>
        public event EventHandler DoActualPlantPartioning;                             // PMF OrganArbitrator.
        /// <summary>Occurs when [do actual plant growth].</summary>
        public event EventHandler DoActualPlantGrowth;                                 //Refactor to DoNutirentLimitedGrowth Plant
        /// <summary>Occurs when [do plant growth].</summary>
        public event EventHandler DoPlantGrowth;                       //This will be removed when comms are better sorted  do not use  MicroClimate only
        /// <summary>Occurs when [do update].</summary>
        public event EventHandler DoUpdate;
        /// <summary>Occurs when [do management calculations].</summary>
        public event EventHandler DoManagementCalculations;
        /// <summary>Occurs when [do report calculations].</summary>
        public event EventHandler DoReportCalculations;
        /// <summary>Occurs when [do report].</summary>
        public event EventHandler DoReport;
        /// <summary> Process stock methods in GrazPlan Stock </summary>
        public event EventHandler DoStock;
        /// <summary> Process a Pest and Disease lifecycle object </summary>
        public event EventHandler DoLifecycle;

		/// <summary>WholeFarm cut and carry</summary>
		public event EventHandler WFDoCutAndCarry;
		/// <summary>WholeFarm update pasture</summary>
		public event EventHandler WFUpdatePasture;
		/// <summary>WholeFarm buy fodder</summary>
		public event EventHandler WFBuyFoodStores;
		/// <summary>WholeFarm Do Animal (Ruminant and Other) Breeding and milk calculations</summary>
		public event EventHandler WFAnimalBreeding;
		/// <summary>Get potential intake. This includes suckling milk consumption</summary>
		public event EventHandler WFPotentialIntake;
		/// <summary>WholeFarm Activities Request Resources for this month</summary>
		public event EventHandler WFRequestResources;
		/// <summary>WholeFarm Resources do Arbitration based on what Activities have asked for it's resource</summary>
		public event EventHandler WFDoResourceAllocation;
		/// <summary>WholeFarm Activities now make decisions based on the resources they were given this month</summary>
		public event EventHandler WFResourcesAllocated;
		/// <summary>WholeFarm Event to call all feed requests for the time step</summary>
		public event EventHandler WFRequestFeed;
		/// <summary>WholeFarm Calculate Animals (Ruminant and Other) actual intake from amount give by the Fodder, pasture and supplement Resource</summary>
		public event EventHandler WFFeedAllocation;
		/// <summary>WholeFarm Calculate Animals (Ruminant and Other) milk production</summary>
		public event EventHandler WFAnimalMilkProduction;
		/// <summary>WholeFarm Calculate Animals(Ruminant and Other) weight gain</summary>
		public event EventHandler WFAnimalWeightGain;
		/// <summary>WholeFarm Do Animal (Ruminant and Other) death</summary>
		public event EventHandler WFAnimalDeath;
		/// <summary>WholeFarm Do Animal (Ruminant and Other) milking</summary>
		public event EventHandler WFAnimalMilking;
		/// <summary>WholeFarm Do Animal (Ruminant and Other) Herd Management (Kulling, Castrating, Weaning, etc.)</summary>
		public event EventHandler WFAnimalManage;
		/// <summary>WholeFarm stock animals to pasture availability or other metrics</summary>
		public event EventHandler WFAnimalStock;
		/// <summary>WholeFarm sell animals to market including transporting and labour</summary>
		public event EventHandler WFAnimalSell;
		/// <summary>WholeFarm Age your resources (eg. Decomose Fodder, Age your labour, Age your Animals)</summary>
		public event EventHandler WFAgeResources;

		///// <summary>WholeFarm Activities Request Resources for this month</summary>
		//public event EventHandler WFRequestResources;
  //      /// <summary>WholeFarm Resources do Arbitration based on what Activities have asked for it's resource</summary>
  //      public event EventHandler WFDoResourceAllocation;
  //      /// <summary>WholeFarm Activities now make decisions based on the resources they were given this month</summary>
  //      public event EventHandler WFResourcesAllocated;
  //      /// <summary>WholeFarm Calculate Ruminant Animals actual intake from the amount given by the Pasture Resource</summary>
  //      public event EventHandler WFRuminantGraze;
  //      /// <summary>WholeFarm Calculate Animals (Ruminant and Other) actual intake from amount give by the Fodder Resource</summary>
  //      public event EventHandler WFAnimalFeed;
  //      /// <summary>WholeFarm Calculate Animals (Ruminant and Other) Growth</summary>
  //      public event EventHandler WFAnimalGrowth;
  //      /// <summary>WholeFarm Do Animal (Ruminant and Other) Breeding</summary>
  //      public event EventHandler WFAnimalBreed;
  //      /// <summary>WholeFarm Do Animal (Ruminant and Other) Herd Management (Kulling, Castrating, Weaning, etc.)</summary>
  //      public event EventHandler WFAnimalManageHerd;
  //      /// <summary>WholeFarm Trade Animals (Ruminant and Other)</summary>
  //      public event EventHandler WFAnimalTrade;
  //      /// <summary>WholeFarm Age your resources (eg. Decomose Fodder, Age your labour, Age your Animals)</summary>
  //      public event EventHandler WFAgeResources;

        // Public properties available to other models.
        /// <summary>Gets the today.</summary>
        /// <value>The today.</value>
        [XmlIgnore]
        public DateTime Today { get; private set; }

        /// <summary>An event handler to allow us to initialise ourselves.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        [EventSubscribe("Commencing")]
        private void OnSimulationCommencing(object sender, EventArgs e)
        {
            Today = StartDate;
        }

        /// <summary>An event handler to signal start of a simulation.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        [EventSubscribe("DoCommence")]
        private void OnDoCommence(object sender, Simulation.CommenceArgs e)
        {
            if (DoInitialSummary != null)
                DoInitialSummary.Invoke(this, args);

            if (StartOfSimulation != null)
                StartOfSimulation.Invoke(this, args);

            while (Today <= EndDate)
            {
                // If this is being run on a background worker thread then check for cancellation
                if (e != null && e.workerThread != null && e.workerThread.CancellationPending)
                {
                    Summary.WriteMessage(this, "Simulation cancelled");
                    return;
                }

                if (DoWeather != null)
                    DoWeather.Invoke(this, args);

                if (DoDailyInitialisation != null)
                    DoDailyInitialisation.Invoke(this, args);

                if (StartOfDay != null)
                    StartOfDay.Invoke(this, args);

                if (Today.Day == 1 && StartOfMonth != null)
                {
                    StartOfMonth.Invoke(this, args);

					if (WFUpdatePasture != null)
						WFUpdatePasture.Invoke(this, args);
					if (WFRequestResources != null)
						WFRequestResources.Invoke(this, args);
					if (WFDoResourceAllocation != null)
						WFDoResourceAllocation.Invoke(this, args);
					if (WFResourcesAllocated != null)
						WFResourcesAllocated.Invoke(this, args);
					if (WFDoCutAndCarry != null)
						WFDoCutAndCarry.Invoke(this, args);
					if (WFBuyFoodStores != null)
						WFBuyFoodStores.Invoke(this, args);
					if (WFAnimalBreeding != null)
						WFAnimalBreeding.Invoke(this, args);
					if (WFPotentialIntake != null)
						WFPotentialIntake.Invoke(this, args);
					if (WFRequestFeed != null)
						WFRequestFeed.Invoke(this, args);
					if (WFFeedAllocation != null)
						WFFeedAllocation.Invoke(this, args);
					if (WFAnimalMilkProduction != null)
						WFAnimalMilkProduction.Invoke(this, args);
					if (WFAnimalWeightGain != null)
						WFAnimalWeightGain.Invoke(this, args);
					if (WFAnimalDeath != null)
						WFAnimalDeath.Invoke(this, args);
					if (WFAnimalMilking != null)
						WFAnimalMilking.Invoke(this, args);
					if (WFAnimalManage != null)
						WFAnimalManage.Invoke(this, args);
					if (WFAnimalStock != null)
						WFAnimalStock.Invoke(this, args);
					if (WFAnimalSell != null)
						WFAnimalSell.Invoke(this, args);
					if (WFAgeResources != null)
						WFAgeResources.Invoke(this, args);

					//if (WFRequestResources != null)
     //                   WFRequestResources.Invoke(this, args);
     //               if (WFDoResourceAllocation != null)
     //                   WFDoResourceAllocation.Invoke(this, args);
     //               if (WFResourcesAllocated != null)
     //                   WFResourcesAllocated.Invoke(this, args);
     //               if (WFRuminantGraze != null)
     //                   WFRuminantGraze.Invoke(this, args);
     //               if (WFAnimalFeed != null)
     //                   WFAnimalFeed.Invoke(this, args);
     //               if (WFAnimalGrowth != null)
     //                   WFAnimalGrowth.Invoke(this, args);
     //               if (WFAnimalBreed != null)
     //                   WFAnimalBreed.Invoke(this, args);
     //               if (WFAnimalManageHerd != null)
     //                   WFAnimalManageHerd.Invoke(this, args);
     //               if (WFAnimalTrade != null)
     //                   WFAnimalTrade.Invoke(this, args);
     //               if (WFAgeResources != null)
     //                   WFAgeResources.Invoke(this, args);
                }

                if (Today.DayOfYear == 1 && StartOfYear != null)
                    StartOfYear.Invoke(this, args);

                if (Today.DayOfWeek == DayOfWeek.Sunday && StartOfWeek != null)
                    StartOfWeek.Invoke(this, args);

                if (Today.DayOfWeek == DayOfWeek.Saturday && EndOfWeek != null)
                    EndOfWeek.Invoke(this, args);

                if (DoManagement != null)
                    DoManagement.Invoke(this, args);

                if (DoEnergyArbitration != null)
                    DoEnergyArbitration.Invoke(this, args);

                if (DoSoilWaterMovement != null)
                    DoSoilWaterMovement.Invoke(this, args);

                if (DoSoilOrganicMatter != null)
                    DoSoilOrganicMatter.Invoke(this, args);

                if (DoSurfaceOrganicMatterDecomposition != null)
                    DoSurfaceOrganicMatterDecomposition.Invoke(this, args);
                if (Today.DayOfYear == 16)
                { }
                if (DoWaterArbitration != null)
                    DoWaterArbitration.Invoke(this, args);

                if (DoPhenology != null)
                    DoPhenology.Invoke(this, args);

                if (DoPotentialPlantGrowth != null)
                    DoPotentialPlantGrowth.Invoke(this, args);

                if (DoPotentialPlantPartioning != null)
                    DoPotentialPlantPartioning.Invoke(this, args);

                if (DoNutrientArbitration != null)
                    DoNutrientArbitration.Invoke(this, args);

                if (DoActualPlantPartioning != null)
                    DoActualPlantPartioning.Invoke(this, args);

                if (DoActualPlantGrowth != null)
                    DoActualPlantGrowth.Invoke(this, args);

                if (DoPlantGrowth != null)
                    DoPlantGrowth.Invoke(this, args);

                if (DoUpdate != null)
                    DoUpdate.Invoke(this, args);

                if (DoManagementCalculations != null)
                    DoManagementCalculations.Invoke(this, args);

                if (DoStock != null)
                    DoStock.Invoke(this, args);

                if (DoLifecycle != null)
                    DoLifecycle.Invoke(this, args);

                if (DoReportCalculations != null)
                    DoReportCalculations.Invoke(this, args);

                if (Today == EndDate && EndOfSimulation != null)
                    EndOfSimulation.Invoke(this, args);

                if (Today.Day == 31 && Today.Month == 12 && EndOfYear != null)
                    EndOfYear.Invoke(this, args);

                if (Today.AddDays(1).Day == 1 && EndOfMonth != null) // is tomorrow the start of a new month?
                    EndOfMonth.Invoke(this, args);

                if (EndOfDay != null)
                    EndOfDay.Invoke(this, args);

                if (DoReport != null)
                    DoReport.Invoke(this, args);

                Today = Today.AddDays(1);
            }

            Summary.WriteMessage(this, "Simulation terminated normally");
        }
    }
}