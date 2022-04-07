using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAdmin.Utils;
using Xunit;

namespace BlazorAdmin.Tests
{
    public class GenericTests
    {
        private class TestObj { }

        private class TestModel
        {
            public string String { get; set; } = "initial";
            public int Int { get; set; } = -1;
            public TestObj Null { get; set; } = new TestObj();
            public string EmtyString { get; set; } = "not empty";
            public DateTime DateTime { get; set; } = DateTime.MaxValue;
            public DateTimeOffset DateTimeOffset { get; set; } = DateTimeOffset.MaxValue;
        }
        [Fact]
        public void SetProperty_Test()
        {
            object model = new TestModel();
            //In most cases blazor provides either string or primitive types from <select>
            //Generic methods assume all input values are strings and convert them to proper types of model
            object valNull = "";
            object valDateTime = "2222-02-02T14:14:14.114";
            object valInt = "125";
            object valDateTimeOffset = "2222-02-02T14:14:14.114"; //DateTimeOffset is treated like normal DateTime
            object valString = "SomeText";
            foreach(var p in typeof(TestModel).GetProperties())
            {
                switch(p.Name)
                {
                    case "String":
                        Generic.SetProperty(model, p, valString);
                        Assert.Equal("SomeText", p.GetValue(model));
                        break;
                    case "EmtyString":
                        Generic.SetProperty(model, p, valNull);
                        Assert.Equal("", p.GetValue(model));
                        break;
                    case "Int":
                        Generic.SetProperty(model, p, valInt);
                        Assert.Equal(125, p.GetValue(model));
                        break;
                    case "Null":
                        Generic.SetProperty(model, p, valNull);
                        Assert.Null(p.GetValue(model));
                        break;
                    case "DateTime":
                        Generic.SetProperty(model, p, valDateTime);
                        Assert.Equal(new DateTime(2222, 02, 02, 14, 14, 14, 114), (DateTime)p.GetValue(model));
                        break;
                    case "DateTimeOffset":
                        Generic.SetProperty(model, p, valDateTimeOffset);
                        Assert.Equal(new DateTimeOffset(new DateTime(2222, 02, 02, 14, 14, 14, 114)), (DateTimeOffset)p.GetValue(model));
                        break;
                    default:
                        throw new();
                }
            }
            
            
        }
    }
}
