using System;

namespace ApiGum.Sdk.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting..");
            //CSharpGenerator.WriteFile();
            JavaScriptGenerator.WriteFile();
            Console.WriteLine("Job completed !");
        }
       

    }
}
