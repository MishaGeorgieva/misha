using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storage_Master
{
    public abstract class Storage
    {
        private string name;
        private int capacity;
        private int garageSlots;
        private Vehicle[] garage;
        private List<Product> products;

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public int Capacity
        {
            get { return this.capacity; }
            set { this.capacity = value; }
        }

        public int GarageSlots
        {
            get { return this.garageSlots; }
            set { this.garageSlots = value; }
        }

        public IReadOnlyCollection<Vehicle> Garage => this.garage;

        public IReadOnlyCollection<Product> Products => this.products;

        public bool IsFull => this.products.Sum(x => x.Weight) >= this.Capacity;

        public Storage(string name,int capacity,int garageSlots,IEnumerable<Vehicle> vehicles)
        {
            this.Name = name;
            this.Capacity = capacity;
            this.GarageSlots = garageSlots;
            this.garage = new Vehicle[garageSlots];
            this.products = new List<Product>();
            int index = 0;
            foreach(var item in vehicles)
            {
                this.garage[index++] = item;
            }
        }

        public Vehicle GetVehicle(int garageSlot)
        {
            if(garageSlot>=this.garageSlots)
            {
                throw new InvalidOperationException("Invalid garage slot!");
            }
            if(this.garage[garageSlot]is null)
            {
                throw new InvalidOperationException("No vehicle in this garage slot!");
            }

            return this.garage[garageSlot];
        }

        public int SendVehicleTo(int garageSlot,Storage deliveryLocation)
        {
            Vehicle vehicle = this.GetVehicle(garageSlot);

            if(deliveryLocation.garage.Count(x=>x is null)==0)
            {
                throw new InvalidOperationException("No room ingarage!");
            }
            this.garage[garageSlot] = null;

            for (int i = 0; i < deliveryLocation.garage.Length; i++)
            {
                if(deliveryLocation.garage[1] is null)
                {
                    deliveryLocation.garage[1] = vehicle;
                    return i;
                }
            }

            return 0;
        }

        public int UnloadVehicle(int garageSlot)
        {
            if(this.IsFull)
            {
                throw new InvalidOperationException("Storage is full!");
            }

            Vehicle vehicle = this.GetVehicle(garageSlot);

            int count = 0;
            while(!vehicle.IsEmpty && !this.IsFull)
            {
                this.products.Add(vehicle.Unload());
                count++;
            }

            return count;
        }
    }
}
