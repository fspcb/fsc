using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using FSPCB.FSC.Devices;
using Microsoft.FlightSimulator.SimConnect;
using USBHIDDRIVER;

namespace FSPCB.FSC
{
    public partial class MainForm : Form
    {
        private readonly USBInterface _usbi = new USBInterface("vid_04d8");
        private SimConnector sco;
        private DeviceContainer _devices = new DeviceContainer();
        
        public MainForm()
        {
            InitializeComponent();
        }

        private void ctrlTimer_Tick(object sender, EventArgs e)
        {
            string[] list = _usbi.getDeviceList();

            if (list.Length < _devices.Count || list.Length == 0)
            {
                _devices.Clear();
                listDevices.Items.Clear();
            }

            for (int i = 0; i < list.Length; i++)
            {
                DeviceIdentifier dev = DeviceIdentifier.Identify(list[i]);
                if (dev == null)
                {
                    continue;
                }

                if (!listDevices.Items.Contains(dev.DeviceType))
                {
                    listDevices.Items.Add(dev.DeviceType);
                    if (!_devices.ContainsKey(list[i]))
                    {
                        IDevice d = DeviceIdentifier.GetDevice(dev);
                        d.Connect();
                        _devices.Add(list[i], d);
                    }
                }
            }
           

            tsConnectedDevices.Text = listDevices.Items.Count + " connected devices";

            if (sco != null && sco.Destroyed)
            {
                sco.Dispose();
                sco = null;
            }

            if (sco == null)
            {
                sco = new SimConnector(this.Handle);
            }

            tsConnectedDevices.BackColor = (listDevices.Items.Count > 0) ? Color.LightGreen : Color.LightCoral;
            tsSimConnected.BackColor = (sco.Connected) ? Color.LightGreen : Color.LightCoral;
            tsSimConnected.Text = (sco.Connected) ? "Simulator CONNECTED" : "Simulator NOT connected";
        }

        private void closeConnections()
        {
            foreach(var dev1 in _devices)
            {
                dev1.Value.Disconnect();
            }
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            Environment.Exit(0);
        }
    }

    
}
