using System;
using System.Collections.Generic;
using System.Text;
using Bimaru.Logic;
using Bimaru.Logic.RemoteObjects;
using GalaSoft.MvvmLight.Ioc;

namespace Bimaru
{
    public class ServiceLocator
    {
        static ServiceLocator()
        {
            SimpleIoc.Default.Register<IPitchProvider>(
                //() => new RawPitchProvider());
                () => new RemotePitchProvider());
        }

        public static IPitchProvider PitchProvider
        {
            get => SimpleIoc.Default.GetInstance<IPitchProvider>();
        }
    }
}
