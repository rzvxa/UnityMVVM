using System;

namespace UnityMVVM.Reactive
{
    public static class MVVMReactiveCommandExtensions
    {
        public static ReactiveCommand ToMVVMReactiveCommand(this IObservable<bool> canExecuteSource, bool initialValue = true)
        {
            return new ReactiveCommand(canExecuteSource, initialValue);
        }
    }
}