using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WareHouse.ViewModel
{
    /// <summary>
    /// Модель-представление главного окна
    /// </summary>
    [Serializable]
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private double _balance;
        private SaleProcurementViewModel _selectedPosition;
        private readonly ObservableCollection<InventoryViewModel> _inventoryList;
        private readonly ObservableCollection<SaleProcurementViewModel> _saleList;
        private readonly ObservableCollection<SaleProcurementViewModel> _procurementList;
        private readonly ObservableCollection<TransactionViewModel> _transactionList;

        [field: NonSerialized()]
        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindowViewModel(double balance)
        {
            _balance = balance;
            _inventoryList = new ObservableCollection<InventoryViewModel>();
            _saleList = new ObservableCollection<SaleProcurementViewModel>();
            _procurementList = new ObservableCollection<SaleProcurementViewModel>();
            _transactionList = new ObservableCollection<TransactionViewModel>();

            AttachHandlers();
        }

        public ObservableCollection<InventoryViewModel> Inventory => _inventoryList;
        public ObservableCollection<TransactionViewModel> Transaction => _transactionList;
        public ObservableCollection<SaleProcurementViewModel> Sale => _saleList;
        public ObservableCollection<SaleProcurementViewModel> Procurement => _procurementList;
        public int CountDoneProcurement => _procurementList.Count(x => x.Status == "Завершён");
        public int CountDoneSale => _saleList.Count(x => x.Status == "Завершён");
        public int CountExpectationProcurement => _procurementList.Count(x => x.Status == "Ожидание");
        public int CountExpectationSale => _saleList.Count(x => x.Status == "Ожидание");
        public int CountOverdueProcurement => _procurementList.Count(x => x.DateDue.Date < DateTime.Now.Date);
        public int CountOverdueSale => _saleList.Count(x => x.DateDue.Date < DateTime.Now.Date);
        public int CountOrder => _transactionList.Count;
        public int CountInventory => _inventoryList.Count;
        public string BalanceString => _balance + " руб.";
        public bool CanOpenSalePosition => _inventoryList.Any();
        public bool CanCloseOrCancelProcurementPosition => _selectedPosition is ProcurementViewModel && _selectedPosition.Status == "Ожидание";

        public bool CanCloseOrCancelSalePosition => _selectedPosition is SaleViewModel && _selectedPosition.Status == "Ожидание";

        public double Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                OnPropertyChanged("BalanceString");
            }
        }
        public SaleProcurementViewModel SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                _selectedPosition = value;
                OnPropertyChanged("CanCloseOrCancelProcurementPosition");
                OnPropertyChanged("CanCloseOrCancelSalePosition");
            }
        }

        private void AttachHandlers()
        {
            _inventoryList.CollectionChanged += items_CollectionChanged;
            _saleList.CollectionChanged += items_CollectionChanged;
            _procurementList.CollectionChanged += items_CollectionChanged;
            _transactionList.CollectionChanged += items_CollectionChanged;
        }

        private void items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RefreshLabels();
        }

        private void RefreshLabels()
        {
            OnPropertyChanged("CountDoneProcurement");
            OnPropertyChanged("CountDoneSale");
            OnPropertyChanged("CountExpectationProcurement");
            OnPropertyChanged("CountExpectationSale");
            OnPropertyChanged("CountOverdueProcurement");
            OnPropertyChanged("CountOverdueSale");
            OnPropertyChanged("CountOrder");
            OnPropertyChanged("CountInventory");
            OnPropertyChanged("CountOverdueProcurement");
            OnPropertyChanged("CanOpenSalePosition");
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void AddProcurement(ProcurementViewModel procurementPosition)
        {
            AddPosition(procurementPosition, _procurementList);
        }

        public void CloseProcurement()
        {
            var inventoryPositionBarCode = _inventoryList.FirstOrDefault(x => x.BarCode == _selectedPosition.BarCode);
            var inventoryPositionName = _inventoryList.FirstOrDefault(x => x.Name == _selectedPosition.Name);

            if (Balance - _selectedPosition.Cost * _selectedPosition.Count < 0)
            {
                MessageBox.Show("Недостаточно средств на счёте.",
                    "Невозможно исполнить сделку",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else if (inventoryPositionBarCode != inventoryPositionName)
            {
                MessageBox.Show("Произошёл конфликт наименований или штрихкодов.",
                    "Невозможно исполнить сделку",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                if (inventoryPositionBarCode != null)
                {
                    inventoryPositionBarCode.Cost =
                        (inventoryPositionBarCode.Cost *
                         inventoryPositionBarCode.Count +
                         _selectedPosition.Cost *
                         _selectedPosition.Count) /
                        (inventoryPositionBarCode.Count +
                         _selectedPosition.Count);

                    inventoryPositionBarCode.Count += _selectedPosition.Count;
                }
                else
                {
                    _inventoryList.Add(new InventoryViewModel(_selectedPosition));
                }

                ClosePosition(_procurementList);
            }
        }

        public void CancelProcurement()
        {
            CancelPosition(_procurementList);
        }

        public void AddSale(SaleViewModel salePosition)
        {
            AddPosition(salePosition, _saleList);
        }

        public void CloseSale()
        {
            var inventoryPosition = _inventoryList.FirstOrDefault(x => x.Name == _selectedPosition.Name);
            
            if (inventoryPosition == null ||
                inventoryPosition.Count - _selectedPosition.Count < 0)
            {
                MessageBox.Show("Недостаточно товара на складе.",
                    "Невозможно исполнить сделку",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                _inventoryList.First(x => x.Name == _selectedPosition.Name).Count -= _selectedPosition.Count;

                if (inventoryPosition.Count == 0)
                {
                    _inventoryList.Remove(inventoryPosition);
                }

                ClosePosition(_saleList);
            }
        }

        public void CancelSale()
        {
            CancelPosition(_saleList);
        }

        private void AddPosition(SaleProcurementViewModel saleProcurementViewModel, ObservableCollection<SaleProcurementViewModel> saleProcurementViewList)
        {
            saleProcurementViewList.Add(saleProcurementViewModel);
            _transactionList.Add(new TransactionViewModel(saleProcurementViewModel));

            SelectedPosition = null;

            RefreshLabels();
        }

        private void ClosePosition(ObservableCollection<SaleProcurementViewModel> saleProcurementViewList)
        {
            var index = saleProcurementViewList.IndexOf(_selectedPosition);
            saleProcurementViewList[index].Status = "Завершён";
            _transactionList.First(x => x.TransactionNum == _selectedPosition.TransactionNum).Status = "Завершён";

            Balance += _selectedPosition.ChangeBalance;

            SelectedPosition = null;

            RefreshLabels();
        }

        private void CancelPosition(ObservableCollection<SaleProcurementViewModel> saleProcurementViewList)
        {
            var index = saleProcurementViewList.IndexOf(_selectedPosition);
            saleProcurementViewList[index].Status = "Отменён";
            _transactionList.First(x => x.TransactionNum == saleProcurementViewList[index].TransactionNum).Status = "Отменён";

            SelectedPosition = null;

            RefreshLabels();
        }
    }
}
