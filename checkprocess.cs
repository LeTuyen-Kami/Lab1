using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Net.NetworkInformation;


namespace lab1
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        string name = "Notepad";
        public Service1()
        {
            InitializeComponent();
        }
        void checkprocess(string name)
        {
            Process[] pname = Process.GetProcessesByName(name);
            if (pname.Length == 0)
            {
                Process process = new Process();
                process.StartInfo.FileName = name;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                process.Start();
                process.WaitForExit();
            }
            else
            {
                foreach (var p in Process.GetProcessesByName(name))
                {
                    p.Kill();
                }
            }
        }
        protected override void OnStart(string[] args)
        {
            WriteToFile("Service is started at " + DateTime.Now);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 10000; //number in milisecinds 
            timer.Enabled = true;
            checkprocess(name);	
        }
        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);

        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            WriteToFile("Service is recall at " + DateTime.Now);                      
            checkprocess(name);
        }
        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory +
           "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') +
           ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }

        }
    }
}
