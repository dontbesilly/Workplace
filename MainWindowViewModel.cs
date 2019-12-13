using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace Workplace1c
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly WorkplaceContext db;

        public ObservableCollection<Base> Bases { get; set; }

        public MainWindowViewModel()
        {
            db = new WorkplaceContext();
            db.Database.EnsureCreated();

            db.Bases.Load();

            Bases = new ObservableCollection<Base>(db.Bases.Local);



            //Bases = new ObservableCollection<Base>
            //{
            //    new Base { Title = "Бп 3 выпуск" },
            //    new Base { Title = "Бп 3 разработка" }
            //};
        }

        #region Old

        public ObservableCollection<SelectableViewModel> Items3 { get; set; }
        private bool? _isAllItems3Selected;

        public bool? IsAllItems3Selected
        {
            get { return _isAllItems3Selected; }
            set
            {
                if (_isAllItems3Selected == value) return;

                _isAllItems3Selected = value;

                OnPropertyChanged();
            }
        }

        public IEnumerable<string> Foods
        {
            get
            {
                yield return "Burger";
                yield return "Fries";
                yield return "Shake";
                yield return "Lettuce";
            }
        }

        private static ObservableCollection<SelectableViewModel> CreateData()
        {
            return new ObservableCollection<SelectableViewModel>
            {
                new SelectableViewModel
                {
                    Code = 'M',
                    Name = "Material Design",
                    Description = "Material Design in XAML Toolkit"
                },
                new SelectableViewModel
                {
                    Code = 'D',
                    Name = "Dragablz",
                    Description = "Dragablz Tab Control",
                    Food = "Fries"
                },
                new SelectableViewModel
                {
                    Code = 'P',
                    Name = "Predator",
                    Description = "If it bleeds, we can kill it"
                }
            };
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class SelectableViewModel
    {
        public char Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Food { get; set; }
    }
}
