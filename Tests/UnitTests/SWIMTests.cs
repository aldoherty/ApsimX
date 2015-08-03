﻿// -----------------------------------------------------------------------
// <copyright file="SWIMTests.cs" company="APSIM Initiative">
//     Copyright (c) APSIM Initiative
// </copyright>
//
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SWIMFrame;

namespace UnitTests
{
    /// <summary>
    /// A suite of tests for the SWIM framework.
    /// All input and output values were retrieved from FORTRAN version using WRITE statements
    /// at start and end of method.
    /// </summary>
    [TestFixture]
    class SWIMTests
    {
        /// <summary>
        /// Test method nonlin.
        /// </summary>
        [Test]
        public void TestNonlin()
        {
            int[] n = new int[] { 23, 23, 23, 23 };
            double[][] x = new double[][] { new double[] { 3.354301E-06, 9.062281E-06, 2.211101E-05, 5.405671E-05, 1.286441E-04, 3.034657E-04, 7.068768E-04, 1.632747E-03, 3.740277E-03, 8.510290E-03, 1.924374E-02, 4.327070E-02, 9.674788E-02, 2.148795E-01, 4.724994E-01, 1.019938E+00, 2.120870E+00, 4.103255E+00, 7.062010E+00, 1.052146E+01, 1.373546E+01, 1.628369E+01, 1.813795E+01 },
                                            new double[] { 3.354301E-06,9.062281E-06,2.211101E-05,5.405671E-05,1.286441E-04,3.034657E-04,7.068768E-04,1.632747E-03,3.740277E-03,8.510290E-03,1.924374E-02,4.327070E-02,9.674788E-02,2.148795E-01,4.724994E-01,1.019938E+00,2.120870E+00,4.103255E+00,7.062010E+00,1.052146E+01,1.373546E+01,1.628369E+01,1.813795E+01 },
                                            new double[] { 1.740164E-06,5.488312E-06,1.750446E-05,5.750873E-05,1.811315E-04,5.503100E-04,1.607630E-03,4.531185E-03,1.234010E-02,3.249982E-02,8.262891E-02,2.015138E-01,4.644696E-01,9.847437E-01,1.851519E+00,3.002137E+00,4.207595E+00,5.252194E+00,6.054463E+00,6.631843E+00,7.034421E+00,7.311037E+00,7.49985218 },
                                            new double[] { 1.740164E-06,5.488312E-06,1.750446E-05,5.750873E-05,1.811315E-04,5.503100E-04,1.607630E-03,4.531185E-03,1.234010E-02,3.249982E-02,8.262891E-02,2.015138E-01,4.644696E-01,9.847437E-01,1.851519E+00,3.002137E+00,4.207595E+00,5.252194E+00,6.054463E+00,6.631843E+00,7.034421E+00,7.311037E+00,7.49985218 }
                                          };
            double[][] y = new double[][] { new double[] { 8.740528E-10, -1.139631E-06, -3.745951E-06, -1.012318E-05, -2.500095E-05, -5.983057E-05, -1.400616E-04, -3.237275E-04, -7.402293E-04, -1.677716E-03, -3.770240E-03, -8.399180E-03, -1.852487E-02, -4.033605E-02, -8.621387E-02, -1.789396E-01, -3.537897E-01, -6.472698E-01, -1.061788E+00, -1.537311E+00, -1.986433E+00, -2.354419E+00, -2.631054E+00 },
                                            new double[] { 8.740528E-10,-5.688333E-07,-1.870282E-06,-5.052952E-06,-1.247203E-05,-2.981977E-05,-6.971090E-05,-1.607958E-04,-3.665741E-04,-8.272015E-04,-1.847065E-03,-4.076566E-03,-8.869883E-03,-1.893890E-02,-3.937207E-02,-7.867789E-02,-1.482514E-01,-2.571233E-01,-4.019165E-01,-5.627090E-01,-7.143035E-01,-8.406170E-01,-0.937671313 },
                                            new double[] { 4.534462E-10,-1.860098E-07,-7.827367E-07,-2.763741E-06,-8.861448E-06,-2.696707E-05,-7.839762E-05,-2.189419E-04,-5.880709E-04,-1.518430E-03,-3.755019E-03,-8.819437E-03,-1.937167E-02,-3.886927E-02,-6.938366E-02,-1.084229E-01,-1.492204E-01,-1.853670E-01,-2.139873E-01,-2.351721E-01,-2.502733E-01,-0.260818333,-0.268098632 },
                                            new double[] { 4.534462E-10,-1.235415E-07,-5.200053E-07,-1.834312E-06,-5.871893E-06,-1.782608E-05,-5.164364E-05,-1.435129E-04,-3.827704E-04,-9.785819E-04,-2.386859E-03,-5.502670E-03,-1.180372E-02,-2.305212E-02,-4.009008E-02,-6.140720E-02,-8.352305E-02,-1.032118E-01,-1.189630E-01,-1.307495E-01,-1.392288E-01,-0.145191196,-0.149328616 }
                                          };
            double[] re = new double[] { 1E-2, 1E-2, 1E-2, 1E-2 };
            int[] expected = new int[] { 9, 7, 5, 4 };

            for (int i = 0; i < n.Length; i++)
                Assert.AreEqual(expected[i], (int)Extensions.TestMethod("Fluxes", "nonlin", new object[] { n[i], x[i], y[i], re[i] }));
        }

        /// <summary>
        /// Test method odef.
        /// </summary>
        [Test]
        public void TestOdef()
        {
            int[] n1 = new int[] { 1, 1, 1, 20 };
            int[] n2 = new int[] { 2, 2, 12, 23 };
            SoilProps sp = Soil.gensptbl(1.0, new SoilParam(10, 103, 0.4, 2.0, -2.0, -10.0, 1.0 / 3.0, 1.0), true);
            Fluxes.FluxTable(1.0, sp);
            double[][] aK = new double[][] {new double[] { 8.740528E-10,3.148991E-09,1.116638E-08,3.906024E-08,1.350389E-07,4.621461E-07,1.567779E-06,5.278070E-06,1.765091E-05,5.868045E-05,1.940329E-04,6.381824E-04,2.086113E-03,6.757548E-03,2.152482E-02,6.618264E-02,1.887549E-01,4.655217E-01,9.153457E-01,1.393520E+00,1.733586E+00,1.916091084,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                                            new double[] { 8.740528E-10,3.148991E-09,1.116638E-08,3.906024E-08,1.350389E-07,4.621461E-07,1.567779E-06,5.278070E-06,1.765091E-05,5.868045E-05,1.940329E-04,6.381824E-04,2.086113E-03,6.757548E-03,2.152482E-02,6.618264E-02,1.887549E-01,4.655217E-01,9.153457E-01,1.393520E+00,1.733586E+00,1.916091E+00,2.000000E+00,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                            new double[] { 8.740528E-10,3.148991E-09,1.116638E-08,3.906024E-08,1.350389E-07,4.621461E-07,1.567779E-06,5.278070E-06,1.765091E-05,5.868045E-05,1.940329E-04,6.381824E-04,2.086113E-03,6.757548E-03,2.152482E-02,6.618264E-02,1.887549E-01,4.655217E-01,9.153457E-01,1.393520E+00,1.733586E+00,1.916091E+00,2.000000E+00,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                            new double[] { 8.740528E-10,3.148991E-09,1.116638E-08,3.906024E-08,1.350389E-07,4.621461E-07,1.567779E-06,5.278070E-06,1.765091E-05,5.868045E-05,1.940329E-04,6.381824E-04,2.086113E-03,6.757548E-03,2.152482E-02,6.618264E-02,1.887549E-01,4.655217E-01,9.153457E-01,1.393520E+00,1.733586E+00,1.916091E+00,2.000000E+00,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
                                            };
            double[][] hpK = new double[][] { new double[] { 1.942348E-09, 6.760092E-09, 2.390674E-08, 8.260039E-08, 2.837631E-07, 9.641152E-07, 3.252644E-06, 1.089420E-05, 3.627295E-05, 1.201039E-04, 3.956002E-04, 1.295509E-03, 4.209049E-03, 1.348672E-02, 4.200805E-02, 1.232292E-01, 3.212703E-01, 6.904247E-01, 1.165940E+00, 1.578200E+00, 1.834723657, 1.963039316, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                              new double[] { 1.942348E-09,6.760092E-09,2.390674E-08,8.260039E-08,2.837631E-07,9.641152E-07,3.252644E-06,1.089420E-05,3.627295E-05,1.201039E-04,3.956002E-04,1.295509E-03,4.209049E-03,1.348672E-02,4.200805E-02,1.232292E-01,3.212703E-01,6.904247E-01,1.165940E+00,1.578200E+00,1.834724E+00,1.963039E+00,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                              new double[] { 1.942348E-09,6.760092E-09,2.390674E-08,8.260039E-08,2.837631E-07,9.641152E-07,3.252644E-06,1.089420E-05,3.627295E-05,1.201039E-04,3.956002E-04,1.295509E-03,4.209049E-03,1.348672E-02,4.200805E-02,1.232292E-01,3.212703E-01,6.904247E-01,1.165940E+00,1.578200E+00,1.834724E+00,1.963039E+00,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                              new double[] { 1.942348E-09,6.760092E-09,2.390674E-08,8.260039E-08,2.837631E-07,9.641152E-07,3.252644E-06,1.089420E-05,3.627295E-05,1.201039E-04,3.956002E-04,1.295509E-03,4.209049E-03,1.348672E-02,4.200805E-02,1.232292E-01,3.212703E-01,6.904247E-01,1.165940E+00,1.578200E+00,1.834724E+00,1.963039E+00,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
                                            };
            double[][] u = new double[][] { new double[] { 4.9914082920553025, 4364796.0347727695 },
                                            new double[] { 5.0000148144063985, 4379861.1673235269 },
                                            new double[] { 4.9682912374903765, -570.76826254349544 },
                                            new double[] { -1.8708427784692783, 0.46037822906582598}
                                          };

            for (int i = 0; i < n1.Length; i++)
                Assert.AreEqual(u[i], (double[])Extensions.TestMethod("Fluxes", "odef", new object[] { n1[i], n2[i], aK[i], hpK[i] }));
        }

        /// <summary>
        /// Test method Sdofh
        /// </summary>
        [Test]
        public void TestSdofh()
        {

        }

        /// <summary>
        /// Test method MVG.Params
        /// </summary>
        [Test]
        public void TestMVGParams()
        {
            Assert.AreEqual(true, MVG.TestParams(103, 9.0, 0.99670220130280185, 9.99999999999998460E-003));
            Assert.AreEqual(true, MVG.TestParams(109, 21.0, 0.99990576371017859, 0.25132106297918300));
        }

        [Test]
        public void TestGensptbl()
        {
            SoilProps sp = (SoilProps)Extensions.TestMethod("Soil", "gensptbl")
        }
    }
}
