using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BL.Pipes;

namespace BL
{
    public class ClientPipeHanlder: IDisposable
    {
        private bool _cont;
        private Thread thread;
        private int recieverHandler;
        private string _serverName;
        public event Action<string> UpdateCalled;

        public void Init(string serverName)
        {
            _serverName = serverName;
            StartThread();
        }
        void StartThread()
        {
            _cont = true;
            thread = new Thread(Reciever);
            thread.Start();
        }

        private void Reciever()
        {
            bool once = true;


            while (_cont)
            {
                recieverHandler = Import.CreateFile(
                    $"\\\\.\\pipe\\{_serverName}",
                    Types.EFileAccess.GenericRead,
                    Types.EFileShare.Read,
                    0,
                    Types.ECreationDisposition.CreateAlways,
                    0,
                    0);

                if (recieverHandler != -1)
                {

                    byte[] buff = new byte[64];
                    uint bytesToRead = 0;

                    if (Import.ReadFile(recieverHandler, buff, Convert.ToUInt32(buff.Length), ref bytesToRead, 0))         // выполняем запись последовательности байт в канал                
                    {
                        if (bytesToRead > 1)
                        {
                            string message = Encoding.Unicode.GetString(buff);
                            string cutted = message.Substring(0, (int)(bytesToRead / 2));

                            UpdateCalled?.Invoke(cutted);
                        }
                    }
                    if (Import.CloseHandle(recieverHandler))
                    {
                        recieverHandler = -1;
                    }
                }

                Thread.Sleep(100);
            }
        }

        public void Dispose()
        {
            _cont = false;
            thread?.Abort();

            if(recieverHandler != -1)
            {
                Import.CloseHandle(recieverHandler);
            }
        }
    }
}
