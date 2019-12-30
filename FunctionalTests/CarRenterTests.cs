using Common;
using NUnit.Framework;
using Renting;
using System;

namespace FunctionalTests
{
    public class CarRenterTests : DbTester
    {
        [Test]
        public void ShouldBeAbleToRentACar()
        {
            var sut = new CarRenter();

            var result = sut.CustomerRentsCar(CarCategories.Compact, "198506141111", 0M);

            Assert.That(!string.IsNullOrWhiteSpace(result), "No booking number was returned");
        }

        [TestCase(CarCategories.Compact, "2019-01-01 12:00:00", "2019-01-02 12:00:00")]
        [TestCase(CarCategories.Premium, "2019-01-01 12:00:00", "2019-01-02 12:00:00")]
        [TestCase(CarCategories.Minivan, "2019-01-01 12:00:00", "2019-01-02 12:00:00")]
        [Test]
        public void ShouldBeAbleToReturnACar(string carCategory, DateTime timeOfRent, DateTime timeOfReturn)
        {
            var time = new FakeDateTimeProvider(timeOfRent);
            var sut = new CarRenter(time);

            var bookingNumber = sut.CustomerRentsCar(carCategory, "198506141111", 0M);

            time.UtcNow = timeOfReturn;
            var result = sut.CustomerReturnsCar(bookingNumber, 0.1M);

            Assert.That(result != null && result > 0);
        }

        [Test]
        public void ShouldFailOnReturningACarThatIsntRented()
        {
            Assert.Throws<Exception>(() => new CarRenter().CustomerReturnsCar("NotARealBookingNumber", 1M));
        }

        [Test]
        public void ShouldFailOnReturningACarWithLessMilageThenAtRentTime()
        {
            Assert.Throws<Exception>(() => 
            {
                var sut = new CarRenter();
                var bookingNumber = sut.CustomerRentsCar(CarCategories.Minivan, "198506141111", 1500M);
                sut.CustomerReturnsCar(bookingNumber, 15M);
            });
        }

        [Test]
        public void ShouldFailOnReturningACarBeforeItWasRented()
        {
            Assert.Throws<Exception>(() =>
            {
                var time = new FakeDateTimeProvider();
                var sut = new CarRenter(time);
                var bookingNumber = sut.CustomerRentsCar(CarCategories.Minivan, "198506141111", 1);
                time.UtcNow = new DateTime(1000, 1, 1);
                sut.CustomerReturnsCar(bookingNumber, 2);
            });
        }

        [TestCase(null, "198506141111", 150)]
        [TestCase("Minivan", null, 150)]
        [Test]
        public void ShouldDetectNullValues(string category, string ssn, decimal milage)
        {
            Assert.Throws<ArgumentNullException>(() => new CarRenter().CustomerRentsCar(category, ssn, milage));
        }

        [TestCase("Minivan", "198506141111", 150, null, 160)]
        [TestCase("Minivan", "198506141111", 150, "UseRealBookingNumber", 0)]
        [Test]
        public void ShouldDetectNullValues2(string category, string ssn, decimal milage, string bn, decimal newmilage)
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var sut = new CarRenter();
                var bookingNumber = sut.CustomerRentsCar(category, ssn, milage);
                sut.CustomerReturnsCar(bn == null ? null: bookingNumber, newmilage);
            });
        }
    }
}