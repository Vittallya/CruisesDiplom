using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BL.Pipes;

namespace BL
{
    public class ServerPipeHandler: IDisposable
    {
        int pipeHandle;

        Thread t;

        bool sendCalled;
        private bool _cont;

        public void Dispose()
        {
            _cont = false;
            t?.Abort();
            if (pipeHandle != -1)
                Import.CloseHandle(pipeHandle);
        }

        public void Init(string serverName)
        {
            pipeHandle = Import.CreateNamedPipe(
                $"\\\\.\\pipe\\{serverName}",
                Types.PIPE_ACCESS_DUPLEX,
                Types.PIPE_TYPE_BYTE,
                Types.PIPE_UNLIMITED_INSTANCES,
                0,
                1024,
                Types.NMPWAIT_WAIT_FOREVER,
                (uint)0);
            _cont = true;
            t = new Thread(Sender);
            t.Start();
        }

        byte[] _message;

        public void Send(string message)
        {
            _send = true;
            _message = Encoding.Unicode.GetBytes(message);
        }

        bool _send;

        int temp = 0;

        int count;

        void Sender()
        {

            while (_cont)
            {
                if (Import.ConnectNamedPipe(pipeHandle, 0))
                {

                    byte[] buffWrite = new byte[] { 0 };
                    uint realBytesWrited = 0;

                    if (_send)
                    {
                        buffWrite = _message;
                        _send = false;
                    }


                    Import.WriteFile(pipeHandle, buffWrite, (uint)buffWrite.Length, ref realBytesWrited, 0);


                    Import.DisconnectNamedPipe(pipeHandle);

                    // приостанавливаем работу потока перед тем, как приcтупить к обслуживанию очередного клиента
                }
                Thread.Sleep(100);
            }
        }

    }
}
