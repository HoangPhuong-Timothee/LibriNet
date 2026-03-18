using System.Runtime.Serialization;

namespace Libri.DAL.Enums
{
    public enum InventoryReceiptStatus
    {
        [EnumMember(Value = "Đang chờ xử lý")]
        Pending,

        [EnumMember(Value = "Hoàn thành")]
        Accept,

        [EnumMember(Value = "Hủy bỏ")]
        Cancel
    }
}
