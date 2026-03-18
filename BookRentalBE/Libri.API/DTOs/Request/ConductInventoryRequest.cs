using System.ComponentModel.DataAnnotations;

namespace Libri.API.DTOs.Request
{
    public class ConductInventoryRequest
    {
        [Required]
        public string BookTitle { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public string StoreName { get; set; }

        [Required]
        public int ActualQuantity { get; set; }

        [Required]
        public int UnitOfMeasureId { get; set; }
    }
}
