using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using AutoMapper;
using Services.Models;

namespace DemoConsoleApp.Mappings;

public class ImportMapProfile : Profile
{
    public ImportMapProfile()
    {
        CreateMap<IList<string>, WfBillTransactionCsvo>()
            .ForMember(dest => dest.TransactionDate, act => act.MapFrom(src => src.ElementAtOrDefault(0)))
            .ForMember(dest => dest.Amount, act => act.MapFrom(src => src.ElementAtOrDefault(1)))
            .ForMember(dest => dest.Description, act => act.MapFrom(src => src.ElementAtOrDefault(4)));

        CreateMap<IList<string>, WfNeedsDebitBillTransactionCsvo>()
            .IncludeBase<IList<string>, WfBillTransactionCsvo>();
        CreateMap<IList<string>, WfWantsDebitBillTransactionCsvo>()
            .IncludeBase<IList<string>, WfBillTransactionCsvo>();
        CreateMap<IList<string>, WfNeedsCreditBillTransactionCsvo>()
            .IncludeBase<IList<string>, WfBillTransactionCsvo>();

        CreateMap<IList<string>, JpmBillTransactionCsvo>()
            .ForMember(dest => dest.TransactionDate, act => act.MapFrom(src => src.ElementAtOrDefault(0)))
            .ForMember(dest => dest.Amount, act => act.MapFrom(src => src.ElementAtOrDefault(5)))
            .ForMember(dest => dest.Description, act => act.MapFrom(src => src.ElementAtOrDefault(2)));

        CreateMap<IList<string>, JpmWantsCreditBillTransactionCsvo>()
            .IncludeBase<IList<string>, JpmBillTransactionCsvo>();
    }
}

public class ExportMapProfile : Profile
{
    public ExportMapProfile()
    {
        CreateMap<IList<string>, BillTransactionExportCsvo>()
            .ForMember(dest => dest.TransactionDate, act => act.MapFrom(src => src.ElementAtOrDefault(0)))
            .ForMember(dest => dest.Amount, act => act.MapFrom(src => src.ElementAtOrDefault(1)))
            .ForMember(dest => dest.Type, act => act.MapFrom(src => src.ElementAtOrDefault(2)))
            .ForMember(dest => dest.Account, act => act.MapFrom(src => src.ElementAtOrDefault(3)))
            .ForMember(dest => dest.Description, act => act.MapFrom(src => src.ElementAtOrDefault(4)))
            .ForMember(dest => dest.Label, act => act.MapFrom(src => src.ElementAtOrDefault(5)));
    }
}

public class TransformMapProfile : Profile
{
    public TransformMapProfile()
    {
        CreateMap<BillTransactionDto, BillTransactionCsvo>()
            .ForMember(dest => dest.TransactionDate, act => act.Ignore()) // Need to define in decsendents
            .ForMember(dest => dest.Amount, act => act.MapFrom(src => src.Amount))
            .ForMember(dest => dest.Description, act => act.MapFrom(src => src.Description))
            .ForMember(dest => dest.Type, act => act.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.Account, act => act.MapFrom(src => src.Account.ToString()))
            .ForMember(dest => dest.Label, act => act.MapFrom(src => src.Label))
            .ReverseMap()
            .ForMember(dest => dest.Type, act => act.MapFrom(src => Enum.Parse<TransactionType>(src.Type)))
            .ForMember(dest => dest.Account, act => act.MapFrom(src => Enum.Parse<TransactionAccount>(src.Account)));

        CreateMap<BillTransactionDto, BillTransactionExportCsvo>()
            .IncludeBase<BillTransactionDto, BillTransactionCsvo>()
            .ForMember(dest => dest.TransactionDate, act => act.MapFrom(src => src.TransactionDate.Value.ToString("yyyy-MM-dd")))
            .ReverseMap()
            .ForMember(dest => dest.TransactionDate, act => act.MapFrom(src => DateTime.ParseExact(src.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)));


        CreateMap<BillTransactionDto, BillTransactionImportCsvo>()
            .IncludeBase<BillTransactionDto, BillTransactionCsvo>()
            .ForMember(dest => dest.TransactionDate, act => act.MapFrom(src => src.TransactionDate.Value.ToString("MM/dd/yyyy")))
            .ReverseMap()
            .ForMember(dest => dest.TransactionDate, act => act.MapFrom(src => DateTime.ParseExact(src.TransactionDate, "MM/dd/yyyy", CultureInfo.InvariantCulture)));

    }
}

// Exmple of Value Converter
// public class BooleanConverter : IValueConverter<string, bool>
// {
//     public bool Convert(string sourceMember, ResolutionContext context)
//     {
//         if(string.IsNullOrEmpty(sourceMember))
//         {
//             return false;
//         }

//         return bool.Parse(sourceMember);
//     }

// }