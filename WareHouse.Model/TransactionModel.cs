using System;

namespace WareHouse.Model
{
    [Serializable]
    public enum TransactionType
    {
        Procurement,
        Sale
    }
    /// <summary>
    /// Модель позиции транзакции
    /// </summary>
    [Serializable]
    public class TransactionModel
    {
        public string TransactionNum { get; }
        public string BarCode { get; }
        public string Organization { get; }
        public TransactionType TransactionType { get; }
        public double Cost { get; set; }
        public DateTime Date { get; } = DateTime.Now;
        public StatusEnum Status { get; set; } = StatusEnum.Expectation;

        public TransactionModel(SaleProcurementModel saleProcurementModel)
        {
            TransactionNum = saleProcurementModel.TransactionNum;
            BarCode = saleProcurementModel.BarCode;
            Organization = saleProcurementModel.Organization;
            TransactionType = saleProcurementModel is SaleModel
                ? TransactionType.Sale
                : TransactionType.Procurement;
            Cost = saleProcurementModel.Cost * saleProcurementModel.Count;
        }
    }
}
