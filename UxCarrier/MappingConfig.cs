using AutoMapper;
using UxCarrier.Helper;
using UxCarrier.Models;
using UxCarrier.Models.Dto;
using UxCarrier.Models.Entities;
using UxCarrier.Models.Request;
using UxCarrier.Models.Response;

namespace UxCarrier
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            CreateMap<EInvoiceBindCard, EInvoiceBindStep2Dto>();
            CreateMap<InvoiceDetails, InvoiceDetailDto>()
                .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductID))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => $"{src.InvoiceProductItem.No}.{src.InvoiceProduct.Brief}"))
                .ForMember(dest => dest.PieceUnit, opt => opt.MapFrom(src => src.InvoiceProductItem.PieceUnit))
                .ForMember(dest => dest.UnitCost, opt => opt.MapFrom(src => src.InvoiceProductItem.UnitCost.ToString("0.######")))
                .ForMember(dest => dest.CostAmount, opt => opt.MapFrom(src => src.InvoiceProductItem.CostAmount.ToString("0.######")))
                .ForMember(dest => dest.Remark, opt => opt.MapFrom(src => src.InvoiceProductItem.Remark));
            CreateMap<InvoiceItem, QueryInvoiceDto>()
               .ForMember(dest => dest.SellerName, opt => opt.MapFrom(src => src.InvoiceSeller.Name))
               .ForMember(dest => dest.SellerReceiptNo, opt => opt.MapFrom(src => src.InvoiceSeller.ReceiptNo))
               .ForMember(dest => dest.BuyerName, opt => opt.MapFrom(src => Utilities.StringMask(src.InvoiceBuyer.Name)))
               .ForMember(dest => dest.BuyerReceiptNo, opt => opt.MapFrom(src => src.InvoiceBuyer.ReceiptNo))
               .ForMember(dest => dest.InvoiceNo, opt => opt.MapFrom(src => $"{src.TrackID}{src.No}"))
               .ForMember(dest => dest.InvoiceDate, opt => opt.MapFrom(src => src.InvoiceDate))
               .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.InvoiceAmountType.CurrencyType.CurrencyName))
               .ForMember(dest => dest.SaleAmount, opt => opt.MapFrom(src => src.InvoiceAmountType.SalesAmount.ToString("0.######")))
               .ForMember(dest => dest.TaxAmount, opt => opt.MapFrom(src => src.InvoiceAmountType.TaxAmount.ToString("0.######")))
               .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.InvoiceAmountType.TotalAmount.ToString("0.######")))
               .ForMember(dest => dest.PrintMark, opt => opt.MapFrom(src => src.PrintMark))
               .ForMember(dest => dest.HasBonus, opt => opt.MapFrom(src => Utilities.IsNotNull(src.InvoiceWinningNumber.InvoiceID)))
               .ForMember(dest => dest.CarrierNo, opt => opt.MapFrom(src => Utilities.EmailMasking(src.InvoiceCarrier.CarrierNo!)))
               .ForMember(dest => dest.DonateMark, opt => opt.MapFrom(src => src.DonateMark))
               .ForMember(dest => dest.CustomerID, opt => opt.MapFrom(src => Utilities.StringMask(src.InvoiceBuyer.CustomerID ?? string.Empty)))
               .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => Utilities.EmailMasking(src.InvoiceBuyer.EMail??string.Empty)))
               .ForMember(dest => dest.CarrierType, opt => opt.MapFrom(src => src.InvoiceCarrier.CarrierType))
               .ForMember(dest => dest.CheckNo, opt => opt.MapFrom(src => src.CheckNo ?? string.Empty))
               .ForMember(dest => dest.RandomNo, opt => opt.MapFrom(src => src.RandomNo))
               .ForMember(dest => dest.InvoiceDetail, opt => opt.MapFrom(src => src.InvoiceDetails)); //要寫才會mapping, 光CreateMap<InvoiceDetails, InvoiceDetailDto>()不會自動map


            CreateMap<UxCardEmail, UserDTO>();

            CreateMap<UxBindCard, UxBindStep1PostDto>()
                .ForMember(x => x.card_no1, y => y.MapFrom(o => Utilities.EncodeBase64(o.card_no1!)))
                .ForMember(x => x.card_no2, y => y.MapFrom(o => Utilities.EncodeBase64(o.card_no2!)))
                .ForMember(x => x.card_type, y => y.MapFrom(o => Utilities.EncodeBase64(o.card_type!)))
                .ReverseMap();

            CreateMap<UxBindStep2Dto, UxBindDto>()
                .ForMember(x => x.card_no1, y => y.MapFrom(o => Utilities.DecodeBase64(o.card_no1!)))
                .ForMember(x => x.card_no2, y => y.MapFrom(o => Utilities.DecodeBase64(o.card_no2!)))
                .ForMember(x => x.card_type, y => y.MapFrom(o => Utilities.DecodeBase64(o.card_type!)));

            CreateMap<UxBindStep4Dto, UxBindDto>()
                .ForMember(x => x.card_no1, y => y.MapFrom(o => Utilities.DecodeBase64(o.card_no1!)))
                .ForMember(x => x.card_no2, y => y.MapFrom(o => Utilities.DecodeBase64(o.card_no2!)))
                .ForMember(x => x.card_type, y => y.MapFrom(o => Utilities.DecodeBase64(o.card_type!)));
        }  
    }
}
