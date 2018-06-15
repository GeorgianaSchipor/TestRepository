using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarketSigndownTestProject
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class MarketSigndownTest
    {
        Market.Market market;
        public MarketSigndownTest()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [Description("Check that empty input generates empty output."), TestMethod]
        public void TestMarketSigndown_01_Empty()
        {
            market = new Market.Market();
            List<Market.Underwriter> underwriters = new List<Market.Underwriter>();
            market.MarketSigndown(0, underwriters);


            Assert.IsTrue(underwriters.Count == 0, "Returned underwriters list is empty when imput list is empty too.");
        }

        [Description("Two UWs, the SUM of written lines = Total Order, not much to do here!"), TestMethod]
        public void TestMarketSigndown_02_Simple_Example_NoMins1()
        {

            // test the trivial case.
            market = new Market.Market();
            List<Market.Underwriter> listUWs = new List<Market.Underwriter>();
            listUWs.Add(new Market.Underwriter(10, "Sponge Bob", 40, 30, 0));
            listUWs.Add(new Market.Underwriter(20, "Patrick Star", 60, 20, 0));

            listUWs = market.MarketSigndown(100, listUWs);

            assertSignedLine(listUWs, "Sponge Bob", 40f);
            assertSignedLine(listUWs, "Patrick Star", 60f);
        }

        [Description("Similar to test 02 but with total order <> 100."), TestMethod]
        public void TestMarketSigndown_03_Simple_Example_NoMins_TotalOrderNot100()
        {

            // 
            market = new Market.Market();
            List<Market.Underwriter> listUWs = new List<Market.Underwriter>();
            listUWs.Add(new Market.Underwriter(10, "Sponge Bob", 30, 0, 0));
            listUWs.Add(new Market.Underwriter(20, "Patrick Star", 30, 0, 0));

            listUWs = market.MarketSigndown(60, listUWs);

            assertSignedLine(listUWs, "Sponge Bob", 30f);
            assertSignedLine(listUWs, "Patrick Star", 30f);
        }

        [Description("A very simple example with no minimums involved yet."), TestMethod]
        public void TestMarketSigndown_04_Simple_Example_NoMins2()
        {

            // test the trivial case.
            market = new Market.Market();
            List<Market.Underwriter> listUWs = new List<Market.Underwriter>();
            listUWs.Add(new Market.Underwriter(10, "Sponge Bob", 80, 10, 0));
            listUWs.Add(new Market.Underwriter(20, "Patrick Star", 40, 10, 0));
            listUWs.Add(new Market.Underwriter(30, "Squidward Tentacles", 40, 20, 0));

            listUWs = market.MarketSigndown(100, listUWs);

            assertSignedLine(listUWs, "Sponge Bob", 50f);
            assertSignedLine(listUWs, "Patrick Star", 25f);
            assertSignedLine(listUWs, "Squidward Tentacles", 25f);
        }

        [Description("A very simple example, same as above but with Total order <> 100."), TestMethod]
        public void TestMarketSigndown_05_Simple_Example_NoMins3()
        {

            // test the trivial case.
            market = new Market.Market();
            List<Market.Underwriter> listUWs = new List<Market.Underwriter>();
            listUWs.Add(new Market.Underwriter(10, "Sponge Bob", 80, 0, 0));
            listUWs.Add(new Market.Underwriter(20, "Patrick Star", 40, 0, 0));
            listUWs.Add(new Market.Underwriter(30, "Squidward Tentacles", 40, 0, 0));

            listUWs = market.MarketSigndown(50, listUWs);

            assertSignedLine(listUWs, "Sponge Bob", 25f);
            assertSignedLine(listUWs, "Patrick Star", 12.5f);
            assertSignedLine(listUWs, "Squidward Tentacles", 12.5f);
        }

        [Description("Minimums involved. The 3rd UW wouldnt get her minimum, so we need to give her the minimum and split what is left between the other 2 UWs."), TestMethod]
        public void TestMarketSigndown_06_Mins1_AsPerExcel_Example()
        {

            // test the trivial case.
            market = new Market.Market();
            List<Market.Underwriter> listUWs = new List<Market.Underwriter>();
            listUWs.Add(new Market.Underwriter(10, "Sponge Bob", 80, 10, 0));
            listUWs.Add(new Market.Underwriter(20, "Patrick Star", 90, 10, 0));
            listUWs.Add(new Market.Underwriter(30, "Squidward Tentacles", 40, 20, 0));

            listUWs = market.MarketSigndown(100, listUWs);

            assertSignedLine(listUWs, "Sponge Bob", 37.65f);
            assertSignedLine(listUWs, "Patrick Star", 42.35f);
            assertSignedLine(listUWs, "Squidward Tentacles", 20f);
        }

        [Description("Same as above. Please notice that the difference in minimums between this test and 06 doesnt affect the final result."), TestMethod]
        public void TestMarketSigndown_07_Mins2_2DecimalPlaces_SameResultsAsTest06()
        {

            // test the trivial case.
            market = new Market.Market();
            List<Market.Underwriter> listUWs = new List<Market.Underwriter>();
            listUWs.Add(new Market.Underwriter(10, "Sponge Bob", 80, 20, 0));
            listUWs.Add(new Market.Underwriter(20, "Patrick Star", 90, 40, 0));
            listUWs.Add(new Market.Underwriter(30, "Squidward Tentacles", 40, 20, 0));

            listUWs = market.MarketSigndown(100, listUWs);

            assertSignedLine(listUWs, "Sponge Bob", 37.65f);
            assertSignedLine(listUWs, "Patrick Star", 42.35f);
            assertSignedLine(listUWs, "Squidward Tentacles", 20f);
        }

        [Description("The second UW gets the min, the rest is assigned to the other UWs."), TestMethod]
        public void TestMarketSigndown_08_Mins3_UW2GetsMinSoTheRestForUW1()
        {

            // test the trivial case.
            market = new Market.Market();
            List<Market.Underwriter> listUWs = new List<Market.Underwriter>();
            listUWs.Add(new Market.Underwriter(10, "Sponge Bob", 80, 20, 0));
            listUWs.Add(new Market.Underwriter(20, "Patrick Star", 90, 70, 0));

            listUWs = market.MarketSigndown(100, listUWs);

            assertSignedLine(listUWs, "Sponge Bob", 30f);
            assertSignedLine(listUWs, "Patrick Star", 70f);
        }


        [Description("Another example with minimums involved. We give the last 2 their minimum and we give the rest to the 1st UW, as we are sorting them by sequence number."), TestMethod]
        public void TestMarketSigndown_09_Mins4_SortUWsBySequenceNumber()
        {

            // test the trivial case.
            market = new Market.Market();
            List<Market.Underwriter> listUWs = new List<Market.Underwriter>();
            listUWs.Add(new Market.Underwriter(10, "Sponge Bob", 80, 20, 0));
            listUWs.Add(new Market.Underwriter(20, "Patrick Star", 90, 50, 0));
            listUWs.Add(new Market.Underwriter(30, "Squidward Tentacles", 40, 20, 0));

            listUWs = market.MarketSigndown(100, listUWs);

            assertSignedLine(listUWs, "Sponge Bob", 30f);
            assertSignedLine(listUWs, "Patrick Star", 50f);
            assertSignedLine(listUWs, "Squidward Tentacles", 20f);
        }

        [Description("The minimums add up to the total order so that's the solution."), TestMethod]
        public void TestMarketSigndown_10_Mins5_MinimumsAreTheSolution()
        {

            // test the trivial case.
            market = new Market.Market();
            List<Market.Underwriter> listUWs = new List<Market.Underwriter>();
            listUWs.Add(new Market.Underwriter(10, "Sponge Bob", 80, 34, 0));
            listUWs.Add(new Market.Underwriter(20, "Patrick Star", 90, 33, 0));
            listUWs.Add(new Market.Underwriter(30, "Squidward Tentacles", 40, 33, 0));

            listUWs = market.MarketSigndown(100, listUWs);

            assertSignedLine(listUWs, "Sponge Bob", 34f);
            assertSignedLine(listUWs, "Patrick Star", 33f);
            assertSignedLine(listUWs, "Squidward Tentacles", 33f);
        }

        public void assertSignedLine(List<Market.Underwriter> listUWs, string name, float signedLine)
        {
            Market.Underwriter u1 = listUWs.Find(u => u.Name.Equals(name));
            Assert.IsNotNull(u1, " underwriter " + name + " was not found!");

            Assert.AreEqual(u1.SignedLine, signedLine, 0.1f, " expected signed line for " + name + " is " + signedLine.ToString() + ". It was " + u1.SignedLine.ToString());
        }
    }
}
