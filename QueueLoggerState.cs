using System;

namespace QueueLogger
{
    public class QueueLoggerState<T> : IDisposable
    {
        T currentState;

        public QueueLoggerState(T currentState) => this.currentState = currentState;

        public void Dispose()
        {
            currentState = default;
        }
    }
}
