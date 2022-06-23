﻿using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Wallone.Authorization.Controls;
using Wallone.Authorization.ViewModels;
using Wallone.Authorization.Views;

namespace Wallone.Authorization
{
    public class AuthorizationModule : IModule
    {
        private readonly IRegionManager regionManager;

        public AuthorizationModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Load, LoadViewModel>();
            containerRegistry.RegisterForNavigation<MessageControl, MessageViewModel>();
            containerRegistry.RegisterForNavigation<Register, RegisterViewModel>();
            containerRegistry.RegisterForNavigation<Login, LoginViewModel>();
            containerRegistry.RegisterForNavigation<Confirm, ConfirmViewModel>();
            containerRegistry.RegisterForNavigation<Photo, PhotoViewModel>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            regionManager.RequestNavigate("ContentRegion", "Main");
        }
    }
}