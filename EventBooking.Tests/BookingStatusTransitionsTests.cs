
using EventBooking.API.Models;
using Xunit;

namespace EventBooking.Tests;

public class BookingStatusTransitionsTests
{
    [Theory]
    [InlineData(BookingStatuses.Pending,
                BookingStatuses.Confirmed, true)]
    [InlineData(BookingStatuses.Pending,
                BookingStatuses.Rejected, true)]
    [InlineData(BookingStatuses.Pending,
                BookingStatuses.Pending, false)]
    [InlineData(BookingStatuses.Confirmed,
                BookingStatuses.Pending, false)]
    [InlineData(BookingStatuses.Confirmed,
                BookingStatuses.Rejected, false)]
    [InlineData(BookingStatuses.Rejected,
                BookingStatuses.Pending, false)]
    [InlineData(BookingStatuses.Rejected,
                BookingStatuses.Confirmed, false)]
    [InlineData(BookingStatuses.Rejected,
                BookingStatuses.Rejected, false)]
    public void CanTransition_ReturnsExpectedResult(
        string currentStatus,
        string nextStatus,
        bool expected)
    {
        bool result = BookingStatusTransitions.CanTransition(
            currentStatus, nextStatus);

        Assert.Equal(expected, result);
    }
}
