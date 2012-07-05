using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace FirePuckStore.Controllers
{
    public class InjectControllerFactory : DefaultControllerFactory
    {
        private readonly IUnityContainer _container;

        public InjectControllerFactory(IUnityContainer container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext,
                                                             Type controllerType)
        {
            return  _container.BuildUp(base.GetControllerInstance(requestContext, controllerType));
        }
    }
}