using ServiceWithAttributes.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWithAttributes
{
    [GenerateCccDecorator]
    public class ClassWithAttribute
    {

    }

    [GenerateCccDecorator]
    public class AnotherClassWithAttribute
    {

    }

    public interface ISomeService
    {
        
    }

    [GenerateCccDecorator]
    public class SomeService : ISomeService
    {

    }
}
