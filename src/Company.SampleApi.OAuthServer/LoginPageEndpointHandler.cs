using Microsoft.AspNetCore.Http;
using RazorEngine;
using RazorEngine.Templating;

public class LoginPageEndpointHandler
{
    private const string _template = """
        <html>
            <head>
                <title>Sample Api - Login</title>
            </head>
            <body style="font-family:Gill Sans, sans-serif; text-align:center;">
                <h1 style="font-size:30px;">Login</h1>
                <form id="formElem">
                    <label>Login</label> <input name="login" />
                    <label>Password</label> <input name="password" />
                    <input type="submit" />
                </form>
                <script>
                    formElem.onsubmit = function submitForm(e) {
                        e.preventDefault();
                        const data = new FormData(formElem);
                        const payload = {};
                        data.forEach((value, name) => {
                            payload[name] = value;
                        });
                        fetch("/oauth/login", {
                            method: "POST",
                            headers: {
                                "Content-Type": "application/json"
                            },
                            body: JSON.stringify(payload)
                        });
                    }
                </script>
            </body>
        </html>
    """;
    public IResult Handle()
    {
        var result = Engine
            .Razor
            .RunCompile(_template,
                "templateKey",
                null,
                new
                {
                    Name = "World"
                });

        return Results.Content(result, "text/html");
    }
}
