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

        public static bool define_Update(string def_old, string def_new)
        {
            return _app.update_Define(def_old, def_new);
        }
        public static bool define_Remove(string def)
        {
            return _app.remove_Define(def);
        }
        public static bool define_Add(string def)
        {
            return _app.add_Define(def);
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
            if (!File.Exists(_file))
                using (Stream ms = new FileStream(_file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
                    Serializer.Serialize(ms, _app);
            else
            {
                using (Stream ms = new FileStream(_file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
                {
                    Serializer.Serialize(ms, _app);
                    if (ms.Position < ms.Length) ms.SetLength(ms.Position); 
                    ms.Close();
                }
            }
        }

        public static List<string> get_DefineDefault()
        {
            oApp app = null;
            if (!File.Exists(_file)) return null; 

            using (Stream file = File.OpenRead(_file)) 
                app = Serializer.Deserialize<oApp>(file);
            return app.Defines;
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
