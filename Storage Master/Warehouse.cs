using System;
using System.Collections.Generic;
using System.Text;

namespace Storage_Master
{
    public class Warehouse: Storage
    {
        public Warehouse(string name)
            :base(name,10,10,new List<Vehicle> { new Semi(),new Semi(),new Semi()})
        {

        }
    }
}
