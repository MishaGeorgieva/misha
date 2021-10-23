using System;
using System.Collections.Generic;
using System.Text;

namespace Storage_Master
{
    public abstract class Product
    {
        private double price;
        private double wieght;

        public double Price
        {
            get { return this.price; }
            set
            {
                if (value < 0)
                {
                    throw new InvalidOperationException("Price connot be negative!");

                }
                this.price = value;
            }
        }

        public double Weight
        {
            get { return this.wieght; }
            set { this.wieght = value; }
        }

        public Product(double price,double weight)
        {
            this.Price = price;
            this.Weight = wieght;
        }


    }
}






