using Blazored.LocalStorage;
using Blazorise;
using judo_univ_rennes.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.StaticFiles;
using MudBlazor;
using MudBlazor.Services;
using Serilog;
using Tewr.Blazor.FileReader;

namespace judo_univ_rennes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddFileReaderService();
            builder.Services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
                config.SnackbarConfiguration.PreventDuplicates = false;
                config.SnackbarConfiguration.NewestOnTop = false;
                config.SnackbarConfiguration.ShowCloseIcon = true;
                config.SnackbarConfiguration.VisibleStateDuration = 2000;
                config.SnackbarConfiguration.HideTransitionDuration = 500;
                config.SnackbarConfiguration.ShowTransitionDuration = 500;
                config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
            });

            builder.Services
                .AddBlazorise(options =>
                {
                    options.Immediate = true;
                }).AddEmptyProviders();

            builder.Services.AddControllers();
            //builder.Services.AddServiceDiscovery(options => options.UseEureka());
            builder.Host.UseSerilog((ctx, lc) =>
                lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));
            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddBlazoredLocalStorage();

            var app = builder.Build();


            var provider = new FileExtensionContentTypeProvider();
            // Add new MIME type mappings
            provider.Mappings[".res"] = "application/octet-stream";
            provider.Mappings[".pexe"] = "application/x-pnacl";
            provider.Mappings[".nmf"] = "application/octet-stream";
            provider.Mappings[".mem"] = "application/octet-stream";
            provider.Mappings[".wasm"] = "application/wasm";
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}