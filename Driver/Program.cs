using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Driver
{
    class Program
    {
        static void Main(string[] args)
        {
            string json =
                "{" +
                "name = 'foo'," +
                "value= 'bar' ," +
                "object: { " +
                "a = 1, " +
                "b = true } }";

            string json2 =
                "{" +
                "name = 'foo'," +
                "value= 'bar' ," +
                "object:{ " +
                "a =1, " +
                "b= true, baz:{" +
                "c = 'false'}, " +
                "new: { } " + 
                "} }";


            JsonParser.JsonParser parser = new JsonParser.JsonParser();
            parser.Print(json);
            parser.Print(json2);
            Console.ReadLine();
        }
    }
}
