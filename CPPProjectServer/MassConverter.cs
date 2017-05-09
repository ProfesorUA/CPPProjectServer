
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPPProjectServer
{
    class MassConverter
    {
        public struct Mass
        {
            string name;
            double value;

            public Mass(string name, double value)
            {
                this.name = name;
                this.value = value;
            }
        }

        public static Dictionary<string, double> massTypes = new Dictionary<string, double>
        {
            {"KG", 1},
            {"G", 1000},
            {"K", 5000},
            {"MG", 1000000},
            {"F", 2.2050717},
            {"U", 35.2733686},
            {"L", 1},
            {"ML", 1000},
            {"GA", 0.2642008},
            {"P", 2.1141649},
        };
        /*
        public static Mass kilogram = new Mass("KG", 1);
        public static Mass gram = new Mass("G", 1000);
        public static Mass karat = new Mass("K", 5000);
        public static Mass miligram = new Mass("MG", 1000000);
        public static Mass funt = new Mass("F", 2.2050717);
        public static Mass uncia = new Mass("U", 35.2733686);

        public static Mass litr = new Mass("L", 1);
        public static Mass mililitr = new Mass("ML", 1000);
        public static Mass galon = new Mass("GA", 0.2642008);
        public static Mass pint = new Mass("P", 2.1141649);
        */

        public static double convert(string from, string to, double count)
        {
            return count / massTypes[from] * massTypes[to];
        }
    }
}
