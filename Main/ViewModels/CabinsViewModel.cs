using BL;
using DAL.Dto;
using DAL.Models;
using Main.Components;
using Main.Services;
using Main.Windows;
using Microsoft.Extensions.Configuration;
using MVVM_Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Main.ViewModels
{
    public class CabinsViewModel: BaseViewModel
    {
        private readonly OrderService orderService;
        private readonly ToursService toursService;
        private readonly IConfiguration config;
        private readonly WindowsService windowsService;
        private readonly PlacementService placementService;
        private readonly PassengersService passengersService;
        private IEnumerable<Passenger> _passengers;
        private List<int> _busyComps;
        private List<int> _busy;

        int _decksCount;
        int _minDeck;

        int childs;
        int adults;

        public ICommand Close => new Command(x =>
        {
            ClosePopup();
        });

        

        public double ImgWidth { get; set; }
        public double ImgHegiht { get; set; }

        public int CurrentDeck { get; set; }

        public BitmapImage CurrentImage { get; set; }
        BitmapImage[] images;

        public ICommand Left => new Command(x =>
        {
            ReloadDeck(--CurrentDeck);
        }, y=> CurrentDeck > _minDeck);

        public ICommand Right => new Command(x =>
        {
            ReloadDeck(++CurrentDeck);
        }, y=> CurrentDeck < _decksCount);


        CabinsWindow _wind;


        public void SetupWindow(CabinsWindow window)
        {
            _wind = window;
            Init();
        }

        void ClosePopup()
        {
            SelectedCabinSchema = default;
            IsSelectedMode = false;
        }

        public bool IsSelectedMode { get; set; }



        public void ReloadDeck(int deck)
        {
            ClosePopup();
            CurrentImage = images[deck - 1];
            ImgHegiht = CurrentImage.Width;
            ImgWidth = CurrentImage.Height;
            SetupSchemasCabins(AllCabins.Where(x => x.Deck == deck).ToArray());
        }

        public ObservableCollection<PlacementDto> Placements { get; set; }


        public ObservableCollection<CabinDto> AllCabins { get; set; }
        public ObservableCollection<CabinDto> SpecialForCabins { get; set; }

        public ObservableCollection<CabinSchema> CabinSchemas { get; set; } = new ObservableCollection<CabinSchema>();

        public CabinsViewModel(OrderService orderService,
                               ToursService toursService,
                               IConfiguration config,
                               WindowsService windowsService,
                               PlacementService placementService,
                               PassengersService cabinsService)
        {
            this.orderService = orderService;
            this.toursService = toursService;
            this.config = config;
            this.windowsService = windowsService;
            this.placementService = placementService;
            this.passengersService = cabinsService;
        }

        private CabinSchema _selected;

        public CabinSchema SelectedCabinSchema
        {
            get => _selected;
            set
            {                
                _selected = value;
                SelectedCabin = SpecialForCabins.FirstOrDefault(x => x.Id == value.CabinNumber);
                OnPropertyChanged(nameof(SelectedCabinSchema));
                ShowPopup();
            }
        }

        void ShowPopup()
        {
            IsSelectedMode = true;

            if(SelectedCabinSchema.X + PopupWidth > WindowWidth)
            {
                OffsetX = SelectedCabinSchema.X - PopupWidth - 10;
            }
            else
            {
                OffsetX = SelectedCabinSchema.X + SelectedCabinSchema.Width + 10;
            }

        }

        public bool IsLoaded { get; set; }

        public double OffsetX { get; set; }

        public double PopupWidth { get; set; } = 320;
        public double WindowWidth { get; set; } = 1420;

        public CabinDto SelectedCabin { get; set; }

        private async void Init()
        {
            ReadFile();

            await placementService.ReloadAsync(orderService.TourId, config["DefaultImagePath"]);
            //await toursService.ReloadAsync(x => $"{config["DefaultImagePath"]}\\{x}");
            //orderService.SetupTour(1);

            _passengers = passengersService.GetPassengers();

            adults = _passengers?.Count(x => !x.IsChild) ?? 0;
            childs = _passengers?.Count(x => x.IsChild) ?? 0;

            int layner = toursService.GetTour(orderService.TourId).LaynerId;

            AllCabins = new ObservableCollection<CabinDto>(placementService.GetAllCabins());
            //Получили не занятые каюты


            _busyComps = passengersService.GetOtherPassengers().
                Where(x => x.IsCabinSelected).
                Select(x => x.SelectedCabin).
                ToList();

            _busy = placementService.GetBusyCabins();


            CurrentDeck = _minDeck = AllCabins.Min(x => x.Deck);
            _decksCount = AllCabins.Max(x => x.Deck);

            images = new BitmapImage[]
            {
                _wind.FindResource("d2") as BitmapImage,
                _wind.FindResource("d3") as BitmapImage,
                _wind.FindResource("d4") as BitmapImage,
                _wind.FindResource("d5") as BitmapImage,
            };

            ReloadDeck(CurrentDeck);
            IsLoaded = true;
        }

        Dictionary<string, IEnumerable<string>> _coords { get; set; } = new Dictionary<string, IEnumerable<string>>();


        void SetupSchemasCabins(CabinDto[] cabs)
        {
            string img = Path.GetFileName(CurrentImage.UriSource.OriginalString);

            CabinSchemas.Clear();

            var list = _coords[img].ToList();

            SpecialForCabins = new ObservableCollection<CabinDto>(
                cabs.Where(x => x.ChildCount == childs && x.AdultCount == adults));

            for (int i = 0; i < list.Count; i++)
            {
                var str = list[i];
                var cab = cabs[i];

                var sc = new CabinSchema(
                    str.Split(';'), cab.Id,
                    _busy.Contains(cab.Id), 
                    _busyComps.Contains(cab.Id), 
                    !SpecialForCabins.Any(c => c.Id == cab.Id));
                CabinSchemas.Add(sc);
            }


        }


        void ReadFile()
        {
            using (var str = new StreamReader("coords.txt"))
            {
                string line = str.ReadLine();
                

                while (line != null)
                {

                    var list = new List<string>();

                    string item;
                    while ((item = str.ReadLine()) != null && item.IndexOf('#') == -1)
                    {
                        list.Add(item);
                    }

                    _coords.Add(line.Substring(1), list);

                    line = item;
                }

            }
        }

        public ICommand SaveCoods => new Command(_ =>
        {
            //using (var str = new StreamWriter("coords.txt", true))
            //{
            //    str.WriteLine($"#{Path.GetFileName( (CurrentImage as BitmapImage).UriSource.OriginalString)}");
            //    foreach (FrameworkElement item in _wind.canv.Children)
            //    {
            //        double x = Canvas.GetLeft(item);
            //        double y = Canvas.GetTop(item);

            //        str.WriteLine($"{x};{y};{item.Width};{item.Height}");

            //    }
            //}
        });

        public ICommand SelectCommand => new Command(x =>
        {
            

            if (IsSelectedMode || x is CabinDto)
            {
                int id = IsSelectedMode ? SelectedCabinSchema.CabinNumber : (x as CabinDto).Id;

                foreach (var p in _passengers)
                {
                    p.IsCabinSelected = true;
                    p.SelectedCabin = id;
                }
                windowsService.CloseWindow();
            }
        });

    }




    public struct CabinSchema
    {
        public enum CabinType
        {
            Free, Busy, BusyComp, NonOverlap, 
        }

        public double X { get; }
        public double Y { get; }
        public double Width { get; }
        public double Heigth { get; }
        public int CabinNumber { get; }

        public CabinType Cabin { get; set; }

        public CabinSchema(string[] arr, int cabinNumber, bool isBusy, bool isBusyComps, bool isNonOverlap)
        {
            X = double.Parse(arr[0]);
            Y = double.Parse(arr[1]);
            Width = double.Parse(arr[2]);
            Heigth = double.Parse(arr[3]);

            Cabin = CabinType.Free;

            if (isBusy)
                Cabin = CabinType.Busy;
            else if (isBusyComps)
                Cabin = CabinType.BusyComp;
            else if (isNonOverlap)
                Cabin = CabinType.NonOverlap;


            CabinNumber = cabinNumber;
        }
    }
}
