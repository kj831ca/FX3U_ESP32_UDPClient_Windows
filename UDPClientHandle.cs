using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace UDPClientExample
{
    public class UDPClientHandle
    {
        UdpClient udpClient;
        IPAddress _serverIP;
        IPEndPoint _endPoint;
        bool isConnect = false;

        public event EventHandler<byte[]> OnReceivePackage;

        public UDPClientHandle()
        {
        }
        private void ReceiveCallBack(IAsyncResult ar)
        {
            UdpClient u = (UdpClient)ar.AsyncState;
            byte[] receiveBytes = u.EndReceive(ar, ref _endPoint);
            OnReceivePackage?.Invoke(this, receiveBytes);
            udpClient.BeginReceive(new AsyncCallback(ReceiveCallBack), udpClient);
        }
        public void Connect(string ipAddress,int portNumber)
        {
            if (udpClient == null)
                udpClient = new UdpClient(portNumber);
            _serverIP = IPAddress.Parse(ipAddress);
            udpClient.Connect(_serverIP, portNumber);
            _endPoint = new IPEndPoint(IPAddress.Any, portNumber);
            udpClient.BeginReceive(new AsyncCallback(ReceiveCallBack), udpClient);
            isConnect = true;
        }
        public void Disconnect()
        {
            if (udpClient != null)
            {
                udpClient.Close();
                udpClient.Dispose();
                udpClient = null;
            }
        }
        public void SendData(string dataStr)
        {
            byte[] sendBytes = Encoding.ASCII.GetBytes(dataStr);
            if(isConnect)
            {
                udpClient.Send(sendBytes, sendBytes.Length);
            }
        }

    }
}
