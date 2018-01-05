
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace gcc_build_app
{
    [ProtoContract]
    public class oInclude
    {
        [ProtoMember(1)]
        public long id { set; get; }

        [ProtoMember(2)]
        public long pid { set; get; }

        [ProtoMember(3)]
        public byte status { set; get; }

        [ProtoMember(4)]
        public int type { set; get; }

        [ProtoMember(5)]
        public string code { set; get; }

        [ProtoMember(6)]
        public string name { set; get; }

        [ProtoMember(7)]
        public string tag { set; get; }
    }
}