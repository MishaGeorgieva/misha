using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storage_Master
{
   public abstract class Vehicle
   {
        private int capacity;
        private List<Product> trunk;
        //private bool isFull;
        //private boll isEmpty;

        public int Capacity
        {
            get { return this.capacity; }
            set { this.capacity = value; }
        }

        public IReadOnlyCollection<Product> Trunk
        {
            get { return this.trunk; }
        }
        
        public bool IsFull => this.trunk.Sum(x => x.Weight) >= this.Capacity;

        public bool IsEmpty => this.trunk.Count == 0;
      
        public Vehicle(int capacity)
        {
            this.Capacity = capacity;
            this.trunk = new List<Product>();
        }

        public void LoadProduct(Product product)
        {
            if(this.IsFull)
            {
                throw new InvalidOperationException("Vehicle is full!");
            }
            this.trunk.Add(product);
        }

        public Product Unload()
        {
            if(this.IsEmpty)
            {
                throw new InvalidOperationException("No products left in vehicl!");
            }
            Product product = this.trunk.Last();
            this.trunk.RemoveAt(this.trunk.Count - 1);
            return product;

        }





   }
}
