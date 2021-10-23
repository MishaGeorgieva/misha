using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storage_Master
{
    class StartUp 
    {
        static void Main(string[] args) 
        {
      
           StorageMaster controller = new StorageMaster();
        StringBuilder stringBuilder = new StringBuilder();
        bool isRunning = true; while (isRunning)
{
List<string> lineArgs = Console.ReadLine()
.Split(':')
.ToList(); string command = lineArgs[0]; lineArgs = lineArgs
.Skip(1)
.ToList();
try
{
switch (command)
{
case "AddProducts":
stringBuilder.AppendLine(controller.AddProduct(lineArgs));
                            
break;
case "ReceiveProduct":
stringBuilder.AppendLine(controller.ReceiveProduct(lineArgs));
break;
case "SellProduct":
stringBuilder.AppendLine(controller.SellProduct(lineArgs));
break;
case "StoreInfo":
stringBuilder.AppendLine(controller.StoreInfo(lineArgs));
break;
case "Shutdown":
stringBuilder.AppendLine(controller.Shutdown());
isRunning = false;
break;
}
}
catch (ArgumentException ex)
{
stringBuilder.AppendLine(ex.Message);
} } Console.WriteLine(stringBuilder.ToString().Trim());



        }
    }
}
