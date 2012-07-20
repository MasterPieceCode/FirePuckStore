using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FirePuckStore.BL.Services.Implementation;
using FirePuckStore.BL.Services.Interfaces;
using FirePuckStore.Controllers;
using FirePuckStore.DAL.Repositories.Implementation;
using FirePuckStore.DAL.Repositories.Interfaces;
using Microsoft.Practices.Unity;

namespace FirePuckStore.App_Start
{
    public class ContainerConfig
    {
        public static void Configure()
        {
            var unityContainer = new UnityContainer();

            unityContainer.RegisterType<HomeController>();
            unityContainer.RegisterType<CartController>();

            unityContainer.RegisterType(typeof(IFileService), typeof(FileService), new ContainerControlledLifetimeManager());

            unityContainer.RegisterType(typeof(ICardService), typeof(CardService), new PerResolveLifetimeManager());
            unityContainer.RegisterType(typeof(ICardRepository), typeof(CardRepository), new PerResolveLifetimeManager());

            unityContainer.RegisterType(typeof (ICartService), typeof (CartService), new ContainerControlledLifetimeManager());
            unityContainer.RegisterType(typeof (ICartRepository), typeof (CartRepository), new ContainerControlledLifetimeManager());

            ControllerBuilder.Current.SetControllerFactory(new InjectControllerFactory(unityContainer));
        }
    }
}