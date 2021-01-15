using Grit.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.WebApi;

namespace Grit
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IOpenExerciseResponse, OpenExerciseResponse>();

            var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
            container.RegisterInstance(serviceProvider.GetService<IHttpClientFactory>());
     
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }


    }
}