using Flection_Sharp;
using test.test;


namespace test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var type = flecion.getSingleType("test", "MyTestClass");
            var data = flecion.dynamicToObject(type, new { Title = "the is title", value = "the is value" }) as MyTestClass;
            Console.WriteLine(data.title);
        }
    }
}
