using Application.Dtos.Chat;

namespace Application.Interfaces
{
    public interface IChatService
    {
        ChatResponseDto ProcessMessage(ChatRequestDto request);
    }
}
