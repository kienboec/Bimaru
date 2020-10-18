using System;
using System.Collections.Generic;
using System.Text;

namespace Bimaru.Logic
{
    public interface IPitch
    {
        public char[] Field { get; } 
        public string AdditionalInfo { get; }
        public int[] LineConstraints { get; } 
        public int[] ColumnConstraints { get; } 

        int Toggle(int x, int y);
        int Toggle(in int index);
        bool IsSolved();
    }
}
