﻿using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Text;

using SourceCode.Framework;
using SourceCode.Framework.Design;
using System.Security.Cryptography;
using SourceCode.Workflow.Management;

using System.Linq;

namespace WorkingHoursInlineFunctions
{
    /// <summary>
    /// A class comprising functions.
    /// </summary>
    // Must be a public class.
    [Category("WorkingHoursInlineFunctions.Properties.Resources", "CategoryName", typeof(WorkingHoursInlineFunctions))]
    public class WorkingHoursInlineFunctions
    {
        /*
         * To register the custom inline function with the K2 platform copy the assembly to the following folder on all K2 Servers:
         *   C:\Program Files\K2 blackpearl (x86)\Host Server\BIN
         * 
         * To register the custom inline function with the K2 client side design tools, copy the assembly to the following folder on all K2
         * development workstations:
         *   C:\Program Files\K2 blackpearl (x86)\BIN\Functions
         * 
         * Note: K2 Install path is usually: "C:\Program Files\K2 blackpearl (x86)"
         ***********************************************************************************************************************************
         * To register the custom inline function with the K2 Designer for SharePoint design tool, ensure the assembly has a strong name and
         * then GAC the assembly on all K2 Servers:
         *   "C:\Program Files\Microsoft SDKs\Windows\v6.0A\bin\gacutil" /if "CustomInlineFunction_Sample.dll"
         * 
         * Copy the icon file to:
         *   "C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\12\TEMPLATE\LAYOUTS\WebDesigner\Images\Functions"
         * 
         * Note: SharePoint Web Server Extensions install path is usually: "C:\Program Files (x86)\Common Files\Microsoft Shared\Web Server Extensions"
         */

        #region Default Constructor
        /// <summary>
        /// Instantiates a new Notification1.
        /// </summary>
        public WorkingHoursInlineFunctions()
        {
            // No implementation necessary.
        }
        #endregion

        #region Double WorkingHoursDateDiff(string zoneName, DateTime startDate, DateTime endDate)

        [DisplayName("WorkingHoursInlineFunctions.Properties.Resources", "DateDiffName", typeof(WorkingHoursInlineFunctions))]
        [Description("WorkingHoursInlineFunctions.Properties.Resources", "DateDiffDescription", typeof(WorkingHoursInlineFunctions))]
        [K2Icon(typeof(WorkingHoursInlineFunctions), "Resources.Icon.png")]
        public static Double WorkingHoursDateDiffAsMinutes(
            [DisplayName("WorkingHoursInlineFunctions.Properties.Resources", "DateDiffZoneName", typeof(WorkingHoursInlineFunctions))]
            [Description("WorkingHoursInlineFunctions.Properties.Resources", "DateDiffZoneDescription", typeof(WorkingHoursInlineFunctions))]
            string zoneName,
            [DisplayName("WorkingHoursInlineFunctions.Properties.Resources", "DateDiffStartDateName", typeof(WorkingHoursInlineFunctions))]
            [Description("WorkingHoursInlineFunctions.Properties.Resources", "DateDiffStartDateDescription", typeof(WorkingHoursInlineFunctions))]
            DateTime startDate,
            [DisplayName("WorkingHoursInlineFunctions.Properties.Resources", "DateDiffEndDateName", typeof(WorkingHoursInlineFunctions))]
            [Description("WorkingHoursInlineFunctions.Properties.Resources", "DateDiffEndDateDescription", typeof(WorkingHoursInlineFunctions))]
            DateTime endDate)
        {
            return WorkingHoursDateDiff(zoneName, startDate, endDate).TotalMinutes;        
        }

        #endregion

        #region DateTime CalculateEscalationDate(zoneName, startDate, seconds);

        /// <summary>
        /// Performs a function.
        /// </summary>
        /// <param name="sourceValue">A string representing the source value for which the function will perform an operation. Does not have to be string.</param>
        /// <returns>A string representing the result of the function. Does not have to be string.</returns>
        [DisplayName("WorkingHoursInlineFunctions.Properties.Resources", "CalculateEscalationDateName", typeof(WorkingHoursInlineFunctions))]
        [Description("WorkingHoursInlineFunctions.Properties.Resources", "CalculateEscalationDateDescription", typeof(WorkingHoursInlineFunctions))]
        [K2Icon(typeof(WorkingHoursInlineFunctions), "Resources.Icon.png")]
        // Must be a public static method.
        public static DateTime CalculateEscalationDate(
            [DisplayName("WorkingHoursInlineFunctions.Properties.Resources", "CalculateEscalationDateZoneNameName", typeof(WorkingHoursInlineFunctions))]
            [Description("WorkingHoursInlineFunctions.Properties.Resources", "CalculateEscalationDateZoneNameDescription", typeof(WorkingHoursInlineFunctions))]
            string zoneName,
            [DisplayName("WorkingHoursInlineFunctions.Properties.Resources", "CalculateEscalationDateStartDateName", typeof(WorkingHoursInlineFunctions))]
            [Description("WorkingHoursInlineFunctions.Properties.Resources", "CalculateEscalationDateStartDateDescription", typeof(WorkingHoursInlineFunctions))]
            DateTime startDate,
            [DisplayName("WorkingHoursInlineFunctions.Properties.Resources", "CalculateEscalationDateSecondsName", typeof(WorkingHoursInlineFunctions))]
            [Description("WorkingHoursInlineFunctions.Properties.Resources", "CalculateEscalationDateSecondsDescription", typeof(WorkingHoursInlineFunctions))]
            int seconds)
        {

            // Return the result.
            return CalculateEscalationDatePrivate(zoneName, startDate, seconds);
        }
        #endregion

        #region internal methods

        private static DateTime CalculateEscalationDatePrivate(string zoneName, DateTime startDate, int seconds)
        {
            WorkflowManagementServer mngServer = null;
            try
            {
                mngServer = new WorkflowManagementServer("localhost", 5555); //fairly safe to do this as inline function runs on K2 Server.
                mngServer.Open();
                //http://www.k2underground.com/forums/p/8275/31827.aspx
                mngServer.ZoneLoad(zoneName);
                return mngServer.ZoneCalculateEvent(zoneName, startDate, new TimeSpan(0, 0, 0, seconds));
            }
            catch (Exception ex)
            {
                //more detailed error in the k2server logs
                throw new Exception(string.Format("Error: {0}{1}", ex.Message, ex.StackTrace), ex);
            }
            finally
            {
                if (mngServer != null)
                {
                    if (mngServer.Connection != null)
                    {
                        mngServer.Connection.Dispose();
                    }
                    mngServer = null;
                }
            }
        }

        private static TimeSpan WorkingHoursDateDiff(string zoneName, DateTime startDate, DateTime endDate)
        {
            TimeSpan workedHours = new TimeSpan(0);

            WorkflowManagementServer mngServer = new WorkflowManagementServer("localhost", 5555); //safe to do this as inline function runs on K2 Server.
            mngServer.Open(); //let K2 catch errors here.

            AvailabilityZone zone = new AvailabilityZone();

            if (string.IsNullOrEmpty(zoneName)) //use default working hours if not specified.
            {
                try
                {
                    zoneName = mngServer.ZoneGetDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("Couldn't get default working hours because: " + ex.Message, ex);
                }
            }

            try
            {
                zone = mngServer.ZoneLoad(zoneName);
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't get working hours named: '" + zoneName + "' Because: " + ex.Message, ex);
            }

            DateTime currentZoneDate = startDate.Date;
            bool carryOn = true;
            while (carryOn)
            {

                List<AvailabilityHours> singleDayListOfHours = zone.AvailabilityHoursList.Where(x => x.WorkDay == currentZoneDate.DayOfWeek).ToList();

                foreach (AvailabilityHours hours in singleDayListOfHours)
                {
                    //quickly filter out exception dates :)
                    if (zone.AvailabilityDateList.Count(x => x.IsNonWorkDate == true && x.WorkDate.Date == currentZoneDate.Date) > 0)
                        continue;
                    DateTime endOfSection = new DateTime(currentZoneDate.Year, currentZoneDate.Month, currentZoneDate.Day);
                    DateTime startOfSection = new DateTime(currentZoneDate.Year, currentZoneDate.Month, currentZoneDate.Day);
                    startOfSection = startOfSection.Add(hours.TimeOfDay);
                    endOfSection = startOfSection.Add(hours.Duration);

                    if ((startDate >= startOfSection && startDate <= endOfSection) && (endDate <= endOfSection && endDate >= startOfSection)) //starts inside, ends inside
                    {
                        workedHours += endDate - startDate;
                    }

                    if (startDate < startOfSection && endDate > endOfSection) // starts outside, ends outside
                    {
                        workedHours += endOfSection - startOfSection;
                    }

                    if (startDate < startOfSection && (endDate <= endOfSection && endDate >= startOfSection)) //starts outside, ends inside
                    {
                        workedHours += endDate - startOfSection;
                    }

                    if ((startDate >= startOfSection && startDate <= endOfSection) && endDate > endOfSection) //starts inside, ends outside
                    {
                        workedHours += endOfSection - startDate;
                    }
                }

                //special dates require more work - basically duplicate the logic above but decrement worked hours for every special day
                foreach (AvailabilityDate specialDate in zone.AvailabilityDateList.Where(x => x.WorkDate.Date == currentZoneDate.Date && x.IsNonWorkDate == false))
                {

                    DateTime specialDateStart = specialDate.WorkDate;
                    DateTime specialDateEnd = specialDate.WorkDate + specialDate.Duration;

                    if ((startDate >= specialDateStart && startDate <= specialDateEnd) && (endDate <= specialDateEnd && endDate >= specialDateStart)) //starts inside, ends inside
                    {
                        workedHours -= endDate - startDate;
                    }

                    if (startDate < specialDateStart && endDate > specialDateEnd) // starts outside, ends outside
                    {
                        workedHours -= specialDateEnd - specialDateStart;
                    }

                    if (startDate < specialDateStart && (endDate <= specialDateEnd && endDate >= specialDateStart)) //starts outside, ends inside
                    {
                        workedHours -= endDate - specialDateStart;
                    }

                    if ((startDate >= specialDateStart && startDate <= specialDateEnd) && endDate > specialDateEnd) //starts inside, ends outside
                    {
                        workedHours -= specialDateEnd - startDate;
                    }
                }

                if (currentZoneDate > endDate)
                    carryOn = false;

                currentZoneDate = currentZoneDate.AddDays(1);

            }

            mngServer.Connection.Close();
            mngServer.Connection.Dispose();

            return workedHours;
        }

        #endregion
    }
}