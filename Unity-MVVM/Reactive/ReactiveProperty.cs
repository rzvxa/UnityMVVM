using System;
using UniRx;

namespace UnityMVVM.Reactive
{
    public class ReactiveProperty<T> : UniRx.ReactiveProperty<T>, IBoxedSubscribe
    {
        public ReactiveProperty()
        {
        }

        public ReactiveProperty(T i) : base(i)
        {
        }

        public IDisposable NonGenericSubscribe(Action<object> onNext)
        {
            return this.Subscribe(obj => onNext.Invoke(obj));
        }
    }
}
