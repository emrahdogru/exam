using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Utility
{
    public class AppSettings
    {
        private static AppSettings current = new AppSettings();

        private AppSettings() { }

        public string TokenSecret { get; set; } = "-- secret key --";

        public static AppSettings Current
        {
            get
            {
                if (current == null)
                    current = new AppSettings();

                return current;
            }
        }
    }
}
