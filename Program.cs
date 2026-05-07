using Blog;
using Blog.Data;
using Blog.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
//Console.WriteLine($"JWT KEY PROGRAM: [{Configuration.JwtKey}]");
//Console.WriteLine($"JWT KEY PROGRAM LENGTH: {Configuration.JwtKey.Length}");
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
    x.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var raw = context.Request.Headers["Authorization"].ToString();
            Console.WriteLine($"OnMessageReceived RAW: [{raw}]");

            if (raw.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                var tok = raw.Substring(7).Trim();
                // log char codes to detect look-alike dots
                var codes = string.Join(" ", tok.Take(60).Select(c => ((int)c).ToString("X2")));
                Console.WriteLine($"OnMessageReceived CHARS(hex): {codes}");
                Console.WriteLine($"OnMessageReceived DOTS count: {tok.Count(c => c == '.')}");
                context.Token = tok;
            }
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"JWT AUTH FAILED: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine($"JWT TOKEN VALIDO - Usuario: {context.Principal?.Identity?.Name}");
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            Console.WriteLine($"JWT CHALLENGE - Error: {context.Error}");
            Console.WriteLine($"JWT CHALLENGE - AuthenticateFailure: {context.AuthenticateFailure?.Message}");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });
builder.Services.AddDbContext<BlogDataContext>();
builder.Services.AddTransient<TokenService>(); //sempre que eu usar vai criar uma nova instancia
//builder.Services.AddScoped(); //Vai criar por transa��o, ou seja, se tiver mais de uma requisi��o. se em uma requisi��o eu usar 4 metodos com o toke, eu reutilizo ele 4 vezes
//builder.Services.AddSingleton(); //um por app, vai carregar na memoria e sempre vai utilizar ele independente das requisi��es
var app = builder.Build();

app.Use(async (context, next) =>
{
    var auth = context.Request.Headers["Authorization"].ToString();

    Console.WriteLine("AUTH HEADER RECEBIDO:");
    Console.WriteLine($"[{auth}]");

    await next();
});
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Run();
