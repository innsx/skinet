using System.Runtime.Serialization;

namespace Core.Entities.OrderAggregate
{
    /// <summary>
    /// Enum with stages of the Order processing at
    /// with string to display
    /// </summary>
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,

         [EnumMember(Value = "Payment Received")]
        PaymentReceived,

         [EnumMember(Value = "Payment Failed")]
        PaymentFailed

    }
}