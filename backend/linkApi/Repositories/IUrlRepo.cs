using linkApi.Entities;

namespace linkApi.Interfaces;

public interface IUrlRepo
{
    Task<Url?> CreateAsync(Url url);
    Task<Url?> FindByIdAsync(int id);
    Task<Url?> FindByOgUrlAsync(string ogUrl);
    Task<Url[]> RetreiveAllAsync();    
    Task<Url?> UpdateAsync(Url url);
    Task<bool?> DeleteAsync(int id);
}
