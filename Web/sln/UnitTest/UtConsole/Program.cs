using Michal.Project.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var mock=new Mock.MockData();
            var mckSla = new Mock.MockSla();

            CalcService service = new CalcService(mock, mckSla);

            int h=7;
            int minutes=h*60;
            var result=service.Calc(Guid.NewGuid(), minutes, DateTime.Now.AddDays(20));

        }
    }
}
