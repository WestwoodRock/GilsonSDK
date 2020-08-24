using System;
using System.Collections.Generic;
using System.Text;

namespace GilsonSdk.Enums
{
    public enum PumpInstruction : byte
    {
        Forward = 0x3e,
        Stop = 0x48,
        Backwards = 0x3c,
        Faster = 0x2b,
        Slower = 0x2d,
        Rabbit = 0x26,
    }
}
