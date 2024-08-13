using GallUni.Data;
using GallUni.DbInitializer;
using GallUni.AppUtilities;
using GallUni.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//DataBase Connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
//To properly Manage the display of the error such as 404
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

builder.Services.AddRazorPages();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
//builder.Services.AddScoped<IDbInitializer, DbInitializer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
//Call DbInizializer
//SeedDataBase();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
//void SeedDataBase()
//{
//    using (var scope = app.Services.CreateScope())
//    {
//        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
//        //Never forget dbInitializer.Initialize();
//        dbInitializer.Initialize();
//    }
//}




//string wwwRootpath = _webHostEnvironment.WebRootPath;
//if (file != null)
//{
//    //Guid.NewGuid().ToString() is used to randomly name files while saving them in the folder
//    //Path.GetExtension(file.FileName) is to get the same extension that the uploaded image
//    //ImageBuilder.Current.Build(Path.GetFullPath(file.Name), Path.Combine(wwwRootpath, @"images\Products"), resizeSetting);
//    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
//    string productpath = Path.Combine(wwwRootpath, @"images\Products");
//    // While update the product we need to check if the image field is updated or not so that to not change 
//    // the value of the current image , however if it's updated we need to change the value and delete the old path
//    if (!string.IsNullOrEmpty(obj.Product.ImageUrl))
//    {
//        //delete the old image
//        var oldpath = Path.Combine(wwwRootpath, obj.Product.ImageUrl.TrimStart('\\'));

//        if (System.IO.File.Exists(oldpath))
//        {
//            System.IO.File.Delete(oldpath);
//        }
//    }
//    using (var filestream = new FileStream(Path.Combine(productpath, filename), FileMode.Create))
//    {
//        file.CopyTo(filestream);
//    }
//    obj.Product.ImageUrl = @"\images\Products\" + filename;
//    //ResizeSettings resizeSetting = new ResizeSettings
//    //{
//    //    Width = 100,
//    //    Height = 150,
//    //    Format = Path.GetExtension(file.FileName)
//    //};            

//}