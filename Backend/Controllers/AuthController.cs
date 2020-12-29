using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendModel;
using RequestModel;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly CornellGoDb _context;

        public AuthController(CornellGoDb context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<GetAuthResponse> CheckLoginStatus(GetAuthIdRequest request)
        {
            return new(await _context.UserSessions.FromToken(request.Session) != null);
        }

        [HttpPost]
        public async Task<PostAuthResponse> Login(PostAuthRequest request)
        {
            User user = (await _context.Authenticators.FirstOrDefaultAsync(u => u.Username == request.Username && u.Password == request.Password))?.User;

            if (user == null) // No such user exists
                return new("");

            UserSession session = await _context.UserSessions.FirstAsync(s => s.User.Id == user.Id);

            if (session != null) // Session already exists
                return new(session.ToToken());

            // Create new session
            string id = (await _context.UserSessions.AddAsync(new UserSession(DateTime.UtcNow, user))).Entity.ToToken();
            await _context.SaveChangesAsync();

            return new(id);
        }

        [HttpDelete]
        public async Task<DeleteAuthResponse> Logout(DeleteAuthRequest request)
        {
            UserSession session = await _context.UserSessions.FromToken(request.Session);

            if (session == null)
                return new(false);

            _context.Remove(session);
            await _context.SaveChangesAsync();

            return new(true);
        }

        [HttpGet("id")]
        public async Task<GetAuthIdResponse> GetUserId(GetAuthIdRequest request)
        {
            return new((await _context.UserSessions.FromToken(request.Session))?.User.Id.ToString() ?? "");
        }
    }
}
