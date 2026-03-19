using HCG.FondoRevolvente.Application.Interfaces;

namespace HCG.FondoRevolvente.Infrastructure;

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.UtcNow;
}
