using System;
using UniRx;

namespace UnityMVVM.Reactive
{
    public class ReactiveCommand<T> : UniRx.ReactiveCommand<T>, IBoxedSubscribe
    {
        public ReactiveCommand()
            : base()
        { }
        public ReactiveCommand(IObservable<bool> canExecuteSource, bool initialValue = true)
            : base(canExecuteSource, initialValue)
        {
        }

        public IDisposable NonGenericSubscribe(Action<object> onNext)
        {
            return CanExecute.Subscribe(obj => onNext.Invoke(obj));
        }
    }

    public class ReactiveCommand : ReactiveCommand<Unit>
    {
        public ReactiveCommand()
            : base()
        { }

        /// <summary>
        /// CanExecute is changed from canExecute sequence.
        /// </summary>
        public ReactiveCommand(IObservable<bool> canExecuteSource, bool initialValue = true)
            : base(canExecuteSource, initialValue)
        {
        }

        /// <summary>Push null to subscribers.</summary>
        public bool Execute()
        {
            return Execute(Unit.Default);
        }

        /// <summary>Force push parameter to subscribers.</summary>
        public void ForceExecute()
        {
            ForceExecute(Unit.Default);
        }
        internal IDisposable NonGenericSubscribe(Action<object> onNext)
        {
            return CanExecute.Subscribe(obj => onNext.Invoke(obj));
        }
    }
}
