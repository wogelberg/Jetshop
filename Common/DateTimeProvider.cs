using System;

namespace Common
{
    public interface IDateTimeProvider
    {
        DateTime GetUtcNow();
    }

    public class RealDateTimeProvider : IDateTimeProvider
    {
        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }
    }

    public class FakeDateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow { get; set; }

        public FakeDateTimeProvider(){ UtcNow = DateTime.UtcNow; }
        public FakeDateTimeProvider(DateTime d){ UtcNow = d; }

        public DateTime GetUtcNow()
        {
            return UtcNow;
        }
    }
}
