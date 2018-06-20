using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsferixisLazyCSharp.Esferixis.Lazy.Functional
{
    public delegate RLazy<S> LFunction<T, S>(RLazy<T> input);
}
