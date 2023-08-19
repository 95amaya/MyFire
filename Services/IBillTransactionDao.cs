using Services.Models;

namespace Services;

public interface IBillTransactionDao
{
    public IEnumerable<BillTransactionDto> GetList();
    public IEnumerable<BillTransactionDto> GetList(TransactionType transactionType);
    public long BulkInsert(IEnumerable<BillTransactionDto> list);
}
