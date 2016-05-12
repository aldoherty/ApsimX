// -----------------------------------------------------------------------
// <copyright file="ICanopy.cs" company="APSIM Initiative">
//     Copyright (c) APSIM Initiative
// </copyright>
//-----------------------------------------------------------------------
namespace Models.Interfaces
{
    using APSIM.Shared.Soils;
    using Models.PMF.Interfaces;
    using System;
    using System.Xml.Serialization;

    /// <summary>This interface describes interface for leaf interaction with Structure.</summary>
    public interface ILeaf
    {
        /// <summary>
        /// 
        /// </summary>
        bool CohortsInitialised { get; }
        /// <summary>
        /// 
        /// </summary>
        double PlantAppearedLeafNo { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProportionRemoved"></param>
        void DoThin(double ProportionRemoved);

        /// <summary>Daily photosynthetic "net" supply of dry matter for the whole plant (g DM/m2/day)</summary>
        /// <value>The dm supply.</value>
        [Units("g/m^2")]
        BiomassSupplyType DMSupply { get; }

        /// <summary>Gets or sets the water demand.</summary>
        /// <value>The water demand.</value>
        [XmlIgnore]
        [Units("mm")]
        double WaterDemand { get;}

         /// <summary>Gets the LAI</summary>
        [Units("m^2/m^2")]
        double LAI{get;}
         }
}

