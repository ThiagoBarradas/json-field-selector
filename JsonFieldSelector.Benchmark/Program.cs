using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Bogus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace JsonFieldSelector.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            //SampleModel.Initialize();
            //var d1= SampleModel.GetRandomInstance();
            //var f1 = SampleModel.GetRandomFields();
            //
            //var j1a = JsonConvert.SerializeObject(d1);
            //var j1b = JsonConvert.SerializeObject(d1.SelectFieldsFromObject<object>(f1));
            //
            //var d2 = SampleModel.GetRandomInstance();
            //var f2 = SampleModel.GetRandomFields();
            //
            //var j2a = JsonConvert.SerializeObject(d2);
            //var j2b = JsonConvert.SerializeObject(d2.SelectFieldsFromObject<object>(f2));

            var summary = BenchmarkRunner.Run<JsonFieldSelectorBenchmark>();
        }
    }

    public class JsonFieldSelectorBenchmark
    {
        private readonly SampleModel Data;

        private readonly string[] Fields;

        public JsonFieldSelectorBenchmark()
        {
            ThreadPool.SetMinThreads(250, 250);
            SampleModel.Initialize();
            this.Data = SampleModel.GetRandomInstance();
            this.Fields = SampleModel.GetRandomFields();
        }

        [Benchmark(Baseline = true)]
        public string OnlySerialize()
        {
            var json = JsonConvert.SerializeObject(this.Data);
            return json;
        }

        [Benchmark]
        public string SelectingFields()
        {
            var json = JsonConvert.SerializeObject(this.Data.SelectFieldsFromObject<object>(this.Fields));
            return json;
        }

    }

    public class SampleModel
    {
        public static Faker<SampleModel> SampleModelGenerator;

        public string[] MyStringArray { get; set; }

        public int[] MyIntArray { get; set; }

        public Dictionary<string, string> MyDictionaryStringString { get; set; }

        public Dictionary<string, object> MyDictionaryStringObject { get; set; }

        public string MyString { get; set; }

        public int MyInt { get; set; }

        public long MyLong { get; set; }

        public bool MyBool { get; set; }

        public Guid MyGuid { get; set; }

        public DateTime MyDateTime { get; set; }

        public int? MyNullableInt { get; set; }

        public long? MyNullableLong { get; set; }

        public bool? MyNullableBool { get; set; }

        public Guid? MyNullableGuid { get; set; }

        public DateTime? MyNullableDateTime { get; set; }

        public List<SubSampleModel> ListSub { get; set; }

        public SubSampleModel Sub { get; set; }

        public static void Initialize()
        {
            var seed = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            Randomizer.Seed = new Random(seed);

            SampleModelGenerator = new Faker<SampleModel>()
                .RuleFor(u => u.MyBool, f => Convert.ToBoolean(Randomizer.Seed.Next(0, 1)))
                .RuleFor(u => u.MyDateTime, f => f.Date.Future(2))
                .RuleFor(u => u.MyGuid, f => Guid.NewGuid())
                .RuleFor(u => u.MyInt, f => Randomizer.Seed.Next(-9999, 9999))
                .RuleFor(u => u.MyLong, f => Randomizer.Seed.Next(-9999, 9999))
                .RuleFor(u => u.MyIntArray, f => new int[] { Randomizer.Seed.Next(-9999, 9999), Randomizer.Seed.Next(-9999, 9999), Randomizer.Seed.Next(-9999, 9999) })
                .RuleFor(u => u.MyDictionaryStringString, f =>
                {
                    return new Dictionary<string, string>
                    {
                        { "Test", "Test" },
                        { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() },
                        { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() },
                        { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() },
                    };
                })
                .RuleFor(u => u.MyDictionaryStringObject, f =>
                {
                    return new Dictionary<string, object>
                    {
                        { "Test", new { value = "Test" } },
                        { Guid.NewGuid().ToString(), new { value = Guid.NewGuid().ToString() } },
                        { Guid.NewGuid().ToString(), new { value = Guid.NewGuid().ToString() } },
                        { Guid.NewGuid().ToString(), new { value = Guid.NewGuid().ToString() } },
                    };
                })
                .RuleFor(u => u.MyStringArray, f => new string[] { Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString() })
                .RuleFor(u => u.MyNullableBool, f => Convert.ToBoolean(Randomizer.Seed.Next(0, 1)))
                .RuleFor(u => u.MyNullableDateTime, f => f.Date.Future(2))
                .RuleFor(u => u.MyNullableGuid, f => Guid.NewGuid())
                .RuleFor(u => u.MyNullableInt, f => Randomizer.Seed.Next(-9999, 9999))
                .RuleFor(u => u.MyNullableLong, f => Randomizer.Seed.Next(-9999, 9999))
                .RuleFor(u => u.Sub, f =>
                {
                    return new SubSampleModel
                    {
                        MyBool = Convert.ToBoolean(Randomizer.Seed.Next(0, 1)),
                        MyString = Guid.NewGuid().ToString(),
                        MyInt = Randomizer.Seed.Next(-9999, 9999),
                        MyLong = Randomizer.Seed.Next(-9999, 9999),
                        Sub2 = new SubSampleModel2
                        {
                            MyBool = Convert.ToBoolean(Randomizer.Seed.Next(0, 1)),
                            MyString = Guid.NewGuid().ToString(),
                            MyInt = Randomizer.Seed.Next(-9999, 9999),
                            MyLong = Randomizer.Seed.Next(-9999, 9999)
                        },
                        ListSub2 = new List<SubSampleModel2>
                         {
                             new SubSampleModel2
                             {
                                 MyBool = Convert.ToBoolean(Randomizer.Seed.Next(0, 1)),
                                 MyString = Guid.NewGuid().ToString(),
                                 MyInt = Randomizer.Seed.Next(-9999, 9999),
                                 MyLong = Randomizer.Seed.Next(-9999, 9999)
                             },
                             new SubSampleModel2
                             {
                                 MyBool = Convert.ToBoolean(Randomizer.Seed.Next(0, 1)),
                                 MyString = Guid.NewGuid().ToString(),
                                 MyInt = Randomizer.Seed.Next(-9999, 9999),
                                 MyLong = Randomizer.Seed.Next(-9999, 9999)
                             }
                         }
                    };
                })
                .RuleFor(u => u.ListSub, f =>
                {
                    return new List<SubSampleModel>
                    {
                        new SubSampleModel
                        {
                            MyBool = Convert.ToBoolean(Randomizer.Seed.Next(0, 1)),
                            MyString = Guid.NewGuid().ToString(),
                            MyInt = Randomizer.Seed.Next(-9999, 9999),
                            MyLong = Randomizer.Seed.Next(-9999, 9999),
                            Sub2 = new SubSampleModel2
                            {
                                MyBool = Convert.ToBoolean(Randomizer.Seed.Next(0, 1)),
                                MyString = Guid.NewGuid().ToString(),
                                MyInt = Randomizer.Seed.Next(-9999, 9999),
                                MyLong = Randomizer.Seed.Next(-9999, 9999)
                            },
                            ListSub2 = new List<SubSampleModel2>
                             {
                                 new SubSampleModel2
                                 {
                                     MyBool = Convert.ToBoolean(Randomizer.Seed.Next(0, 1)),
                                     MyString = Guid.NewGuid().ToString(),
                                     MyInt = Randomizer.Seed.Next(-9999, 9999),
                                     MyLong = Randomizer.Seed.Next(-9999, 9999)
                                 },
                                 new SubSampleModel2
                                 {
                                     MyBool = Convert.ToBoolean(Randomizer.Seed.Next(0, 1)),
                                     MyString = Guid.NewGuid().ToString(),
                                     MyInt = Randomizer.Seed.Next(-9999, 9999),
                                     MyLong = Randomizer.Seed.Next(-9999, 9999)
                                 }
                             }
                        },
                        new SubSampleModel
                        {
                            MyBool = Convert.ToBoolean(Randomizer.Seed.Next(0, 1)),
                            MyString = Guid.NewGuid().ToString(),
                            MyInt = Randomizer.Seed.Next(-9999, 9999),
                            MyLong = Randomizer.Seed.Next(-9999, 9999),
                            Sub2 = new SubSampleModel2
                            {
                                MyBool = Convert.ToBoolean(Randomizer.Seed.Next(0, 1)),
                                MyString = Guid.NewGuid().ToString(),
                                MyInt = Randomizer.Seed.Next(-9999, 9999),
                                MyLong = Randomizer.Seed.Next(-9999, 9999)
                            },
                            ListSub2 = new List<SubSampleModel2>
                             {
                                 new SubSampleModel2
                                 {
                                     MyBool = Convert.ToBoolean(Randomizer.Seed.Next(0, 1)),
                                     MyString = Guid.NewGuid().ToString(),
                                     MyInt = Randomizer.Seed.Next(-9999, 9999),
                                     MyLong = Randomizer.Seed.Next(-9999, 9999)
                                 },
                                 new SubSampleModel2
                                 {
                                     MyBool = Convert.ToBoolean(Randomizer.Seed.Next(0, 1)),
                                     MyString = Guid.NewGuid().ToString(),
                                     MyInt = Randomizer.Seed.Next(-9999, 9999),
                                     MyLong = Randomizer.Seed.Next(-9999, 9999)
                                 }
                             }
                        }
                    };
                });
        }

        public static SampleModel GetRandomInstance()
        {
            return SampleModelGenerator.Generate();
        }

        public static List<string[]> FieldsList = new List<string[]>
        {
            new string[] { "myBool", "myLong", "myDateTime", "myInt", "myString", "myIntArray", "myStringArray", "myGuid" },
            new string[] { "myString", "myIntArray", "myStringArray", "myGuid", "myDictionaryStringString.test", "sub.listSub2" },
            new string[] { "myString", "myIntArray", "myDictionaryStringString", "sub.listSub2.myBool" },
            new string[] { "myString", "myIntArray", "sub.listSub2", "listSub" },
            new string[] { "myString", "myIntArray", "sub.listSub2", "listSub.listSub2.myLong", "listSub.listSub2.myBool", "listSub.listSub2.myInt", "listSub.listSub2.myInvalid" },
            new string[] { "myBool", "myLong", "myDateTime", "myInt", "myString", "myIntArray", "myStringArray", "myGuid", "myNullableBool", "myNullableLong", "myNullableDateTime", "myNullableInt", "myNullableGuid", "myDictionaryStringString.value", "myDictionaryStringObject", "sub.myBool", "sub.myInt", "sub.myLong", "sub.myString", "sub.listSub2", "listSub.myBool", "listSub.myInt", "listSub.myLong", "listSub.myString", "listSub.listSub2" },
        };

        public static string[] GetRandomFields()
        {
            return FieldsList.OrderBy(x => Guid.NewGuid()).Take(1).Single();
        }
    }

    public class SubSampleModel
    {
        public string MyString { get; set; }

        public int MyInt { get; set; }

        public long MyLong { get; set; }

        public bool MyBool { get; set; }

        public List<SubSampleModel2> ListSub2 { get; set; }

        public SubSampleModel2 Sub2 { get; set; }
    }

    public class SubSampleModel2
    {
        public string MyString { get; set; }

        public int MyInt { get; set; }

        public long MyLong { get; set; }

        public bool MyBool { get; set; }
    }
}
