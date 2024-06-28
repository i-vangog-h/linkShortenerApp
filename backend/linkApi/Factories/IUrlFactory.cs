using linkApi.Entities;

namespace linkApi.Interfaces;

public interface IUrlFactory
{
    /// <summary>
    /// Creates a Url object for a given url string.
    /// </summary>
    /// <param name="ogUrl"></param>
    /// <returns>Returns a Url object or null if provided url was in an incorrect format.</returns>
    Url? Create(string ogUrl, bool ensureValidity = true);
    bool IsValidUrl(string url);
    string Normalize(string url);
}
