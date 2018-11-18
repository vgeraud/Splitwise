using Splitwise.Controllers;
using Splitwise.Data.Infrastracture;
using Splitwise.Data.Infrastructure;
using Splitwise.Data.Repositories;
using Splitwise.Model.Validators;
using Splitwise.Models;
using Splitwise.Service;
using System;

using Unity;
using Unity.Injection;

namespace Splitwise
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();

            container.RegisterType<ExpenseController>();
            container.RegisterType<UserController>();
            container.RegisterType<GroupsController>();

            container.RegisterType<IDbFactory, DbFactory>();

            var factoryInjection = new InjectionConstructor(container.Resolve<IDbFactory>());

            container.RegisterType<IUnitOfWork, UnitOfWork>(factoryInjection);
            var unitOfWorkInjection = new InjectionConstructor(container.Resolve<IUnitOfWork>());

            container.RegisterType<IExpenseRepository, ExpenseRepository>(factoryInjection);
            container.RegisterType<IExpenseService, ExpenseService>(new InjectionConstructor(new object[] { container.Resolve<IExpenseRepository>(), container.Resolve<IUnitOfWork>() }));

            container.RegisterType<IUserRepository, UserRepository>(factoryInjection);
            container.RegisterType<IUserService, UserService>(new InjectionConstructor(new object[] { container.Resolve<IUserRepository>(), container.Resolve<IUnitOfWork>() }));

            container.RegisterType<IGroupRepository, GroupRepository>(factoryInjection);
            container.RegisterType<IValidator<Group>, GroupValidator>();
            container.RegisterType<IGroupService, GroupService>(new InjectionConstructor(new object[] { container.Resolve<IGroupRepository>(), container.Resolve<IUnitOfWork>(), container.Resolve<IValidator<Group>>() }));
        }
    }
}