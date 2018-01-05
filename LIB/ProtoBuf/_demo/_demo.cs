using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using ProtoBuf;
using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace core
{
    public class demo_protobuf
    {
        
        public static void run6()
        {

            DMO_First first = new DMO_First { Foo = 12 };
            DMO_Second second = Serializer.ChangeType<DMO_First, DMO_Second>(first);


            ImplicitFieldPOCO foo = new ImplicitFieldPOCO
            {
                D_public = 100,
                E_private = 101,
                F_ignoreDirect = 102,
                G_ignoreIndirect = 103,
                H_nonSerialized = 104,
                X_explicitField = 105,
                Z_explicitProperty = 106
            }; 

            ImplicitFieldPOCOEquiv equiv = Serializer.ChangeType<ImplicitFieldPOCO, ImplicitFieldPOCOEquiv>(foo);
        }


        public void run5()
        {
            Tst t = new Tst();
            t.ValInt = 128;
            t.Str1 = "SOme string text value ttt";
            t.ArrayData = new byte[] { };

            MemoryStream stm = new MemoryStream();
            Serializer.Serialize(stm, t);
            Console.WriteLine(stm.Length);
        }

        public static void run4()
        {
            Database db0 = LoadDatabaseFromFile<Database>(RuntimeTypeModel.Default);

            var model = TypeModel.Create();

            var db1 = LoadDatabaseFromFile<Database>(model);

            model.CompileInPlace();
            var db2 = LoadDatabaseFromFile<Database>(model);

            var db3 = LoadDatabaseFromFile<Database>(model.Compile());

            //var db4 = LoadDatabaseFromFile<Database>(model.Compile("NWindModel", "NWindModel.dll"));

            string proto = Serializer.GetProto<Database>();

        }

        public static void run3()
        {
            byte[] blob = File.ReadAllBytes("nwind.proto.bin");
            using (MemoryStream ms = new MemoryStream(blob))
            {
                var model = ProtoBuf.Meta.TypeModel.Create();
                Type type = typeof(Database);

                model.Deserialize(ms, null, type);

                var compiled = model.Compile();

                /*erializer.PrepareSerializer<Database>();
                Serializer.Deserialize<Database>(ms);*/
                Stopwatch watch = Stopwatch.StartNew();
                for (int i = 0; i < 1; i++)
                {
                    ms.Position = 0;

                    var o1 = compiled.Deserialize(ms, null, type);


                    var o2 = Serializer.Deserialize<Database>(ms);

                }
                watch.Stop();

                Console.WriteLine(watch.ElapsedMilliseconds);
                //if (Debugger.IsAttached)
                //{
                //    Console.WriteLine("(press any key)");
                //    Console.ReadKey();
                //}

            }
        }

        public static void run2()
        {

            BasicType original = new BasicType { Id = 123, Name = "abc" }, clone;
            using (var client = new MemcachedClient())
            {
                client.Store(StoreMode.Set, "ShouldBeAbleToCacheBasicTypes", original);
            }

            using (var client = new MemcachedClient())
            {
                clone = client.Get<BasicType>("ShouldBeAbleToCacheBasicTypes");
            }
        }


        public static void run1()
        {

            MemcachedClientConfiguration config = new MemcachedClientConfiguration();
            config.Servers.Add(new IPEndPoint(IPAddress.Loopback, 11211));
            //config.Protocol = MemcachedProtocol.Binary;
            //config.Authentication.Type = typeof(PlainTextAuthenticator);
            //config.Authentication.Parameters["userName"] = "demo";
            //config.Authentication.Parameters["password"] = "demo";

            var mc = new MemcachedClient(config);
            for (var i = 0; i < 100; i++)
                mc.Store(StoreMode.Set, "Hello", "World");
        }

        public static void run0()
        {
            //var person = new mPerson
            //{
            //    Id = 12345,
            //   // Name = "Fred",
            //    Address = new mAddress
            //    {
            //        Line1 = "Flat 1",
            //        Line2 = "The Meadows"
            //    }
            //};
            //var person2 = new mPerson
            //{
            //    Id = 12345,
            //    //Name = "Nguyễn Văn Thịnh",
            //    Address = new mAddress
            //    {
            //        Line1 = "Flat 1",
            //        Line2 = "The Meadows"
            //    }
            //};

            //using (var file = File.Create("person.bin"))
            //{
            //    Serializer.Serialize<mPerson[]>(file,new mPerson[] { person, person2 });
            //}

            List<mPerson> newPerson;
            using (var file = File.OpenRead("person.bin"))
            {
                newPerson = Serializer.Deserialize<List<mPerson>>(file);
            }


        }//


        public static T LoadDatabaseFromFile<T>(TypeModel model) where T : class, new()
        {
            using (Stream fs = File.OpenRead("nwind.proto.bin"))
            {
                return (T)model.Deserialize(fs, null, typeof(T));
            }
        }

        static Database ReadFromFile(string path)
        {
            Database database;
            using (Stream fs = File.OpenRead(path))
            {
                database = Serializer.Deserialize<Database>(fs);
                fs.Close();
            }
            return database;
        }

        static void WriteToFile(string path, Database database)
        {
            using (Stream fs = File.Create(path))
            {
                Serializer.Serialize(fs, database);
                fs.Close();
            }
        }

        public void ShouldBeAbleToCacheBasicTypes()
        {
            BasicType original = new BasicType { Id = 123, Name = "abc" }, clone;
            using (var client = new MemcachedClient())
            {
                client.Store(StoreMode.Set, "ShouldBeAbleToCacheBasicTypes", original);
            }
            using (var client = new MemcachedClient())
            {
                clone = client.Get<BasicType>("ShouldBeAbleToCacheBasicTypes");
            }
        }

    }//end class


}
