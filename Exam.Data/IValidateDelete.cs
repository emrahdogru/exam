using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Data
{
    public interface IValidateDelete
    {
        /// <summary>
        /// Kayıt silinebilir mi? Silinemez ise <see cref="Exam.Utility.Exceptions.UserException"/> fırlatır.
        /// </summary>
        void ValidateDelete();
    }
}
