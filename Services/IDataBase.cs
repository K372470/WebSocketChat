namespace WebSocketChat.Services
{
    public interface IDataBase
    {
        public void RegisterUser(string name, string password);
        public bool UserExists(string name);
        public bool UserExists(string name, string password);
    }
}