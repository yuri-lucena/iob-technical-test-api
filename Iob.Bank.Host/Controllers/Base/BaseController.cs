using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Iob.Bank.Host.Controllers.Base;

/// <summary>
/// Base controller for all controllers in the application.
/// </summary>
public class BaseController : ControllerBase
{
    /// <summary>
    /// Gets a claim from the current user's identity.
    /// </summary>
    /// <param name="name">The name of the claim to retrieve.</param>
    /// <returns>The claim with the specified name.</returns>

    protected Claim GetClaim(string name) => ((ClaimsIdentity)User.Identity!).Claims.First(x => x.Type == name);

    /// <summary>
    /// Gets the user ID from the current user's identity.
    /// </summary>
    /// <returns>The user ID.</returns>
    protected long UserId() => Convert.ToInt64(GetClaim("UserId").Value);
}
