using BL;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MVVM_Core;
using System.Threading.Tasks;
using DAL;
using System;
using Main.Services;
using DAL.Models;
using WpfControlLibrary1;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Windows.Media.Imaging;

namespace Main.ViewModels
{
    [Singleton]
    public class MainViewModel : BaseSliderViewModel
    {
        private readonly PageService pageService;
        private readonly DbContextLoader contextLoader;
        private readonly ClientPipeHanlder pipeHanlder;
        private readonly EventBus eventBus;
        private readonly ValuteGetterService valuteGetter;
        private readonly SplashScreenService splashScreenService;
        private readonly SourceService sourceService;
        private readonly UpdateHandlerService handlerService;
        private readonly IConfiguration configuration;


        public string Message { get; set; }
        public string MessageDetail { get; set; }

        public MainViewModel(PageService pageService, 
            DbContextLoader contextLoader, 
            ClientPipeHanlder pipeHanlder, 
            EventBus eventBus,
            ValuteGetterService valuteGetter,
            SplashScreenService splashScreenService,
            SourceService sourceService,
            Services.UpdateHandlerService handlerService, IConfiguration configuration)
        {
            this.pageService = pageService;
            this.contextLoader = contextLoader;
            this.pipeHanlder = pipeHanlder;
            this.eventBus = eventBus;
            this.valuteGetter = valuteGetter;
            this.splashScreenService = splashScreenService;
            this.sourceService = sourceService;
            this.handlerService = handlerService;
            this.configuration = configuration;
            pageService.PageChanged += PageService_PageChanged;

            splashScreenService.OverlapScreen += SplashScreenService_OverlapScreen;
            splashScreenService.ShowPromtBtn += SplashScreenService_ShowPromtBtn;
            splashScreenService.ClearScreen += SplashScreenService_ClearScreen;


            Init();
        }

        private void SplashScreenService_ClearScreen()
        {
            ClearScreen();
        }

        public ICommand ClearScreenCommand => new Command(x =>
        {
            ClearScreen();
        });

        private void ClearScreen()
        {
            IsLoadingAnimation = false;
            IsMessageVisible = false;
            IsSplashVisible = false;
            IsButtonVisible = false;
        }

        

        private void SplashScreenService_ShowPromtBtn(string obj)
        {
            IsSplashVisible = true;
            IsMessageVisible = true;
            IsButtonVisible = true;
            IsLoadingAnimation = false;
            Message = obj;
        }

        private void SplashScreenService_OverlapScreen(string obj)
        {
            IsSplashVisible = true;
            IsLoadingAnimation = true;
            IsMessageVisible = true;
            Message = obj;
        }

        private void ImagesCopy()
        {
            string path = configuration["DefaultImagePath"];

            if (!Directory.Exists(path))
            {
                BitmapImage[] bitmaps = new BitmapImage[]
                {
                    App.Current.MainWindow.FindResource("layner.jpg") as BitmapImage,
                    App.Current.MainWindow.FindResource("layner1.jpg") as BitmapImage,
                    App.Current.MainWindow.FindResource("layner2.jpg") as BitmapImage,
                    App.Current.MainWindow.FindResource("turImage1.jpg") as BitmapImage,
                    App.Current.MainWindow.FindResource("turImage2.jpg") as BitmapImage,
                    App.Current.MainWindow.FindResource("turImage3.jpg") as BitmapImage,
                    App.Current.MainWindow.FindResource("c1.jpg") as BitmapImage,
                    App.Current.MainWindow.FindResource("c2.jpg") as BitmapImage,
                    App.Current.MainWindow.FindResource("c3.jpg") as BitmapImage,
                    App.Current.MainWindow.FindResource("c4.jpg") as BitmapImage,
                };
                Directory.CreateDirectory(path);

                foreach (BitmapImage map in bitmaps)
                {
                    BitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(map));
                    string fileName = Path.GetFileName(map.UriSource.OriginalString);
                    using (FileStream fs = new FileStream(Path.Combine(path, fileName), FileMode.CreateNew, FileAccess.Write))
                        encoder.Save(fs);

                }
                
            }
        }

        private async Task ContractCopy()
        {
            if (!File.Exists(configuration["WordFile"]))
            {
                using (FileStream fs = new FileStream(configuration["WordFile"], FileMode.Create, FileAccess.Write))
                {
                    await fs.WriteAsync(CommonResources.ContractDocument, 0, CommonResources.ContractDocument.Length);
                }

            }
        }
        public string LoadingText { get; set; }

        public bool IsMessageVisible { get; set; }
        public bool IsButtonVisible { get; set; }
        public bool IsSplashVisible { get; set; }

        async void Init()
        {
            IsSplashVisible = true;
            IsLoadingAnimation = true;

            pipeHanlder.Init("CruisesPipe");
            pipeHanlder.UpdateCalled += PipeHanlder_UpdateCalled;

            ImagesCopy();
            await ContractCopy();
            
            IsLoaded = await contextLoader.LoadAsync<Layner>();
            await sourceService.ReloadAsync();

            IsLoadingAnimation = false;

            if (IsLoaded)
            {
                ClearScreen();
                pageService.ChangePage<Pages.HomePage>(Rules.Pages.MainPool, defaultAnim);
            }
            else
            {
                IsMessageVisible = true;
                Message = contextLoader.Message;
                MessageDetail = contextLoader.MessageDetail;
            }
            
        }

        private void PipeHanlder_UpdateCalled(string msg)
        {
            handlerService.Handle(msg);
        }

        public bool IsLoaded { get; set; }

        public bool IsLoadingAnimation { get; set; } = true;

        public int Width { get; set; } = 800;
        public override Page CurrentPage { get; set; }
    }
}
