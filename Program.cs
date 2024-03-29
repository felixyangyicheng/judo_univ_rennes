

using Autofac.Core;
using Google;

namespace judo_univ_rennes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.UseUrls(new[] { "http://*:8080" });
            // Add services to the container.
            builder.Services.Configure<ConnectionStringModel>(
            builder.Configuration.GetSection("MongoDatabase"));

#if DEBUG
            builder.Services.Configure<BaseAddress>(
            builder.Configuration.GetSection("BaseAddress").GetRequiredSection("dev"));

#else
             builder.Services.Configure<BaseAddress>(
            builder.Configuration.GetSection("BaseAddress").GetRequiredSection("prod"));
#endif

            builder.Services.Configure<Endpoints>(
            builder.Configuration.GetSection("Endpoints"));

            builder.Services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                      new[] { "application/octet-stream" });
            });

            var connString = builder.Configuration.GetConnectionString("Account");
            builder.Services.AddDbContextPool<JudoDbContext>(options =>
                {
                    options.UseNpgsql(connString);
                }
                //ServiceLifetime.Transient
            );


            builder.Services
                .AddBlazorise(options =>
                {
                    options.Immediate = true;
                })
                .AddBootstrapProviders()
                .AddFontAwesomeIcons();
            builder.Services.AddControllers();
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<HttpContextAccessor>();
            builder.Services.AddMemoryCache();
            builder.Services.AddIdentityCore<ApiUser>(opt =>
            {
                opt.Lockout.AllowedForNewUsers = false;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultPhoneProvider;
            })
                            .AddRoles<IdentityRole>()
                            .AddEntityFrameworkStores<JudoDbContext>()
                            .AddSignInManager()
                            //.AddDefaultTokenProviders();
                            .AddTokenProvider(TokenOptions.DefaultProvider, typeof(DataProtectorTokenProvider<ApiUser>))
                            .AddTokenProvider(TokenOptions.DefaultEmailProvider, typeof(EmailTokenProvider<ApiUser>))
                            .AddTokenProvider(TokenOptions.DefaultPhoneProvider, typeof(PhoneNumberTokenProvider<ApiUser>))
                            .AddTokenProvider(TokenOptions.DefaultAuthenticatorProvider, typeof(AuthenticatorTokenProvider<ApiUser>));
            builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>opt.TokenLifespan = TimeSpan.FromHours(2));
            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
                };
            });
            //builder.Services.AddCors(
            //    options =>
            //    {
            //        options.AddPolicy("AllowSpecificOrigins",
            //            policy =>
            //            {
            //                policy.WithOrigins("*")
            //                .AllowAnyHeader()
            //                .AllowAnyMethod();
            //            });
            //    });
            builder.Services.AddCors(policy =>
            {
                policy.AddPolicy("CorsPolicy", opt => opt
                    //.WithOrigins("http://localhost:*/", "http://127.0.0.1:*/", "http://realtime_d3_client:80/","http://+:80/","http://heisreadonly.ddns.net", "https://heisreadonly.ddns.net", "http://log_generator:*/")
                   .AllowAnyOrigin()
                    //.AllowAnyHeader()
                    //.AllowAnyMethod()
                    //.AllowAnyMethod()
                    //.SetIsOriginAllowed(origin => true)
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                      // .AllowCredentials()

                    //.WithMethods("GET", "POST")
                    //.SetIsOriginAllowed((host)  => true)
                    // .AllowCredentials()
                    );
            });

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
            builder.Services.AddBlazoriseRichTextEdit(); // richText Supports

            builder.Services.AddAutoMapper(typeof(MapperConfig));
            builder.Services.AddControllers();
            //builder.Services.AddServiceDiscovery(options => options.UseEureka());
            builder.Host.UseSerilog((ctx, lc) =>
                lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));
            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services
                .AddServerSideBlazor(opt => {
                    opt.DetailedErrors = true;
                    opt.DisconnectedCircuitMaxRetained = 100;
                    opt.DisconnectedCircuitRetentionPeriod= TimeSpan.FromSeconds(180);
                    opt.JSInteropDefaultCallTimeout = TimeSpan.FromMinutes(1);
                    opt.MaxBufferedUnacknowledgedRenderBatches = 10;
                })
                .AddHubOptions(opt => {
                    opt.EnableDetailedErrors = true;
                    opt.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
                    opt.HandshakeTimeout= TimeSpan.FromSeconds(15);
                    opt.KeepAliveInterval= TimeSpan.FromSeconds(15);
                    opt.MaximumParallelInvocationsPerClient = 1;
                    opt.MaximumReceiveMessageSize = 32 * 1024;
                    opt.StreamBufferCapacity = 10;

                  
                });
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<ApiAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(p => p.GetRequiredService<ApiAuthenticationStateProvider>());
            builder.Services.AddScoped<JwtSecurityTokenHandler>();
            var emailConfig = builder.Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            builder.Services.AddSingleton(emailConfig);
            builder.Services.AddScoped<SignInManager<ApiUser>>();
            builder.Services.AddScoped<IPdfRepo, PdfService>();
            builder.Services.AddScoped<IAuthRepo, AuthService>();
            builder.Services.AddScoped<INewsRepo, NewsService>();
            builder.Services.AddScoped<INewsRepo, NewsService>();
            builder.Services.AddScoped<IIndexMarkdownRepo, IndexMarkdownService>();
            builder.Services.AddScoped<IPostRepo, PostService>();
            builder.Services.AddScoped<ICommandRepo, CommandService>();
            builder.Services.AddScoped<ICommentRepo, CommentService>();
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddScoped<ITokenRepo, TokenService>();
            builder.Services.AddSingleton<IChatRoomService, InMemoryChatRoomService>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options => {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                                Reference = new OpenApiReference {
                                    Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                }
                            },
                        new string[] {}
                    }
                });
            });
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
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                //options.RoutePrefix = string.Empty;
            });
          //  app.UseResponseCompression();

            app.UseMetricServer();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

         

            app.Use((context, next) =>
            {
                // Http Context
                var counter = Metrics.CreateCounter
                ("PathCounter", "Count request",
                new CounterConfiguration
                {
                    LabelNames = new[] { "method", "endpoint" }
                });
                // method: GET, POST etc.
                // endpoint: Requested path
                counter.WithLabels(context.Request.Method, context.Request.Path).Inc();
                return next();
            });
            app.UseEndpoints(e =>
            {

                e.MapBlazorHub();
                e.MapHub<ChatHub>("/chathub");
                e.MapControllers();
                e.MapHub<NotifHub>("/notifhub");
                e.MapFallbackToPage("/_Host");

            });
            app.Run();
        }
    }
}