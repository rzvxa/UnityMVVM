using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;
using System;
using System.Linq;
using System.Reflection;
using UnityMVVM.Util;
using UnityMVVM.Model;
using UniRx;
using Util;

namespace UnityMVVM.Binding
{
    public class CollectionViewSource : DataBindingBase
    {
        INotifyCollectionChanged srcCollection;

        [HideInInspector]
        public List<BindablePropertyInfo> SrcCollections = new List<BindablePropertyInfo>();

        [HideInInspector]
        public BindablePropertyInfo SrcCollectionName;


        public Action<int, object> OnElementAdded;
        public Action<int, object> OnElementRemoved;
        public Action<int, object, object> OnElementReplaced;
        public Action<int, int> OnElementMove;
        public Action OnCollectionReset;

        public Action<IModel> OnSelectedItemUpdated;

        public IModel SelectedItem
        {
            get { return _selectedItem; }

            set
            {
                if (value != _selectedItem)
                {
                    _selectedItem = value;

                    OnSelectedItemUpdated?.Invoke(value);
                }
            }
        }

        IModel _selectedItem;

        BindTarget src;

        DataBindingConnection _conn;

        private IDisposable _subscription = null;

        bool isBound = false;

        public override bool KeepConnectionAliveOnDisable => true;

        public int Count
        {
            get
            {
                return (src.GetValue() as IList).Count;
            }
        }

        public string SelectedItemPropName;

        public object this[int key]
        {
            get
            {
                var list = (src.GetValue() as IList);
                return list[key];
            }
            set
            {
                (src.GetValue() as IList)[key] = value;
            }
        }

        public override void RegisterDataBinding()
        {
            if (isBound) return;
            if (_viewModel == null)
            {
                Debug.LogErrorFormat("Binding Error | Could not Find ViewModel {0} for collection {1}", ViewModelName,
                    SrcCollectionName);

                return;
            }

            src = SrcCollectionName.ToBindTarget(_viewModel, true);
            if (!(src is null))
                _subscription = src.ReactiveCollectionBind(CollectionAdd, CollectionRemove, CollectionReplace,
                    CollectionMove, CollectionReset);


            isBound = true;
        }

        public override void UnregisterDataBinding()
        {
            if (!isBound) return;
            if (!(_subscription is null))
            {
                _subscription.Dispose();
                _subscription = null;
            }
            if (srcCollection != null)
                srcCollection.CollectionChanged -= CollectionChanged;
            isBound = false;
        }

        protected virtual void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
//            switch (e.Action)
//            {
//                case NotifyCollectionChangedAction.Add:
//                    OnElementsAdded?.Invoke(e.NewStartingIndex, e.NewItems);
//                    break;
//                case NotifyCollectionChangedAction.Move:
//                    break;
//                case NotifyCollectionChangedAction.Remove:
//                    OnElementsRemoved?.Invoke(e.OldStartingIndex, e.OldItems);
//                    break;
//                case NotifyCollectionChangedAction.Replace:
//                    OnElementUpdated.Invoke(e.NewStartingIndex, e.NewItems);
//                    break;
//                case NotifyCollectionChangedAction.Reset:
//                    OnCollectionReset?.Invoke(e.NewStartingIndex, e.NewItems);
//                    break;
//                default:
//                    break;
//            }

            OnSelectedItemUpdated?.Invoke(SelectedItem);
        }

        protected virtual void CollectionAdd(Reactive.CollectionAddEvent e) => OnElementAdded?.Invoke(e.Index, e.Value);

        protected virtual void CollectionRemove(Reactive.CollectionRemoveEvent e) => OnElementRemoved?.Invoke(e.Index, e.Value);

        protected virtual void CollectionReplace(Reactive.CollectionReplaceEvent e) => OnElementReplaced?.Invoke(e.Index, e.OldValue, e.NewValue);
        protected virtual void CollectionMove(Reactive.CollectionMoveEvent e) => OnElementMove?.Invoke(e.OldIndex, e.NewIndex);

        protected virtual void CollectionReset(Unit e) => OnCollectionReset?.Invoke();

        public override void UpdateBindings()
        {
            base.UpdateBindings();

            if (!string.IsNullOrEmpty(ViewModelName))
            {
                var props = ViewModelProvider.GetViewModelFields(ViewModelName);

                SrcCollections = props.Where(prop =>
                        prop.FieldType.IsAssignableToGenericType(typeof(ReactiveCollection<>))
                        && !prop.GetCustomAttributes(typeof(ObsoleteAttribute), true).Any()
                    ).Select(e => new BindablePropertyInfo(e.Name, e.FieldType.GenericTypeArguments[0].Name)).ToList();


            }
        }
        protected override void OnDestroy()
        {
            if (_conn != null && !_conn.isDisposed)
                _conn.Dispose();

            base.OnDestroy();
        }
    }
}
