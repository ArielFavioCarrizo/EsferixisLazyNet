using EsferixisLazyCSharp.Esferixis.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsferixisLazyCSharp.Esferixis.Lazy
{
    public static class Trampoline
    {
        [ThreadStatic]
        private static Queue<Action> pendingTasks;

        [ThreadStatic]
        private static int numberOfLoops;

        /**
         * @pre La acción no puede ser nula
         * @post Agrega una tarea al trampoline
         */
        public static void addTask(Action action)
        {
            Preconditions.checkNotNull(action, "action");

            pendingTasks.Enqueue(action);

            if ( numberOfLoops == 0 )
            {
                doAllPendingTasks();
            }
        }

        /**
         * @post Realiza todas las tareas pendientes
         */
        public static void doAllPendingTasks()
        {
            numberOfLoops++;

            while ( pendingTasks.Count > 0 )
            {
                pendingTasks.Dequeue()();
            }

            numberOfLoops--;
        }

        /**
         * @post Encapsulates the action
         */
        public static Action encapsulate(Action action)
        {
            Preconditions.checkNotNull(action, "action");

            return () => addTask(action);
        }

        /**
         * @post Asegura la inicialización
         */
        private static void ensureInitialization()
        {
            if ( pendingTasks == null )
            {
                pendingTasks = new Queue<Action>();
            }
        }
    }
}
