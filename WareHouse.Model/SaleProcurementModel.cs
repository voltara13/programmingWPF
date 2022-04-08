using System;
using System.Linq;

namespace WareHouse.Model
{
    [Serializable]
    public enum StatusEnum
    {
        Done,
        Expectation,
        Canceled
    }
    /// <summary>
    /// Модель позиций продажи и покупки
    /// </summary>
    [Serializable]
    public abstract class SaleProcurementModel : InventoryModel
    {
        public string TransactionNum { get; } = string.Concat(DateTime.Now.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss.fffffffK").Where(char.IsDigit));
        public DateTime DateDue { get; set; } = DateTime.Now;
        public string Organization { get; set; }
        public string Note { get; set; }
        public StatusEnum Status { get; set; } = StatusEnum.Expectation;
    }
}
