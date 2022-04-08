using System;
using System.ComponentModel;

using WareHouse.Model;

namespace WareHouse.ViewModel
{
    /// <summary>
    /// Модель-представление позиций продажи и покупки
    /// </summary>
    [Serializable]
    public abstract class SaleProcurementViewModel : InventoryViewModel, IDataErrorInfo
    {
        protected SaleProcurementViewModel(SaleProcurementModel model) : base(model) {}

        public string TransactionNum => ((SaleProcurementModel)_model).TransactionNum;
        public DateTime DateDue
        {
            get => ((SaleProcurementModel)_model).DateDue;
            set
            {
                ((SaleProcurementModel)_model).DateDue = value;
                OnPropertyChanged("DateDue");
            }
        }
        public string Organization
        {
            get => ((SaleProcurementModel)_model).Organization;
            set
            {
                ((SaleProcurementModel)_model).Organization = value;
                OnPropertyChanged("Organization");
            }
        }
        public string Status
        {
            get
            {
                return ((SaleProcurementModel)_model).Status switch
                {
                    StatusEnum.Expectation => "Ожидание",
                    StatusEnum.Canceled => "Отменён",
                    StatusEnum.Done => "Завершён",
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            set
            {
                ((SaleProcurementModel)_model).Status = value switch
                {
                    "Ожидание" => StatusEnum.Expectation,
                    "Отменён" => StatusEnum.Canceled,
                    "Завершён" => StatusEnum.Done,
                    _ => throw new ArgumentOutOfRangeException()
                };
                OnPropertyChanged("Status");
            }
        }
        public string Note
        {
            get => ((SaleProcurementModel)_model).Note;
            set
            {
                ((SaleProcurementModel)_model).Note = value;
                OnPropertyChanged("Note");
            }
        }
        public string DateDueString => ((SaleProcurementModel)_model).DateDue.ToShortDateString();
        public string Error => throw new NotImplementedException();
        public abstract double ChangeBalance { get; }

        public string this[string columnName]
        {
            get
            {
                string result = null;

                if (columnName == "BarCode")
                {
                    if (string.IsNullOrEmpty(BarCode))
                        result = "Поле 'Штрихкод' не может быть пустым";
                }
                if (columnName == "DateDue")
                {
                    if (DateDue.Date < DateTime.Now.Date)
                        result = "Поле 'Дата исполнения' не может быть некорректным";
                }
                if (columnName == "Organization")
                {
                    if (string.IsNullOrEmpty(Organization))
                        result = "Поле 'Организация' не может быть пустым";
                }
                if (columnName == "Name")
                {
                    if (string.IsNullOrEmpty(Name))
                        result = "Поле 'Наименование' не может быть пустым";
                }
                if (columnName == "Count")
                {
                    if (Count <= 0)
                        result = "Поле 'Количество' не может быть отрицательным или равным нулю";
                }
                if (columnName == "Cost")
                {
                    if (Cost < 0)
                        result = "Поле 'Цена' не может быть отрицательным";
                }

                return result;
            }
        }
    }
}