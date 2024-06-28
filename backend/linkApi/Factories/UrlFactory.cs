using linkApi.Entities;
using linkApi.Interfaces;

namespace linkApi.Factories;

public class UrlFactory : IUrlFactory
{
    public UrlFactory()
    {
        WriteLine("Factory instantiated.");
    }
    /// <summary>
    /// Creates a Url object for a given url string.
    /// </summary>
    /// <param name="ogUrl"></param>
    /// <returns>Returns a Url object or null if provided url was in incorrect format</returns>
    public Url? Create(string ogUrl, bool ensureValidity = true)
    {
        Url url;

        if(ensureValidity)
        {
            if (IsValidUrl(ogUrl))
            {
                url = new() { OriginalUrl = ogUrl };
                return url;
            }
            return null;
        }

        return new() { OriginalUrl = ogUrl };
    }

    public bool IsValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    public string Normalize(string url)
    {
        int k = url.Length;

        while (url[k - 1] == '/') k--;
       
        return url.Substring(0, k);
    }
}
