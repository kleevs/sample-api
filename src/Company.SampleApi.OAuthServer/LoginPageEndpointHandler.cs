using Microsoft.AspNetCore.Http;

namespace Company.SampleApi.OAuthServer;

public class LoginPageEndpointHandler
{
    public IResult Handle() => Results.Content($"""
        <head>
            <title>Sample Api - Login</title>
        </head>
        <body style="font-family:Gill Sans, sans-serif; text-align:center;">
            <h1 style="font-size:30px;">Login</h1>
            <form method="post" action="/oauth/login">
                <label>Login</label> <input name="login" />
                <label>Password</label> <input name="password" />
                <input type="submit" />
            </form>
        </body>
    """, "text/html");
}

