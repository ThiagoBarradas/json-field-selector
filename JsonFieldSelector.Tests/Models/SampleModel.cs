using System;
using System.Collections.Generic;

namespace JsonFieldSelector.Tests.Models
{
    public class SampleModel
    {
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

        public static SampleModel GetFullInstance()
        {
            return new SampleModel
            {
                MyBool = false,
                MyInt = 0,
                MyLong = 0,
                MyDateTime = new DateTime(),
                MyGuid = Guid.Empty,
                MyIntArray = new int[] { 0, 1, 2 },
                MyString = "test",
                MyStringArray = new string[] { "test1", "test1" },
                MyDictionaryStringString = new Dictionary<string, string>
                {
                    { "key1", "value1" },
                    { "key2", "value2" }
                },
                MyDictionaryStringObject = new Dictionary<string, object>
                {
                    { "key11", new { TestProperty = 1, TestProperty2 = "2" } },
                    { "key1", new { TestProperty = 2, TestProperty2 = "3" } }
                },
                MyNullableBool = false,
                MyNullableDateTime = new DateTime(),
                MyNullableGuid = Guid.Empty,
                MyNullableInt = 0,
                MyNullableLong = 0,
                Sub = new SubSampleModel
                {
                    MyBool = true,
                    MyInt = 1,
                    MyLong = 1,
                    MyString = "test",
                    Sub2 = new SubSampleModel2
                    {
                        MyBool = true,
                        MyInt = 2,
                        MyLong = 2,
                        MyString = "test2"
                    },
                    ListSub2 = new List<SubSampleModel2>
                    {
                        new SubSampleModel2
                        {
                            MyBool = true,
                            MyInt = 22,
                            MyLong = 22,
                            MyString = "test22"
                        },
                        new SubSampleModel2
                        {
                            MyBool = false,
                            MyInt = 3,
                            MyLong = 3,
                            MyString = "test3"
                        }
                    }
                },
                ListSub = new List<SubSampleModel>
                {
                    new SubSampleModel
                    {
                        MyBool = true,
                        MyInt = 1,
                        MyLong = 1,
                        MyString = "test",
                        Sub2 = new SubSampleModel2
                        {
                            MyBool = true,
                            MyInt = 2,
                            MyLong = 2,
                            MyString = "test2"
                        },
                        ListSub2 = new List<SubSampleModel2>
                        {
                            new SubSampleModel2
                            {
                                MyBool = true,
                                MyInt = 22,
                                MyLong = 22,
                                MyString = "test22"
                            },
                            new SubSampleModel2
                            {
                                MyBool = false,
                                MyInt = 3,
                                MyLong = 3,
                                MyString = "test3"
                            }
                        }
                    },
                    new SubSampleModel
                    {
                        MyBool = true,
                        MyInt = 1,
                        MyLong = 1,
                        MyString = "test",
                        Sub2 = new SubSampleModel2
                        {
                            MyBool = true,
                            MyInt = 2,
                            MyLong = 2,
                            MyString = "test2"
                        },
                        ListSub2 = new List<SubSampleModel2>
                        {
                            new SubSampleModel2
                            {
                                MyBool = true,
                                MyInt = 22,
                                MyLong = 22,
                                MyString = "test22"
                            },
                            new SubSampleModel2
                            {
                                MyBool = false,
                                MyInt = 3,
                                MyLong = 3,
                                MyString = "test3"
                            }
                        }
                    }
                }
            };
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
