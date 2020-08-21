using System;
using System.Collections.Generic;
using System.Text;

namespace GilsonSdk
{
    public class GSIOCDeviceInfo
    {
        public byte Id { get; set; }

        public string ModuleInfo { get; set; }

        public GSIOCDeviceInfo(byte id, string moduleInfo)
        {
            Id = id;
            ModuleInfo = moduleInfo;
                
        }

        public override string ToString()
        {
            return $"{Id} - {ModuleInfo}";
        }
    }
}
