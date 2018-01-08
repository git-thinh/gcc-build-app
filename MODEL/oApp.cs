
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace gcc_build_app
{
    [ProtoContract]
    public class oCommand
    {
        [ProtoMember(1)]
        public bool isOptionCustom { set; get; }

        [ProtoMember(2)]
        public string FileName { set; get; }

        [ProtoMember(3)]
        public string Options { set; get; }

        [ProtoMember(4)]
        public int Order { set; get; }
    }

    [ProtoContract]
    public class oApp
    {
        [ProtoMember(1)]
        public string Name { set; get; }
        
        [ProtoMember(2)]
        public string PathRootCpp { set; get; }

        /*============================================================*/
        
        [ProtoMember(3)]
        public List<string> Defines { get { return _defines; } }

        private List<string> _defines = new List<string>();
        public void set_Define(List<string> defs)
        {
            _defines = defs;
        }

        public bool update_Define(string def_old, string def_new)
        {
            int index = _defines.FindIndex(x => x == def_old);
            if (index >= 0 && _defines.FindIndex(x => x == def_new) == -1)
            {
                _defines[index] = def_new;
                return true;
            }
            return false;
        }
        public bool remove_Define(string def)
        {
            int index = _defines.FindIndex(x => x == def);
            if (index >= 0)
            {
                _defines.RemoveAt(index);
                return true;
            }
            return false;
        }
        public bool add_Define(string def)
        {
            int index = _defines.FindIndex(x => x == def);
            if (index == -1)
            {
                _defines.Insert(0, def);
                return true;
            }
            return false;
        }
        /*============================================================*/
        [ProtoMember(4)]
        public List<oProject> Projects { get { return _projects; } }

        private List<oProject> _projects = new List<oProject>();
        public void update_Project(oProject o)
        {

        }
        public void remove_Project(string projectName)
        {

        }
        /*============================================================*/
        [ProtoMember(5)]
        public List<oModule> Modules { get { return _modules; } }

        private List<oModule> _modules = new List<oModule>();
        public void update_Module(oModule o)
        {

        }
        public void remove_Module(string moduleName)
        {

        }
        /*============================================================*/
        [ProtoMember(6)]
        public List<oGCC> GCCs { get { return _gccs; } }

        private List<oGCC> _gccs = new List<oGCC>();
        public void update_GCC(oGCC o)
        {

        }
        public void remove_GCC(string gccName)
        {

        }
        /*============================================================*/
        [ProtoMember(7)]
        public List<oBookMark> BookMarks { get { return _bookmarks; } }

        private List<oBookMark> _bookmarks = new List<oBookMark>();
        public void update_BookMark(oBookMark o)
        {

        }
        public void remove_BookMark(int id)
        {

        }
        /*============================================================*/
        [ProtoMember(8)]
        public List<oCommand> Commands { get { return _commands; } }

        private List<oCommand> _commands = new List<oCommand>();
        public void addnew_Command(string makefile)
        {

        }
        public void remove_Command(int index)
        {

        }
        /*============================================================*/
        [ProtoMember(9)]
        public List<string> Makefiles { get { return _makefiles; } }

        private List<string> _makefiles = new List<string>();
        public void addnew_Makefile(string makefile)
        {

        }
        public void remove_Makefile(int index)
        {

        }
         
        /*============================================================*/

        public override string ToString()
        {
            return string.Format("{0}: {1}", Name, PathRootCpp);
        }
    }
}