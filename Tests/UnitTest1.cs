using System;
using Xunit;
using SolarTime;

namespace Tests
{
    public class UnitTest1
    {
        GlobalPosition Whāngārei = new GlobalPosition()
        {
            Latitude = -35.73167,
            Longitude = 174.32833,
            Altitude = 46,
        };

        // expected values are taken from http://www.suncalc.org and tested to within one degree

        [Fact]
        public void TestAroundMidday()
        {
            var testTime = new DateTime(2020, 10, 31, 0, 0, 0, DateTimeKind.Utc); // 13:00 local time NZ

            var position = Program.SolarPosition(Whāngārei, testTime);

            Assert.Equal(4.16, position.Azimuth, 0);
            Assert.Equal(68.41, position.Altitude, 0);
        }

        [Fact]
        public void TestAroundSunrise()
        {
            var testTime = new DateTime(2020, 10, 30, 17, 0, 0, DateTimeKind.Utc); // 06:00 local time NZ

            var position = Program.SolarPosition(Whāngārei, testTime);

            Assert.Equal(111.13, position.Azimuth, 0);
            Assert.Equal(-4.70, position.Altitude, 0);
        }

        [Fact]
        public void TestAroundSunset()
        {
            var testTime = new DateTime(2020, 10, 31, 6, 0, 0, DateTimeKind.Utc); // 19:00 local time NZ

            var position = Program.SolarPosition(Whāngārei, testTime);

            Assert.InRange(position.Azimuth, 259, 260);
            Assert.InRange(position.Altitude, 9, 10);
        }


    }
}
