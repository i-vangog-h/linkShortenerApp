namespace linkApi.Services;
using System.Text;
using linkApi.Interfaces;

public class HashingService : IHashingService
{
    public HashingService() 
    { 
        WriteLine ("Hashing Service instantiated."); 
    }

    // Url can only consist of one of these 62 characters
    private readonly char[] baseLiterals = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();


    /// <summary>
    /// </summary>
    /// <param name="base10Id"></param>
    /// <param name="hash"></param>
    /// <returns>Returns encoded id which will be used as a hash for the URL.</returns>
    public string EncodeBase10To62(int base10Id, out string hash)
    {
        StringBuilder sb = new StringBuilder();
        int n = base10Id;
        while(n > 0)
        {
            sb.Append(baseLiterals[n % 62]);
            n = n / 62;
        }
        hash = Reverse(sb.ToString());

        return hash;
    }
    public string EncodeBase10To62(int base10Id)
    {
        StringBuilder sb = new StringBuilder();
        int n = base10Id;
        while (n > 0)
        {
            sb.Append(baseLiterals[n % 62]);
            n = n / 62;
        }
        return Reverse(sb.ToString());

    }

    private string Reverse(string hash)
    {
        char[] chars = hash.ToCharArray();
        int r = hash.Length - 1;
        for (int l = 0; l < r; l++, r--)
        {
            char temp = chars[l];
            chars[l] = chars[r];
            chars[r] = temp;
        }
        return new string(chars);
    }

    public int DecodeBase62To10(string base62Hash)
    {
        int id = 0;

        for (int i = 0; i < base62Hash.Length; i++)
        {
            // Determining the value of base62 signs in decimal 
            // according to their position in baseLiterals array and their ASCII code.

            if ('a' <= base62Hash[i] &&
                       base62Hash[i] <= 'z')
                id = id * 62 + base62Hash[i] - 'a';
            if ('A' <= base62Hash[i] &&
                       base62Hash[i] <= 'Z')
                id = id * 62 + base62Hash[i] - 'A' + 26;
            if ('0' <= base62Hash[i] &&
                       base62Hash[i] <= '9')
                id = id * 62 + base62Hash[i] - '0' + 52;
        }

        return id;
    }


}
