using Services.Models;

namespace Services;

public interface IBillTransactionSheetDao
{
    public List<BillTransactionDto> GetTransactions(BillTransactionSheet sheet);
}
