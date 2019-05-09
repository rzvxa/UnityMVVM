using System;
using UniRx;

namespace UnityMVVM.Reactive
{
    public class ReactiveCollection<T> : UniRx.ReactiveCollection<T>, IBoxedCollectionSubscribe
    {
        public IDisposable NonGenericSubscribeAdd(Action<object> onNext)
        {
            return ObserveAdd().Subscribe(e => onNext.Invoke(new CollectionAddEvent(e.Index, e.Value)));
        }

        public IDisposable NonGenericSubscribeRemove(Action<object> onNext)
        {
            return ObserveRemove().Subscribe(e => onNext.Invoke(new CollectionRemoveEvent(e.Index, e.Value)));
        }

        public IDisposable NonGenericSubscribeReplace(Action<object> onNext)
        {
            return ObserveReplace().Subscribe(e =>
                onNext.Invoke(new CollectionReplaceEvent(e.Index, e.OldValue, e.NewValue)));
        }

        public IDisposable NonGenericSubscribeMove(Action<object> onNext)
        {
            return ObserveMove()
                .Subscribe(e => onNext.Invoke(new CollectionMoveEvent(e.OldIndex, e.NewIndex, e.Value)));
        }

        public IDisposable NonGenericSubscribeReset(Action<object> onNext)
        {
            return ObserveReset()
                .Subscribe(o => onNext.Invoke(o));
        }
    }
}
