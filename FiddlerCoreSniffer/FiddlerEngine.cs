namespace FiddlerCoreConsoleApplication
{
    using Fiddler;
    using System;
    using System.Collections.Generic;

    public class FiddlerEngine
    {
        public string HostToSniff { get; private set; }

        public FiddlerEngine(string hostToSniff)
        {
            HostToSniff = hostToSniff;
        }

        public void Start()
        {
            FiddlerApplication.AfterSessionComplete += HandleFiddlerSessionComplete;
            FiddlerApplication.Startup(8888, FiddlerCoreStartupFlags.CaptureLocalhostTraffic | FiddlerCoreStartupFlags.RegisterAsSystemProxy);
        }

        public void Stop()
        {
            FiddlerApplication.AfterSessionComplete -= HandleFiddlerSessionComplete;

            if (FiddlerApplication.IsStarted())
            {
                FiddlerApplication.Shutdown();
            }
        }

        private void HandleFiddlerSessionComplete(Session session)
        {
            // Ignore HTTPS connect requests
            if (session.RequestMethod == "CONNECT")
            {
                return;
            }

            if (session.hostname.ToLower() != this.HostToSniff)
            {
                return;
            }

            string url = session.fullUrl.ToLower();

            var extensions = new List<string> { ".ico", ".gif", ".jpg", ".png", ".axd", ".css" };
            foreach (var ext in extensions)
            {
                if (url.Contains(ext))
                {
                    return;
                }
            }

            if (session == null || session.oRequest == null || session.oRequest.headers == null)
            {
                return;
            }

            string headers = session.oRequest.headers.ToString();
            var reqBody = session.GetRequestBodyAsString();

            Console.WriteLine(headers);
            if (!string.IsNullOrEmpty(reqBody))
            {
                Console.WriteLine(string.Join(Environment.NewLine, reqBody.Split(new char[] {'&'})));
            }

            Console.WriteLine(Environment.NewLine);
            
            // if you wanted to capture the response
            //string respHeaders = session.oResponse.headers.ToString();
            //var respBody = session.GetResponseBodyAsString();
        }
    }
}