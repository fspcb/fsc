using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FSPCB.FSC.Devices;
using Microsoft.FlightSimulator.SimConnect;
using USBHIDDRIVER;

namespace FSPCB.FSC.Devices
{
    class RMPDevice : IDevice
    {
        [StructLayout(LayoutKind.Explicit, Size = 1, CharSet = CharSet.Ansi)]
        public struct RMPData
        {
            [FieldOffset(0)] public byte Nothing;
            [FieldOffset(1)] public UInt16 Button1;
            [FieldOffset(3)] public UInt16 VHF1Active;
            [FieldOffset(5)] public UInt16 VHF1Stby;
            [FieldOffset(7)] public UInt16 VHF2Active;
            [FieldOffset(9)] public UInt16 VHF2Stby;
            [FieldOffset(11)] public UInt16 VORActive;
        };

        public static DeviceIdentifier Identifier = new DeviceIdentifier(0x04d8, 0x1607, "RMP", typeof(RMPDevice));

        public USBInterface UsbInterface { get; set; }
        private RMPData previousRmpData;

        public void OnReadEvent(object sender, EventArgs e)
        {
            ArrayList x = (ArrayList)sender;
            if (x.Count != 1) return;

            var r = ConvertToRMPData((byte[])x[0]);

            if (r.VHF1Stby != previousRmpData.VHF1Stby)
            {
                SimConnector.SimConnect?.TransmitClientEvent(SimConnect.SIMCONNECT_OBJECT_ID_USER, ClientEvents.Com1StbyRadioSet, (uint)(r.VHF1Stby + 0x10000), NotificationGroup.HighestPriority, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
            }

            if (r.VHF1Active != previousRmpData.VHF1Active)
            {
                SimConnector.SimConnect?.TransmitClientEvent(SimConnect.SIMCONNECT_OBJECT_ID_USER, ClientEvents.Com1ActiveRadioSet, (uint)(r.VHF1Active + 0x10000), NotificationGroup.HighestPriority, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
            }

            if (r.VHF2Stby != previousRmpData.VHF2Stby)
            {
                SimConnector.SimConnect?.TransmitClientEvent(SimConnect.SIMCONNECT_OBJECT_ID_USER, ClientEvents.Com2StbyRadioSet, (uint)(r.VHF2Stby + 0x10000), NotificationGroup.HighestPriority, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
            }

            if (r.VHF2Active != previousRmpData.VHF2Active)
            {
                SimConnector.SimConnect?.TransmitClientEvent(SimConnect.SIMCONNECT_OBJECT_ID_USER, ClientEvents.Com2ActiveRadioSet, (uint)(r.VHF2Active + 0x10000), NotificationGroup.HighestPriority, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
            }

            if (r.VORActive != previousRmpData.VORActive)
            {
                SimConnector.SimConnect?.TransmitClientEvent(SimConnect.SIMCONNECT_OBJECT_ID_USER, ClientEvents.NavRadioSet, (uint)(r.VORActive + 0x10000), NotificationGroup.HighestPriority, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
            }

            previousRmpData = r;
            lock (USBHIDDRIVER.USBInterface.usbBuffer.SyncRoot)
            {
                USBHIDDRIVER.USBInterface.usbBuffer.RemoveAt(0);
            }
        }

        RMPData ConvertToRMPData(byte[] bytes)
        {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            RMPData stuff;
            try
            {
                stuff = (RMPData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(RMPData));
            }
            finally
            {
                handle.Free();
            }
            return stuff;
        }

        

        public void Connect()
        {
            UsbInterface = new USBInterface("vid_" + Identifier.VendorId.ToString("x4"), "pid_" + Identifier.ProductId.ToString("x4"));
            UsbInterface?.Connect();
            UsbInterface?.enableUsbBufferEvent(OnReadEvent);
            UsbInterface?.startRead();
        }

        public void Disconnect()
        {
            UsbInterface?.Disconnect();
        }
    }
}
