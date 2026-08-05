namespace WebSocketChat.Services
{
    public interface IUserDataBase
    {
        public void RegisterUser(string name, string password);
        public bool UserExists(string name);
        public bool UserExists(string name, string password);
    }
}