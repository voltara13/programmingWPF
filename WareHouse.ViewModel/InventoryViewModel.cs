using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using WareHouse.Model;

namespace WareHouse.ViewModel
{
    /// <summary>
    /// Модель-представление позиции инвенторя
    /// </summary>
    [Serializable]
    public class InventoryViewModel : INotifyPropertyChanged
    {
        protected readonly InventoryModel _model;

        [field: NonSerialized()]
        public event PropertyChangedEventHandler? PropertyChanged;

        public InventoryViewModel(InventoryViewModel inventoryViewModel)
        {
            _model = new InventoryModel(inventoryViewModel._model);
        }

        protected InventoryViewModel(InventoryModel model)
        {
            _model = model;
        }

        public InventoryModel Model => _model;

        public string BarCode
        {
            get => _model.BarCode;
            set
            {
                _model.BarCode = value;
                OnPropertyChanged("BarCode");
            }
        }
        public string Name
        {
            get => _model.Name;
            set
            {
                _model.Name = value;
                OnPropertyChanged("Name");
            }
        }
        public int Count
        {
            get => _model.Count;
            set
            {
                _model.Count = value;
                OnPropertyChanged("Count");
            }
        }
        public double Cost
        {
            get => _model.Cost;
            set
            {
                _model.Cost = value;
                OnPropertyChanged("Cost");
            }
        }
        public string CostString => Math.Round(_model.Cost, 2).ToString();

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
