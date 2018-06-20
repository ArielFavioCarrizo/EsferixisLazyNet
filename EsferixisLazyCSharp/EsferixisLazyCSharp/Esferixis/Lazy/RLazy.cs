using EsferixisLazyCSharp.Esferixis.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EsferixisLazyCSharp.Esferixis.Lazy
{
    public sealed class RLazy<T>
    {
        private Action calculateValue;
        private int mustCalculateValue;

        private Task<T> deferredValue;

        /**
         * @post Creates a lazy value with the specified value generation function
         */
        public RLazy(Action<TaskCompletionSource<T>> generateValue)
        {
            Preconditions.checkNotNull(generateValue, "generateValue");

            TaskCompletionSource<T> valuePromise = new TaskCompletionSource<T>();

            this.calculateValue = Trampoline.encapsulate( () => generateValue(valuePromise) );

            this.deferredValue = valuePromise.Task;
        }

        /**
         * @post Creates a lazy value with the specified async function
         */
        public RLazy( Func<Task<T>> generateValue ) :
            this( taskCompletionSource => generateValue().sendTo( taskCompletionSource )) {}

        public RLazy(Func<T> generateValue) :
            this(taskCompletionSource => taskCompletionSource.SetResult(generateValue())) {}
        /**
         * @post Creates a lazy value with the specified immediate value
         */
        public RLazy( T value )
        {
            this.calculateValue = null;
            this.mustCalculateValue = 0;

            this.deferredValue = Task<T>.FromResult(value);
        }

        public static implicit operator RLazy<T>(T value)
        {
            return new RLazy<T>(value);
        }

        /**
         * @post Gets the value
         */
        public async Task<T> get()
        {
            this.calculate();

            return await this.deferredValue;
        }

        /**
         * @post Calculates the value
         */
        private void calculate()
        {
            if ( Interlocked.Exchange(ref this.mustCalculateValue, 0) != 0 )
            {
                Action calculateValue = this.calculateValue;
                this.calculateValue = null;

                calculateValue();
            }
        }
    }
}
