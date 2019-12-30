using System;

namespace Common
{
    public class CarRental
    {
        public long Id { get; set; }
        public string BookingNumber { get; set; }
        public string CustomerSocialSecurityNumber { get; set; }
        public DateTime? Rented { get; set; }
        public DateTime? Returned { get; set; }
        public decimal? CarMilageAtRentInKm { get; set; }
        public decimal? CarMilageAtReturnInKm { get; set; }

        private string _carCategory;
        public string CarCategory
        {
            get => _carCategory;
            set { _carCategory = CarCategories.IsOfThisType(value) ? value : null; }
        }



        private readonly decimal _kilometerPrice = 30M;
        private readonly decimal _baseCostPerDayForRental = 500M;
        public decimal? CalculateRentalCost()
        {
            if (!Rented.HasValue || !Returned.HasValue) return null;

            if (CarCategory == CarCategories.Compact)
                return _baseCostPerDayForRental * GetDaysOfRent();

            if (MilageValuesAreJustWrong())
                return null;

            if (CarCategory == CarCategories.Premium)
                return _baseCostPerDayForRental * GetDaysOfRent() * 1.2M + (_kilometerPrice * GetCustomerMilageInKm());

            if (CarCategory == CarCategories.Minivan)
                return _baseCostPerDayForRental * GetDaysOfRent() * 1.7M + (_kilometerPrice * GetCustomerMilageInKm() * 1.5M);

            return null;
        }

        private bool MilageValuesAreJustWrong()
        {
            return !CarMilageAtReturnInKm.HasValue || !CarMilageAtRentInKm.HasValue || CarMilageAtReturnInKm == 0M || CarMilageAtRentInKm > CarMilageAtReturnInKm;
        }

        private int GetDaysOfRent()
        {
            var shouldNotGivePlus1DayDueToTickDiffWhenRoundingUp = NormalizeDateTimeToSecondsAccuracy(Returned.Value).Subtract(NormalizeDateTimeToSecondsAccuracy(Rented.Value)).TotalDays;
            var roundUp = (int)Math.Ceiling(shouldNotGivePlus1DayDueToTickDiffWhenRoundingUp);
            var shouldAlwaysGiveAtleast1DayOfRent = roundUp == 0 ? 1 : roundUp;
            
            return shouldAlwaysGiveAtleast1DayOfRent;
        }

        private DateTime NormalizeDateTimeToSecondsAccuracy(DateTime d)
        {
            return d.AddTicks(-(d.Ticks % TimeSpan.TicksPerSecond));
        }

        private decimal GetCustomerMilageInKm()
        {
            return CarMilageAtReturnInKm.Value - CarMilageAtRentInKm.Value;
        }
    }
}
