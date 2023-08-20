using Services.Models;

namespace Services;

public interface IBillTransactionDaoLoader
{
    public IEnumerable<BillTransactionDto> Load();
}
