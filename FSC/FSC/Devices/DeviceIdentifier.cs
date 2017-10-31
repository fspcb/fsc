using System;
using System.Text.RegularExpressions;

namespace FSPCB.FSC.Devices
{


    public class DeviceIdentifier
    {
        public static DeviceIdentifier[] KnownDevices = {
            RMPDevice.Identifier
        };

        public int VendorId;
        public int ProductId;
        public string FilePath;
        public string DeviceType;
        public Type DeviceClass;


        public DeviceIdentifier(int vendor, int product, string type, Type deviceClass)
        {
            VendorId = vendor;
            ProductId = product;
            DeviceType = type;
            DeviceClass = deviceClass;
        }
        public DeviceIdentifier(int vendor, int product, string file, string type)
        {
            VendorId = vendor;
            ProductId = product;
            FilePath = file;
            DeviceType = type;
        }

        public static DeviceIdentifier Identify(string p)
        {
            var pattern = @"vid_([0-9a-f]{4})&pid_([0-9a-f]{4})";
            var match = Regex.Match(p, pattern, RegexOptions.IgnoreCase);
            if (!match.Success) return null;

            if (!int.TryParse(match.Groups[1].Value, System.Globalization.NumberStyles.HexNumber,null,out var vId ))
            {
                return null;
            }
            if (!int.TryParse(match.Groups[2].Value, System.Globalization.NumberStyles.HexNumber, null, out var pId))
            {
                return null;
            }
            for (byte i = 0; i < KnownDevices.Length; i++)
            {
                if (KnownDevices[i].ProductId == pId && KnownDevices[i].VendorId == vId)
                {
                    return KnownDevices[i];
                }
            }
            return null;
        }

        public static IDevice GetDevice(DeviceIdentifier di)
        {
            for (byte i = 0; i < KnownDevices.Length; i++)
            {
                if (KnownDevices[i].ProductId == di.ProductId && KnownDevices[i].VendorId == di.VendorId)
                {
                    return (IDevice) Activator.CreateInstance(di.DeviceClass);
                }
            }

            return null;
        }
    }
}