using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Proto;

namespace SampleWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloController : ControllerBase
    {
        public HelloController(IRootContext actorRoot,
                               ILogger<HelloController> logger)
        {
            Root = actorRoot;
            Logger = logger;
        }

        public IRootContext Root { get; }
        public ILogger<HelloController> Logger { get; }

        [HttpGet]
        public async Task<string> Get()
        {
            var response = await Root.RequestAsync<string>(new PID(Root.System.Address,
                                                                   "HelloManage"),
                                                           "Hello", TimeSpan.FromSeconds(30));
            return await Task.FromResult(response); 
        }
    }
}
