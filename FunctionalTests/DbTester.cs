using DatabaseModels;

namespace FunctionalTests
{
    public class DbTester
    {
        private static readonly string TestDbName = "JetshopTEST";
        private static readonly string TestConnectionString = @"Data Source=DESKTOP-GH53D24\SQLEXPRESS;Initial Catalog=" + TestDbName + ";Integrated Security=True";

        public DbTester()
        {
            CreateDbIfItDoesntExit();
        }

        private void CreateDbIfItDoesntExit()
        {
            CarRentalContext.ConnectionString = TestConnectionString;
            using (var db = new CarRentalContext())
            {
                db.Database.EnsureCreated();
            }
        }
    }
}