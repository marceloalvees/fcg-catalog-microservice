namespace Application.Contracts
{
    public record class MessageResponseDto<T>(bool Success, string Message, T? Data = default);
}
