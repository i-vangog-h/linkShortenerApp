using linkApi.DataContext;
using linkApi.Entities;
using linkApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace linkApi.Repositories;

public class UrlRepo : IUrlRepo
{
    private readonly LinkShortenerContext _db;
    public UrlRepo(LinkShortenerContext db)
    {
        _db = db;
        WriteLine("Repo instantiated.");
    }

    public async Task<Url?> FindByIdAsync(int id)
    {
        Url? url = await _db.Urls.SingleOrDefaultAsync(u => u.Id == id);

        if (url is null)
        {
            return null;
        }

        return url; 
    }

    public async Task<Url?> FindByOgUrlAsync(string ogUrl)
    {
        Url? url = await _db.Urls.SingleOrDefaultAsync(u => u.OriginalUrl == ogUrl);

        if (url is null)
        {
            WriteLine("Record with such url is not present in a db");
            return null;
        }

        return url;
    }

    public async Task<Url[]> RetreiveAllAsync()
    {
        return await _db.Urls.OrderBy(u => u.Id).ToArrayAsync();
    }

    public async Task<Url?> CreateAsync(Url url)
    {
        await _db.Urls.AddAsync(url);
        var affected = await _db.SaveChangesAsync();
        if (affected == 1)
        {
            return await _db.Urls.FirstAsync(u => u.OriginalUrl == url.OriginalUrl);
        }

        return null;
    }

    public async Task<Url?> UpdateAsync(Url url)
    {
        _db.Update(url);

        int affected = await _db.SaveChangesAsync();

        if (affected == 1)
        {
            return url;
        }

        return null;
    }

    public async Task<bool?> DeleteAsync(int id)
    {
        Url? url = await FindByIdAsync(id);
        if (url is null) return null;

        _db.Urls.Remove(url);
        int affected = await _db.SaveChangesAsync();
        if(affected == 1)
        {
            return true;
        }

        return false;

    }
}
