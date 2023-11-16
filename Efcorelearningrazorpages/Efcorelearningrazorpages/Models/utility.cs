namespace Efcorelearningrazorpages.Models
{
    public static class utility
    {

        public static string Getlastchars(byte[] token)
        {
            return token[7].ToString();
        }
    }
}
