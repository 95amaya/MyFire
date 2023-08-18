using AutoMapper;
using Dapper;
using dpExt = Dapper.Contrib.Extensions;
using Services.CoreLibraries;
using Services.Models;

namespace Services;

public class BillTransactionDaoDb : IBillTransactionDao
{
    private IDbConnectionManager _dbConnectionManager { get; set; }
    private IMapper _mapper { get; set; }
    public BillTransactionDaoDb(IDbConnectionManager dbConnectionManager, IMapper mapper)
    {
        _dbConnectionManager = dbConnectionManager;
        _mapper = mapper;
    }

    public long BulkInsert(IEnumerable<BillTransactionDto> list)
    {
        var dboList = _mapper.Map<IEnumerable<BillTransactionDbo>>(list);
        using var conn = _dbConnectionManager.CreateConnection();
        return dpExt.SqlMapperExtensions.Insert(conn, dboList);
    }

    public IEnumerable<BillTransactionDto> GetList(BillTransactionDto criteria)
    {
        var criteriaDbo = _mapper.Map<BillTransactionDbo>(criteria);
        using var conn = _dbConnectionManager.CreateConnection();
        var billTransactionDbos = conn.GetList<BillTransactionDbo>(criteriaDbo);

        return _mapper.Map<IEnumerable<BillTransactionDto>>(billTransactionDbos);
    }

    public IEnumerable<BillTransactionDto> GetList()
    {
        throw new NotImplementedException();
    }
}
