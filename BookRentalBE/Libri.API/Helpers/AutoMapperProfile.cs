using Libri.API.DTOs.Request;
using Libri.DAL.Models.Custom.CustomAuthor;
using Libri.DAL.Models.Custom.CustomBasket;
using Libri.DAL.Models.Custom.CustomBook;
using Libri.DAL.Models.Custom.CustomGenre;
using Libri.DAL.Models.Custom.CustomInventory;
using Libri.DAL.Models.Custom.CustomOrder;
using Libri.DAL.Models.Custom.CustomPublisher;
using Libri.DAL.Models.Custom.CustomUser;
using Libri.DAL.Models.Xml;
using Libri.DAL.Models.Domain;
using Libri.DAL.Models.Custom.CustomBookStore;
using Libri.DAL.Models.Custom.CustomUnitOfMeasure;
using Libri.DAL.Models.Custom.CustomDeliveryMethod;
using Libri.API.DTOs.Response.Inventory;
using Libri.API.DTOs.Response.User;
using Libri.API.DTOs.Response.Basket;
using Libri.API.DTOs.Response.Book;
using Libri.API.DTOs.Response.Order;

namespace Libri.API.Helpers
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            //Address
            CreateMap<ModifyAddressRequest, Address>();

            CreateMap<ShippingAddress, AddressDTO>();

            CreateMap<Address, AddressDTO>().ReverseMap();

            //Auth & User
            CreateMap<RegisterRequest, Register>();

            CreateMap<MemberWithTotalCount, MemeberDTO>()
                .ForMember(d => d.DateOfBirth, opt => opt.MapFrom(s => s.DateOfBirth.ToString("dd/MM/yyyy")))
                .ForMember(d => d.Gender, opt => opt.MapFrom(s => s.Gender == "male" ? "Nam" : "Nữ"))
                .ForMember(d => d.Address, opt => opt.MapFrom(s =>
                    (s.Street ?? "") +
                    (string.IsNullOrEmpty(s.Street) ? "" : ", ") +
                    (s.Ward ?? "") +
                    (string.IsNullOrEmpty(s.Ward) ? "" : ", ") +
                    (s.District ?? "") +
                    (string.IsNullOrEmpty(s.District) ? "" : ", ") +
                    (s.City ?? "")))
                .ReverseMap()
                .ForMember(s => s.TotalCount, opt => opt.Ignore());

            CreateMap<CurrentUser, UserDTO>()
                .ForMember(d => d.DateOfBirth, opt => opt.MapFrom(s => s.DateOfBirth.ToString("dd/MM/yyyy")))
                .ForMember(d => d.Gender, opt => opt.MapFrom(s => s.Gender == "male" ? "Nam" : "Nữ"));

            CreateMap<ModifyProfileRequest, Profile>();
            
            //Author
            CreateMap<DAL.Models.Domain.Author, AuthorDTO>();

            CreateMap<ModifyAuthorRequest, DAL.Models.Domain.Author>();

            CreateMap<AuthorWithTotalCount, AuthorDTO>()
                .ForMember(d => d.CreateInfo, opt => opt.MapFrom(s => s.CreatedBy + ", " + s.CreatedAt.ToString("dd/MM/yyyy")))
                .ForMember(d => d.UpdateInfo, opt => opt.MapFrom(s => s.UpdatedBy + ", " + s.UpdatedAt.ToString("dd/MM/yyyy")))
                .ReverseMap()
                .ForMember(s => s.TotalCount, opt => opt.Ignore());

            //Basket
            CreateMap<BasketDTO, Basket>().ReverseMap();

            CreateMap<BasketItem, BasketItemDTO>().ReverseMap();

            //Book
            CreateMap<BookWithDetails, BookDTO>()
                .ForMember(d => d.CreateInfo, opt => opt.MapFrom(s => s.CreatedBy + ", " + s.CreatedAt.ToString("dd/MM/yyyy")))
                .ForMember(d => d.UpdateInfo, opt => opt.MapFrom(s => s.UpdatedBy + ", " + s.UpdatedAt.ToString("dd/MM/yyyy")));

            CreateMap<BookWithDetailsAndTotalCount, BookDTO>()
                .ReverseMap()
                .ForMember(s => s.TotalCount, opt => opt.Ignore());

            CreateMap<ModifyBookRequest, DAL.Models.Domain.Book>();

            //BookStore
            CreateMap<ModifyBookStoreRequest, DAL.Models.Domain.BookStore>();
       
            CreateMap<DAL.Models.Domain.BookStore, BookStoreDTO>()
                .ForMember(d => d.CreateInfo, opt => opt.MapFrom(s => s.CreatedBy + ", " + s.CreatedAt.ToString("dd/MM/yyyy")))
                .ForMember(d => d.UpdateInfo, opt => opt.MapFrom(s => s.UpdatedBy + ", " + s.UpdatedAt.ToString("dd/MM/yyyy")));

            CreateMap<BookStoreWithTotalCount, BookStoreDTO>()
                .ForMember(d => d.CreateInfo, opt => opt.MapFrom(s => s.CreatedBy + ", " + s.CreatedAt.ToString("dd/MM/yyyy")))
                .ForMember(d => d.UpdateInfo, opt => opt.MapFrom(s => s.UpdatedBy + ", " + s.UpdatedAt.ToString("dd/MM/yyyy")))
                .ForMember(d => d.TotalQuantity, opt => opt.MapFrom(s => s.TotalQuantity))
                .ReverseMap()
                .ForMember(s => s.TotalCount, opt => opt.Ignore());

            //Delivery method
            CreateMap<DAL.Models.Domain.DeliveryMethod, DeliveryMethodDTO>();

            CreateMap<DeliveryMethodWithTotalCount, DeliveryMethodDTO>()
                .ForMember(d => d.CreateInfo, opt => opt.MapFrom(s => s.CreatedBy + ", " + s.CreatedAt.ToString("dd/MM/yyyy")))
                .ForMember(d => d.UpdateInfo, opt => opt.MapFrom(s => s.UpdatedBy + ", " + s.UpdatedAt.ToString("dd/MM/yyyy")))
                .ReverseMap()
                .ForMember(d => d.TotalCount, opt => opt.Ignore());

            //Genre
            CreateMap<DAL.Models.Domain.Genre, GenreDTO>();

            CreateMap<ModifyGenreRequest, DAL.Models.Domain.Genre>();

            CreateMap<GenreWithTotalCount, GenreDTO>()
                .ForMember(d => d.CreateInfo, opt => opt.MapFrom(s => s.CreatedBy + ", " + s.CreatedAt.ToString("dd/MM/yyyy")))
                .ForMember(d => d.UpdateInfo, opt => opt.MapFrom(s => s.UpdatedBy + ", " + s.UpdatedAt.ToString("dd/MM/yyyy")))
                .ReverseMap()
                .ForMember(s => s.TotalCount, opt => opt.Ignore());

            //Inventory
            CreateMap<BookInventoryWithTotalCount, InventoryDTO>()
                 .ForMember(d => d.UpdateInfo, opt => opt.MapFrom(s =>
                    (s.UpdatedBy != null && s.UpdatedAt != null) ? 
                    s.UpdatedBy + ", " + s.UpdatedAt.ToString("dd-MM-yyyy")
                    : string.Empty   
                 ))
                 .ForMember(d => d.CreateInfo, opt => opt.MapFrom(s =>
                    (s.CreatedBy != null && s.CreatedAt != null) ?
                    s.CreatedBy + ", " + s.CreatedAt.ToString("dd-MM-yyyy")
                    : string.Empty
                 ))
                 .ForMember(d => d.InventoryStatus, opt => opt.MapFrom(s => 
                    s.Quantity >= 1 && s.Quantity <= 20 ? "Sắp hết hàng" :
                    s.Quantity <= 0 ? "Hết hàng" :
                    "Bình thường"
                 ))
                 .ReverseMap()
                 .ForMember(d => d.TotalCount, opt => opt.Ignore());

            //Inventory audit
            CreateMap<InventoryAuditWithTotalCount, InventoryAuditDTO>()
                .ForMember(d => d.AuditStatus, opt => opt.MapFrom(s => 
                    s.AuditStatus == "done" ? "Đã thực hiện" :
                    s.AuditStatus == "undone" ? "Chưa thực hiện" :
                    "Đang thực hiện"
                ))
                .ForMember(d => d.AuditDate, opt => opt.MapFrom(s => s.AuditDate.ToString("dd-MM-yyyy")))
                .ForMember(d => d.CreatedAt, opt => opt.MapFrom(s => s.CreatedAt.ToString("dd-MM-yyyy")))
                .ReverseMap()
                .ForMember(d => d.TotalCount, opt => opt.Ignore());

            CreateMap<AuditDetails, InventoryAuditDetailsDTO>()
                .ForMember(d => d.UpdateInfo, opt => opt.MapFrom(s => s.UpdatedBy + ", " + s.UpdatedAt.ToString("dd-MM-yyyy")));

            CreateMap<InventoryAuditItemDTO, InventoryAuditItem>();
            CreateMap<AddInventoryAuditRequest, DAL.Models.Xml.InventoryAudit>();

            CreateMap<ConductInventoryRequest, ConductInventory>();

            //Inventory receipt
            CreateMap<InventoryReceiptWithTotalCount, InventoryReceiptDTO>()
                .ForMember(d => d.ReceiptStatus, opt => opt.MapFrom(s =>
                    s.ReceiptStatus == "Pending" ? "Đang chờ xử lý" :
                    s.ReceiptStatus == "CompleteReceipt" ? "Hoàn thành" :
                    "Hủy bỏ"
                ))
                .ForMember(d => d.ReceiptType, opt => opt.MapFrom(s => s.ReceiptType == "Export" ? "Xuất kho" : "Nhập kho"))
                .ReverseMap()
                .ForMember(d => d.TotalCount, opt => opt.Ignore());
            
            CreateMap<AddImportInventoryReceiptRequest, ImportReceipt>();
            CreateMap<ImportReceiptItemDTO, DAL.Models.Xml.ImportReceiptItem>();

            CreateMap<AddExportInventoryReceiptRequest, ExportReceipt>();
            CreateMap<ExportReceiptItemDTO, DAL.Models.Xml.ExportReceiptItem>();

            //Inventory transaction
            CreateMap<BookInventoryTransaction, InventoryTransactionDTO>()
                .ForMember(d => d.TransactionDate, opt => opt.MapFrom(s => s.TransactionDate.ToString("dd-MM-yyyy")))
                .ForMember(d => d.TransactionType, opt => opt.MapFrom(s => s.TransactionType == "Export" ? "Xuất kho" : "Nhập kho"));

            //Order
            CreateMap<CreateOrderRequest, DAL.Models.Domain.Order>()
                .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.ShippingAddress.FullName))
                .ForMember(d => d.Street, opt => opt.MapFrom(s => s.ShippingAddress.Street))
                .ForMember(d => d.Ward, opt => opt.MapFrom(s => s.ShippingAddress.Ward))
                .ForMember(d => d.District, opt => opt.MapFrom(s => s.ShippingAddress.District))
                .ForMember(d => d.City, opt => opt.MapFrom(s => s.ShippingAddress.City))
                .ForMember(d => d.PostalCode, opt => opt.MapFrom(s => s.ShippingAddress.PostalCode));

            CreateMap<ShippingAddress, AddressDTO>();

            CreateMap<OrderWithDetails, OrderWithDetailsDTO>()
                .ForMember(d => d.OrderDate, opt => opt.MapFrom(s => s.OrderDate.ToString("dd/MM/yyyy")));

            CreateMap<DAL.Models.Custom.CustomOrder.OrderItem, OrderItemDTO>();

            CreateMap<OrderWithTotalCount, OrderDTO>()
                .ForMember(d => d.OrderDate, opt => opt.MapFrom(s => s.OrderDate.ToString("dd/MM/yyyy")))
                .ForMember(d => d.OrderTotal, opt => opt.MapFrom(s => s.Subtotal + s.DeliveryPrice))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => 
                    s.Status == "Pending" ? "Đang chờ xử lý" :
                    s.Status == "PaymentReceived" ? "Đã giao hàng" :
                    "Đơn hàng bị từ chối"
                ))
                .ReverseMap()
                .ForMember(d => d.TotalCount, opt => opt.Ignore());

            //Publisher
            CreateMap<DAL.Models.Domain.Publisher, PublisherDTO>();

            CreateMap<ModifyPublisherRequest, DAL.Models.Domain.Publisher>();

            CreateMap<PublisherWithTotalCount, PublisherDTO>()
                .ForMember(d => d.CreateInfo, opt => opt.MapFrom(s => s.CreatedBy + ", " + s.CreatedAt.ToString("dd/MM/yyyy")))
                .ForMember(d => d.UpdateInfo, opt => opt.MapFrom(s => s.UpdatedBy + ", " + s.UpdatedAt.ToString("dd/MM/yyyy")))
                .ReverseMap()
                .ForMember(s => s.TotalCount, opt => opt.Ignore());

            //Specific
            CreateMap<ConvertedInputAndRemainingQuantity, ConvertedInputAndRemainingQuantityDTO>();

            //Unit of measure
            CreateMap<UnitOfMeasureWithTotalCount, UnitOfMeasureDTO>()
                .ForMember(d => d.CreateInfo, opt => opt.MapFrom(s => s.CreatedBy + ", " + s.CreatedAt.ToString("dd/MM/yyyy")))
                .ForMember(d => d.UpdateInfo, opt => opt.MapFrom(s => s.UpdatedBy + ", " + s.UpdatedAt.ToString("dd/MM/yyyy")))
                .ReverseMap()
                .ForMember(s => s.TotalCount, opt => opt.Ignore());

            CreateMap<ModifyUnitOfMeasureRequest, MeasureUnit>()
                .ForMember(d => d.DestUnitId, opt => opt.MapFrom(s => s.MappingUnitId))
                .ForMember(d => d.SrcUnitName, opt => opt.MapFrom(s => s.Name));

            CreateMap<UnitOfMeasure, UnitOfMeasureDTO>();
        }
    }
}
