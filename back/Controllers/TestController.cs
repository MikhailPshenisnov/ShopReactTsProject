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

    [HttpGet("{username?}")]
    public IActionResult SaveUser(string? username)
    {
        if (username is null)
        {
            HttpContext.Response.Cookies.Append("username", "");
            return Ok();
        }

        if (username.Length < 4 || username.Any(char.IsPunctuation)) return Ok("incorrect_login");
        HttpContext.Response.Cookies.Append("username", username);
        return Ok();
    }

    [HttpGet]
    public IActionResult GetUser()
    {
        var cookies = HttpContext.Request.Cookies;
        cookies.TryGetValue("username", out var curUsername);
        var result = "{\"username\":\"" + $"{curUsername ?? ""}" + "\"}";
        return Ok(result);
    }

    [HttpGet]
    public IActionResult SetEmptyCookies()
    {
        var cookies = HttpContext.Request.Cookies;
        if (!cookies.TryGetValue("username", out _))
            HttpContext.Response.Cookies.Append("username", "");
        return Ok();
    }
    
    [HttpGet("{username}/{password}/{cart}/{end_date}")]
    public IActionResult TestDbRequest(string username, string password, string cart, string end_date)
    {
        DbFunctions.AddUser(username, password, cart, end_date);
        return Ok();
    }
}