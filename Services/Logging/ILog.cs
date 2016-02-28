namespace CountDownAPI.Services
{
    public interface ILog
    {
        void Log(string text, string memberName = "");
    }
}