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

    [HttpPost]
    public IActionResult TestDbRequest()
    {
        DbFunctions.DeleteExpiredUsers();
        
        var headers = HttpContext.Request.Headers;
        if (headers.TryGetValue("username", out var username)
            && headers.TryGetValue("password", out var password)
            && headers.TryGetValue("cart", out var cart))
        {
            if (username == "" ^ password == "") return BadRequest("Пароль без логина или логин без пароля!");

            var date = username == ""
                ? DateOnly.FromDateTime(DateTime.Now).AddDays(3)
                : DateOnly.FromDateTime(DateTime.Now).AddYears(2);

            DbFunctions.AddUser(username!, password!, cart!, date);
            return Ok();
        }

        return BadRequest("Некорректные заголовки");
    }
}