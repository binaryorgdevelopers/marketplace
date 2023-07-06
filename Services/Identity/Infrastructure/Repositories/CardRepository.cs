// using System.Linq.Expressions;
// using Identity.Domain.Entities;
// using Microsoft.EntityFrameworkCore;
//
// namespace Identity.Infrastructure.Repositories;
//
// public class CardRepository:ICardRepository
// {
//     private readonly DbContext _context;
//
//     public CardRepository(DbContext context)
//     {
//         _context = context;
//     }
//     public ValueTask<CardDetail> GetById(Expression<Func<CardDetail, bool>> expression)
//     {
//         
//     }
// }
//
// public interface ICardRepository
// {
//     public ValueTask<CardDetail> GetById(Expression<Func<CardDetail, bool>> expression);
//     public ValueTask<CardDetail> GetById(Expression<Func<CardDetail, bool>> expression);
// }