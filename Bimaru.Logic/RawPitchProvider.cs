using System;
using System.Collections.Generic;
using System.Text;

namespace Bimaru.Logic
{
    public class RawPitchProvider : IPitchProvider
    {
        public string GetNextPitchRaw()
        {
            return @"
+------+
|O    O|2
|      |1
|      |1
|  O   |3
|      |1
| X    |2
+------+
 212203
1x3, 2x2, 3x1
";
        }
    }
}
