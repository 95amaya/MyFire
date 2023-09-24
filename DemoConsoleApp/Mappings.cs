using System;
using System.Collections.Generic;
using System.Globalization;
using AutoMapper;
using Services.Models;

namespace DemoConsoleApp.Mappings;

public class ImportMapProfile : Profile
{
    public ImportMapProfile()
    {
        // TODO: IEnumerable.ElementAtOrDefault
        CreateMap<IList<object>, WfBillTransactionDto>()
            .ForMember(dest => dest.TransactionDate, act => act.MapFrom(src => DateTime.ParseExact(src[0] as string, "MM/dd/yyyy", CultureInfo.InvariantCulture)))
            .ForMember(dest => dest.Amount, act => act.MapFrom(src => src[1]))
            .ForMember(dest => dest.Description, act => act.MapFrom(src => src[4]));


        CreateMap<IList<object>, WfNeedsDebitBillTransactionDto>()
            .IncludeBase<IList<object>, WfBillTransactionDto>();
        CreateMap<IList<object>, WfWantsDebitBillTransactionDto>()
            .IncludeBase<IList<object>, WfBillTransactionDto>();
        CreateMap<IList<object>, WfNeedsCreditBillTransactionDto>()
            .IncludeBase<IList<object>, WfBillTransactionDto>();

        CreateMap<IList<object>, JpmBillTransactionDto>()
            .ForMember(dest => dest.TransactionDate, act => act.MapFrom(src => DateTime.ParseExact(src[0] as string, "MM/dd/yyyy", CultureInfo.InvariantCulture)))
            .ForMember(dest => dest.Amount, act => act.MapFrom(src => src[5]))
            .ForMember(dest => dest.Description, act => act.MapFrom(src => src[2]));

        CreateMap<IList<object>, JpmWantsCreditBillTransactionDto>()
            .IncludeBase<IList<object>, JpmBillTransactionDto>();

        CreateMap<IList<object>, BillTransactionCsv>()
            .ForMember(dest => dest.transaction_date, act => act.MapFrom(src => DateTime.ParseExact(src[0] as string, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
            .ForMember(dest => dest.amount, act => act.MapFrom(src => src[1]))
            .ForMember(dest => dest.transaction_type, act => act.MapFrom(src => src[2]))
            .ForMember(dest => dest.transaction_account, act => act.MapFrom(src => src[3]))
            .ForMember(dest => dest.description, act => act.MapFrom(src => src[4]))
            .ForMember(dest => dest.is_noise, act => act.MapFrom(src => src[5]));
    }
}

public class TransformMapProfile : Profile
{
    public TransformMapProfile()
    {
        CreateMap<BillTransactionDto, BillTransactionCsv>()
            .ForMember(dest => dest.transaction_date, act => act.MapFrom(src => src.TransactionDate))
            .ForMember(dest => dest.amount, act => act.MapFrom(src => src.Amount))
            .ForMember(dest => dest.description, act => act.MapFrom(src => src.Description))
            .ForMember(dest => dest.transaction_type, act => act.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.transaction_account, act => act.MapFrom(src => src.Account.ToString()))
            .ForMember(dest => dest.is_noise, act => act.MapFrom(src => src.IsNoise))
            .ReverseMap()
            .ForPath(dest => dest.Type, act => act.MapFrom(src => Enum.Parse<TransactionType>(src.transaction_type)))
            .ForPath(dest => dest.Account, act => act.MapFrom(src => Enum.Parse<TransactionAccount>(src.transaction_account)));
    }
}