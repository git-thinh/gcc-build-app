using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using ProtoBuf;

namespace gcc_build_app
{
    static class App
    {
        const string _file = "gcc.bin";
        static oApp _app = new oApp();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ReadFile();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new fApp());
        }

        public static oApp GetData()
        {
            return _app;
        }

        public static void WriteFile()
        {
            using (Stream file = File.Create(_file))
            {
                Serializer.Serialize(file, _app);
                file.Close();
            }

            Console.WriteLine("{0}: {1} bytes", Path.GetFileName(_file), new FileInfo(_file).Length);
        }

        private static void ReadFile()
        {
            if (!File.Exists(_file))
            {
                WriteFile();
                return;
            }

            using (Stream file = File.OpenRead(_file))
            {
                _app = Serializer.Deserialize<oApp>(file);
            }
        }
    }
}
