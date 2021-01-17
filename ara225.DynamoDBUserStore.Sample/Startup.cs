using Amazon.DynamoDBv2;
using ara225.DynamoDBUserStore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace ara225.DynamoDBUserStore.Sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add DynamoDB user store
            // To test with a local DynamoDB in Docker
            services.AddSingleton<DynamoDBDataAccessLayer>(x => new DynamoDBDataAccessLayer(new AmazonDynamoDBClient(new AmazonDynamoDBConfig { ServiceURL = "http://localhost:8000" }), "UserStoreTable", "RoleStoreTable"));
            // To test with DynamoDB in the cloud
            //services.AddSingleton<DynamoDBDataAccessLayer>(x => new DynamoDBDataAccessLayer(new Amazon.DynamoDBv2.AmazonDynamoDBClient(), "UserStoreTable", "RoleStoreTable"));
            services.AddDefaultIdentity<DynamoDBUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddUserStore<DynamoDBUserStore<DynamoDBUser>>()
                .AddRoles<DynamoDBRole>()
                .AddRoleStore<DynamoDBRoleStore<DynamoDBRole>>();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public async Task CreateTestUserAndRole(UserManager<DynamoDBUser> userManager, RoleManager<DynamoDBRole> roleManager)
        {
            if (await roleManager.FindByNameAsync("DynamoDBUserStoreTestRole") == null)
            {        
                DynamoDBRole role = new DynamoDBRole();
                role.Name = "DynamoDBUserStoreTestRole";
                await roleManager.CreateAsync(role);
            }
            if (await userManager.FindByNameAsync("user@example.com") == null)
            {
                DynamoDBUser user = new DynamoDBUser("user@example.com");
                user.Email = "user@example.com";
                user.EmailConfirmed = true;
                await userManager.CreateAsync(user);
                await userManager.AddPasswordAsync(user, "TestPassword123!");
                await userManager.AddToRoleAsync(user, "DynamoDBUserStoreTestRole");
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<DynamoDBUser> userManager, RoleManager<DynamoDBRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            CreateTestUserAndRole(userManager, roleManager).Wait();
        }
    }
}
