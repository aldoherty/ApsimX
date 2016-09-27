﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models.Core;
using Models.Factorial;
using APSIM.Shared.Utilities;
using System.ComponentModel;
using System.IO;

namespace Models.Factorial
{
    /// <summary>
    /// Encapsulates a factorial experiment.f
    /// </summary>
    [Serializable]
    [ViewName("UserInterface.Views.MemoView")]
    [PresenterName("UserInterface.Presenters.ExperimentPresenter")]
    [ValidParent(ParentType = typeof(Simulations))]
    public class Experiment : Model, JobManager.IRunnable
    {
        /// <summary>Called to start the job.</summary>
        /// <param name="jobManager">The job manager running this job.</param>
        /// <param name="workerThread">The thread this job is running on.</param>
        public void Run(JobManager jobManager, BackgroundWorker workerThread)
        {
            List<List<FactorValue>> allCombinations = AllCombinations();
            Simulation baseSimulation = Apsim.Child(this, typeof(Simulation)) as Simulation;
            Simulations parentSimulations = Apsim.Parent(this, typeof(Simulations)) as Simulations;

            Stream serialisedBase = Apsim.SerialiseToStream(baseSimulation) as Stream;

            List<Simulation> simulations = new List<Simulation>();
            foreach (List<FactorValue> combination in allCombinations)
            {
                string newSimulationName = Name;
                foreach (FactorValue value in combination)
                    newSimulationName += value.Name;

                Simulation newSimulation = Apsim.DeserialiseFromStream(serialisedBase) as Simulation;
                newSimulation.Name = newSimulationName;
                newSimulation.Parent = null;
                newSimulation.FileName = parentSimulations.FileName;
                Apsim.ParentAllChildren(newSimulation);

                // Make substitutions.
                Simulations.MakeSubstitutions(parentSimulations, new List<Simulation> { newSimulation });

                // Call OnLoaded in all models.
                foreach (Model child in Apsim.ChildrenRecursively(newSimulation))
                    Apsim.CallEventHandler(child, "Loaded", null);

                foreach (FactorValue value in combination)
                    value.ApplyToSimulation(newSimulation);
                
                PushFactorsToReportModels(newSimulation, combination);
                jobManager.AddChildJob(this, newSimulation);
            }
        }

        /// <summary>Find all report models and give them the factor values.</summary>
        /// <param name="factorValues">The factor values to send to each report model.</param>
        /// <param name="simulation">The simulation to search for report models.</param>
        private void PushFactorsToReportModels(Simulation simulation, List<FactorValue> factorValues)
        {
            List<string> names = new List<string>();
            List<string> values = new List<string>();

            foreach (FactorValue factorValue in factorValues)
            {
                Factor topLevelFactor = factorValue.Factor;
                if (topLevelFactor.Parent is Factor)
                    topLevelFactor = topLevelFactor.Parent as Factor;
                string name = topLevelFactor.Name;
                string value = factorValue.Name.Replace(topLevelFactor.Name, "");
                if (value == string.Empty)
                {
                    name = "Factors";
                    value = factorValue.Name;
                }
                names.Add(name);
                values.Add(value);
            }

            foreach (Report.Report report in Apsim.ChildrenRecursively(simulation, typeof(Report.Report)))
            {
                report.ExperimentFactorNames = names;
                report.ExperimentFactorValues = values;
            }
        }

        /// <summary>
        /// Gets the base simulation
        /// </summary>
        public Simulation BaseSimulation
        {
            get
            {
                return Apsim.Child(this, typeof(Simulation)) as Simulation;
            }
        }

        /// <summary>
        /// Create a specific simulation.
        /// </summary>
        public Simulation CreateSpecificSimulation(string name)
        {
            List<List<FactorValue>> allCombinations = AllCombinations();
            Simulation baseSimulation = Apsim.Child(this, typeof(Simulation)) as Simulation;
            Simulations parentSimulations = Apsim.Parent(this, typeof(Simulations)) as Simulations;

            foreach (List<FactorValue> combination in allCombinations)
            {
                string newSimulationName = Name;
                foreach (FactorValue value in combination)
                    newSimulationName += value.Name;

                if (newSimulationName == name)
                {
                    Simulation newSimulation = Apsim.Clone(baseSimulation) as Simulation;
                    newSimulation.Name = newSimulationName;
                    newSimulation.Parent = null;
                    newSimulation.FileName = parentSimulations.FileName;
                    Apsim.ParentAllChildren(newSimulation);

                    // Make substitutions.
                    Simulations.MakeSubstitutions(parentSimulations, new List<Simulation> { newSimulation });

                    // Connect events and links in our new  simulation.
                    foreach (Model child in Apsim.ChildrenRecursively(newSimulation))
                        Apsim.CallEventHandler(child, "Loaded", null);

                    foreach (FactorValue value in combination)
                        value.ApplyToSimulation(newSimulation);

                    PushFactorsToReportModels(newSimulation, combination);

                    return newSimulation;
                }
            }

            return null;
        }

        /// <summary>
        /// Return a list of simulation names.
        /// </summary>
        public string[] Names()
        {
            List<List<FactorValue>> allCombinations = AllCombinations();

            List<string> names = new List<string>();
            if (allCombinations != null)
            {
                foreach (List<FactorValue> combination in allCombinations)
                {
                    string newSimulationName = Name;

                    foreach (FactorValue value in combination)
                        newSimulationName += value.Name;

                    names.Add(newSimulationName);
                }
            }
            return names.ToArray();
        }

        /// <summary>
        /// Return a list of list of factorvalue objects for all permutations.
        /// </summary>
        private List<List<FactorValue>> AllCombinations()
        {
            Factors Factors = Apsim.Child(this, typeof(Factors)) as Factors;

            // Create a list of list of factorValues so that we can do permutations of them.
            List<List<FactorValue>> allValues = new List<List<FactorValue>>();
            if (Factors != null)
            {
                bool doFullFactorial = false;
                foreach (Factor factor in Factors.factors)
                {
                    List<FactorValue> factorValues = factor.CreateValues();
                    allValues.Add(factorValues);
                    doFullFactorial = doFullFactorial || factorValues.Count > 1;
                }
                if (doFullFactorial)
                    return MathUtilities.AllCombinationsOf<FactorValue>(allValues.ToArray());
                else
                    return allValues;
            }
            return null;
        }
    }
}
