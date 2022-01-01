using System.Collections.Concurrent;
using AliceMafia.Application;
using AliceMafia.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Ninject;
using Ninject.Parameters;

namespace AliceMafia.Controllers
{
    [ApiController]
    [Route("/")]
    public class AliceMafiaController : ControllerBase
    {
        private static ControllerData data = new ControllerData();
        private static ConcurrentDictionary<string, UserContextBase> activeUsers = new ConcurrentDictionary<string, UserContextBase>();

        [HttpPost]
        public AliceResponse AlicePost(AliceRequest request)
        {
            var sessionId = request.Session.SessionId;
            if (!activeUsers.ContainsKey(sessionId))
            {
                var kernel = new StandardKernel(new ServiceModule());
                var context = kernel.Get<UserContextBase>(new ConstructorArgument("data", data, true));
                activeUsers[sessionId] = context;
                context.ChangeState(new DialogStartState(context));
            }

            return activeUsers[sessionId].HandleUserRequest(request);
        }
    }
}