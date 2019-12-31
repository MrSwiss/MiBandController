using System;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace MiBandController
{
    public abstract class ServiceBase
    {
        public static Guid guid;
        public static GattDeviceService GattDeviceService;
        /*public static async Task GetService()
        {
            GattDeviceService = (await BluetoothLEDevice.GetGattServicesForUuidAsync(guid)).Services[0];
        }*/
        //it's a little difficult.
    }
}
