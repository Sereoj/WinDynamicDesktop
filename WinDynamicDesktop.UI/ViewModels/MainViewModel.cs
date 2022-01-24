﻿using ModernWpf.Controls;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class MainViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        private ObservableCollection<NavigationViewItem> brands = new ObservableCollection<NavigationViewItem>();
        public ObservableCollection<NavigationViewItem> Brands
        {
            get { return brands; }
            set { SetProperty(ref brands, value); }
        }

        private ObservableCollection<NavigationViewItem> categories = new ObservableCollection<NavigationViewItem>();
        public ObservableCollection<NavigationViewItem> Categories
        {
            get { return categories; }
            set { SetProperty(ref categories, value); }
        }

        private int footerHeight = 0;
        public int FooterHeight
        {
            get { return footerHeight; }
            set { SetProperty(ref footerHeight, value); }
        }

        private string text;
        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value); }
        }
        public DelegateCommand<NavigationViewItemInvokedEventArgs> MenuItemInvokedCommand { get; set; }
        public MainViewModel()
        {
            MenuItemInvokedCommand = new DelegateCommand<NavigationViewItemInvokedEventArgs>(OnMenuItemInvoked);
        }
        public MainViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            LoadBrands();
            LoadCategory();

            MenuItemInvokedCommand = new DelegateCommand<NavigationViewItemInvokedEventArgs>(OnMenuItemInvoked);
        }

        private void OnMenuItemInvoked(NavigationViewItemInvokedEventArgs e)
        {
            switch (e.InvokedItemContainer.Tag.ToString())
            {
                case "Library":
                    regionManager.RequestNavigate("PageRegion", "Wallpapers");
                    break;
                case "New":
                    regionManager.RequestNavigate("PageRegion", "WallpapersNew");
                    break;
                case "Popular":
                    regionManager.RequestNavigate("PageRegion", "WallpapersPopular");
                    break;
                case "Wait":
                    regionManager.RequestNavigate("PageRegion", "WallpapersWait");
                    break;
                case "Install":
                    regionManager.RequestNavigate("PageRegion", "InstalledWallpapers");
                    break;
                case "Favorite":
                    regionManager.RequestNavigate("PageRegion", "FavoriteWallpapers");
                    break;
                case "Load":
                    regionManager.RequestNavigate("PageRegion", "LoadWallpapers");
                    break;
                case "Profile":
                    regionManager.RequestNavigate("PageRegion", "Profile");
                    break;
                case "Account":
                    regionManager.RequestNavigate("PageRegion", "Account");
                    break;
                default:
                    var param = new NavigationParameters
                    {
                        { "Root", e.InvokedItemContainer.Tag.ToString() },
                        { "Page", e.InvokedItemContainer.Name.ToString() },
                        { "ID", e.InvokedItemContainer.Uid.ToString() }
                    };

                    if (e.IsSettingsInvoked)
                    {
                        regionManager.RequestNavigate("PageRegion", "Settings", param);
                    }
                    else
                    {
                        regionManager.RequestNavigate("PageRegion", "Wallpapers", param);
                    }
                    break;
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            regionManager.RequestNavigate("PageRegion", "Wallpapers");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
        public async void LoadBrands()
        {
            var font = new FontFamily(new Uri(App.Current.Resources["Fonts"].ToString()), "#IcoMoon-Free");
            try
            {
                var items = await BrandsService.GetBrandAsync(null);

                foreach (var item in items)
                {
                    Brands.Add(new NavigationViewItem()
                    {
                        Uid = item.ID,
                        Content = item.Name,
                        Name = item.Tag.ToLower(),
                        Icon = new FontIcon() { FontFamily = font, Glyph = item.Icon.ToString() },
                        Tag = "brand"
                    });
                }
            }
            catch (Exception ex)
            {
                var param = new NavigationParameters
                {
                    { "Text", ex.Message }
                };

                regionManager.RequestNavigate("PageRegion", "NotFound", param);
            }
        }
        public async void LoadCategory()
        {
            var font = (FontFamily)App.Current.Resources["FontIconMoonFree"];

            try
            {
                var items = await CategoriesService.GetCategoryAsync(null);

                foreach (var item in items)
                {
                    Categories.Add(new NavigationViewItem()
                    {
                        Uid = item.ID,
                        Content = item.Name,
                        Name = item.Tag.ToLower(),
                        Icon = new FontIcon() { FontFamily = font, Glyph = item.Icon.ToString() },
                        Tag = "category"
                    });
                }
            }
            catch (Exception ex)
            {
                var param = new NavigationParameters
                {
                    { "Text", ex.Message }
                };

                regionManager.RequestNavigate("PageRegion", "NotFound", param);
            }
        }
    }
}
