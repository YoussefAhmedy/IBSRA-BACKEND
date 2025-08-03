using System;
using System.Web.Http;
using System.Web.Http.Cors;

[EnableCors(origins: "*", headers: "*", methods: "*")]
[RoutePrefix("api/users")]
public class UsersController : ApiController
{
    private DatabaseHelper db = new DatabaseHelper();

    [HttpPost]
    [Route("login")]
    public IHttpActionResult Login([FromBody] LoginRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.PhoneNumber))
                return BadRequest("Phone number is required");

            User user = db.LoginUser(request.PhoneNumber);
            
            if (user != null)
                return Ok(new { success = true, user = user, message = "Login successful" });
            else
                return Ok(new { success = false, message = "User not found" });
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpPost]
    [Route("register")]
    public IHttpActionResult Register([FromBody] RegisterRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Name))
                return BadRequest("Name is required");
            
            if (request.Age <= 0)
                return BadRequest("Invalid age");
            
            if (string.IsNullOrEmpty(request.PhoneNumber))
                return BadRequest("Phone number is required");

            User newUser = db.RegisterUser(request.Name, request.Age, request.PhoneNumber);
            
            if (newUser != null)
                return Ok(new { success = true, user = newUser, message = "Registration successful" });
            else
                return Ok(new { success = false, message = "User already exists" });
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }
}

// Request Models
public class LoginRequest
{
    public string PhoneNumber { get; set; }
}

public class RegisterRequest
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string PhoneNumber { get; set; }
}
