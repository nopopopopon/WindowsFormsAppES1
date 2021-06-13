using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;   // Mutexはこれが必要
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc; // ソリューションExploに参照でsystem.runtime.remoteingを追加が必要
using System.Runtime.Remoting.Lifetime;

namespace WindowsFormsAppES1
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 初めからあるもの
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            // 初めからあるものここまで

            // 以下　追加
            string APP_NAME = Application.ProductName;
            string OBJECT_NAME = "Form1";
            string URL = "ipc://" + Application.ProductName + "/" + OBJECT_NAME;

            bool createdNew = false;
            Mutex mutex = new Mutex(true, APP_NAME, out createdNew);

            // 初回起動時
            if (createdNew)
            {
                Form2 f = new Form2();

                LifetimeServices.LeaseTime = TimeSpan.Zero;
                LifetimeServices.RenewOnCallTime = TimeSpan.Zero;
                IpcChannel ipc = new IpcChannel(APP_NAME);
                ChannelServices.RegisterChannel(ipc, false); //チャネル サービスを備えたチャネルを登録　ipc:チャンネル　falseでセキュリティ無効だがIPCではもともとパラメータ無効
                RemotingServices.Marshal(f, OBJECT_NAME);

                Application.Run(f);


            }
            else
            {
                // 多重起動時

                I_RemoteObject remoteObject = (I_RemoteObject)RemotingServices.Connect(typeof(I_RemoteObject), URL);
                // 引数を渡す
                
                               // ファイルがダブルクリックされたとき
                //string[] files = System.Environment.GetCommandLineArgs();
                //string  files1 = files.Skip(2).ToString();
                // ファイルがダブルクリックされたとき
                string[] files = System.Environment.GetCommandLineArgs();
                var files1 = files.Skip(1);
                foreach (var filePath in files1)
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        try
                        {
                           

                            //string str2 = files1.ToString();
                            MessageBox.Show("既に。引数は：" + filePath);
                            remoteObject.StartupNextInstance(filePath);
                            //pictureBox1.ImageLocation = filePath;

                            break;
                        }
                        catch
                        {
                            MessageBox.Show(String.Format("{0}をダブルクリックで開くことができません", filePath), "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                Application.Exit(); // 2つ目なので自分は終わる
               
            }
            // Application.Run(new Form1());

        }
    }
    public interface I_RemoteObject
    {
        void StartupNextInstance(string s);
    }
}



