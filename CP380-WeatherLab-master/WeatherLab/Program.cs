﻿using System;
using System.Linq;


namespace WeatherLab
{
    class Program
    {
        static string dbfile = @".\data\climate.db";

        static void Main(string[] args)
        {
            var measurements = new WeatherSqliteContext(dbfile).Weather;

            var total_2020_precipitation = (from data in measurements
                                            where data.year == 2020
                                            select data.precipitation).Sum();
            Console.WriteLine($"Total precipitation in 2020: {total_2020_precipitation} mm\n");

            var mean_temp = from meanYear in measurements
                            group meanYear by meanYear.year into resGroup
                            orderby resGroup.Key
                            select new
                            {
                                Key = resGroup.Key,
                                hdd = resGroup.Where(row => row.meantemp < 18).Count(),
                                cdd = resGroup.Where(row => row.meantemp >= 18).Count(),
                            };
            //
            // Heating Degree days have a mean temp of < 18C
            //   see: https://en.wikipedia.org/wiki/Heating_degree_day
            //

            // ?? TODO ??

            //
            // Cooling degree days have a mean temp of >=18C
            //

            // ?? TODO ??

            //
            // Most Variable days are the days with the biggest temperature
            // range. That is, the largest difference between the maximum and
            // minimum temperature
            //
            // Oh: and number formatting to zero pad.
            // 
            // For example, if you want:
            //      var x = 2;
            // To display as "0002" then:
            //      $"{x:d4}"
            //
            Console.WriteLine("Year\tHDD\tCDD");

            // ?? TODO ??
            foreach (var meanYear in mean_temp)
            {
                Console.WriteLine($"{ meanYear.Key}\t{ meanYear.hdd}\t{ meanYear.cdd}");
            };
            var value = from temp in measurements
                           orderby (temp.maxtemp - temp.mintemp) descending
                           select new
                           {
                               date = $"{temp.year}-{temp.month:d2}-{temp.day:d2}",
                               delta = (temp.maxtemp - temp.mintemp),
                           };
            Console.WriteLine("\nTop 5 Most Variable Days");
            Console.WriteLine("YYYY-MM-DD\tDelta");

            // ?? TODO ??
            int j = 0;
            foreach (var i in value)
            {
                if (j < 5)
                {
                    Console.WriteLine($"{i.date}\t{i.delta}");
                    j++;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
