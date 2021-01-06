using Microsoft.Extensions.DependencyInjection;

namespace UM.Core.Application
{
    public static class ServiceRegister
    {
        /// <summary>
        /// Register all dependencies and services of Application layer. This method makes solution get the maintainability and
        /// can be used in every layer of solution
        /// </summary>
        /// <param name="services"></param>
        public static void AddApplicationServices(this IServiceCollection services)
        {
        }
    }
}
