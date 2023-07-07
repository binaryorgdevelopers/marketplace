using Identity.Models.Messages;

namespace Identity.Models.Dtos;

public record UserDto(Guid Id, string FirstName,string Email, string LastName, RoleDto Role, string[] Authorities, string Locale,
    IEnumerable<CardReadDto> Cards);