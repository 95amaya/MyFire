using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Models;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BillTransactionController : ControllerBase
{
    private readonly IBillTransactionDao _billTransactionDao;
    private readonly ILogger<BillTransactionController> _logger;

    public BillTransactionController(ILogger<BillTransactionController> logger, IBillTransactionDao billTransactionDao)
    {
        _logger = logger;
        _billTransactionDao = billTransactionDao;
    }


    [HttpGet]
    public IEnumerable<BillTransactionDto> Get()
    {
        return new List<BillTransactionDto>() {
            new BillTransactionDto()
            {
                Id = 1,
                TransactionDate = DateTime.Now,
                Amount = -50,
                Description = "TX ROADHOUSE",
                Type = TransactionType.CREDIT,
                Account = TransactionAccount.WANTS
            },
            new BillTransactionDto()
            {
                Id = 2,
                TransactionDate = DateTime.Now,
                Amount = 1200,
                Description = "Bullhorn",
                Type = TransactionType.DEBIT,
                Account = TransactionAccount.NEEDS
            }
        };
    }
}
