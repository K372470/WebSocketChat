using WebSocketChat.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages((x) =>
{
    x.Conventions.AuthorizePage("/chat");//TODO:Move names to Constants class
});
builder.Services.AddAuthentication("Cookies").AddCookie((x) =>
{
    x.Cookie.Name = "session";
    x.LoginPath = "/login";
    x.LogoutPath = "/";
    x.AccessDeniedPath = "/";
    x.ExpireTimeSpan = TimeSpan.FromMinutes(2);
});
builder.Services.AddAuthorization();
builder.Services.AddSingleton<ChatService>();
builder.Services.AddSingleton<IDataBase, MemoryDataBase>();
builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001");


var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseWebSockets();
app.UseMiddleware<ChatHubMiddleware>();

app.MapRazorPages();

app.Run();
