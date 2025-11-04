var builder = WebApplication.CreateBuilder(args);

// ✅ Add services
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

var app = builder.Build();

// ✅ Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// ✅ Redirect HTTP to HTTPS
app.UseHttpsRedirection();

// ✅ Tell the app it’s running in a subfolder
app.UsePathBase("/demo");

// ✅ Use static files (must come after UsePathBase)
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

// ✅ Run the app
app.Run();
