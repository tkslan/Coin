using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var timestamp = System.DateTime.UtcNow.ToFileTimeUtc().ToString();
            using (var hmacsha256 = new HMACSHA256(GetKey()))
            {
                hmacsha256.Initialize();
                var hash = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(textBox1.Text));
                var ed25519 = new Rebex.Security.Cryptography.Ed25519();
                ed25519.FromSeed(hash);
                label1.Text =$@"Key:{ByteToString( ed25519.GetPrivateKey())} Hash:{ByteToString(hash)}" ;
            }
        }

        string GetBytes(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }
        
        string ByteToString(byte[] buff)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < buff.Length; i++)
            {
                builder.Append(buff[i].ToString("X2")); // hex format
            }
            return builder.ToString();
        }
        private byte[] GetKey()
        {
            byte[] secretkey = new Byte[64];
            //RNGCryptoServiceProvider is an implementation of a random number generator.
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                // The array is now filled with cryptographically strong random bytes.
                rng.GetBytes(secretkey);
            }

            return secretkey;
        }
    }
}