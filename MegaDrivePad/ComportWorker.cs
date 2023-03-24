using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace xna
{
    public static class ComportWorker
    {
        public static SerialPort _serialPort = null;
        public static string PreviouslySent = "000000000000" + "000000000000";

        public static void PortWorking()
        {
            string CurrentPadsInputs;
            if (!GamepadManager.Swapped)
            {
                CurrentPadsInputs = GamepadManager.GenesisPad.Inputs + GamepadManager.GenesisPad2.Inputs;
            } else
            {
                CurrentPadsInputs = GamepadManager.GenesisPad2.Inputs + GamepadManager.GenesisPad.Inputs;
            }

            if (PreviouslySent != CurrentPadsInputs)
            {
                PreviouslySent = CurrentPadsInputs;

                if (_serialPort == null)
                {
                    _serialPort = new SerialPort();
                    _serialPort.PortName = "COM4";
                    _serialPort.BaudRate = 2000000;
                    _serialPort.Parity = Parity.None;
                    _serialPort.DataBits = 8;
                    _serialPort.StopBits = StopBits.One;
                    _serialPort.Handshake = Handshake.None;
                    try
                    {
                        _serialPort.Open();
                    }
                    catch (InvalidOperationException)
                    {
                        _serialPort = null;
                    }
                    catch (Exception)
                    {
                        _serialPort = null;
                    }
                }

                if (_serialPort == null)
                {
                    return;
                }
                try
                {
                    _serialPort.WriteLine(CurrentPadsInputs);
                }
                catch (InvalidOperationException)
                {
                    _serialPort.Close();
                    _serialPort = null;
                }
                catch (Exception)
                {
                    _serialPort.Close();
                    _serialPort = null;
                }
            }
        }
    }
}
