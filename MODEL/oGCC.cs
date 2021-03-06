﻿using System.Linq;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace gcc_build_app
{
    [ProtoContract]
    public class oGCC
    {
        [ProtoMember(1)]
        public string Name { set; get; }

        [ProtoMember(2)]
        public List<string> Binaries = new List<string>() { };

        [ProtoMember(3)]
        public List<string> Libraries = new List<string>() { };

        [ProtoMember(4)]
        public List<string> C_Includes = new List<string>() { };

        [ProtoMember(5)]
        public List<string> Cpp_Includes = new List<string>() { };

        [ProtoMember(6)]
        public string Version { set; get; }

        [ProtoMember(7)]
        public string PathRoot { set; get; }

        [ProtoMember(8)]
        public bool Option_Wall { set; get; }

        [ProtoMember(9)]
        public bool Option_G3 { set; get; }

        [ProtoMember(10)]
        public bool Option_0O { set; get; }

        [ProtoMember(11)]
        public bool Option_is64bit { set; get; }

        [ProtoMember(12)]
        public string Option_Define { set; get; }

        [ProtoMember(13)]
        public string Option_Compiler { set; get; }

        [ProtoMember(14)]
        public string Option_Linker { set; get; }

        public oGCC() { }
        public oGCC(string pathRoot)
        {
            Option_is64bit = false;
            Option_0O = false;
            Option_G3 = false;
            Option_Wall = false;
            Option_Compiler = "";

            PathRoot = pathRoot;
            Version = "";
            Binaries = new List<string>() { _CONST.GCC_PATH_ROOT + @"\bin", _CONST.GCC_PATH_ROOT + @"\x86_64-w64-mingw32\bin" };
            Libraries = new List<string>() { _CONST.GCC_PATH_ROOT + @"\x86_64-w64-mingw32\lib32" };
            List<string> incs = new List<string>(){
                _CONST.GCC_PATH_ROOT + @"\include", 
                _CONST.GCC_PATH_ROOT + @"\x86_64-w64-mingw32\include"};
            string incPath = pathRoot + @"\lib\gcc\x86_64-w64-mingw32";
            string inc_cpp = ""; 
            if(Directory.Exists(incPath)){
                foreach (string dir in Directory.GetDirectories(incPath)) {
                    string _inc = dir + @"\include";
                    if (Directory.Exists(_inc)) {
                        Version = Path.GetFileName(dir);
                        if (Directory.Exists(_inc + @"\c++"))
                            inc_cpp = _inc + @"\c++";
                        _inc = _inc.Substring(pathRoot.Length, _inc.Length - pathRoot.Length);
                        incs.Add(_CONST.GCC_PATH_ROOT + _inc);
                        break;
                    }
                }
            }
            C_Includes = incs;
            List<string> incs_cpp = incs.CloneList();
            if (!string.IsNullOrEmpty(inc_cpp)) {
                inc_cpp = inc_cpp.Substring(pathRoot.Length, inc_cpp.Length - pathRoot.Length);
                incs_cpp.Add(_CONST.GCC_PATH_ROOT + inc_cpp);
            }
            Cpp_Includes = incs_cpp;
            Name = Version == "" ? "GCC" : ("GCC-" + Version);
        }
    }
}