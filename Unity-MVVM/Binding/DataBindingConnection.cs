using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UniRx;
using UnityEngine;
using UnityMVVM.Binding.Converters;

namespace UnityMVVM.Binding
{
    [Serializable]
    public class DataBindingConnection : IDisposable
    {
        public BindTarget SrcTarget { get { return _src; } }
        public BindTarget DstTarget { get { return _dst; } }
        public string Owner { get { return _gameObject?.name; } }

        public Action PropertyChangedAction;

        public bool isDisposed = false;

        readonly BindTarget _src;
        readonly BindTarget _dst;
        private readonly IValueConverter[] _converters;
        private IDisposable _subscription = null;
        GameObject _gameObject;

        public bool IsBound;

        public DataBindingConnection()
        { }

        public DataBindingConnection(GameObject owner, BindTarget src, BindTarget dst, IValueConverter[] converters = null)
        {
            _gameObject = owner;
            _src = src;
            _dst = dst;
            _converters = converters;

            PropertyChangedAction = OnSrcUpdated;

            BindingMonitor.RegisterConnection(this);
        }

        public void AddHandler(Action action)
        {
            PropertyChangedAction = action;
        }

        public void DstUpdated()
        {
            if (_converters != null && _converters.Length > 0)
                _src.SetValue(_converters.ChainConvertBack(_dst.GetValue(), _src.property.PropertyType, null));
            else
                _src.SetValue(Convert.ChangeType(_dst.GetValue(), _src.property.PropertyType));
        }

        internal void ClearHandler()
        {
            PropertyChangedAction = null;
            IsBound = false;
        }

        internal void Unbind()
        {
            if (!IsBound) return;
            if (_src.IsReactive)
            {
                if (!(_subscription is null))
                {
                    _subscription.Dispose();
                    _subscription = null;
                }
            }
            else
                (_src.propertyOwner as INotifyPropertyChanged).PropertyChanged -= PropertyChangedHandler;

            IsBound = false;
        }

        public static string GetName<T>(Expression<Func<T>> e)
        {
            var member = (MemberExpression)e.Body;
            return member.Member.Name;
        }

        internal void Bind()
        {
            if (IsBound) return;
            if (_src.IsReactive)
            {
//                var methodInfo = _src.propertyOwner.GetType().GetMethod("NonGenericSubscribe",BindingFlags.NonPublic|BindingFlags.Instance);
//                _subscription = (IDisposable) methodInfo.Invoke(_src.propertyOwner,
//                    new[]
//                    {
//                        new Action<object>(o => PropertyChangedHandler(_src.propertyOwner,
//                            new PropertyChangedEventArgs(_src.propertyName)))
//                    });
                _subscription = _src.ReactiveBind(PropertyChangedHandler);
            }
            else
                (_src.propertyOwner as INotifyPropertyChanged).PropertyChanged += PropertyChangedHandler;
            IsBound = true;
        }

        public void OnSrcUpdated()
        {
            try
            {
                if (_converters != null && _converters.Length > 0)
                    _dst.SetValue(_converters.ChainConvert(_src.GetValue(), _dst.property.PropertyType, null));
                else if (_src.GetValue() is IConvertible)
                    _dst.SetValue(Convert.ChangeType(_src.GetValue(), _dst.property.PropertyType));
                else
                    _dst.SetValue(_src.GetValue());
            }
            catch (Exception e)
            {
                Debug.LogError("Data binding error in: " + _gameObject.name + ": " + e.Message);

                if (e.InnerException != null)
                    Debug.LogErrorFormat("Inner Exception: {0}", e.InnerException.Message);
            }
        }

        public void SetHandler(Action handler)
        {
            PropertyChangedAction = handler;
        }

        public static object GetOwner<T>(Expression<Func<T>> e)
        {
            var member = (MemberExpression)e.Body;
            return member.Expression.Type;
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(_src.propertyName))
                PropertyChangedAction?.Invoke();
        }

        public void Dispose()
        {
            BindingMonitor.UnRegisterConnection(this);

            Dispose(true);
        }

        void Dispose(bool disposing)
        {
            if (isDisposed)
                return;

            if (disposing && _src.propertyOwner != null)
            {
                var notifyPropertyChanged = _src.propertyOwner as INotifyPropertyChanged;
                if (notifyPropertyChanged != null)
                {
                    notifyPropertyChanged.PropertyChanged -= PropertyChangedHandler;
                }
            }

            isDisposed = true;
        }

    }
}
