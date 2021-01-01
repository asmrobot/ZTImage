using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace ZTImage.DbLite
{
    /// <summary>
    /// 依赖注入扩展
    /// </summary>
    public static class ServiceCollectionExtensionForDbLite
    {
        /// <summary>
        /// 添加DbConnectionFactory服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="initOptionAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddDBLite(this IServiceCollection services,Action<DbConnectionFactoryBuilder> initOptionAction)
        {
            if (initOptionAction == null)
            {
                throw new ArgumentNullException("initOptionAction");
            }
            var builder = new DbConnectionFactoryBuilder();
            initOptionAction(builder);
            services.AddSingleton<DbConnectionFactory>((provider) => {
                return builder.Build();
            });
            return services;
        }

        /// <summary>
        /// 添加DbConnectionFactory服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddDBLite(this IServiceCollection services,IEnumerable<DbConnectionOptions> options)
        {
            if (options == null || options.Count()<=0)
            {
                throw new ArgumentNullException("options");
            }

            return AddDBLite(services, (build) => {
                foreach (var item in options)
                {
                    build.AddDbConnectionOption(item);   
                }
            });
        }

        /// <summary>
        /// 添加数据库
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddDBLite(this IServiceCollection services, IConfiguration config)
        {
            var options=config.Get<IEnumerable<DbConnectionOptions>>();
            return AddDBLite(services,options);
        }

    }
}
