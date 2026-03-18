using System.Runtime.Serialization;

namespace Libri.DAL.Enums
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Đang chờ xử lý")]
        Pending,

        [EnumMember(Value = "Đã nhận thanh toán")]
        PaymentReceived,

        [EnumMember(Value = "Thanh toán thất b")]
        PaymentFailed
    }
}
