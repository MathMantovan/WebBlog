using Blog.Data;
using Blog.Services;

var builder = WebApplication.CreateBuilder(args);

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

app.MapControllers();

app.Run();
