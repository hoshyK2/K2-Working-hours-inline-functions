using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SourceCode.Workflow.Management;
using K2NE;

namespace WorkingHoursInlineFunctionsTest
{
    /// <summary>
    /// *** This require that it's run on a K2 Server machine.
    /// *** This requries the a set of working hours as follows:
    /// *** Name: Test Working Hours
    /// *** Monday - Friday, 9am - 5pm, excluding 1 hour lunch from 12pm - 1pm
    /// *** Exception Dates: 
    ///         December 26th 2011 (25th is a Sunday!)
    ///         Jan 2nd 2012 
    /// *** Special Dates: 
    ///         June 30th 2011 9am - 12pm
    ///         March 11th 2011 9am - 12pm
    /// </summary>
    [TestClass]
    public class TestInlineFunctions
    {
        private string workingHoursZoneName = "Test Working Hours";

        public TestInlineFunctions()
        {
            //
            // TODO: Add constructor logic here
            //
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

        [TestMethod]
        public void DateDiffTestCase()
        {

            //TODO: Refactor to seperate test methods.

            ValidateWorkingHours();

            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            Double result = new Double();

            //starts inside, ends outside - crosses multiple sections
            startDate = new DateTime(2011, 03, 14, 10, 00, 00);
            endDate = new DateTime(2011, 03, 14, 19, 00, 00);
            result = WorkingHoursInlineFunctions.WorkingHoursDateDiffAsMinutes(workingHoursZoneName, startDate, endDate);
            Assert.AreEqual(new TimeSpan(6, 0, 0).TotalMinutes, result);

            //starts outside, ends inside - crossing multiple sections
            startDate = new DateTime(2011,03,14,08,00,00);
            endDate = new DateTime(2011,03,14,15,00,00);
            result = WorkingHoursInlineFunctions.WorkingHoursDateDiffAsMinutes(workingHoursZoneName, startDate, endDate);
            Assert.AreEqual(new TimeSpan(5, 0, 0).TotalMinutes, result);

            //starts inside, ends inside - crossing multiple sections
            startDate = new DateTime(2011, 03, 14, 10, 00, 00);
            endDate = new DateTime(2011, 03, 14, 15, 00, 00);
            result = WorkingHoursInlineFunctions.WorkingHoursDateDiffAsMinutes(workingHoursZoneName, startDate, endDate);
            Assert.AreEqual(new TimeSpan(4, 0, 0).TotalMinutes, result);

            //one exception day
            startDate = new DateTime(2011, 12, 26, 09, 00, 00);
            endDate = new DateTime(2011, 12, 26, 17, 00, 00);
            result = WorkingHoursInlineFunctions.WorkingHoursDateDiffAsMinutes(workingHoursZoneName, startDate, endDate);
            Assert.AreEqual(new TimeSpan(0, 0, 0).TotalMinutes, result);

            //starts inside, ends inside - crossing multiple sections and one exception day and one weekend day
            startDate = new DateTime(2011, 12, 26, 09, 00, 00);
            endDate = new DateTime(2011, 12, 27, 17, 00, 00);
            result = WorkingHoursInlineFunctions.WorkingHoursDateDiffAsMinutes(workingHoursZoneName, startDate, endDate);
            Assert.AreEqual(new TimeSpan(7, 0, 0).TotalMinutes, result);

            //crosses year boundry, starts outside, ends outside, crosses exception date
            startDate = new DateTime(2011, 12, 31, 09, 00, 00);
            endDate = new DateTime(2012, 01, 03, 19, 00, 00);
            result = WorkingHoursInlineFunctions.WorkingHoursDateDiffAsMinutes(workingHoursZoneName, startDate, endDate);
            Assert.AreEqual(new TimeSpan(7, 0, 0).TotalMinutes, result);
           
        }

        /// <summary>
        /// Check that the local K2 Server has a valid set of working hours for this test.
        /// </summary>
        private void ValidateWorkingHours()
        {

            WorkflowManagementServer mngServer = new WorkflowManagementServer("localhost", 5555);
            mngServer.Open();
            AvailabilityZone zone = mngServer.ZoneLoad(workingHoursZoneName);

            List<AvailabilityDate> specialDates = zone.AvailabilityDateList.Where(x => x.IsNonWorkDate == false).ToList();
            if (specialDates.Count > 2)
                Assert.Fail("Too many special dates");

            List<AvailabilityDate> exceptionDates = zone.AvailabilityDateList.Where(x => x.IsNonWorkDate == true).ToList();
            if (exceptionDates.Count > 2)
                Assert.Fail("Too many exception dates");

            if (specialDates.Where(x => x.WorkDate == new DateTime(2011, 06, 30, 09, 00, 00) && x.Duration == new TimeSpan(3, 0, 0)).ToList().Count != 1)
                Assert.Fail("Problem finding special date June 30th 2011 09:00 - 12:00");

            if (specialDates.Where(x => x.WorkDate == new DateTime(2011, 03, 11, 09, 00, 00) && x.Duration == new TimeSpan(3, 0, 0)).ToList().Count != 1)
                Assert.Fail("Problem finding special date March 11th 2011 09:00 - 12:00");

            if (exceptionDates.Where(x => x.WorkDate == new DateTime(2011, 12, 26)).ToList().Count != 1)
                Assert.Fail("Problem finding exception date December 26th 2011");

            if (exceptionDates.Where(x => x.WorkDate == new DateTime(2012, 1, 2)).ToList().Count != 1)
                Assert.Fail("Problem finding exception date Jan 2nd 2012");

            if (zone.AvailabilityHoursList.Where(x => x.WorkDay == DayOfWeek.Monday && x.Duration == new TimeSpan(3, 0, 0) && x.TimeOfDay == new TimeSpan(9, 0, 0)).ToList().Count != 1)
                Assert.Fail("Problem with Monday 9am - 12pm working hours");

            if (zone.AvailabilityHoursList.Where(x => x.WorkDay == DayOfWeek.Tuesday && x.Duration == new TimeSpan(3, 0, 0) && x.TimeOfDay == new TimeSpan(9, 0, 0)).ToList().Count != 1)
                Assert.Fail("Problem with Tuesday 9am - 12pm working hours");

            if (zone.AvailabilityHoursList.Where(x => x.WorkDay == DayOfWeek.Wednesday && x.Duration == new TimeSpan(3, 0, 0) && x.TimeOfDay == new TimeSpan(9, 0, 0)).ToList().Count != 1)
                Assert.Fail("Problem with Wednesday 9am - 12pm working hours");

            if (zone.AvailabilityHoursList.Where(x => x.WorkDay == DayOfWeek.Thursday && x.Duration == new TimeSpan(3, 0, 0) && x.TimeOfDay == new TimeSpan(9, 0, 0)).ToList().Count != 1)
                Assert.Fail("Problem with Thursday 9am - 12pm working hours");

            if (zone.AvailabilityHoursList.Where(x => x.WorkDay == DayOfWeek.Friday && x.Duration == new TimeSpan(3, 0, 0) && x.TimeOfDay == new TimeSpan(9, 0, 0)).ToList().Count != 1)
                Assert.Fail("Problem with Friday 9am - 12pm working hours");

            if (zone.AvailabilityHoursList.Where(x => x.WorkDay == DayOfWeek.Monday && x.Duration == new TimeSpan(4, 0, 0) && x.TimeOfDay == new TimeSpan(13, 0, 0)).ToList().Count != 1)
                Assert.Fail("Problem with Monday 1pm - 5pm working hours");

            if (zone.AvailabilityHoursList.Where(x => x.WorkDay == DayOfWeek.Tuesday && x.Duration == new TimeSpan(4, 0, 0) && x.TimeOfDay == new TimeSpan(13, 0, 0)).ToList().Count != 1)
                Assert.Fail("Problem with Tuesday 1pm - 5pm working hours");

            if (zone.AvailabilityHoursList.Where(x => x.WorkDay == DayOfWeek.Wednesday && x.Duration == new TimeSpan(4, 0, 0) && x.TimeOfDay == new TimeSpan(13, 0, 0)).ToList().Count != 1)
                Assert.Fail("Problem with Wednesday 1pm - 5pm working hours");

            if (zone.AvailabilityHoursList.Where(x => x.WorkDay == DayOfWeek.Thursday && x.Duration == new TimeSpan(4, 0, 0) && x.TimeOfDay == new TimeSpan(13, 0, 0)).ToList().Count != 1)
                Assert.Fail("Problem with Thursday 1pm - 5pm working hours");

            if (zone.AvailabilityHoursList.Where(x => x.WorkDay == DayOfWeek.Friday && x.Duration == new TimeSpan(4, 0, 0) && x.TimeOfDay == new TimeSpan(13, 0, 0)).ToList().Count != 1)
                Assert.Fail("Problem with Friday 1pm - 5pm working hours");

            if (zone.AvailabilityHoursList.Where(x => x.WorkDay == DayOfWeek.Saturday).ToList().Count > 0)
                Assert.Fail("Found working hours on Saturday where there should be none");

            if (zone.AvailabilityHoursList.Where(x => x.WorkDay == DayOfWeek.Sunday).ToList().Count > 0)
                Assert.Fail("Found working hours on Sunday wherer there should be none");

            mngServer.Connection.Close();
            mngServer.Connection.Dispose();

        }
    }
}
