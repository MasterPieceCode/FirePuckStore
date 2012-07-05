using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FirePuckStore.BL.Services.Implementation;
using FirePuckStore.BL.Services.Interfaces;
using FirePuckStore.Controllers;
using Microsoft.Practices.Unity;

namespace FirePuckStore.App_Start
{
    public class ContainerConfig
    {
        public static void Configure()
        {
            var unityContainer = new UnityContainer();

            unityContainer.RegisterType(typeof(ICardService), typeof(CardService), new ContainerControlledLifetimeManager());
            /*unityContainer.RegisterType(typeof(ICartService), typeof(CartService), new ContainerControlledLifetimeManager());*/

             ControllerBuilder.Current.SetControllerFactory(new InjectControllerFactory(unityContainer));
        }
    }
}