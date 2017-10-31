using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USBHIDDRIVER;

namespace FSPCB.FSC.Devices
{
    public interface IDevice
    {
        USBInterface UsbInterface { get; set; }
        void OnReadEvent(object sender, EventArgs e);
        void Connect();
        void Disconnect();
    }
}
