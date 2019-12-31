using System;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Foundation;
using Windows.Storage.Streams;

namespace MiBandController
{
    public static class UUID
    {
        //MAC:D7:D3:66:6D:BA:BB
        public static readonly ulong MAC = 237302956538555;
        public static BluetoothLEDevice BluetoothLEDevice = null;

        public static async Task ConnectDevice()
        {
            BluetoothLEDevice = await BluetoothLEDevice.FromBluetoothAddressAsync(MAC);
        }

        public static class DeviceInformation
        {
            public static readonly Guid guid = new Guid("0000180A-0000-1000-8000-00805F9B34FB");
            public static GattDeviceService GattDeviceService;
            public static async Task GetService()
            {
                GattDeviceService = (await BluetoothLEDevice.GetGattServicesForUuidAsync(guid)).Services[0];
            }
            public static class SerialNumber
            {
                public static readonly Guid guid = new Guid("00002A25-0000-1000-8000-00805F9B34FB");
                public static string serialNumber;
                public static GattCharacteristic GattCharacteristic;
                public static async Task GetCharacteristic()
                {
                    GattCharacteristic = (await GattDeviceService.GetCharacteristicsForUuidAsync(guid)).Characteristics[0];
                }
                public static async Task<string> GetValue()
                {
                    DataReader dataReader = DataReader.FromBuffer((await GattCharacteristic.ReadValueAsync()).Value);
                    return dataReader.ReadString(dataReader.UnconsumedBufferLength);
                }
            }
            public static class HardwareRevision{
                public static readonly Guid guid = new Guid("00002A27-0000-1000-8000-00805F9B34FB");
                public static string hardwareRevision;
                public static GattCharacteristic GattCharacteristic;
                public static async Task GetCharacteristic()
                {
                    GattCharacteristic = (await GattDeviceService.GetCharacteristicsForUuidAsync(guid)).Characteristics[0];
                }
                public static async Task<string> GetValue()
                {
                    DataReader dataReader = DataReader.FromBuffer((await GattCharacteristic.ReadValueAsync()).Value);
                    return dataReader.ReadString(dataReader.UnconsumedBufferLength);
                }
            }
            public static class SoftwareRevision
            {
                public static readonly Guid guid = new Guid("00002A28-0000-1000-8000-00805F9B34FB");
                public static string softwareRevision;
                public static GattCharacteristic GattCharacteristic;
                public static async Task GetCharacteristic()
                {
                    GattCharacteristic = (await GattDeviceService.GetCharacteristicsForUuidAsync(guid)).Characteristics[0];
                }
                public static async Task<string> GetValue()
                {
                    DataReader dataReader = DataReader.FromBuffer((await GattCharacteristic.ReadValueAsync()).Value);
                    return dataReader.ReadString(dataReader.UnconsumedBufferLength);
                }
            }
        }
        public static class AlertService
        {
            public static readonly Guid guid = new Guid("00001811-0000-1000-8000-00805F9B34FB");
            public static GattDeviceService GattDeviceService;
            public static async Task GetService()
            {
                GattDeviceService = (await BluetoothLEDevice.GetGattServicesForUuidAsync(guid)).Services[0];
            }
            public static class NewAlert
            {
                public static readonly Guid guid = new Guid("00002A46-0000-1000-8000-00805F9B34FB");
                public static GattCharacteristic GattCharacteristic;
                public static async Task GetCharacteristic()
                {
                    GattCharacteristic = (await GattDeviceService.GetCharacteristicsForUuidAsync(guid)).Characteristics[0];
                }
                public static IAsyncOperation<GattWriteResult> SendAlert(byte type,string content)
                {
                    DataWriter dataWriter = new DataWriter();
                    dataWriter.WriteByte(type);
                    dataWriter.WriteByte(1);
                    dataWriter.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
                    dataWriter.WriteString(content);
                    //UUID.AlertService.NewAlert.GattCharacteristic.ProtectionLevel = GattProtectionLevel.EncryptionRequired;
                     return GattCharacteristic.WriteValueWithResultAsync(dataWriter.DetachBuffer(), GattWriteOption.WriteWithResponse);
                }
                //read only just equal 5
            }//have result

        }
        public static class MiBand2Service
        {
            public static readonly Guid guid = new Guid("0000FEE0-0000-1000-8000-00805F9B34FB");
            public static GattDeviceService GattDeviceService;
            public static async Task GetService()
            {
                GattDeviceService = (await BluetoothLEDevice.GetGattServicesForUuidAsync(guid)).Services[0];
            }
            public static class Date
            {
                public static readonly Guid guid = new Guid("00002A2B-0000-1000-8000-00805F9B34FB");
                public static GattCharacteristic GattCharacteristic;
                //227 07 12 30 23 01 15 01 00 00 32
                //227 07 12 31 19 21 29 02 00 00 32
                public static async Task GetCharacteristic()
                {
                    GattCharacteristic = (await GattDeviceService.GetCharacteristicsForUuidAsync(guid)).Characteristics[0];
                }
                public static async Task<Structure.Date> GetDate()
                {
                    DataReader dataReader = DataReader.FromBuffer((await GattCharacteristic.ReadValueAsync()).Value);
                    dataReader.ByteOrder = ByteOrder.LittleEndian;
                    //It must be setted here.
                    /*
                    date.year = dataReader.ReadUInt16();
                    date.month = dataReader.ReadByte();
                    date.day = dataReader.ReadByte();
                    date.hours = dataReader.ReadByte();
                    date.minutes = dataReader.ReadByte();
                    date.seconds = dataReader.ReadByte();
                    date.day_of_week = dataReader.ReadByte();
                    date.fractions256 = dataReader.ReadByte();
                    //there are two bytes left???
                    */
                    Structure.Date date = new Structure.Date(dataReader,true);
                    return date;
                }
            }
            public  static class BatteryInfo
            {
                public static readonly Guid guid = new Guid("00000006-0000-3512-2118-0009AF100700");
                public static GattCharacteristic GattCharacteristic;
                //15 61 00 227 07 12 05 17 12 07 32 227 07 12 30 15 40 10 32 71
                //15 57 00 227 07 12 05 17 12 07 32 227 07 12 30 15 40 10 32 71
                public static async Task GetCharacteristic()
                {
                    GattCharacteristic = (await GattDeviceService.GetCharacteristicsForUuidAsync(guid)).Characteristics[0];
                }
                public static async Task<Structure.BatteryInfo> GetBatteryInfo()
                {
                    Structure.BatteryInfo batteryinfo = new Structure.BatteryInfo();
                    DataReader dataReader = DataReader.FromBuffer((await GattCharacteristic.ReadValueAsync()).Value);
                    dataReader.ByteOrder = ByteOrder.LittleEndian;
                    dataReader.ReadByte();
                    batteryinfo.level = dataReader.ReadByte();
                    batteryinfo.status = Convert.ToBoolean(dataReader.ReadByte());
                    batteryinfo.datetime_last_charge = new Structure.Date(dataReader, false);
                    dataReader.ReadByte();
                    batteryinfo.datetime_last_off = new Structure.Date(dataReader, false);
                    dataReader.ReadByte();
                    batteryinfo.last_level = dataReader.ReadByte();
                    return batteryinfo;
                }
            }
            public static class Music
            {
                public static readonly Guid guid = new Guid("00000020-0000-3512-2118-0009AF100700");
                public static GattCharacteristic GattCharacteristic;
                //00 c3 00 0f 00 01 00 00 00 01 00 e4 bd a0 e5 a5 bd 00
                public static async Task GetCharacteristic()
                {
                    GattCharacteristic = (await GattDeviceService.GetCharacteristicsForUuidAsync(guid)).Characteristics[0];
                }
                public static IAsyncOperation<GattWriteResult> SetTest()
                {
                    byte[] data = new byte[] { 00, 0xc3, 0x00, 0x0f, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0xe4, 0xbd, 0xa0, 0xe5, 0xa5, 0xbd, 0x00 };
                    DataWriter dataWriter = new DataWriter();
                    dataWriter.WriteBytes(data);
                    return GattCharacteristic.WriteValueWithResultAsync(dataWriter.DetachBuffer(),GattWriteOption.WriteWithoutResponse);
                }
                public static async Task SetMusicName(string name, byte state = 0)
                {
                    byte flag = 1;
                    if (name.Length > 0) flag |= 0x0e;
                    DataWriter dataWriter = new DataWriter();
                    dataWriter.ByteOrder = ByteOrder.LittleEndian;
                    dataWriter.WriteByte(flag);
                    dataWriter.WriteByte(state);
                    dataWriter.WriteUInt32(1);
                    dataWriter.WriteUInt16(1);
                    if (name.Length > 0)
                    {
                        byte[] tmp = System.Text.Encoding.UTF8.GetBytes(name);
                        dataWriter.WriteBytes(tmp);
                        dataWriter.WriteByte(0);
                    }
                    await WriteChunk(dataWriter.DetachBuffer());
                }
                private static async Task WriteChunk(IBuffer buff, byte type = 3)
                {
                    const int MAX_CHUNKLENGTH = 17;
                    DataReader dataReader = DataReader.FromBuffer(buff);
                    byte[] data = new byte[dataReader.UnconsumedBufferLength];
                    dataReader.ReadBytes(data);
                    int remaining = data.Length;
                    int count = 0;
                    while(remaining > 0)
                    {
                        int copybytes = Math.Min(remaining, MAX_CHUNKLENGTH);
                        DataWriter dataWriter = new DataWriter();
                        byte flag = 0;
                        if (remaining <= MAX_CHUNKLENGTH)
                        {
                            flag |= 0x80;
                            if (count == 0) flag |= 0x40;
                        }
                        else if (count > 0) flag |= 0x40;
                        dataWriter.WriteByte(0);
                        dataWriter.WriteByte((byte)(flag | type));
                        dataWriter.WriteByte((byte)(count & 0xff));
                        for(int i=count * MAX_CHUNKLENGTH;i<count * MAX_CHUNKLENGTH + copybytes;++i)
                            dataWriter.WriteByte(data[i]);
                        count++;
                        await GattCharacteristic.WriteValueWithResultAsync(dataWriter.DetachBuffer(), GattWriteOption.WriteWithoutResponse);
                        remaining -= copybytes;
                    }
                }
            }
            public static class MusicNotify
            {
                public static readonly Guid guid = new Guid("00000010-0000-3512-2118-0009AF100700");
                public static GattCharacteristic GattCharacteristic;
                //254 0/1/2/3/4/5/6/225/254
                public static async Task GetCharacteristic()
                {
                    GattCharacteristic = (await GattDeviceService.GetCharacteristicsForUuidAsync(guid)).Characteristics[0];
                }
                public static IAsyncOperation<GattWriteResult> SetCallBack(TypedEventHandler<GattCharacteristic, GattValueChangedEventArgs> callback)
                {
                    GattCharacteristic.ValueChanged += callback;
                    return GattCharacteristic.WriteClientCharacteristicConfigurationDescriptorWithResultAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
                }
            }
        }
    }
}
