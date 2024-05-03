using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;

namespace back.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        using var client = new HttpClient();
        
        client.BaseAddress = new Uri("https://fakestoreapi.com/");
        var request = new HttpRequestMessage(HttpMethod.Get, "/products");
        
        using var response = await client.SendAsync(request);
        
        if (!response.IsSuccessStatusCode) return BadRequest();
        
        var json = await response.Content.ReadAsStringAsync();
        return Ok(json);
    }
    
    [HttpGet("{username}")]
    public IActionResult SaveUser(string username)
    {
        if (username.Contains("@"))
        {
            StaticStorage.CurUser = "";
        }
        else
        {
            StaticStorage.CurUser = username;
        }
        return Ok(StaticStorage.CurUser);
    }
    
    [HttpGet]
    public IActionResult GetUser()
    {
        var result = "{\"username\":\"" + $"{StaticStorage.CurUser}" + "\"}";
        return Ok(result);
    }
}