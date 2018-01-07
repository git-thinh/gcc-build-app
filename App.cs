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

        public static void gcc_Update(oGCC gcc)
        {
            switch (_app.GCCs.Count)
            {
                case 0:
                    _app.GCCs.Add(gcc);
                    break;
                case 1:
                    _app.GCCs[0] = gcc;
                    break;
            }
        }

        public static void gcc_RemoveAll()
        {
            _app.GCCs.Clear();
        }

        public static oApp GetData()
        {
            return _app;
        }

        public static void WriteFile()
        {
            using (Stream file = new FileStream(_file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
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
