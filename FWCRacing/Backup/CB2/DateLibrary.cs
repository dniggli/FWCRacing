using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using CodeBase2.URMC;

namespace CodeBase2
{
    public static class DateLibrary
    {
        /// <summary>
        /// Counts a specified number of work days after the given date, skipping weekends and holidays.
        /// </summary>
        /// <param name="daysToAdd"></param>
        /// <param name="originalDate"></param>
        /// <returns></returns>
        public static DateTime SkipWeekends(int daysToAdd, DateTime originalDate)
        {
            // The direction simply will indicate whether I should be adding days or removing days
            int direction = 1;

            if (daysToAdd > 0)
                direction = 1;
            else
                direction = -1;

            for (int i = 0; i < Math.Abs(daysToAdd); i++)
            {
                DateTime testDate = originalDate.AddDays(i * direction);

                if (testDate.DayOfWeek == DayOfWeek.Saturday || testDate.DayOfWeek == DayOfWeek.Sunday || IsDayOff(testDate))
                    daysToAdd += direction;
            }


            DateTime newDate = originalDate.AddDays(daysToAdd);
            if (IsDayOff(newDate)) newDate = newDate.AddDays(1);
            if (newDate.DayOfWeek == DayOfWeek.Saturday) newDate = newDate.AddDays(2);
            if (newDate.DayOfWeek == DayOfWeek.Sunday) newDate = newDate.AddDays(1);


            return newDate;

        }

        /// <summary>
        /// Returns the number of workDays it has been since a specified date
        /// </summary>
        /// <param name="daysToAdd"></param>
        /// <param name="originalDate"></param>
        /// <returns></returns>
        public static int WorkdaysSince(DateTime beginDate)
        {
            if (beginDate.Date == DateTime.Now.Date) return 0;
            int count = 1;

            while (beginDate.Date != DateTime.Now.Date)
            {
                beginDate = beginDate.AddDays(1);

                if (!(beginDate.DayOfWeek == DayOfWeek.Saturday || beginDate.DayOfWeek == DayOfWeek.Sunday || IsDayOff(beginDate)))
                    count++;
            }

            return count;

        }


        /// <summary>
        /// Gets the list of Actual Holidays which the URMC recognizes given a year.
        /// </summary>
        /// <param name="currentDate"></param>
        /// <returns></returns>
        public static List<DateTime> Holidays(int year)
        {
            //New Years
            //Christmas
            //Memorial Day
            //Labor day
            //Thanksgiving
            //The Day After Thanksgiving

            List<DateTime> holidays = new List<DateTime>();

            holidays.Add(new DateTime(year, 1, 1));  // New Years;
            holidays.Add(new DateTime(year, 12, 25));  // Christmas
            holidays.Add(new DateTime(year, 7, 4));  // Christmas

            DateTime memDay = new DateTime(year, 6, 1);

            //Find Last monday in May for memorial day, start at june and count backwards
            do
            {
                memDay = memDay.AddDays(-1);
            } while (memDay.DayOfWeek != DayOfWeek.Monday);

            holidays.Add(memDay);  // Memorial Day

            DateTime laborDay = new DateTime(year, 9, 1);
            //Find First monday in September for labor day, start at September first and count forwards
            while (laborDay.DayOfWeek != DayOfWeek.Monday)
            {
                laborDay = laborDay.AddDays(1);
            }
            holidays.Add(laborDay);//Labor Day


            DateTime thanksDay = new DateTime(year, 11, 1);
            //Find Fourth thursday in November for thanksgiving day, start at November first and count forwards
            int thursdayCount = 0;

            while (true)
            {
                if (thanksDay.DayOfWeek == DayOfWeek.Thursday)
                {
                    thursdayCount++;
                    if (thursdayCount == 4) break;
                }
                thanksDay = thanksDay.AddDays(1);
    
            }

            holidays.Add(thanksDay);  //Thanksgiving Day
            holidays.Add(thanksDay.AddDays(1)); //Day after Thanksgiving Day


            return holidays;
        }

    

        /// <summary>
        /// Gives the list of dates the URMC recognizes as a day off.
        /// </summary>
        /// <param name="currentDate"></param>
        /// <returns></returns>
        public static List<DateTime> DaysOff(int year)
        {
            List<DateTime> dOff = new List<DateTime>();

            foreach (DateTime holiday in Holidays(year))
            {
                if (holiday.DayOfWeek == DayOfWeek.Saturday)
                {
                    //if falls on saturday Day Off is friday, though if it is new years it wont fall on this year.
                    if (holiday != new DateTime(year, 1, 1)) dOff.Add(holiday.AddDays(-1));
                }
                else if (holiday.DayOfWeek == DayOfWeek.Sunday)
                {
                    //if falls on sunday Day Off is monday
                    dOff.Add(holiday.AddDays(1));
                }
                else
                {
                    dOff.Add(holiday);
                }
            }

            DateTime nextNewYear = new DateTime(year+1, 1, 1);  // next New Years;
            // If next new years falls on a saturday we get it off this year as 12/31.
            if (nextNewYear.DayOfWeek == DayOfWeek.Saturday) dOff.Add(nextNewYear.AddDays(-1));

            return dOff;
        }

        /// <summary>
        /// Determines if the given date is a Day the URMC recognizes as a Day Off
        /// </summary>
        /// <param name="currentDate"></param>
        /// <returns></returns>
        public static bool IsDayOff(DateTime currentDate)
        {
            return DaysOff(currentDate.Year).Contains(currentDate);              
        }

        /// <summary>
        /// Determines if the given date is a Day the URMC recognizes as a Holiday
        /// </summary>
        /// <param name="currentDate"></param>
        /// <returns></returns>
        public static bool IsHoliday(DateTime currentDate)
        {
            return Holidays(currentDate.Year).Contains(currentDate);
        }

    }

   
}

    
