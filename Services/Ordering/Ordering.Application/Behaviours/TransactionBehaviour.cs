// using EventBus.Extensions;
// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;
// using Ordering.Application.IntegrationEvents;
// using Ordering.Infrastructure;
// using Serilog.Context;
//
// namespace Ordering.Application.Behaviours;
//
// public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
//     where TRequest : IRequest<TResponse>
// {
//     private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;
//     private readonly OrderingContext _dbContext;
//     private readonly IOrderingIntegrationEventService _orderingIntegrationEventService;
//
//
//     public TransactionBehaviour(OrderingContext dbContext,
//         IOrderingIntegrationEventService orderingIntegrationEventService,
//         ILogger<TransactionBehaviour<TRequest, TResponse>> logger)
//     {
//         _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
//         _orderingIntegrationEventService = orderingIntegrationEventService ??
//                                            throw new ArgumentNullException(nameof(orderingIntegrationEventService));
//         _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//     }
//
//     public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
//         CancellationToken cancellationToken)
//     {
//         var response = default(TResponse);
//
//         var typeName = request.GetGenericTypeName();
//
//         try
//         {
//             if (_dbContext.HasActiveTransaction)
//             {
//                 return await next();
//             }
//
//             var strategy = _dbContext.Database.CreateExecutionStrategy();
//
//             await strategy.ExecuteAsync(async () =>
//             {
//                 await using var transaction = await _dbContext.BeginTransactionAsync();
//                 using (LogContext.PushProperty("TransactionContext", transaction.TransactionId))
//                 {
//                     _logger.LogInformation("------ Begin transaction {TransactionId} for {CommandName} ({@Command})",
//                         transaction.TransactionId, typeName, request);
//
//                     response = await next();
//
//                     _logger.LogInformation("---Commit transaction {TransactionId} for {CommandName}",
//                         transaction.TransactionId, typeName);
//
//                     await _dbContext.CommitTransactionAsync(transaction);
//
//                     var transactionId = transaction.TransactionId;
//
//                     await _orderingIntegrationEventService.PublishEventsThroughEventBusAsync(transactionId);
//                 }
//             });
//             return response;
//         }
//         catch (Exception e)
//         {
//             _logger.LogError(e, "ERROR while Handling transaction for {CommandName} ({@Command})", typeName, request);
//             throw;
//         }
//     }
// }