using EsferixisLazyCSharp.Esferixis.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsferixisLazyCSharp.Esferixis.Lazy.Functional
{
    public sealed class LazyList<T>
    {
        private struct Next
        {
            private RLazy<T> head_m;
            private RLazy<LazyList<T>> tail_m;

            public Next(RLazy<T> head, RLazy<LazyList<T>> tail)
            {
                Preconditions.checkNotNull(head, "head");
                Preconditions.checkNotNull(tail, "tail");

                this.head_m = head;
                this.tail_m = tail;
            }

            public RLazy<T> head() => this.head_m;
            public RLazy<LazyList<T>> tail() => this.tail_m;
        };

        private Optional<Next> next_m;

        /**
         * @post Creates a empty list
         */
        public LazyList()
        {
            this.next_m = new Optional<Next>();
        }

        public LazyList(RLazy<T> head, RLazy<LazyList<T>> tail)
        {
            this.next_m = new Optional<Next>(new Next(head, tail));
        }

        public LazyList(RLazy<T> head)
        {
            this.next_m = new Optional<Next>(new Next(head, new LazyList<T>() ));
        }

        /**
         * @post Return if it's empty
         */
        public bool isEmpty() => this.next_m.isEmpty();

        /**
         * @post Returns head
         */
        public RLazy<T> head() => this.next().head();

        /**
         * @post Returns tail
         */
        public RLazy<LazyList<T>> tail() => this.next().tail();

        /**
         * @post Maps in the image of specified function
         */
        public RLazy<LazyList<S>> map<S>(RLazy<LFunction<T, S>> function) => new RLazy<LazyList<S>>( async () =>
        {
            if ( this.isEmpty() )
            {
                return new LazyList<S>();
            }
            else
            {
                return new LazyList<S>( (await function.get())(this.head()), (await this.tail().get()).map<S>(function) );
            }
        });

        /**
         * @post Zip with the specified list and the specified function
         */
        public RLazy<LazyList<U>> zipWith<S, U>( RLazy<LazyList<S>> otherList, RLazy<Func< RLazy<T>, RLazy<S>, RLazy<U>>> function) => new RLazy<LazyList<U>>(async () =>
        {
            LazyList<S> otherList_eager = await otherList.get();

            if (this.isEmpty() || otherList_eager.isEmpty())
            {
                return new LazyList<U>();
            }
            else
            {
                return new LazyList<U>((await function.get())(this.head(), otherList_eager.head()), (await this.tail().get()).zipWith<S, U>(otherList_eager.tail(), function));
            }
        });

        /**
         * @post Returns next
         */
        private Next next()
        {
            if ( !this.next_m.isEmpty())
            {
                return this.next_m.GetValue();
            }
            else {
                throw new InvalidOperationException("Unexpected empty list");
            }
        }
    }
}
