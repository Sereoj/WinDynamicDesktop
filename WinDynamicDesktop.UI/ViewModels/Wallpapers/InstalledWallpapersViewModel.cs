﻿using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WinDynamicDesktop.Core.Helpers;
using WinDynamicDesktop.Core.Services;
using WinDynamicDesktop.UI.Interfaces;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class InstalledWallpapersViewModel : BindableBase, INavigationAware, IPage
    {
        private readonly IRegionManager regionManager;

        private string header = "Установленные";
        public string Header { get => header; set => SetProperty(ref header, value); }

        public ObservableCollection<ArticleViewModel> Library { get; set; } = new ObservableCollection<ArticleViewModel>();

        public InstalledWallpapersViewModel()
        {

        }
        public InstalledWallpapersViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Loaded(null, null);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public async void Loaded(string page, List<Core.Models.Parameter> parameters)
        {
            Library.Clear();
            try
            {
                var items = await ThumbService.GetThumbsInstallAsync(page, parameters);

                if (ThumbService.CheckItems(items))
                {
                    foreach (var item in items)
                    {
                        Library.Add(new ArticleViewModel(regionManager)
                        {
                            ID = item.ID,
                            Name = item.Name,
                            ImageSource = new BitmapImage(UriHelper.Get(item.Preview))
                        });
                        await Task.CompletedTask;
                    }
                }
                else
                {
                    var param = new NavigationParameters
                    {
                        { "Text", "Это не ошибка, просто не найдены изображения!" }
                    };

                    regionManager.RequestNavigate("PageRegion", "NotFound", param);
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
