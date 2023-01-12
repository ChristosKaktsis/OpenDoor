using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDoor.Uuids
{
    public class BlueToothUuids
    {
        public static Guid LedService { get; private set; } = new Guid("6E400001-B5A3-F393-E0A9-E50E24DCCA9E");//open close led service
        public static Guid LedCharacteristic { get; private set; } = new Guid("6E400003-B5A3-F393-E0A9-E50E24DCCA9E");//observe led Characteristic
        public static Guid WriteCharacteristic { get; private set; } = new Guid("6E400002-B5A3-F393-E0A9-E50E24DCCA9E");//open close led Characteristic

    }
}
