namespace WAppLocaliza.Services
{
    public class UserMessageException : Exception
    {
        public readonly int StatusCode;

        public UserMessageException(int statusCode, string message): base(message)
        {
            this.StatusCode = statusCode;
        }
    }
}