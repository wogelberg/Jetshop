using Common;
using NUnit.Framework;
using System;

namespace UnitTests
{
    public class CarRentingPriceTests
    {
        [TestCase(CarCategories.Compact, 0, 0, 500)]
        [TestCase(CarCategories.Compact, 1, 0, 500)]
        [TestCase(CarCategories.Compact, 0, 5000, 500)]
        [TestCase(CarCategories.Compact, 2, 0, 1000)]
        [TestCase(CarCategories.Premium, 0, 0, 600)]
        [TestCase(CarCategories.Premium, 1, 0, 600)]
        [TestCase(CarCategories.Premium, 2, 0, 1200)]
        [TestCase(CarCategories.Premium, 1, 1, 630)]
        [TestCase(CarCategories.Premium, 1, 2, 660)]
        [TestCase(CarCategories.Minivan, 0, 0, 850)]
        [TestCase(CarCategories.Minivan, 1, 0, 850)]
        [TestCase(CarCategories.Minivan, 2, 0, 1700)]
        [TestCase(CarCategories.Minivan, 1, 1, 895)]
        [TestCase(CarCategories.Minivan, 1, 2, 940)]
        [Test]
        public void ShouldGiveExpectedPrice(string category, int days, decimal milage, decimal expectedPrice)
        {
            var carRental = new CarRental()
            {
                CarCategory = category,
                Rented = DateTime.UtcNow,
                Returned = DateTime.UtcNow.AddDays(days),
                CarMilageAtRentInKm = 1,
                CarMilageAtReturnInKm = 1 + milage
            };
            Assert.AreEqual(expectedPrice, carRental.CalculateRentalCost(), "Default prices no longer valid. Have they changed?");
        }

        [TestCase(null, "2019-01-01", "2019-01-01", 0, 1)]
        [TestCase(CarCategories.Premium, null, "2019-01-01", 0, 1)]
        [TestCase(CarCategories.Premium, "2019-01-01", null, 0, 1)]
        [TestCase(CarCategories.Premium, "2019-01-01", "2019-01-01", null, 1)]
        [TestCase(CarCategories.Premium, "2019-01-01", "2019-01-01", 0, null)]
        [TestCase(CarCategories.Premium, "2019-01-01", "2019-01-01", 0, 0)]
        [TestCase("NotARealCarCategory", "2019-01-01", "2019-01-01", 0, 1)]
        [Test]
        public void ShouldGiveNullWhenTryingToCalculatePriceFromBadState(string category, DateTime? rented, DateTime? returned, decimal? milageAtRent, decimal? milageAtReturn)
        {
            var carRental = new CarRental()
            {
                CarCategory = category,
                Rented = rented,
                Returned = returned,
                CarMilageAtRentInKm = milageAtRent,
                CarMilageAtReturnInKm = milageAtReturn
            };
            Assert.AreEqual(null, carRental.CalculateRentalCost());
        }
    }
}