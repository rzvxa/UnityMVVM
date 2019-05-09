namespace UnityMVVM.Reactive
{
   interface IBoxedSubscribe
   {
       System.IDisposable NonGenericSubscribe(System.Action<object> onNext);
   }
}
