using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class Result : IResult
    {

        public Result(bool success, string message) : this(success)  //this(success) => tek parametreli olanı çalıştır.
        {
            //this => classın kendisi demektir. Result

            Message = message;
            //Success = success;

            //success set etme işlemi aşağıdaki kodda çalışacaktır. çünkü eğer mesaj varsa success set etme işlemide olacaktır
            //DRY
        }

        public Result(bool success)
        {
            Success = success;
            //product manager mesaj kısmını boş bırakabiliriz.
            //overloading
        }
        // ** get readonly dir ve readonly ler constuctor da set edilebilir.
        public bool Success { get; }
        public string Message { get; }
    }
}
