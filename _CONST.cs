using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ProtoBuf;

namespace gcc_build_app
{
    public class _CONST
    {
        public const string APP_NAME = "GCC BUILD";
        public const int APP_WIDTH = 1024;
        public const int APP_HEIGHT = 600;

        public const int BOX_PROJECT_WIDTH = 200;
        public const string GCC_PATH_ROOT = "%GCC_PATH_ROOT%";
    }

    public static class ExtClass
    {
        public static List<T> CloneList<T>(this List<T> t)
        {
            using (Stream ms = new MemoryStream())
            {
                Serializer.Serialize(ms, t);
                ms.Seek(0, SeekOrigin.Begin);
                return Serializer.Deserialize<List<T>>(ms);
            }
        }

        public static T CloneObject<T>(this T t)
        {
            using (Stream ms = new MemoryStream())
            {
                Serializer.Serialize(ms, t);
                return Serializer.Deserialize<T>(ms);
            }
        }
    }
}

