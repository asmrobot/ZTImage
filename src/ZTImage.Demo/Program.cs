using System;
using System.Collections.Generic;
using ZTImage.Reflection.Reflector;
using System.Linq;
using ZTImage.DbLite;
using System.Data;

namespace ZTImage.Demo
{
    class Program
    {

        [AttributeUsage(AttributeTargets.Property)]
        public class KeyAttribute : Attribute
        {
            public KeyAttribute()
            {

            }
        }

        public class person
        {
            public decimal age { get; set; }

            [Key]
            public string name { get; set; }

            public byte Sex { get; set; }

            public int MyProperty { get; set; }

            public int MyProperty1 { get; set; }

            public int MyProperty2 { get; set; }

            public int MyProperty3 { get; set; }





            public int aaaaaaa1 { get; set; }
            public int aaaaaaa2 { get; set; }
            public int aaaaaaa3 { get; set; }
            public int aaaaaaa4   {get;set;}
            public int aaaaaaa15  {get;set;}
            public int aaaaaaa16  {get;set;}
            public int aaaaaaa17  {get;set;}
            public int aaaaaaa18  {get;set;}
            public int aaaaaaa19  {get;set;}
            public int aaaaaaa111 {get;set;}
            public int aaaaaaa121 {get;set;}
            public int aaaaaaa122 {get;set;}
            public int aaaaaaa123 {get;set;}
            public int aaaaaaa124 {get;set;}
            public int aaaaaaa125 {get;set;}
            public int aaaaaaa126 {get;set;}
            public int aaaaaaa127 {get;set;}
            public int aaaaaaa128 {get;set;}
            public int aaaaaaa129 { get; set; }

        }

        public class ani
        { 
            public Int32 Age { get; set; }

            public string Name { get; set; }

            public bool sex { get; set; }

            public override string ToString()
            {
                return $"age:{Age},name:{Name},sex:{sex}";
            }
        }

        public struct hii
        {
            public Int32 age;
        }

        static void Main(string[] args)
        {
            DbConnectionFactoryBuilder builder = new DbConnectionFactoryBuilder();
            var factory=builder.AddDbConnectionOption(null)
                .AddDbConnectionOption(null)
                .AddDbConnectionOption(null)
                .Build();

           



                new System.Threading.ManualResetEvent(false).WaitOne();
        }
    }
}
