using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentValidation;
using Iob.Bank.Domain.Data;
using Iob.Bank.Domain.Data.Dtos;
using Iob.Bank.Domain.Interfaces.Infra;
using Iob.Bank.Domain.Interfaces.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Iob.Bank.Domain.Services.Auth;

public class AuthService(IDbModule dbModule, IOptions<AppSettings> appSettings, IValidator<UserAuthRequestDto> validator, IHashService hashService) : IAuthService
{
    private readonly AppSettings _appSettings = appSettings.Value;
    private readonly IDbModule _dbModule = dbModule;
    private readonly IValidator<UserAuthRequestDto> _validator = validator;
    private readonly IHashService _hashService = hashService;

    public async Task<UserAuthResponseDto> SignInAsync(UserAuthRequestDto authRequest)
    {
        var result = _validator.Validate(authRequest);
        if (!result.IsValid)
            throw new ValidationException(result.ToString());

        authRequest.Pass = _hashService.GetSha256(authRequest.Pass!);

        var user = await _dbModule.UserRepository.AuthenticateUser(authRequest);

        if (user == null)
            throw new ArgumentException(
                "Não foi possível realizar a autentição. Por favor, verifique os dados e tente novamente.");

        var token = GenerateToken(user);

        return new UserAuthResponseDto
        {
            Name = user.Name,
            Email = user.Email,
            Token = token
        };
    }

    private string GenerateToken(UserDto user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Jwt.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new ("UserId", user.Id.ToString()),
                new(ClaimTypes.Name, user.Name!),
                new(ClaimTypes.Role, user.UserType!.Type)
            }),
            Expires = DateTime.UtcNow.AddHours(_appSettings.Jwt.Expiration),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
