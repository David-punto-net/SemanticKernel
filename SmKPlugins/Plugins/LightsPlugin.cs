using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmKPlugins.Plugins
{
    internal class LightsPlugin(bool turnedOn = false)
    {
        private bool _turnedOn = turnedOn;

        [KernelFunction, Description("Returns whether this light is on")]
        public bool IsTurnedOn() => _turnedOn;

        [KernelFunction, Description("Turn on this light")]
        public void TurnOn() => _turnedOn = true;

        [KernelFunction, Description("Turn off this light")]
        public void TurnOff() => _turnedOn = false;
    }
}
