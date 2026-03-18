using System.ComponentModel.DataAnnotations;

namespace Libri.API.DTOs.Request
{
    public class ModifyUnitOfMeasureRequest
    {
        [Required(ErrorMessage = "Tên đơn vị đo không được để trống")]
        [MaxLength(255, ErrorMessage = "Tên đơn vị đo không được quá 255 ký tự")]
        public string Name { get; set; }

        [MaxLength(255, ErrorMessage = "Mô tả không được quá 255 ký tự")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Tỉ lệ quy đổi không được để trống")]
        public decimal ConversionRate { get; set; }

        [Required(ErrorMessage = "Đơn vị quy đổi không được để trống")]
        public int MappingUnitId { get; set; }
    }
}
