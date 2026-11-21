
namespace EventBooking.API.Models;

public static class BookingStatusTransitions
{
    public static bool CanTransition(
        string currentStatus,
        string nextStatus)
    {
        return currentStatus == BookingStatuses.Pending &&
               (nextStatus == BookingStatuses.Confirmed ||
                nextStatus == BookingStatuses.Rejected);
    }
}
