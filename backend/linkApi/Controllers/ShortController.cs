using Microsoft.AspNetCore.Mvc;
using linkApi.Entities;
using linkApi.Interfaces;

namespace linkApi.Controllers;

[Route("api")]
[ApiController]
public class ShortController : ControllerBase
{
    private IUrlRepo _repo;
    private IHashingService _hashingService;
    private IUrlFactory _urlFactory;

    public ShortController(IUrlRepo repo, IHashingService hashingService, IUrlFactory urlFactory)
    {
        _repo = repo;
        _hashingService = hashingService;
        _urlFactory = urlFactory;
    }
    
    [HttpPost("generate")]
    [ProducesResponseType(200)] // Ok
    [ProducesResponseType(201)] // Created
    [ProducesResponseType(400)] // Bad Request
    public async Task<IActionResult> Generate([FromBody] string ogUrl)
    {
        if (!_urlFactory.IsValidUrl(ogUrl)) 
        {
            return BadRequest("Incorrect url format"); //400
        }

        ogUrl = _urlFactory.Normalize(ogUrl); //truncate trailing forward-slash

        string baseUri = $"{Request.Scheme}://{Request.Host}/api"; 

        Url? url;
        url = await _repo.FindByOgUrlAsync(ogUrl);


        //url already exists in a db
        if (url is not null)
        {
            // generate and assign hash if it was missing in a db
            if (url.Hash != _hashingService.EncodeBase10To62(url.Id, out string newHash))
            {
                url.Hash = newHash;

                var result = await _repo.UpdateAsync(url);
                if (result is null) WriteLine($"Unable to add hash to url {url.Id}");
            }

            return Ok(
                value: new {
                    shortUrl = $"{baseUri}/get-original/{url.Hash}"
                }
            ); //200
        }

        //url doesnt exist -> generate 
        url = _urlFactory.Create(ogUrl, ensureValidity: false);
        url = await _repo.CreateAsync(url!);

        if (url is null)
        {
            return BadRequest($"DB: Failed to add url {ogUrl} to a db"); //400
        }

        // Need to save url to a DB first for an Id to be auto-assigned to it.
        // Then use this Id to generate a Hash for the url's record.

        string hash = _hashingService.EncodeBase10To62(url.Id);
        url.Hash = hash;

        var updated = await _repo.UpdateAsync(url);

        if (updated is null) 
            return BadRequest($"DB: Failed to add hash to url {url.Id}"); //400
        
        return Created( 
            uri: $"{baseUri}/get-record/{url.Id}",
            value: new {
                    shortUrl = $"{baseUri}/get-original/{url.Hash}",
                }
            ); //201
    }


    
    [HttpGet("get-original/{hash}")]
    [ProducesResponseType(200, Type = typeof(string))] // Ok
    [ProducesResponseType(302)] // Found -> redirect 
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> GetOriginal(string hash)
    {
        int urlId = _hashingService.DecodeBase62To10(hash);

        Url? url = await _repo.FindByIdAsync(urlId);

        if (url is null)
        {
            return NotFound("Provided hash is not assigned to any url at the moment.");
        }

        url.AccessCount++;
        await _repo.UpdateAsync(url);

        // just return ogUrl to a caller, let it redirect itself
        //return Ok(url.OriginalUrl);

        
        return Redirect(url.OriginalUrl); //302

    }

    [HttpGet("get-record/{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetRecord(int id)
    {
        Url? url = await _repo.FindByIdAsync(id);

        if(url is null)
        {
            return NotFound();
        }

        return Ok(url);
    }

    [HttpGet("get-all", Name = "GETALL")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetAll()
    {
        Url[] urls = await _repo.RetreiveAllAsync();

        return Ok(urls);
    }

    [HttpDelete("remove-record/{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> RemoveRecord(int id)
    {
        bool? deleted = await _repo.DeleteAsync(id);

        if(deleted is null)
        {
            return NotFound();
        }

        if (deleted.Value)
        {
            return NoContent(); //204
        }
        else
        {
            return BadRequest($"Url {id} was found but failed to delete."); //400
        }
    }
}
