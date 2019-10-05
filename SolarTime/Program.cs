using System;

namespace SolarTime
{
    class Program
    {
        static GlobalPosition Whāngārei = new GlobalPosition() 
        {
            Latitude = -35.73167,
            Longitude = 174.32833,
            Altitude = 46,
        };

        static void Main(string[] args)
        {
            var testTime = DateTime.Now.Date + new TimeSpan(12, 19, 0);

            Console.WriteLine(SolarPosition(Whāngārei, testTime));
        }

        static HorizontalPosition SolarPosition(GlobalPosition location, DateTimeOffset time)
        {
            double dtor = Math.PI / 180;

            int daysThisYear = DateTime.IsLeapYear(time.Year) ? 366 : 365;

            double fractionalYear = (2 * Math.PI / daysThisYear) * (time.DayOfYear - 1 + (time.Hour - 12) / 24);

            double equationOfTime = 229.18 * (0.000075
                                      + 0.001868 * Math.Cos(fractionalYear)
                                      - 0.032077 * Math.Sin(fractionalYear)
                                      - 0.014615 * Math.Cos(2 * fractionalYear)
                                      - 0.040849 * Math.Sin(2 * fractionalYear));
            
            double declination = 0.006918 
                           - 0.399912 * Math.Cos(fractionalYear)
                           + 0.070257 * Math.Sin(fractionalYear)
                           - 0.006758 * Math.Cos(2 * fractionalYear)
                           + 0.000907 * Math.Sin(2 * fractionalYear)
                           - 0.002697 * Math.Cos(3 * fractionalYear)
                           + 0.001480 * Math.Sin(3 * fractionalYear);

            double timeOffset = equationOfTime + 4 * location.Longitude - time.Offset.TotalMinutes;
            double trueSolarTime = time.TimeOfDay.TotalMinutes + timeOffset;
            double hourAngle = trueSolarTime / 4 - 180;
            double alt = Math.Asin( Math.Sin(location.Latitude * dtor) * Math.Sin(declination) + Math.Cos(location.Latitude * dtor) * Math.Cos(declination) * Math.Cos(hourAngle * dtor) ) / dtor;

            double azNom = -Math.Sin(hourAngle * dtor);
            double azDen = Math.Tan(declination) * Math.Cos(location.Latitude * dtor) - Math.Sin(location.Latitude * dtor) * Math.Cos(hourAngle * dtor);
            double az = Math.Atan(azNom / azDen);

            if (azDen < 0)
            {
                az += Math.PI;
            }
            else if (azNom < 0)
            {
                az += 2 * Math.PI;
            }

            return new HorizontalPosition() { Altitude = alt, Azimuth = az / dtor };
        
        }

            

    }

    struct HorizontalPosition
    {
        public double Altitude;
        public double Azimuth;

        public override string ToString()
        {
            return $"Alitude: {Altitude.ToString("#.##") } Azimuth: {Azimuth.ToString("#.##")}";
        }
    }

    class GlobalPosition {
        public double Latitude;
        public double Longitude;
        /// <summary>
        /// The altitude, in metres.
        /// </summary>
        public double Altitude;    
    }
}

