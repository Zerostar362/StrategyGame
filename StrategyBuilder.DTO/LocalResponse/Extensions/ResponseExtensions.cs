using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.DTO.LocalResponse.Extensions
{
    public static class ResponseExtensions
    {
        public static Type GetDTOType(this Response response)
        {
            var type = Type.GetType(response.Head.DTOName);

            if (type is null)
                throw new NullReferenceException($"{response.Head.DTOName} type not found");

            return type;
        }

        public static bool TryGetValue<T>(this Response response, out T? value) where T : class
        {
            var type = response.GetDTOType();
            var Ttype = typeof(T);


            if(Ttype == type)
            {
                value = response.Payload.Content as T;
                return true;
            }

            value = null;
            return false;
        }
    }
}
