using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var objeto = new ObjetoTest {Nombre = "Juan", Apellido = "NA"};
            var objeto2 = new ObjetoTest { Nombre = "Juan", Apellido = "NA1" };

            var strXml = Helper.Serialize(objeto);

            Console.WriteLine(strXml);

            var objeto3 = Helper.Deserialize<ObjetoTest>(strXml);

            Console.WriteLine(objeto2.Nombre);

            Console.WriteLine(Helper.GenerateHash(strXml));

            var strXml1 = Helper.Serialize(objeto2);
            Console.WriteLine(Helper.GenerateHash(strXml1));

            Console.ReadKey();

        }
    }
}
