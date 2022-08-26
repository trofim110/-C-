using System;
using System.Collections.Generic;


namespace KbAis.OpenPit.Core
{
    public class Program
    {
        static void Main()
        {
            double[][][] aray = new double[][][]
            {
                new double[][]
                {
                    new double[] { 0, 0 },
                    new double[] { 2, 0 },
                    new double[] { 2, 2 },
                    new double[] { 0, 2 },
                }
            };
            // Console.WriteLine($"     {this.x}    {this.y}           {distance}       {max}");
            PolyLabel asd = new PolyLabel(aray,0.00001);
            Console.WriteLine(asd.x);
            Console.WriteLine(asd.y);
            Console.WriteLine(asd.distance);
            Console.WriteLine("--------------");

        }
    }

}