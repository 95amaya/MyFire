using Services.Models;

namespace Services;

public interface IBillTransactionDao
{
    public IEnumerable<BillTransactionDto> GetList();
    public IEnumerable<BillTransactionDto> GetList(BillTransactionDto criteria);
    public long BulkInsert(IEnumerable<BillTransactionDto> list);
}
