using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using W6Simulator.Command;

namespace W6Simulator.Devices
{
    interface IEmulator
    {
        bool HandleMessage(Message message);
    }
}
