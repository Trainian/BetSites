using NUnit.Framework;
using System;
using WPF.Converters;

namespace Test
{
    public class ParsTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ParsTimeToTimeSpan_InputString_ReturnEquel()
        {
            var str = "50:11";
            TimeSpan ts1 = new TimeSpan(0, 50, 11);
            TimeSpan ts2 = new TimeSpan();

            ts2 = ts2.ConvertToTimeSpan(str);

            Assert.AreEqual(ts1, ts2);
        }

        [Test]
        public void ParsTimeToTimeSpan_InputStringWithHours_ReturnEquel()
        {
            var str = "66:11";
            TimeSpan ts1 = new TimeSpan(1, 06, 11);
            TimeSpan ts2 = new TimeSpan();

            ts2 = ts2.ConvertToTimeSpan(str);

            Assert.AreEqual(ts1, ts2);
        }

        [Test]
        public void ParsTimeToTimeSpan_InputString_ReturnNotEquel()
        {
            var str = "50:12";
            TimeSpan ts1 = new TimeSpan(0, 50, 11);
            TimeSpan ts2 = new TimeSpan();

            ts2 = ts2.ConvertToTimeSpan(str);

            Assert.AreNotEqual(ts1, ts2);
        }
    }
}