using Microsoft.AspNetCore.Mvc;
using Services.Models;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BillTransactionController : ControllerBase
{
    private readonly ILogger<BillTransactionController> _logger;

    public BillTransactionController(ILogger<BillTransactionController> logger)
    {
        _logger = logger;
    }


    [HttpGet]
    public IEnumerable<BillTransactionDto> Get()
    {
        return new List<BillTransactionDto>() {
            new BillTransactionDto()
            {
                TransactionDate = DateTime.Now,
                Amount = -50,
                Description = "TX ROADHOUSE",
                Type = TransactionType.CREDIT,
                Account = TransactionAccount.WANTS
            },
            new BillTransactionDto()
            {
                TransactionDate = DateTime.Now,
                Amount = 1200,
                Description = "Bullhorn",
                Type = TransactionType.DEBIT,
                Account = TransactionAccount.NEEDS
            }
        };
    }
}
