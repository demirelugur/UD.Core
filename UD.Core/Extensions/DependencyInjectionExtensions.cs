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
        /// <summary>Verilen assembly içerisinde bulunan ve <paramref name="typeInterface"/> arayüzünü uygulayan veya <paramref name="typeBaseclass"/> sınıfından türeyen tüm repository sınıflarını otomatik olarak tarar ve bağımlılık enjeksiyonuna Scoped yaşam süresi ile ekler. Bu sayede her repository için manuel olarak AddScoped tanımı yapmaya gerek kalmaz.</summary>
        /// <param name="services">Bağımlılık enjeksiyon konteyneri</param>
        /// <param name="assembly">Repository sınıflarının bulunduğu assembly</param>
        /// <param name="typeInterface">Repository arayüzünün tipi (örneğin, typeof(IRepository&lt;&gt;))</param>
        /// <param name="typeBaseclass">Repository> sınıflarının türediği temel sınıfın tipi (örneğin, typeof(BaseRepository&lt;&gt;))</param>
        /// <returns>Güncellenmiş IServiceCollection nesnesi</returns>
        public static IServiceCollection AddScopedRange(this IServiceCollection services, Assembly assembly, Type typeInterface, Type typeBaseclass)
        {
            Guard.ThrowIfNull(services, nameof(services));
            Guard.ThrowIfNull(assembly, nameof(assembly));
            Guard.ThrowIfNull(typeInterface, nameof(typeInterface));
            Guard.ThrowIfNull(typeBaseclass, nameof(typeBaseclass));
            var types = assembly.GetTypes().Where(x => !x.IsAbstract && !x.IsInterface && x.IsSubclassOfOpenGeneric(typeBaseclass)).ToArray();
            foreach (var implementation in types)
            {
                var interfaces = implementation.GetInterfaces().Where(x => x.IsImplementsOpenGenericInterface(typeInterface)).ToArray();
                foreach (var service in interfaces) { services.AddScoped(service, implementation); }
            }
            return services;
        }
        /// <summary>Verilen assembly içerisinde bulunan ve <see cref="IBaseInfrastructureService{TContext, TEntity}"/> arayüzünü uygulayan veya <see cref="BaseInfrastructureService{TContext, TEntity}"/> sınıfından türeyen tüm servis sınıflarını otomatik olarak tarar ve bağımlılık enjeksiyonuna Scoped yaşam süresi ile ekler. Bu sayede her servis için manuel olarak AddScoped tanımı yapmaya gerek kalmaz.</summary>
        public static IServiceCollection AddScopedRangeBaseInfrastructureService(this IServiceCollection services, Assembly assembly)
        {
            services.AddScopedRange(assembly, typeof(IBaseInfrastructureService<,>), typeof(BaseInfrastructureService<,>));
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