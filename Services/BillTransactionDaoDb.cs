using AutoMapper;
using Dapper;
using dpExt = Dapper.Contrib.Extensions;
using Services.CoreLibraries;
using Services.Models;


namespace Services;

public class BillTransactionDaoDb : IBillTransactionDao
{
    private readonly IDbConnectionManager _dbConnectionManager;
    private readonly IMapper _mapper;
    private readonly BillTransactionDboFilter _filterHelper;
    public BillTransactionDaoDb(IDbConnectionManager dbConnectionManager, IMapper mapper)
    {
        _dbConnectionManager = dbConnectionManager;
        _mapper = mapper;
        _filterHelper = new BillTransactionDboFilter();
    }

    public long BulkInsert(IEnumerable<BillTransactionDto> list)
    {
        var dboList = _mapper.Map<IEnumerable<BillTransactionDbo>>(list);
        using var conn = _dbConnectionManager.CreateConnection();
        return dpExt.SqlMapperExtensions.Insert(conn, dboList);
    }

    public IEnumerable<BillTransactionDto> Get(DateTime transactionDateInclusive, bool filterOutNoise = true)
    {
        var isNoise = !filterOutNoise;
        var whereClause = @$" where 
            {_filterHelper.GetTransactionDateFilterStr(nameof(transactionDateInclusive))} ";

        using var conn = _dbConnectionManager.CreateConnection();
        var billTransactionDbos = conn.GetList<BillTransactionDbo>(whereClause, new { transactionDateInclusive });

        return _mapper.Map<IEnumerable<BillTransactionDto>>(billTransactionDbos);
    }

}
