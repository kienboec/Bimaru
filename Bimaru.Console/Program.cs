using System;
using System.Runtime.CompilerServices;
using Bimaru.Logic;

namespace Bimaru.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            IPitchProvider provider = new RemotePitchProvider();
            var pitch = new Pitch(provider.GetNextPitchRaw());

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
                            int index = (x - 1) + (y - 1) * Pitch.XDimension;
                            pitch.Toggle(index);
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
        }
    }
}
