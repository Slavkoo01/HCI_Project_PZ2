using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Helper
{
    using System;
    using System.IO;

    public class LogWriter
    {
        private readonly string filePath;

        public LogWriter(string filePath)
        {
            this.filePath = filePath;
        }

        public  void AppendToLog(string message)
        {
            try
            {
                using (StreamWriter writer = File.AppendText(filePath))
                {
                    writer.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
    }
}
