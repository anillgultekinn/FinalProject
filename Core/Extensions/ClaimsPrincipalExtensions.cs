using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        //bir kişinin claimlerini ararken .net bizi biraz uğraştırır.
        //ClaimsPrincipal bir kişinin claimlerine ulaşmak için .net de olan bir class dır. 
        public static List<string> Claims(this ClaimsPrincipal claimsPrincipal, string claimType)//ilgili claimtype göre getirir
        {
            var result = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value).ToList();
            return result;
        }

        public static List<string> ClaimRoles(this ClaimsPrincipal claimsPrincipal) //direkt rollere göre döner
        {
            return claimsPrincipal?.Claims(ClaimTypes.Role);
        }
    }
}