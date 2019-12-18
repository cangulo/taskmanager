namespace TaskManagerAPI.Exceptions.Handlers
{
    public interface IExceptionHandler
    {
        string CreateResponseContent();

        int GetHttpStatusCode();
    }
}