namespace UnityMVVM.Reactive
{
    interface IBoxedCollectionSubscribe
    {
        System.IDisposable NonGenericSubscribeAdd(System.Action<object> onNext);
        System.IDisposable NonGenericSubscribeRemove(System.Action<object> onNext);
        System.IDisposable NonGenericSubscribeReplace(System.Action<object> onNext);
        System.IDisposable NonGenericSubscribeMove(System.Action<object> onNext);
        System.IDisposable NonGenericSubscribeReset(System.Action<object> onNext);
    }
}
