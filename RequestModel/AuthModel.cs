using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestModel
{
    public record GetAuthRequest(string Session);

    public record GetAuthResponse(bool IsLoggedIn);

    public record PostAuthRequest(string Username, string Password);

    public record PostAuthResponse(string Session);

    public record DeleteAuthRequest(string Session);

    public record DeleteAuthResponse(bool Success);

    public record GetAuthIdRequest(string Session);

    public record GetAuthIdResponse(string UserId);
}
