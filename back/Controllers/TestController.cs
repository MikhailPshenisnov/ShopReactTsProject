using Microsoft.AspNetCore.Mvc;

namespace back.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ShopApiController : ControllerBase
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

    // [HttpGet("{username?}")]
    // public IActionResult SaveUser(string? username)
    // {
    //     if (username is null)
    //     {
    //         HttpContext.Response.Cookies.Append("username", "");
    //         return Ok();
    //     }
    //
    //     if (username.Length < 4 || username.Any(char.IsPunctuation)) return Ok("incorrect_login");
    //     HttpContext.Response.Cookies.Append("username", username);
    //     return Ok();
    // }

    [HttpGet]
    public IActionResult RegisterUser()
    {
        var headers = HttpContext.Request.Headers;
        if (headers.TryGetValue("username", out var username)
            && headers.TryGetValue("password", out var password))
        {
            var cookies = HttpContext.Request.Cookies;
            cookies.TryGetValue("id", out var curUserId);
            curUserId ??= "";

            if (username == "" || password == "") return Ok("fill_in_your_login_and_password");

            if (username.ToString().Length < 6
                || username.ToString().Any(char.IsPunctuation))
                return Ok("incorrect_login_or_password");

            if (password.ToString().Length < 8
                || !password.ToString().Any(char.IsLetter)
                || !password.ToString().Any(char.IsDigit)
                || !password.ToString().Any(char.IsPunctuation)
                || !password.ToString().Any(char.IsLower)
                || !password.ToString().Any(char.IsUpper))
                return Ok("incorrect_login_or_password");

            if (!DbFunctions.IsUsernameFree(username!)) return Ok("login_has_already_been_used");

            DbFunctions.MakeConstUser(int.Parse(curUserId), username!, password!);
            HttpContext.Response.Cookies.Append("username", username!);

            return Ok("");
        }

        return BadRequest("incorrect_headers");
    }

    [HttpGet]
    public IActionResult LoginUser()
    {
        var headers = HttpContext.Request.Headers;
        if (headers.TryGetValue("username", out var username)
            && headers.TryGetValue("password", out var password))
        {
            var cookies = HttpContext.Request.Cookies;
            cookies.TryGetValue("cart", out var curCart);
            curCart ??= "";
            cookies.TryGetValue("username", out var curUsername);
            curUsername ??= "";

            if (username == "" || password == "") return Ok("fill_in_your_login_and_password");

            if (username.ToString().Length < 6
                || username.ToString().Any(char.IsPunctuation)) 
                return Ok("incorrect_login_or_password");

            if (password.ToString().Length < 8
                || !password.ToString().Any(char.IsLetter)
                || !password.ToString().Any(char.IsDigit)
                || !password.ToString().Any(char.IsPunctuation)
                || !password.ToString().Any(char.IsLower)
                || !password.ToString().Any(char.IsUpper)) 
                return Ok("incorrect_login_or_password");

            var tmpUser = DbFunctions.TryLoginUser(username!, password!);

            if (tmpUser is null)
                return Ok("incorrect_account_login_or_password");
            
            var newCart = curUsername == "" ? DbFunctions.CombineCart(curCart, tmpUser.cart) : tmpUser.cart;
            
            HttpContext.Response.Cookies.Append("id", tmpUser.id.ToString());
            HttpContext.Response.Cookies.Append("username", tmpUser.username);
            HttpContext.Response.Cookies.Append("cart", newCart);
            return Ok("");
        }

        return BadRequest("incorrect_headers");
    }

    [HttpGet]
    public IActionResult LogoutUser()
    {
        HttpContext.Response.Cookies.Append("id", "");
        HttpContext.Response.Cookies.Append("username", "");
        HttpContext.Response.Cookies.Append("cart", "");
        InitializeUser("", "", "");
        return Ok("");
    }

    [HttpGet]
    public IActionResult GetUser()
    {
        DbFunctions.DeleteExpiredUsers();

        var cookies = HttpContext.Request.Cookies;
        cookies.TryGetValue("id", out var curId);
        cookies.TryGetValue("username", out var curUsername);
        cookies.TryGetValue("cart", out var curCart);

        curId ??= "";
        curUsername ??= "";
        curCart ??= "";

        if (curId == "") return InitializeUser("", "", "");

        var result =
            "{" +
            "\"username\":\"" + $"{curUsername}" + "\"," +
            "\"cart\":\"" + $"{curCart}" + "\"" +
            "}";

        return Ok(result);
    }

    private IActionResult InitializeUser(string username, string password, string cart)
    {
        var date = DateOnly.FromDateTime(DateTime.Now).AddDays(3);
        var addedUserId = DbFunctions.AddUser(username, password, cart, date);
        HttpContext.Response.Cookies.Append("id", addedUserId.ToString());
        HttpContext.Response.Cookies.Append("username", username);
        HttpContext.Response.Cookies.Append("cart", cart);
        return Ok("");
    }

    [HttpGet]
    public IActionResult SetEmptyCookies()
    {
        var cookies = HttpContext.Request.Cookies;
        if (!cookies.TryGetValue("id", out _))
            HttpContext.Response.Cookies.Append("id", "");
        if (!cookies.TryGetValue("username", out _))
            HttpContext.Response.Cookies.Append("username", "");
        if (!cookies.TryGetValue("cart", out _))
            HttpContext.Response.Cookies.Append("cart", "");
        return Ok("");
    }

    [HttpGet]
    public IActionResult UpdateUserCart()
    {
        var headers = HttpContext.Request.Headers;
        if (headers.TryGetValue("cart", out var cart))
        {
            var cookies = HttpContext.Request.Cookies;
            cookies.TryGetValue("id", out var curId);
            curId ??= "";

            if (curId != "")
            {
                try
                {
                    HttpContext.Response.Cookies.Append("cart", cart!);
                    DbFunctions.UpdateUserCart(int.Parse(curId), cart!);
                    return Ok("");
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            return BadRequest("current_user_is_unknown");
        }

        return BadRequest("incorrect_headers");
    }

    // [HttpPost]
    // public IActionResult TestDbRequest()
    // {
    //     DbFunctions.DeleteExpiredUsers();
    //
    //     var headers = HttpContext.Request.Headers;
    //     if (headers.TryGetValue("username", out var username)
    //         && headers.TryGetValue("password", out var password)
    //         && headers.TryGetValue("cart", out var cart))
    //     {
    //         if (username == "" ^ password == "") return BadRequest("Пароль без логина или логин без пароля!");
    //
    //         var date = username == ""
    //             ? DateOnly.FromDateTime(DateTime.Now).AddDays(3)
    //             : DateOnly.FromDateTime(DateTime.Now).AddYears(2);
    //
    //         DbFunctions.AddUser(username!, password!, cart!, date);
    //         return Ok();
    //     }
    //
    //     return BadRequest("Некорректные заголовки");
    // }
}