using System;
using System.Management;
using System.Text;
using System.Windows.Forms;

namespace MirKvartir
{
    public partial class Activate : Form
    {
        public Activate()
        {
            InitializeComponent();
      
        }

        public bool flag = false;
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Activate_Load(object sender, EventArgs e)
        {
            label1.Text += CreateMD5( Workstation.GenerateWorkstationId());
        }
        public string GetKey()
        {

            string key = CreateMD5(Workstation.GenerateWorkstationId());
            for(int i=0; i< 2452; i++)
            {
                key = CreateMD5(key);
            }
            return key;
        }
       
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(label1.Text.Split(':')[1].Trim());
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            string text = textBox_key.Text;
            string md5 = label1.Text.Split(':')[1].Trim();
            for(int i=0; i< 2452; i++)
            {
                md5 = CreateMD5(md5);
            }

            if (text == md5)
            {
                flag = true;
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Неверный ключ!");
            }
        }
    }
    public static class Workstation
    {
        public static string GenerateWorkstationId()
        {
            ManagementObjectSearcher _searcher = new ManagementObjectSearcher();
            StringBuilder _value = new StringBuilder();

            /* processors */
            _searcher.Query = new ObjectQuery("select * from Win32_Processor");
            foreach (ManagementObject _object in _searcher.Get())
            {
                _value.Append(ManagmentObjectPropertyData(_object.Properties["ProcessorId"]));
                _value.Append(',');
            }
            /* baseboard */
            _searcher.Query = new ObjectQuery("select * from Win32_BaseBoard");
            foreach (ManagementObject _object in _searcher.Get())
            {
                _value.Append(ManagmentObjectPropertyData(_object.Properties["Product"]));
                _value.Append(',');
            }

            return _value.ToString();
        }

        private static string ManagmentObjectPropertyData(PropertyData data)
        {
            string _result = string.Empty;
            if (data.Value != null && !string.IsNullOrEmpty(data.Value.ToString()))
            {
                switch (data.Value.GetType().ToString())
                {
                    case "System.String[]":
                        string[] _str = (string[])data.Value;
                        foreach (string _st in _str)
                            _result += _st + " ";
                        break;
                    case "System.UInt16[]":
                        ushort[] _shortData = (ushort[])data.Value;
                        foreach (ushort _st in _shortData)
                            _result += _st + " ";
                        break;
                    default:
                        _result = data.Value.ToString();
                        break;
                }
            }
            return _result;
        }
    }
}
