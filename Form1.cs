using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDPClientExample
{
    public partial class Form1 : Form
    {
        UDPClientHandle updClient;
        public Form1()
        {
            InitializeComponent();
            updClient = new UDPClientHandle();
            updClient.OnReceivePackage += UpdClient_OnReceivePackage;
        }

        private delegate void UpdateTextbox(TextBox txtBox, string msg);
        private void updateTextbox(TextBox txtBox, string msg)
        {
            if(txtBox.InvokeRequired)
            {
                this.Invoke(new UpdateTextbox(updateTextbox), new object[] { txtBox, msg });
                return;
            }
            txtBox.Text = msg;
        }

        private void UpdClient_OnReceivePackage(object sender, byte[] e)
        {
            string retData = Encoding.ASCII.GetString(e);
            
            updateTextbox(textBoxReceive, retData);
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            string ipString = textBoxServer.Text;
            int port = 3333;
            updClient.Connect(ipString, port);
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            string sendString = textBoxSendData.Text;
           
            updClient.SendData(sendString);
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            updClient.Disconnect();
        }
    }
}
