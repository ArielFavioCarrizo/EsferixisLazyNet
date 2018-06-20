using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsferixisLazyCSharp.Esferixis.Misc
{
    public sealed class Optional<T>
    {
        private T value_m;
        private bool isEmpty_m;

        public Optional(T value)
        {
            this.value_m = value;
            this.isEmpty_m = false;
        }

        public Optional()
        {
            this.isEmpty_m = true;
        }

        public T GetValue()
        {
            if ( !this.isEmpty() )
            {
                return this.value_m;
            }
            else
            {
                throw new InvalidOperationException("Empty value");
            }
        }

        public bool isEmpty()
        {
            return this.isEmpty_m;
        }
    }
}
