using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MapMaker.TxtHelper
{
    class TxtHelper
    {
        private string temp_path = "./template/temp.html";
        private string ptemp_path = "./temp/ptemp.html";
        private string stemp_path = "./temp/stemp.html";
        private string ctemp_path = "./temp/ctemp.html";
        private string GetContent()
        {
            return File.ReadAllText("./template/temp.html", Encoding.UTF8);
        }
    }
}
