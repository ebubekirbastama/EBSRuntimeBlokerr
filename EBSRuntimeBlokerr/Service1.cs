using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading;

namespace EBSRuntimeBlokerr
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        Thread th; string ProcessName = "RuntimeBroker"; //white timer...
        protected override void OnStart(string[] args)//Başladığında çalışan kodumuz...
        {
            while (true)
            {
                programkapat();
                Thread.Sleep(50000);//50 sn bekle
            }
        }
        private void programkapat()
        {
            string processName = "RuntimeBroker"; // Kapatmak İstediğimiz Program
            Process[] processes = Process.GetProcesses();// Tüm Çalışan Programlar
            foreach (Process process in processes)
            {
                if (process.ProcessName == processName)
                {
                    process.Kill();
                    WriteToFile(process.ProcessName + " : Durduruldu.");
                }
            }
        }
        private void WriteToFile(string Message)
        {

            string path = @"C:\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = path + "\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message + " : " + DateTime.Now.Date.ToShortDateString());
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message + " : " + DateTime.Now.Date.ToShortDateString());
                }
            }
        }
        protected override void OnStop()//Kapandığındaki kodumuz.
        {
            WriteToFile("Service Stop" + DateTime.Now);
            th.Abort();//thread stop.
        }
    }
}
