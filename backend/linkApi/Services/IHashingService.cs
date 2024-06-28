using linkApi.Entities;

namespace linkApi.Interfaces;

public interface IHashingService
{
    /// <summary>
    /// </summary>
    /// <param name="base10Id"></param>
    /// <param name="hash"></param>
    /// <returns>Returns encoded id which will be used as a hash for the URL.</returns>
    string EncodeBase10To62(int base10Id, out string hash);
    /// <summary>
    /// </summary>
    /// <param name="base10Id"></param>
    /// <returns>Returns encoded id which will be used as a hash for the URL.</returns>
    string EncodeBase10To62(int base10Id);
    int DecodeBase62To10(string base62Hash);
}
