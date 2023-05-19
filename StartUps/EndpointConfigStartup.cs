using judo_univ_rennes.Configurations;

namespace judo_univ_rennes.StartUps
{
    public static class EndpointConfigStartup
    {
        public static IServiceCollection UseEndpointConfiguration (this IServiceCollection services, IConfiguration configuration)
        {
#if DEBUG
            services.Configure<BaseAddress>(
            configuration.GetSection("BaseAddress").GetRequiredSection("dev"));

#else
            services.Configure<BaseAddress>(
            configuration.GetSection("BaseAddress").GetRequiredSection("prod"));
#endif



            return services;
        }
    }
}
