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
    //x.Events = new JwtBearerEvents
    //{
    //    OnAuthenticationFailed = context =>
    //    {
    //        Console.WriteLine("JWT ERROR:");
    //        Console.WriteLine(context.Exception.Message);
    //        return Task.CompletedTask;
    //    }
    //};
});

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });
builder.Services.AddDbContext<BlogDataContext>();
builder.Services.AddTransient<TokenService>(); //sempre que eu usar vai criar uma nova instancia
//builder.Services.AddScoped(); //Vai criar por transação, ou seja, se tiver mais de uma requisição. se em uma requisição eu usar 4 metodos com o toke, eu reutilizo ele 4 vezes
//builder.Services.AddSingleton(); //um por app, vai carregar na memoria e sempre vai utilizar ele independente das requisições
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
