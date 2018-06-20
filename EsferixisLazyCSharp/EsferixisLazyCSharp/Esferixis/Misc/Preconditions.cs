using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsferixisLazyCSharp.Esferixis.Misc
{
    public static class Preconditions
    {
        public static void checkNotNull<T>(T value, String parameterName)
        {
            if ( parameterName == null )
            {
                throw new ArgumentNullException(parameterName, value);
            }
        }
    }
}
