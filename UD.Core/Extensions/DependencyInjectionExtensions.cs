namespace UD.Core.Extensions
{
    using AutoMapper;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Reflection;
    using UD.Core.Helper.Services;
    using UD.Core.Helper.Validation;
    public static class DependencyInjectionExtensions
    {
        /// <summary><paramref name="services"/> içerisinde, <paramref name="assembly"/> içinde bulunan ve <paramref name="typeInterface"/> arayüzünü uygulayan veya <paramref name="typeBaseclass"/> sınıfından türeyen tüm sınıfları tarar ve bunları bağımlılık enjeksiyonuna Scoped yaşam süresi ile ekler. Bu yöntem, belirli bir assembly içinde yer alan servis sınıflarını otomatik olarak tespit edip kaydetmek için kullanışlıdır, böylece her bir servis için manuel olarak AddScoped tanımı yapmaya gerek kalmaz.</summary>
        /// <param name="services">Bağımlılık enjeksiyon konteyneri</param>
        /// <param name="assembly">Tarama yapılacak assembly</param>
        /// <param name="typeInterface">Uygulanacak arayüz tipi</param>
        /// <param name="typeBaseclass">Türeyecek temel sınıf tipi</param>
        /// <returns>Güncellenmiş IServiceCollection nesnesi</returns>
        public static IServiceCollection AddScopedRange(this IServiceCollection services, Assembly assembly, Type typeInterface, Type typeBaseclass)
        {
            Guard.ThrowIfNull(services, nameof(services));
            Guard.ThrowIfNull(assembly, nameof(assembly));
            Guard.ThrowIfNull(typeInterface, nameof(typeInterface));
            Guard.ThrowIfNull(typeBaseclass, nameof(typeBaseclass));
            Type[] interfaces, types = assembly.GetTypes().Where(x => !x.IsAbstract && !x.IsInterface && x.IsSubclassOfOpenGeneric(typeBaseclass)).ToArray();
            var isOpenGeneric = typeInterface.IsGenericTypeDefinition;
            foreach (var implementation in types)
            {
                if (isOpenGeneric) { interfaces = implementation.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeInterface).ToArray(); }
                else { interfaces = implementation.GetInterfaces().Where(x => x != typeInterface && typeInterface.IsAssignableFrom(x)).ToArray(); }
                foreach (var service in interfaces) { services.AddScoped(service, implementation); }
            }
            return services;
        }
        /// <summary>Verilen assembly içerisinde bulunan ve <see cref="IBaseServiceInfrastructure{TContext, TEntity}"/> arayüzünü uygulayan veya <see cref="BaseServiceInfrastructure{TContext, TEntity}"/> sınıfından türeyen tüm servis sınıflarını otomatik olarak tarar ve bağımlılık enjeksiyonuna Scoped yaşam süresi ile ekler. Bu sayede her servis için manuel olarak AddScoped tanımı yapmaya gerek kalmaz.</summary>
        public static IServiceCollection AddScopedRangeBaseServiceInfrastructure(this IServiceCollection services, Assembly assembly)
        {
            services.AddScopedRange(assembly, typeof(IBaseServiceInfrastructure<,>), typeof(BaseServiceInfrastructure<,>));
            return services;
        }
        /// <summary>AutoMapper ve MediatR kütüphanelerini aynı anda yapılandırır ve servis koleksiyonuna ekler.</summary>
        /// <typeparam name="TProfile">Kullanılacak AutoMapper Profile sınıfı (<see cref="Profile"/>&#39;dan kalıtım almalıdır)</typeparam>
        /// <typeparam name="TProgram">MediatR handler&#39;larının taranacağı assembly&#39;yi belirlemek için kullanılan herhangi bir sınıf (genellikle Program sınıfı)</typeparam>
        /// <param name="services">Servis koleksiyonu</param>
        /// <param name="licenseKey">AutoMapper ve MediatR için lisans anahtarı (opsiyonel)</param>
        /// <returns>IServiceCollection (fluent kullanım için)</returns>
        public static IServiceCollection AddAutoMapperAndMediatR<TProfile, TProgram>(this IServiceCollection services, string? licenseKey = null) where TProfile : Profile, new() where TProgram : class
        {
            licenseKey = licenseKey.ToStringOrEmpty();
            services.AddAutoMapper(options =>
            {
                if (licenseKey != "") { options.LicenseKey = licenseKey; }
                options.AddProfile<TProfile>();
            });
            services.AddMediatR(options =>
            {
                if (licenseKey != "") { options.LicenseKey = licenseKey; }
                options.RegisterServicesFromAssemblyContaining<TProgram>();
            });
            return services;
        }
    }
}