﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Utility.Exceptions
{
    [Serializable]
    public class UserException : Exception
    {
        public UserException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public UserException(string message)
            : base(message)
        { }
    }


}
