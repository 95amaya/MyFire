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

    public IEnumerable<BillTransactionDto> GetList(TransactionType transactionType)
    {
        var whereClause = $" where {_filterHelper.GetTransactionTypeFilterStr(nameof(transactionType))} ";

        using var conn = _dbConnectionManager.CreateConnection();
        var billTransactionDbos = conn.GetList<BillTransactionDbo>(whereClause, new { transactionType });

        return _mapper.Map<IEnumerable<BillTransactionDto>>(billTransactionDbos);
    }

}
