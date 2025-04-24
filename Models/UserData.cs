namespace WebSocketChat.Models
{
    public class UserData
    {
        public string NickName;

        public UserData(string nickName, string passwordHash)
        {
            NickName = nickName;
            PasswordHash = passwordHash;
        }

        /// <summary>
        /// BASE64 For Example
        /// </summary>
        public string PasswordHash;
    }
}
