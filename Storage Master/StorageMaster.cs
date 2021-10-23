using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storage_Master
{
    public class StorageMaster
    {
        private List<Product> products;
        private List<Storage> storages;
        private List<Vehicle> vehicles;

        private int currentStorageIndex;
        private int currentGarageSlot;

        public StorageMaster()
        {
            this.products = new List<Product>();
            this.storages = new List<Storage>();
            this.vehicles = new List<Vehicle>();

            this.currentStorageIndex = -1;
            this.currentGarageSlot = -1;
        }


        public string AddProduct(string type, double price)
        {
            switch (type)
            {
                case "Gpu":
                    products.Add(new Gpu(price));
                    break;
                case "HardDrive":
                    products.Add(new HardDrive(price));
                    break;
                case "Ram":
                    products.Add(new Ram(price));
                    break;
                case "SolidtateDrive":
                    products.Add(new SolidStateDrive(price));
                    break;

                default:
                    throw new InvalidOperationException("Invalid product type!");


            }
            return $"Added {type} to pool";
        }

        public string RegisterStorage(string type, string name)
        {
            switch (type)
            {
                case "AutomatedWarehouse":
                    this.storages.Add(new AutomatedWarehouse(name));
                    break;
                case "DistributionCenter":
                    this.storages.Add(new DistributionCenter(name));
                    break;
                case "Warehouse":
                    this.storages.Add(new Warehouse(name));
                    break;

                default:
                    throw new InvalidOperationException("Invalid storage tyoe!");
            }

            return $"Registered {name}";
        }

        public string SelectVehicle(string storageName, int garageSlot)
        {
            Storage storage = this.storages.First(s => s.Name == storageName);
            Vehicle vehicle = storage.GetVehicle(garageSlot);
            this.currentGarageSlot = garageSlot;
            this.currentStorageIndex = this.storages.IndexOf(storage);
            return $"Selected {vehicle.GetType().Name}";

        }

        public string LoadVehicle(IEnumerable<string> productNames)
        {
            Vehicle vehicle = this.storages[currentStorageIndex]
                .Garage.ToList()[currentGarageSlot];

            int count = 0;
            Product last;
            foreach (var name in productNames)
            {
                switch (name)
                {
                    case "Gpu":
                        if (this.products.Count(p => p is Gpu) == 0)
                        {
                            throw new InvalidOperationException($"{ name } is out of stock!");
                        }
                        else
                        {
                            last = this.products.Last(p => p is Gpu);
                        }
                        break;
                    case "HardDrive":
                        if (this.products.Count(p => p is HardDrive) == 0)
                        {
                            throw new InvalidOperationException($"{ name } is out of stock!");
                        }
                        else
                        {
                            last = this.products.Last(p => p is HardDrive);
                        }
                        break;
                    case "Ram":
                        if (this.products.Count(p => p is Ram) == 0)
                        {
                            throw new InvalidOperationException($"{ name } is out of stock!");
                        }
                        else
                        {
                            last = this.products.Last(p => p is Ram);
                        }
                        break;
                    case "SolidStateDrive":
                        if (this.products.Count(p => p is SolidStateDrive) == 0)
                        {
                            throw new InvalidOperationException($"{ name } is out of stock!");
                        }
                        else
                        {
                            last = this.products.Last(p => p is SolidStateDrive);
                        }
                        break;
                    default:
                        throw new InvalidOperationException($"{ name } is out of stock!");

                }
                if (vehicle.IsFull)
                {
                    break;
                }
                else
                {
                    vehicle.LoadProduct(last);
                    this.products.Remove(last);
                    count++;
                }

            }
            return $"Loaded {count}/{productNames.Count()} products into {vehicle.GetType().Name}";
        }


        public string SendVehicleTo(string sourceName, int sourceGarageSlot, string destinationName)
        {
            if (this.storages.Count(s => s.Name == sourceName) == 0)
            {
                throw new InvalidOperationException("Invalid source storage!");
            }
            else if (this.storages.Count(s => s.Name == destinationName) == 0)
            {
                throw new InvalidOperationException("Invalid destination storage!");
            }
            Vehicle vehicle = this.storages.First(s => s.Name == sourceName).Garage.ToList()[sourceGarageSlot];
            int destinationGarageSlot = this.storages.First(s => s.Name == sourceName).SendVehicleTo(sourceGarageSlot,
                this.storages.First(s => s.Name == destinationName));
            return $"Sent {vehicle.GetType().Name} to {destinationName} (slot {destinationGarageSlot})";
        }

        public string UnloadVehicle(string storageName, int garageSlot)
        {
            Storage storage = this.storages.First(s => s.Name == storageName);
            Vehicle vehicle = storage.GetVehicle(garageSlot);
            int countProductVehicle = vehicle.Trunk.Count;
            int unloadedProductCount = storage.UnloadVehicle(garageSlot);
            return $"Unloaded {unloadedProductCount}{countProductVehicle} products at {storageName}";
        }

        public string GetStorageStatus(string storageName)
        {
            Storage storage = this.storages.First(s => s.Name == storageName);
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            foreach (var product in storage.Products)
            {
                string productType = product.GetType().Name;
                if (dictionary.ContainsKey(productType))
                {
                    dictionary[productType]++;
                }
                else
                {
                    dictionary.Add(productType, 1);
                }
            }
            dictionary = dictionary
                .OrderByDescending(item => item.Value)
                .ThenBy(item => item.Key)
                .ToDictionary(item => item.Key, item => item.Value);

            string vehicleNames = string.Empty;
            foreach (var item in storage.Garage)
            {
                if (item is null)
                {
                    vehicleNames += "empty|";
                }
                else
                {
                    vehicleNames += item.GetType().Name + "|";
                }

            }
            double weighAll = storage.Products.Sum(p => p.Weight);
            string productInfo = string.Empty;
            foreach (var item in dictionary)
            {
                productInfo += item.Key + "(" + item.Value + ")";
            }
            string result = $"Stock ({weighAll}/{storage.Capacity}) :[{productInfo}]";
            result += "\n";
            result += $"Garage: [{vehicleNames}]";
            return result;
        }

        public string GetSummary()
        {
            Dictionary<string, double> dictionary = new Dictionary<string, double>();

            foreach (var item in storages)
            {
                dictionary.Add(item.Name, item.Products.Sum(p => p.Price));
            }
            dictionary = dictionary.OrderByDescending(x => x.Value)
                .ToDictionary(x => x.Key, x => x.Value);
            string result = string.Empty;
            foreach (var item in dictionary)
            {
                result += item.Key + $":\nStorage worth: ${item.Value:F2}\n";
            }
            return result;
        }

    }   
}




