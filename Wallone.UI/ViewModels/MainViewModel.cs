﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using ModernWpf.Controls;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Wallone.Core.Services;
using Wallone.UI.Services;

namespace Wallone.UI.ViewModels
{
    public class MainViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;

        public ManagerViewModel ManagerViewModel { get; }

        private ObservableCollection<NavigationViewItem> brands = new ObservableCollection<NavigationViewItem>();

        private ObservableCollection<NavigationViewItem> categories = new ObservableCollection<NavigationViewItem>();

        private int footerHeight;
        private string text = "This is text";

        public MainViewModel()
        {
        }

        public MainViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            LoadBrands();
            LoadCategory();
            ManagerViewModel = new ManagerViewModel(regionManager);

            MenuItemInvokedCommand = new DelegateCommand<NavigationViewItemInvokedEventArgs>(OnMenuItemInvoked);
        }

        public ObservableCollection<NavigationViewItem> Brands
        {
            get => brands;
            set => SetProperty(ref brands, value);
        }

        public ObservableCollection<NavigationViewItem> Categories
        {
            get => categories;
            set => SetProperty(ref categories, value);
        }

        public int FooterHeight
        {
            get => footerHeight;
            set => SetProperty(ref footerHeight, value);
        }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public DelegateCommand<NavigationViewItemInvokedEventArgs> MenuItemInvokedCommand { get; set; }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            navigationContext.NavigationService.Navigated += NavigationService_Navigated;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            GC.Collect(2);
        }

        private void NavigationService_Navigated(object sender, RegionNavigationEventArgs e)
        {
            var param = new NavigationParameters
            {
                {"Root", "Gallery"},
                {"Page", ""},
                {"ID", "Main"},
                {"Text", "Библиотека"}
            };

            regionManager.RequestNavigate("PageRegion", "Wallpapers", param);
        }

        private void OnMenuItemInvoked(NavigationViewItemInvokedEventArgs e)
        {
            var text = e.InvokedItemContainer.Content;

            var param = new NavigationParameters
            {
                {"Root", e.InvokedItemContainer.Tag.ToString()},
                {"Page", e.InvokedItemContainer.Name},
                {"ID", e.InvokedItemContainer.Uid},
                {"Text", text}
            };

            Trace.WriteLine(param);

            switch (e.InvokedItemContainer.Tag.ToString())
            {
                case "Downloads":
                    regionManager.RequestNavigate("PageRegion", "DownloadsPage", param);
                    break;
                case "Profile":
                    regionManager.RequestNavigate("PageRegion", "Profile", param);
                    break;
                case "Account":
                    regionManager.RequestNavigate("PageRegion", "Account", param);
                    break;
                default:

                    if (e.IsSettingsInvoked)
                        regionManager.RequestNavigate("PageRegion", "Settings", param);
                    else
                        regionManager.RequestNavigate("PageRegion", "Wallpapers", param);
                    break;
            }
        }

        public async void LoadBrands()
        {
            try
            {
                var items = await BrandsService.GetBrandAsync();

                foreach (var item in items.Where(item => item.Status))
                    Brands.Add(new NavigationViewItem
                    {
                        Uid = item.ID,
                        Content = item.Name,
                        Name = "Brands",
                        Icon = item.Icon != null ? FontIconService.SetIcon("ultimate", item.Icon) : null,
                        Tag = "Gallery"
                    });
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        public async void LoadCategory()
        {
            try
            {
                var items = await CategoriesService.GetCategoryAsync();

                foreach (var item in items.Where(item => item.Status))
                    Categories.Add(new NavigationViewItem
                    {
                        Uid = item.ID,
                        Content = item.Name,
                        Name = "Categories",
                        Icon = item.Icon != null ? FontIconService.SetIcon("ultimate", item.Icon) : null,
                        Tag = "Gallery"
                    });
            }
            catch (Exception ex)
            {
                // ignored
            }
        }
    }
}