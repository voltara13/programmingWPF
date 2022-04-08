using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using WareHouse.Model;

namespace WareHouse.ViewModel
{
    /// <summary>
    /// Модель-представление позиции транзакции
    /// </summary>
    [Serializable]
    public class TransactionViewModel : INotifyPropertyChanged
    {
        public readonly TransactionModel Model;

        [field: NonSerialized()]
        public event PropertyChangedEventHandler? PropertyChanged;

        public TransactionViewModel(SaleProcurementViewModel saleProcurementViewModel)
        {
            Model = new TransactionModel((SaleProcurementModel)saleProcurementViewModel.Model);
        }

        public string TransactionNum => Model.TransactionNum;
        public string BarCode => Model.BarCode;
        public string DateString => Model.Date.ToShortDateString();
        public string Organization => Model.Organization;
        public double Cost => Model.Cost;
        public string CostString => Math.Round(Model.Cost, 2).ToString();
        public string TransactionType
        {
            get
            {
                return Model.TransactionType switch
                {
                    WareHouse.Model.TransactionType.Procurement => "Отправка",
                    WareHouse.Model.TransactionType.Sale => "Получение",
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
        public string Status
        {
            get
            {
                return Model.Status switch
                {
                    StatusEnum.Expectation => "Ожидание",
                    StatusEnum.Canceled => "Отменён",
                    StatusEnum.Done => "Завершён",
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            set
            {
                Model.Status = value switch
                {
                    "Ожидание" => StatusEnum.Expectation,
                    "Отменён" => StatusEnum.Canceled,
                    "Завершён" => StatusEnum.Done,
                    _ => throw new ArgumentOutOfRangeException()
                };
                OnPropertyChanged("Status");
            }
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
