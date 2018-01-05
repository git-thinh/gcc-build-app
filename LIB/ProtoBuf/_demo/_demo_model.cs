using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace core
{

    [ProtoContract]
    [ProtoInclude(2, typeof(Bar))]
    public class Foo
    {
        [ProtoMember(1)]
        public string BaseProp { get; set; }
    }

    [ProtoContract]
    public class Bar : Foo
    {
        [ProtoMember(1)]
        public string ChildProp { get; set; }
    }





    [ProtoContract]
    class DMO_First
    {
        [ProtoMember(5)]
        public int Foo { get; set; }
    }
     
    [ProtoContract(DataMemberOffset = 2)]
    //[ProtoContract]
    class DMO_Second
    {
        [ProtoMember(3)]
        public int Bar { get; set; }
    }


    [ProtoContract]
    public class mAddress
    {
        [ProtoMember(1)]
        public string Line1 { get; set; }
        [ProtoMember(2)]
        public string Line2 { get; set; }
    }

    [ProtoContract]
    public class mPerson
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Name { set; get; }

        [ProtoMember(3)]
        public mAddress Address { get; set; }

        [ProtoMember(4)]
        public string Email { get; set; }

    }


    [ProtoContract, Serializable]
    public partial class Order
    {
        [ProtoMember(1)]
        public int OrderID { set; get; }

        [ProtoMember(2)]
        public string CustomerID { set; get; }

        [ProtoMember(3)]
        public System.Nullable<int> EmployeeID { set; get; }

        [ProtoMember(4, DataFormat = Database.SubObjectFormat)]
        public System.Nullable<System.DateTime> OrderDate { set; get; }

        [ProtoMember(5, DataFormat = Database.SubObjectFormat)]
        public System.Nullable<System.DateTime> RequiredDate { set; get; }

        [ProtoMember(6, DataFormat = Database.SubObjectFormat)]
        public System.Nullable<System.DateTime> ShippedDate { set; get; }

        [ProtoMember(7)]
        public System.Nullable<int> ShipVia { set; get; }

        [ProtoMember(8, DataFormat = Database.SubObjectFormat)]
        public System.Nullable<decimal> Freight { set; get; }

        [ProtoMember(9)]
        public string ShipName { set; get; }

        [ProtoMember(10)]
        public string ShipAddress { set; get; }

        [ProtoMember(11)]
        public string ShipCity { set; get; }

        [ProtoMember(12)]
        public string ShipRegion { set; get; }

        [ProtoMember(13)]
        public string ShipPostalCode { set; get; }

        [ProtoMember(14)]
        public string ShipCountry { set; get; }
    }

    [ProtoContract, Serializable]
    public class Database
    {
        public const DataFormat SubObjectFormat = DataFormat.Default;
        [ProtoMember(1, DataFormat = Database.SubObjectFormat)]
        public List<Order> Orders { get; private set; }

        public Database()
        {
            Orders = new List<Order>();
        }
    }

    [ProtoContract]
    internal class Tst
    {
        [ProtoMember(1)]
        public int ValInt
        {
            get;
            set;
        }

        [ProtoMember(2)]
        public byte[] ArrayData
        {
            get;
            set;
        }

        [ProtoMember(3)]
        public string Str1
        {
            get;
            set;
        }
    }



    [ProtoContract(ImplicitFields = ImplicitFields.AllFields, ImplicitFirstTag = 4)]
    [ProtoPartialIgnore("g_ignoreIndirect")]
    public class ImplicitFieldPOCO
    {
        public event EventHandler Foo;
        protected virtual void OnFoo()
        {
            if (Foo != null) Foo(this, EventArgs.Empty);
        }
        public Action Bar;

        public int D_public;

        private int e_private;
        public int E_private
        {
            get { return e_private; }
            set { e_private = value; }
        }

        [ProtoIgnore]
        private int f_ignoreDirect;
        public int F_ignoreDirect
        {
            get { return f_ignoreDirect; }
            set { f_ignoreDirect = value; }
        }

        private int g_ignoreIndirect;
        public int G_ignoreIndirect
        {
            get { return g_ignoreIndirect; }
            set { g_ignoreIndirect = value; }
        }
        [NonSerialized]
        public int H_nonSerialized;

        [ProtoMember(1)]
        private int x_explicitField;

        public int X_explicitField
        {
            get { return x_explicitField; }
            set { x_explicitField = value; }
        }

        [ProtoIgnore]
        private int z_explicitProperty;
        [ProtoMember(2)]
        public int Z_explicitProperty
        {
            get { return z_explicitProperty; }
            set { z_explicitProperty = value; }
        }

    }

    [ProtoContract]
    public class ImplicitFieldPOCOEquiv
    {
        [ProtoMember(4)]
        public int D { get; set; }
        [ProtoMember(5)]
        public int E { get; set; }
        [ProtoMember(1)]
        public int X { get; set; }
        [ProtoMember(2)]
        public int Z { get; set; }
    }

    [Serializable]
    public class BasicType
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
