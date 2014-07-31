namespace FiddlerCoreConsoleApplication
{
    using System;

    class Program
    {
        static int Main(string[] args)
        {
            string hostToSniff = "localhost";
            if (args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
            {
                hostToSniff = args[0];
            }

            var fiddlerEngine = new FiddlerEngine(hostToSniff);
            fiddlerEngine.Start();
            Console.ReadKey();
            fiddlerEngine.Stop();
            return 0;
        }
    }
}
