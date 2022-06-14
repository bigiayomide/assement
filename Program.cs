
using System.Data;

var warehouse = System.IO.File.ReadAllLines("warehouses.csv").Select(x => x.Split(','));
var warehousesku = System.IO.File.ReadAllLines("warehouses-with-skus.csv").Select(x => x.Split(','));
var warehousenumber = warehouse.FirstOrDefault();

int[] array1 = new int[] { 0 };
string skuSearch = "104755";
string state = "IN";
int totalsum = 0;

var data = warehousesku.FirstOrDefault(a => a[0].Equals(skuSearch))
            .Skip(1).Select((x, y) => new { Value = x, Index = y }).Where(x => x.Value.Trim() != "0");

if (data == null)
{
    Console.WriteLine("No item in stock");
    return;
}

int lineLength = warehouse.FirstOrDefault().Count();
var warehousecsv = warehouse.Skip(1)
           .SelectMany(x => x)
           .Select((v, i) => new { Value = v, Index = i % lineLength, warehouseNumber = warehousenumber[i % lineLength] })
           .Where(x => x.Value == state);

if (data?.Count() > 0)
{
    foreach (var item in warehousecsv)
    {
        var warehouseskuitem = data.Where(x => (x.Index + 1).ToString() == item.warehouseNumber).FirstOrDefault();
        if (warehouseskuitem != null)
        {
            int.TryParse(warehouseskuitem.Value, out int sku);
            totalsum += sku;
        }
    }
}

Console.WriteLine($"Total Items for state {state} is {totalsum} for sku number #{skuSearch}");


