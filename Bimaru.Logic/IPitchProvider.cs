using System;
using System.Collections.Generic;
using System.Text;

namespace Bimaru.Logic
{
    public interface IPitchProvider : IDisposable
    {
        IPitch GetNextPitch();
        void Close();
    }
}
