using System;

namespace WareHouse.Model
{
    /// <summary>
    /// Модель позиции инвентаря
    /// </summary>
    [Serializable]
    public class InventoryModel
    {
        public InventoryModel() {}

        public InventoryModel(InventoryModel inventoryModel)
        {
            BarCode = inventoryModel.BarCode;
            Name = inventoryModel.Name;
            Count = inventoryModel.Count;
            Cost = inventoryModel.Cost;
        }
        public string BarCode { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public double Cost { get; set; }
    }
}
