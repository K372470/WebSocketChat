using System.Text;

namespace WebSocketChat.Utils
{
    public static class StringUtilites
    {
        /// <summary>
        /// Trim Line after first indexOf('\0') symbol
        /// </summary>
        /// <returns>input[..indexOf('\0')]</returns>
        public static string TrimNullCharacters(this string inputLine)
        {
            var endOfMessage = inputLine.IndexOf('\0');
            return endOfMessage != -1 ? inputLine[..endOfMessage] : inputLine;
        }
        public static string Encode(this string input) => Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
        public static string Decode(this string input) => Encoding.UTF8.GetString(Convert.FromBase64String(input));
    }
}
