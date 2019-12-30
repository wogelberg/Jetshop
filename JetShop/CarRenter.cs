using Common;
using DatabaseModels;
using System;
using System.Linq;

namespace Renting
{
    public interface ICarRenter
    {
        string CustomerRentsCar(string carCategory, string CustomerSocialSecurityNumber, decimal currentMilage);
        decimal? CustomerReturnsCar(string bookingNumber, decimal currentMilage);
    }

    public class CarRenter : ICarRenter
    {
        private IDateTimeProvider _dateTimeProvider;
        public CarRenter()
        {
            _dateTimeProvider = new RealDateTimeProvider();
        }
        public CarRenter(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public string CustomerRentsCar(string carCategory, string customerSocialSecurityNumber, decimal currentMilage)
        {
            if (!CarCategories.IsOfThisType(carCategory)) throw new ArgumentNullException("carCategory");
            if (!SocialSecurityNumber.IsValid(customerSocialSecurityNumber)) throw new ArgumentNullException("customerSocialSecurityNumber");

            return CreateAndSaveACarRental(carCategory, customerSocialSecurityNumber, currentMilage);
        }

        protected string CreateAndSaveACarRental(string carCategory, string customerSocialSecurityNumber, decimal currentMilage)
        {
            using (var db = new CarRentalContext())
            {
                var rental = new CarRental()
                {
                    CarCategory = carCategory,
                    CustomerSocialSecurityNumber = customerSocialSecurityNumber,
                    CarMilageAtRentInKm = currentMilage,
                    Rented = _dateTimeProvider.GetUtcNow()
                };
                db.Add(rental);
                db.SaveChanges();
                
                rental.BookingNumber = "BN" + rental.Id; //No real req. here, just it made up for now
                db.SaveChanges();

                return rental.BookingNumber;
            }
        }

        public decimal? CustomerReturnsCar(string bookingNumber, decimal currentMilage)
        {
            if (string.IsNullOrWhiteSpace(bookingNumber)) throw new ArgumentNullException("bookingNumber");
            if (currentMilage == default(decimal)) throw new ArgumentNullException("currentMilage");

            var finishedRental = ReturnRental(bookingNumber, currentMilage);
            if (finishedRental == null) return null;

            return finishedRental.CalculateRentalCost();
        }

        protected CarRental ReturnRental(string bookingNumber, decimal milageAtReturn)
        {
            using (var db = new CarRentalContext())
            {
                CarRental rental = FirstOrDefaultCarRental(bookingNumber, db);

                if (rental == null) throw new Exception("no such rental: " + bookingNumber);
                if (rental.CarMilageAtRentInKm > milageAtReturn) throw new Exception("Milage is lower at return than at rent time. Customer may be cheating");

                rental.CarMilageAtReturnInKm = milageAtReturn;

                rental.Returned = _dateTimeProvider.GetUtcNow();
                if (rental.Returned < rental.Rented) throw new Exception("Returned before it was rented");

                db.SaveChanges();

                return rental;
            }
        }

        protected CarRental FirstOrDefaultCarRental(string bookingNumber, CarRentalContext db)
        {
            //Isolating Query for testing, annoying to mock otherwise
            return db.Query<CarRental>().FirstOrDefault(x => x.BookingNumber == bookingNumber);
        }
    }
}
