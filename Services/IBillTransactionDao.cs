using Services.Models;

namespace Services;

public interface IBillTransactionDao
{
    public long BulkInsert(IEnumerable<BillTransactionDto> list);
    public IEnumerable<BillTransactionDto> Get(DateTime transactionDateInclusive, bool filterOutNoise = true);

}

