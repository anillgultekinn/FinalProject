using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Business
{
    public class BusinessRules
    {
        //params verdiğimiz zaman  Run içerisine istediğimiz kadar IResult verebliyoruz
        public static IResult Run(params IResult[] logics) //logics = iş kuralı
        {
            foreach (var logic in logics) //her bir iş kuralını gez
            {
                if (!logic.Success)
                {
                    return logic; //başarısız olanı business a  haber veriyoruz (iş kuralının kendisimi)
                }
            }
            return null;
        }
    }
}
