using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConversionPath.Shared.Dtos
{
    public class ValidationResultDto<T>
    {
        public T? Data { get; set; }
        public ICollection<string> Messages { get; set; }
        public bool IsSuccessfull { get; set; }
    }
}
