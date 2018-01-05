using System.Linq;
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
        public string name { set; get; }

        [ProtoMember(2)]
        public List<string> Binaries = new List<string>() { };

        [ProtoMember(3)]
        public List<string> Libraries = new List<string>() { };

        [ProtoMember(4)]
        public List<string> C_Includes = new List<string>() { };

        [ProtoMember(5)]
        public List<string> Cpp_Includes = new List<string>() { };

        [ProtoMember(6)]
        public string Option_Compiler { set; get; }

        [ProtoMember(7)]
        public string Option_Linker { set; get; }

        public oGCC() { }
        public oGCC(string pathRoot)
        {
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
        }
    }
}