using System;
using System.Runtime.CompilerServices;
using Bimaru.Logic;
using Bimaru.Logic.RemoteObjects;

namespace Bimaru.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            using IPitchProvider provider = new RemotePitchProvider();
            var pitch = provider.GetNextPitch();

            string command = null;
            do
            {
                if (!string.IsNullOrWhiteSpace(command))
                {
                    var parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        if (int.TryParse(parts[0], out int x) && 
                            int.TryParse(parts[1], out int y))
                        {
                            var index = pitch.Toggle(x, y);
                            System.Console.WriteLine($"Field at index {index} set");
                            System.Console.WriteLine();
                            if (pitch.IsSolved())
                            {
                                System.Console.WriteLine("congratulations you won");
                                break;
                            }
                        }
                    }
                }

                System.Console.WriteLine(pitch.ToString());
                System.Console.Write("toggle: ");
            }
            while ((command = System.Console.ReadLine()) != "quit");

            System.Console.WriteLine("press any key to continue");
            System.Console.ReadLine();
        }
    }
}
