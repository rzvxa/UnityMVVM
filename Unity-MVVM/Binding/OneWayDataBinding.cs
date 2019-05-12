using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UniRx;
using UnityEngine;
using UnityMVVM.Binding.Converters;
using UnityMVVM.Util;
using Util;

namespace UnityMVVM.Binding
{
    public class OneWayDataBinding
        : DataBindingBase
    {
        public DataBindingConnection Connection { get { return _connection; } }

        protected DataBindingConnection _connection;

        [HideInInspector]
        public List<BindablePropertyInfo> SrcProps = new List<BindablePropertyInfo>();

        [HideInInspector]
        public List<BindablePropertyInfo> DstProps = new List<BindablePropertyInfo>();


        [HideInInspector]
        public BindablePropertyInfo SrcPropertyName = null;

        [HideInInspector]
        public BindablePropertyInfo DstPropertyName = null;

        [SerializeField]
        public UnityEngine.Component _dstView;

        [SerializeField][HideInInspector]
        protected ValueConverterBase[] Converters;

        [HideInInspector]
        protected string PropertyPath = null;

        private IValueConverter[] IConverters => Array.ConvertAll(Converters, x => (IValueConverter) x);

        bool _isStartup = true;

        public override void RegisterDataBinding()
        {
            base.RegisterDataBinding();

            if (_viewModel == null)
            {
                Debug.LogErrorFormat("Binding Error | Could not Find ViewModel {0} for Property {1}", ViewModelName, SrcPropertyName);

                return;
            }
            if (_connection == null)
            {
                _connection = new DataBindingConnection(
                    gameObject, SrcPropertyName.ToBindTarget(_viewModel, true,
                        PropertyPath), DstPropertyName.ToBindTarget(_dstView), IConverters);
            }

            _connection.Bind();
        }

        public override void UnregisterDataBinding()
        {
            base.UnregisterDataBinding();

            if (_connection != null)
                _connection.Unbind();
        }

        public override void UpdateBindings()
        {
            base.UpdateBindings();

            if (_dstView != null)
            {
                var props = _dstView.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                DstProps =  props.Where(prop => prop.GetSetMethod(false) != null
                                                && prop.GetSetMethod(false) != null
                                                && !prop.GetCustomAttributes(typeof(ObsoleteAttribute), true).Any()
                ).Select(e => new BindablePropertyInfo {PropertyName = e.Name, PropertyType = e.PropertyType.Name}).ToList();

            }

            if (!string.IsNullOrEmpty(ViewModelName))
            {
                var props = ViewModelProvider.GetViewModelProperties(ViewModelName);
//                SrcProps = props.Where(prop =>
//                        prop.PropertyType.IsAssignableToGenericType(typeof(Reactive.ReactiveProperty<>))
//                        && !prop.GetCustomAttributes(typeof(ObsoleteAttribute), true)
//                            .Any())
                SrcProps = SrcPropsSearch(props).Select(e => new BindablePropertyInfo(e.Name,
                    e.PropertyType.IsGenericType
                        ? e.PropertyType.GetGenericArguments()[0].Name
                        : e.PropertyType.BaseType.GetGenericArguments()[0].Name)).ToList();
                SrcProps.AddRange(GetExtraViewModelProperties(props));
            }
        }

        public virtual IEnumerable<PropertyInfo> SrcPropsSearch(IEnumerable<PropertyInfo> props) =>
            props.Where(prop =>
                prop.PropertyType.IsAssignableToGenericType(typeof(Reactive.ReactiveProperty<>))
                && !prop.GetCustomAttributes(typeof(ObsoleteAttribute), true)
                    .Any());

        protected virtual IEnumerable<BindablePropertyInfo> GetExtraViewModelProperties(PropertyInfo[] props)
        {
            return props.Where(field => field.PropertyType.IsAssignableToGenericType(typeof(Reactive.ReactiveCommand<>))
                    && !field.GetCustomAttributes(typeof(ObsoleteAttribute), true)
                    .Any()
                    ).Select(e => new BindablePropertyInfo(e.Name, "Can Execute"));
        }

        private void Start()
        {
            _connection.OnSrcUpdated();
            _isStartup = false;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (!_isStartup)
                _connection.OnSrcUpdated();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_connection != null)
                _connection.Dispose();
        }
    }
}
