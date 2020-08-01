using System.Collections.Generic;
using System.Security.Claims;

namespace Cedita.Essence.Abstractions.Security
{
    public interface IApiKey
    {
        string Key { get; set; }

        string UserId { get; set; }

        IEnumerable<Claim> AdditionalClaims { get; set; }
    }
}
