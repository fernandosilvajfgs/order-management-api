public interface IStatusService
{
    Task<(bool Success, string? Message, IEnumerable<string> StatusList)> ChangeStatusAsync(StatusRequest request);
}
