using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Extenstions
{
    public static class ClaimPrincipleExtentions
    {
        public static string GetUserName(this ClaimsPrincipal user){
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }
        public static int GetUserId(this ClaimsPrincipal user){
            // return 11;
            return  int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}