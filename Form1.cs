using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppES1
{
    public partial class Form2 : Form,I_RemoteObject
    {
        public Form2()
        {
            InitializeComponent();

            pictureBox1.AllowDrop = true;  // Drop許可に必要

            // ファイルがダブルクリックされたとき
            string[] files = System.Environment.GetCommandLineArgs();
            var files1 = files.Skip(1);
            foreach (var filePath in files1)
            {
                if (System.IO.File.Exists(filePath))
                {
                    try
                    {
                        pictureBox1.ImageLocation = filePath;

                        break;
                    }
                    catch
                    {
                        MessageBox.Show(String.Format("{0}をダブルクリックで開くことができません", filePath), "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        public void StartupNextInstance(string str1)
        {

            // メイン・ウィンドウが最小化されていれば元に戻す
            /*
             if (IsIconic(this.Handle))
             {
                 ShowWindowAsync(this.Handle, SW_RESTORE);
             }

             // メイン・ウィンドウを最前面に表示する
             SetForegroundWindow(this.Handle);
            */

            //string[] args = (string[])parameters[0];

            MessageBox.Show("既に起動しています。引数の中身：" + str1);

            Form3 f = new Form3();
            f.Show();

            if (System.IO.File.Exists(str1))
            {
                try
                {
                    MessageBox.Show("イメージをセットしています>引数の中身：" + str1);
                    f.pictureBox3.WaitOnLoad = true;  // イメージロードまで待つ これがないとなぜかフリーズする
                    //f.pictureBox3.ImageLocation = @"z:\tmp2\QVB.jpg";
                    f.pictureBox3.ImageLocation = str1;
                    f.Show();
                    Application.DoEvents();
                }
                catch
                {
                    MessageBox.Show(String.Format("例外:", str1), "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else
            {
                MessageBox.Show(String.Format("ファイルがない:", str1), "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //f.Show();
            Application.Run(f);  // 別プロセスでフォームを開くにすると　応答なしにならなくなる


        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
