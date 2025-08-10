using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Application.Models;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Core.Models;
using PetFamily.Framework;
using PetFamily.SharedKernel;


namespace PetFamily.Accounts.Infrastructure;

public class JwtTokenProvider : ITokenProvider
{
    private readonly AccountDbContext _accountDbContext;
    private readonly RefreshSessionOptions _refreshSessionOptions;
    private readonly JwtOptions _jwtOptions;

    public JwtTokenProvider(
        IOptions<JwtOptions> options,
        IOptions<RefreshSessionOptions> refreshSessionOptions,
        AccountDbContext accountDbContext)
    {
        _accountDbContext = accountDbContext;
        _refreshSessionOptions = refreshSessionOptions.Value;
        _jwtOptions = options.Value;
    }

    public JwtTokenResult GenerateAccessToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));

        var signingCredentials = new SigningCredentials(
            securityKey, SecurityAlgorithms.HmacSha256);

        var roleClaims = user.Roles
            .Select(u => new Claim(CustomClaims.Role, u.Name ?? string.Empty));

        var jti = Guid.NewGuid();

        Claim[] claims =
        [
            new(CustomClaims.Id, user.Id.ToString()),
            new(CustomClaims.Jti, jti.ToString()),
            new(CustomClaims.Email, user.Email ?? ""),
        ];

        claims = claims.Concat(roleClaims).ToArray();

        var jwtToken = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_jwtOptions.ExpiredMinutesTime)),
            signingCredentials: signingCredentials,
            claims: claims);

        var jwtStringToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return new JwtTokenResult(jwtStringToken, jti);
    }

    public async Task<Guid> GenerateRefreshToken(User user, Guid jti, CancellationToken cancellationToken)
    {
        var refreshSession = new RefreshSession()
        {
            User = user,
            CreatedAt = DateTime.UtcNow,
            ExpiresIn = DateTime.UtcNow.AddDays(int.Parse(_refreshSessionOptions.ExpiredDaysTime)),
            Jti = jti,
            RefreshToken = Guid.NewGuid()
        };

        _accountDbContext.RefreshSessions.Add(refreshSession);
        await _accountDbContext.SaveChangesAsync(cancellationToken);

        return refreshSession.RefreshToken;
    }

    public async Task<Result<IReadOnlyList<Claim>,Error>> GetUserClaims(
        string jwtToken,
        CancellationToken cancellationToken)
    {
        var jwtHandler = new JwtSecurityTokenHandler();

        var validateParameters = TokenValidationParametersFactory.CreateWithoutLifeTime(_jwtOptions);

        var validationResult = await jwtHandler.ValidateTokenAsync(jwtToken, validateParameters);
        if (validationResult.IsValid == false)
            return Errors.Tokens.InvalidToken();

        return validationResult.ClaimsIdentity.Claims.ToList();

    }
}