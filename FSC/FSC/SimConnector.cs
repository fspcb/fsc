using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.FlightSimulator.SimConnect;


namespace FSPCB.FSC
{

    class SimConnector : IDisposable
    {

        const int WM_USER_SIMCONNECT = 0x0402;
        private static SimConnect simconnect;
        private Thread simConnectorThread;
        public static readonly AutoResetEvent SimConnectorAutoResetEvent = new AutoResetEvent(false);
        private readonly MessageHandler messageHandler = new MessageHandler();
        public static SimConnect SimConnect { get { return simconnect; } }
        private IntPtr Handle;
        
        public delegate void ReceiveRadioData(RadioData r);
        public event ReceiveRadioData OnRadioDataReceived;

        public bool Connected = false;
        public bool Destroyed = false;
        public SimConnector(IntPtr handle)
        {
            Handle = handle;
            simConnectorThread = new Thread(StartSimConnectThread){Name = "SimConnectorThread"};
            simConnectorThread.Start();
            
        }

        private void StartSimConnectThread()
        {
            try
            {
                simconnect = new SimConnect("SimConnector", messageHandler.Handle, WM_USER_SIMCONNECT, null, 0);
            }
            catch (COMException e)
            {
                Destroyed = true;
                return;
            }

            simconnect.OnRecvOpen += new SimConnect.RecvOpenEventHandler(simconnect_OnRecvOpen);
            simconnect.OnRecvQuit += new SimConnect.RecvQuitEventHandler(simconnect_OnRecvQuit);

            // listen to exceptions 
            simconnect.OnRecvException += new SimConnect.RecvExceptionEventHandler(simconnect_OnRecvException);

            // Def            
            simconnect.RegisterDataDefineStruct<RadioData>(ClientEvents.Com1StbyRadioSet);

            simconnect.MapClientEventToSimEvent(ClientEvents.Com1StbyRadioSet, "COM_STBY_RADIO_SET");
            simconnect.MapClientEventToSimEvent(ClientEvents.Com1ActiveRadioSet, "COM_RADIO_SET");
            simconnect.MapClientEventToSimEvent(ClientEvents.Com2StbyRadioSet, "COM2_STBY_RADIO_SET");
            simconnect.MapClientEventToSimEvent(ClientEvents.Com2ActiveRadioSet, "COM2_RADIO_SET");
            simconnect.MapClientEventToSimEvent(ClientEvents.NavRadioSet, "NAV1_RADIO_SET");
            SimConnect.AddToDataDefinition(Definitions.RadioInformation, "COM ACTIVE FREQUENCY:1", "Frequency BCD16", SIMCONNECT_DATATYPE.INT32, 0f, SimConnect.SIMCONNECT_UNUSED);
            SimConnect.AddToDataDefinition(Definitions.RadioInformation, "COM STANDBY FREQUENCY:1", "Frequency BCD16", SIMCONNECT_DATATYPE.INT32, 0f, SimConnect.SIMCONNECT_UNUSED);
            SimConnect.AddToDataDefinition(Definitions.RadioInformation, "COM ACTIVE FREQUENCY:2", "Frequency BCD16", SIMCONNECT_DATATYPE.INT32, 0f, SimConnect.SIMCONNECT_UNUSED);
            SimConnect.AddToDataDefinition(Definitions.RadioInformation, "COM STANDBY FREQUENCY:2", "Frequency BCD16", SIMCONNECT_DATATYPE.INT32, 0f, SimConnect.SIMCONNECT_UNUSED);
            SimConnect.AddToDataDefinition(Definitions.RadioInformation, "NAV ACTIVE FREQUENCY:1", "Frequency BCD16", SIMCONNECT_DATATYPE.INT32, 0f, SimConnect.SIMCONNECT_UNUSED);
            
            
            //simconnect.RequestDataOnSimObject(DataRequest.REQUEST_RADIO, Definitions.RadioInformation, SimConnect.SIMCONNECT_OBJECT_ID_USER, SIMCONNECT_PERIOD.SECOND, SIMCONNECT_DATA_REQUEST_FLAG.CHANGED, 0, 1, 0);
            
            // READ

            SimConnect.RegisterDataDefineStruct<RadioData>(Definitions.RadioInformation);

            SimConnect.OnRecvSimobjectDataBytype += OnRecvSOData;
            SimConnect.OnRecvSimobjectData += OnRecvSODataEvent;


            

            SimConnectorAutoResetEvent.WaitOne();
            Destroyed = true;
        }

        private void OnRecvSODataEvent(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA data)
        {
            switch ((DataRequest) data.dwRequestID)
            {
                case DataRequest.REQUEST_RADIO:
                    RadioData r = (RadioData) data.dwData[0];
                    OnRadioDataReceived?.Invoke(r);

                    break;

            }
        }

        private void simconnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
        {
            throw new NotImplementedException();
        }

        private void simconnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            Connected = false;
            Console.Write("Prepar3d quit.");
            SimConnectorAutoResetEvent.Set();
        }

        private void simconnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            Console.Write("Prepar3d connected.");
            Connected = true;
        }

        private void OnRecvSOData(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {
            if (data != null)
            {
                this.GetType();
            }
           
        }


        public void Dispose()
        {
            SimConnectorAutoResetEvent?.Set();
            simconnect?.Dispose();
            
            simConnectorThread?.Abort();
            messageHandler?.DestroyHandle();

            simconnect = null;
            simConnectorThread = null;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    struct RadioData
    {
        public UInt32 com1active;
        public UInt32 com1stby;
        public UInt32 com2active;
        public UInt32 com2stby;
        public UInt32 nav1active;
    }

    public enum Definitions
    {
       RadioInformation
    };

    public enum ClientEvents
    {
        Com1StbyRadioSet,
        Com1ActiveRadioSet,
        Com2StbyRadioSet,
        Com2ActiveRadioSet,
        NavRadioSet
    };

    public enum NotificationGroup
    {
        Default,
        HighPriority,
        HighestPriority
    }

    public enum DataRequest
    {
        REQUEST_RADIO
    }
    internal class MessageHandler : NativeWindow
    {
        const int WM_USER_SIMCONNECT = 0x0402;
        public MessageHandler()
        {
            CreateHandle(new CreateParams());
        }
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == WM_USER_SIMCONNECT)
            {
                try
                {
                    SimConnector.SimConnect?.ReceiveMessage();
                }
                catch (COMException c)
                {
                    SimConnector.SimConnectorAutoResetEvent.Set();
                }
            }
            base.WndProc(ref m);
        }
    }
}