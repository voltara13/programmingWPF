using System;

using WareHouse.Model;

namespace WareHouse.ViewModel
{
    /// <summary>
    /// Модель-представление позиции продажи
    /// </summary>
    [Serializable]
    public class SaleViewModel : SaleProcurementViewModel
    {
        public SaleViewModel() : base(new SaleModel()) { }
        public override double ChangeBalance => Cost * Count;
    }
}
