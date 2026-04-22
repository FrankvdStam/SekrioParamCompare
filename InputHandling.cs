using System;

namespace InputHandling;

class InputHandler
{
    public (int, int) GetVersionNumber()
    {
        int ver1 = 0, ver2 = 0;
        Console.WriteLine("Enter first version number:\n 2 - 1.02 \n 3 - 1.03 \n 4 - 1.04 \n 5 - 1.05 \n 6 - 1.06");
        while (ver1 == 0)
        {
            try
            {
                string tmp = Console.ReadLine() ?? "6";
                ver1 = int.Parse(tmp);
                if (ver1 > 6 || ver1 < 1)
                {
                    Console.WriteLine("Version number out of range - defaulting to 6");
                    ver1 = 6;
                }
                
            }
            catch (FormatException)
            {
                Console.WriteLine("Version number must be string");
            }
        }
        Console.WriteLine("Enter second version number:\n 2 - 1.02 \n 3 - 1.03 \n 4 - 1.04 \n 5 - 1.05 \n 6 - 1.06");
        while (ver2 == 0)
        {
            try
            {
                string tmp = Console.ReadLine() ?? "6";
                ver2 = int.Parse(tmp);
                if (ver2 > 6 || ver2 < 1)
                {
                    Console.WriteLine("Version number out of range - defaulting to 6");
                    ver2 = 6;
                }
                
            }
            catch (FormatException)
            {
                Console.WriteLine("Version number must be string");
            }
        }
        return (ver1, ver2);
    }
}