using BL;
using DAL.Dto;
using DAL.Models;
using Microsoft.Extensions.Configuration;
using MVVM_Core;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class PicturesPage : BasePageViewModel
    {
        public override int PoolIndex => Rules.Pages.MainPool;

        private dynamic selectedItem;
        private readonly SourceService sourceService;
        private readonly IConfiguration config;

        public string Header { get; set; }

        public ObservableCollection<LaynerDto> Images { get; set; }
        public PicturesPage(PageService pageservice, SourceService sourceService, IConfiguration config) : base(pageservice)
        {
            this.sourceService = sourceService;
            this.config = config;
            if (sourceService.IsLoaded)
            {
                Images = new ObservableCollection<LaynerDto>(sourceService.GetLayners(config["DefaultImagePath"]));
                Header = sourceService.Header;
            }
        }

        public bool IsCentralImageVis { get; set; }

        public ICommand Close => new Command(x =>
        {
            IsCentralImageVis = false;
        });

        public Layner SelectedItem 
        { 
            get => selectedItem; 
            set
            {
                IsCentralImageVis = value != null;
                selectedItem = value; 
            } 
        }

    }
}