using Services.Models;

namespace Services;

public interface IBillTransactionDaoReader
{
    public IEnumerable<BillTransactionDto> GetList();
}

public interface IBillTransactionDaoWriter
{
    public long BulkInsert(IEnumerable<BillTransactionDto> list);
}

public interface IBillTransactionDaoFilter
{
    public IEnumerable<BillTransactionDto> GetList(TransactionType transactionType);
}
