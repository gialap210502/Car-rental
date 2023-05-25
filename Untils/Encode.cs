using System.Text;
namespace Car_rental.Untils
{

    public class Encode
    {
        public string encode(string password)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] bytes = Encoding.Default.GetBytes(password);
            byte[] encoded = md5.ComputeHash(bytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < encoded.Length; i++)
                sb.Append(encoded[i].ToString("x2"));

            return sb.ToString();
        }
    }
}