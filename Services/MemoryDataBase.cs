using WebSocketChat.Models;
using WebSocketChat.Utils;

namespace WebSocketChat.Services
{
    public class MemoryDataBase : IDataBase
    {
        private List<UserData> UserPasswords { get;set; }
        public MemoryDataBase()
        {
            UserPasswords = new() {
                new("a","1234".Encode()),
                new("b","lox".Encode()),
                new("c","334".Encode()),
            };
        }
        public void RegisterUser(string name, string password)
        {
            UserPasswords.Add(new(name, password.Encode()));
        }
        public bool UserExists(string name)
        {
            return UserPasswords.Exists((x) => x.NickName == name);
        }
        public bool UserExists(string name, string password)
        {
            return UserPasswords.Exists((x) => x.NickName == name && x.PasswordHash == password.Encode());
        }
    }
}
