using JsonFieldSelector.Tests.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Xunit;

namespace JsonFieldSelector.Tests
{
    public class JsonFieldSelectorExtensionTests
    {
        [Fact]
        public static void SelectFields_Should_Return_Some_Root_Fields()
        {
            // arrange
            var obj = SampleModel.GetFullInstance();
            var fields = new string[] { "myInt", "myString" };

            // act
            var result1 = obj.SelectFieldsFromObject<object>(fields); // by obj
            var json1 = JsonConvert.SerializeObject(result1);

            var result2 = JToken.FromObject(obj).SelectFieldsFromJToken(fields); // by jtoken
            var json2 = JsonConvert.SerializeObject(result2);

            var json3 = JsonConvert.SerializeObject(obj).SelectFieldsFromString(fields); // by string

            // assert
            Assert.Equal("{\"MyString\":\"test\",\"MyInt\":0}", json1);
            Assert.Equal("{\"MyString\":\"test\",\"MyInt\":0}", json2);
            Assert.Equal("{\"MyString\":\"test\",\"MyInt\":0}", json3);
        }

        [Fact]
        public static void SelectFields_Should_Return_Some_Root_Fields__With_String_And_Separator()
        {
            // arrange
            var obj = SampleModel.GetFullInstance();
            var fields = "myInt|myString";
            var separator = '|';

            // act
            var result1 = obj.SelectFieldsFromObject<object>(fields, separator); // by obj
            var json1 = JsonConvert.SerializeObject(result1);

            var result2 = JToken.FromObject(obj).SelectFieldsFromJToken(fields, separator); // by jtoken
            var json2 = JsonConvert.SerializeObject(result2);

            var json3 = JsonConvert.SerializeObject(obj).SelectFieldsFromString(fields, separator); // by string

            // assert
            Assert.Equal("{\"MyString\":\"test\",\"MyInt\":0}", json1);
            Assert.Equal("{\"MyString\":\"test\",\"MyInt\":0}", json2);
            Assert.Equal("{\"MyString\":\"test\",\"MyInt\":0}", json3);
        }

        [Fact]
        public static void SelectFields_Should_Return_Some_Array_Fields()
        {
            // arrange
            var obj = SampleModel.GetFullInstance();
            var fields = new string[] { "myLong", "myBool", "listSub.myBool", "sub.ListSub2" };

            // act
            var result1 = obj.SelectFieldsFromObject<object>(fields); // by obj
            var json1 = JsonConvert.SerializeObject(result1);

            var result2 = JToken.FromObject(obj).SelectFieldsFromJToken(fields); // by jtoken
            var json2 = JsonConvert.SerializeObject(result2);

            var json3 = JsonConvert.SerializeObject(obj).SelectFieldsFromString(fields); // by string

            // assert
            Assert.Equal("{\"MyLong\":0,\"MyBool\":false,\"ListSub\":[{\"MyBool\":true},{\"MyBool\":true}],\"Sub\":{\"ListSub2\":[{\"MyString\":\"test22\",\"MyInt\":22,\"MyLong\":22,\"MyBool\":true},{\"MyString\":\"test3\",\"MyInt\":3,\"MyLong\":3,\"MyBool\":false}]}}", json1);
            Assert.Equal("{\"MyLong\":0,\"MyBool\":false,\"ListSub\":[{\"MyBool\":true},{\"MyBool\":true}],\"Sub\":{\"ListSub2\":[{\"MyString\":\"test22\",\"MyInt\":22,\"MyLong\":22,\"MyBool\":true},{\"MyString\":\"test3\",\"MyInt\":3,\"MyLong\":3,\"MyBool\":false}]}}", json2);
            Assert.Equal("{\"MyLong\":0,\"MyBool\":false,\"ListSub\":[{\"MyBool\":true},{\"MyBool\":true}],\"Sub\":{\"ListSub2\":[{\"MyString\":\"test22\",\"MyInt\":22,\"MyLong\":22,\"MyBool\":true},{\"MyString\":\"test3\",\"MyInt\":3,\"MyLong\":3,\"MyBool\":false}]}}", json3);
        }

        [Fact]
        public static void SelectFields_Should_Return_Some_Object_Fields()
        {
            // arrange
            var obj = SampleModel.GetFullInstance();
            var fields = new string[] { "myLong", "myBool", "sub.myBool", "sub.sub2", "myDictionaryStringString.", "myDictionaryStringObject.key11" };

            // act
            var result1 = obj.SelectFieldsFromObject<object>(fields); // by obj
            var json1 = JsonConvert.SerializeObject(result1);

            var result2 = JToken.FromObject(obj).SelectFieldsFromJToken(fields); // by jtoken
            var json2 = JsonConvert.SerializeObject(result2);

            var json3 = JsonConvert.SerializeObject(obj).SelectFieldsFromString(fields); // by string

            // assert
            Assert.Equal("{\"MyDictionaryStringObject\":{\"key11\":{\"TestProperty\":1,\"TestProperty2\":\"2\"}},\"MyLong\":0,\"MyBool\":false,\"Sub\":{\"MyBool\":true,\"Sub2\":{\"MyString\":\"test2\",\"MyInt\":2,\"MyLong\":2,\"MyBool\":true}}}", json1);
            Assert.Equal("{\"MyDictionaryStringObject\":{\"key11\":{\"TestProperty\":1,\"TestProperty2\":\"2\"}},\"MyLong\":0,\"MyBool\":false,\"Sub\":{\"MyBool\":true,\"Sub2\":{\"MyString\":\"test2\",\"MyInt\":2,\"MyLong\":2,\"MyBool\":true}}}", json2);
            Assert.Equal("{\"MyDictionaryStringObject\":{\"key11\":{\"TestProperty\":1,\"TestProperty2\":\"2\"}},\"MyLong\":0,\"MyBool\":false,\"Sub\":{\"MyBool\":true,\"Sub2\":{\"MyString\":\"test2\",\"MyInt\":2,\"MyLong\":2,\"MyBool\":true}}}", json3);
        }

        [Fact]
        public static void SelectFields_Should_Return_Full_Object_Without_Fields()
        {
            // arrange
            var obj = SampleModel.GetFullInstance();

            // act
            var result1 = obj.SelectFieldsFromObject<object>(null); // by obj
            var json1 = JsonConvert.SerializeObject(result1);

            var result2 = JToken.FromObject(obj).SelectFieldsFromJToken(null); // by jtoken
            var json2 = JsonConvert.SerializeObject(result2);

            var json3 = JsonConvert.SerializeObject(obj).SelectFieldsFromString(new string[] { }); // by string

            // assert
            Assert.Equal("{\"MyStringArray\":[\"test1\",\"test1\"],\"MyIntArray\":[0,1,2],\"MyDictionaryStringString\":{\"key1\":\"value1\",\"key2\":\"value2\"},\"MyDictionaryStringObject\":{\"key11\":{\"TestProperty\":1,\"TestProperty2\":\"2\"},\"key1\":{\"TestProperty\":2,\"TestProperty2\":\"3\"}},\"MyString\":\"test\",\"MyInt\":0,\"MyLong\":0,\"MyBool\":false,\"MyGuid\":\"00000000-0000-0000-0000-000000000000\",\"MyDateTime\":\"0001-01-01T00:00:00\",\"MyNullableInt\":0,\"MyNullableLong\":0,\"MyNullableBool\":false,\"MyNullableGuid\":\"00000000-0000-0000-0000-000000000000\",\"MyNullableDateTime\":\"0001-01-01T00:00:00\",\"ListSub\":[{\"MyString\":\"test\",\"MyInt\":1,\"MyLong\":1,\"MyBool\":true,\"ListSub2\":[{\"MyString\":\"test22\",\"MyInt\":22,\"MyLong\":22,\"MyBool\":true},{\"MyString\":\"test3\",\"MyInt\":3,\"MyLong\":3,\"MyBool\":false}],\"Sub2\":{\"MyString\":\"test2\",\"MyInt\":2,\"MyLong\":2,\"MyBool\":true}},{\"MyString\":\"test\",\"MyInt\":1,\"MyLong\":1,\"MyBool\":true,\"ListSub2\":[{\"MyString\":\"test22\",\"MyInt\":22,\"MyLong\":22,\"MyBool\":true},{\"MyString\":\"test3\",\"MyInt\":3,\"MyLong\":3,\"MyBool\":false}],\"Sub2\":{\"MyString\":\"test2\",\"MyInt\":2,\"MyLong\":2,\"MyBool\":true}}],\"Sub\":{\"MyString\":\"test\",\"MyInt\":1,\"MyLong\":1,\"MyBool\":true,\"ListSub2\":[{\"MyString\":\"test22\",\"MyInt\":22,\"MyLong\":22,\"MyBool\":true},{\"MyString\":\"test3\",\"MyInt\":3,\"MyLong\":3,\"MyBool\":false}],\"Sub2\":{\"MyString\":\"test2\",\"MyInt\":2,\"MyLong\":2,\"MyBool\":true}}}", json1);
            Assert.Equal("{\"MyStringArray\":[\"test1\",\"test1\"],\"MyIntArray\":[0,1,2],\"MyDictionaryStringString\":{\"key1\":\"value1\",\"key2\":\"value2\"},\"MyDictionaryStringObject\":{\"key11\":{\"TestProperty\":1,\"TestProperty2\":\"2\"},\"key1\":{\"TestProperty\":2,\"TestProperty2\":\"3\"}},\"MyString\":\"test\",\"MyInt\":0,\"MyLong\":0,\"MyBool\":false,\"MyGuid\":\"00000000-0000-0000-0000-000000000000\",\"MyDateTime\":\"0001-01-01T00:00:00\",\"MyNullableInt\":0,\"MyNullableLong\":0,\"MyNullableBool\":false,\"MyNullableGuid\":\"00000000-0000-0000-0000-000000000000\",\"MyNullableDateTime\":\"0001-01-01T00:00:00\",\"ListSub\":[{\"MyString\":\"test\",\"MyInt\":1,\"MyLong\":1,\"MyBool\":true,\"ListSub2\":[{\"MyString\":\"test22\",\"MyInt\":22,\"MyLong\":22,\"MyBool\":true},{\"MyString\":\"test3\",\"MyInt\":3,\"MyLong\":3,\"MyBool\":false}],\"Sub2\":{\"MyString\":\"test2\",\"MyInt\":2,\"MyLong\":2,\"MyBool\":true}},{\"MyString\":\"test\",\"MyInt\":1,\"MyLong\":1,\"MyBool\":true,\"ListSub2\":[{\"MyString\":\"test22\",\"MyInt\":22,\"MyLong\":22,\"MyBool\":true},{\"MyString\":\"test3\",\"MyInt\":3,\"MyLong\":3,\"MyBool\":false}],\"Sub2\":{\"MyString\":\"test2\",\"MyInt\":2,\"MyLong\":2,\"MyBool\":true}}],\"Sub\":{\"MyString\":\"test\",\"MyInt\":1,\"MyLong\":1,\"MyBool\":true,\"ListSub2\":[{\"MyString\":\"test22\",\"MyInt\":22,\"MyLong\":22,\"MyBool\":true},{\"MyString\":\"test3\",\"MyInt\":3,\"MyLong\":3,\"MyBool\":false}],\"Sub2\":{\"MyString\":\"test2\",\"MyInt\":2,\"MyLong\":2,\"MyBool\":true}}}", json2);
            Assert.Equal("{\"MyStringArray\":[\"test1\",\"test1\"],\"MyIntArray\":[0,1,2],\"MyDictionaryStringString\":{\"key1\":\"value1\",\"key2\":\"value2\"},\"MyDictionaryStringObject\":{\"key11\":{\"TestProperty\":1,\"TestProperty2\":\"2\"},\"key1\":{\"TestProperty\":2,\"TestProperty2\":\"3\"}},\"MyString\":\"test\",\"MyInt\":0,\"MyLong\":0,\"MyBool\":false,\"MyGuid\":\"00000000-0000-0000-0000-000000000000\",\"MyDateTime\":\"0001-01-01T00:00:00\",\"MyNullableInt\":0,\"MyNullableLong\":0,\"MyNullableBool\":false,\"MyNullableGuid\":\"00000000-0000-0000-0000-000000000000\",\"MyNullableDateTime\":\"0001-01-01T00:00:00\",\"ListSub\":[{\"MyString\":\"test\",\"MyInt\":1,\"MyLong\":1,\"MyBool\":true,\"ListSub2\":[{\"MyString\":\"test22\",\"MyInt\":22,\"MyLong\":22,\"MyBool\":true},{\"MyString\":\"test3\",\"MyInt\":3,\"MyLong\":3,\"MyBool\":false}],\"Sub2\":{\"MyString\":\"test2\",\"MyInt\":2,\"MyLong\":2,\"MyBool\":true}},{\"MyString\":\"test\",\"MyInt\":1,\"MyLong\":1,\"MyBool\":true,\"ListSub2\":[{\"MyString\":\"test22\",\"MyInt\":22,\"MyLong\":22,\"MyBool\":true},{\"MyString\":\"test3\",\"MyInt\":3,\"MyLong\":3,\"MyBool\":false}],\"Sub2\":{\"MyString\":\"test2\",\"MyInt\":2,\"MyLong\":2,\"MyBool\":true}}],\"Sub\":{\"MyString\":\"test\",\"MyInt\":1,\"MyLong\":1,\"MyBool\":true,\"ListSub2\":[{\"MyString\":\"test22\",\"MyInt\":22,\"MyLong\":22,\"MyBool\":true},{\"MyString\":\"test3\",\"MyInt\":3,\"MyLong\":3,\"MyBool\":false}],\"Sub2\":{\"MyString\":\"test2\",\"MyInt\":2,\"MyLong\":2,\"MyBool\":true}}}", json3);
        }

        [Fact]
        public static void SelectFields_Should_Throws_Exception_When_Param_Is_Null()
        {
            // act & assert
            Assert.Throws<ArgumentNullException>(() => JsonFieldSelectorExtension.SelectFieldsFromObject<object>(null, new string[] { })); // by obj
            Assert.Throws<ArgumentNullException>(() => JsonFieldSelectorExtension.SelectFieldsFromJToken(null, new string[] { }));  // by jtoken
            Assert.Throws<ArgumentNullException>(() => JsonFieldSelectorExtension.SelectFieldsFromString(null, new string[] { }));  // by string
        }
    }
}
