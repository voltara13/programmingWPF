using System;

using WareHouse.Model;

namespace WareHouse.ViewModel
{
    /// <summary>
    /// Модель-представление позиции покупки
    /// </summary>
    [Serializable]
    public class ProcurementViewModel : SaleProcurementViewModel
    {
        public ProcurementViewModel() : base(new ProcurementModel()) {}
        public override double ChangeBalance => - Cost * Count;
    }
}
