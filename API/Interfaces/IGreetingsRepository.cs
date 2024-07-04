using API.Dto;

namespace API.Interfaces
{
    public interface IGreetingsRepository
    {
        Task<GreetingDto> GreetAsync(string name);
    }
}
