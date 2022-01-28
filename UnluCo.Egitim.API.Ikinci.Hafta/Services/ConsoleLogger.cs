using System;

namespace UnluCo.Egitim.API.Ikinci.Hafta.Services
{
    public class ConsoleLogger: ILoggerService
    {
        public void Write(string message)
        {
            Console.WriteLine("[ConsoleLogger] - " + message);
        }
    }
}
