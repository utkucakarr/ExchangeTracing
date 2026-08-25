using ExchangeTracing.Modules.Transactions.Application;
using ExchangeTracing.Modules.Transactions.Application.CreateTransaction;
using ExchangeTracing.Modules.Transactions.Application.GetTransaction;
using ExchangeTracing.Modules.Transactions.Application.ListTransactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeTracing.Modules.Transactions.Presentation;

[ApiController]
[Route("transactions")]
public sealed class TransactionsController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<TransactionDto>> Create(
        CreateTransactionCommand command,
        CancellationToken cancellationToken)
    {
        var transaction = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TransactionDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var transaction = await sender.Send(new GetTransactionQuery(id), cancellationToken);
        return transaction is null ? NotFound() : Ok(transaction);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TransactionDto>>> List(
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var transactions = await sender.Send(new ListTransactionsQuery(userId), cancellationToken);
        return Ok(transactions);
    }
}
