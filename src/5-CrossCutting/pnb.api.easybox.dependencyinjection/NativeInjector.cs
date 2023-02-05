using Microsoft.Extensions.DependencyInjection;
using pnb.api.easybox.aplication.Services;
using pnb.api.easybox.domain.Interface;
using pnb.api.easybox.repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pnb.api.easybox.dependencyinjection
{
    public static class NativeInjector
    {
        public static void RegisterDependencies(this IServiceCollection services) 
        {
            #region Aplicacao
            services.AddSingleton<IPerfilUsuarioService, PerfilUsuarioService>();
            #endregion

            #region Infra
            services.AddSingleton<IPerfilUsuarioRepository,PerfilUsuarioRepository>();
            #endregion
        }
    }
}
