using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/hello")]
    public class GreetingsController : ControllerBase
    {
        private readonly IGreetingsRepository _greetingsRepository;

        public GreetingsController(IGreetingsRepository greetingsRepository)
        {
            _greetingsRepository = greetingsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Greet([FromQuery] string visitor_name)
        {
            var greeting = await _greetingsRepository.GreetAsync(visitor_name);
            return Ok(greeting);
        }
    }
}
