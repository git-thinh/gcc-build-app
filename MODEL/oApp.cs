
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace gcc_build_app
{
    [ProtoContract]
    public class oApp
    {
        [ProtoMember(1)]
        public List<oProject> Projects { get { return _projects; } }

        private List<oProject> _projects = new List<oProject>();
        public void update_Project(oProject o)
        {

        }
        public void remove_Project(string projectName)
        {

        }
        /*============================================================*/
        [ProtoMember(2)]
        public List<oModule> Modules { get { return _modules; } }

        private List<oModule> _modules = new List<oModule>();
        public void update_Module(oModule o)
        {

        }
        public void remove_Module(string moduleName)
        {

        }
        /*============================================================*/
        [ProtoMember(3)]
        public List<oGCC> GCCs { get { return _gccs; } }

        private List<oGCC> _gccs = new List<oGCC>();
        public void update_GCC(oGCC o)
        {

        }
        public void remove_GCC(string gccName)
        {

        }
        /*============================================================*/
        [ProtoMember(4)]
        public List<oBookMark> BookMarks { get { return _bookmarks; } }

        private List<oBookMark> _bookmarks = new List<oBookMark>();
        public void update_BookMark(oBookMark o)
        {

        }
        public void remove_BookMark(int id)
        {

        }
        /*============================================================*/
        [ProtoMember(5)]
        public List<string> Makefiles { get { return _makefiles; } }

        private List<string> _makefiles = new List<string>();
        public void addnew_Makefile(string makefile)
        {

        }
        public void remove_Makefile(int index)
        {

        }
    }
}