// using Inventory.Domain.Abstractions.Repositories;
// using Inventory.Domain.Entities;
// using Microsoft.AspNetCore.Identity;
// using Shared.Abstraction.Messaging;
// using Shared.Messages;
// using Shared.Models;
// using Shared.Models.Constants;
//
// namespace Marketplace.Application.Queries.Query.Auth;
//
// public class UserSignInQueryHandler : ICommandHandler<UserSignInCommand,AuthResult>
// {
//     private readonly IGenericRepository<User> _genericRepository;
//     private readonly IJwtTokenGenerator _tokenGenerator;
//     private readonly IPasswordHasher<User> _passwordHasher;
//
//     public UserSignInQueryHandler(IJwtTokenGenerator tokenGenerator,
//         IPasswordHasher<User> passwordHasher, IGenericRepository<User> genericRepository)
//     {
//         _tokenGenerator = tokenGenerator;
//         _passwordHasher = passwordHasher;
//         _genericRepository = genericRepository;
//     }
//
//     public async Task<Result<AuthResult>> Handle(UserSignInCommand request, CancellationToken cancellationToken)
//     {
//         var user = await _genericRepository.GetAsync(e => e.Email == request.Email);
//         if (user is null)
//         {
//             return Result.Failure<AuthResult>(new Error(Codes.UserNotFound,
//                 $"User with email :'{request.Email}' not found"));
//         }
//
//         var validationResult = user.ValidatePassword(request.Password, _passwordHasher);
//         if (!validationResult)
//         {
//             return Result.Failure<AuthResult>(new Error(Codes.InvalidCredential,
//                 "Invalid password"));
//         }
//
//         Authorized authorized =
//             new Authorized(user.Id, user.FirstName, user.LastName, user.PhoneNumber, user.Email, user.Role.ToString());
//
//         return Result.Success(new AuthResult(authorized,
//             _tokenGenerator.GenerateToken(user.ToTokenRequest())));
//     }
// }