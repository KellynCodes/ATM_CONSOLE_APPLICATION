using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSPROJECT.UI;


namespace CSPROJECT.ATMAPP.App
{
    class Entry
    {
        static void Main(string[] args)
        {
            AtmApp atmApp = new AtmApp();
            atmApp.InitializeData();
            atmApp.Run();
        }
    }
}