using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityMVVM.Binding;
using UnityMVVM.Model;

namespace UnityMVVM
{
    namespace View
    {
        [RequireComponent(typeof(CollectionViewSource))]
        public class CollectionViewBase : ViewBase
        {
            [SerializeField]
            protected GameObject _listItemPrefab;

            public List<GameObject> InstantiatedItems = new List<GameObject>();

            [SerializeField]
            protected CollectionViewSource _src;

            public UnityEvent<IModel> OnSelectionChanged { get; set; }

            // Use this for initialization
            protected void Awake()
            {
                if (_src != null)
                {
                    _src.OnElementAdded += AddElement;
                    _src.OnElementRemoved += RemoveElement;
                    _src.OnElementReplaced += ReplaceElement;
                    _src.OnElementMove += MoveElement;
                    _src.OnCollectionReset += ResetView;

                }

                _src.OnSelectedItemUpdated += UpdateSelectedItem;
            }

            protected virtual void MoveElement(int oldIndex, int newIndex)
            {
                // Do Nothing
            }

            protected virtual void ReplaceElement(int index, object oldValue, object newValue)
            {
                // Do Nothing
            }

            private void UpdateSelectedItem(IModel obj)
            {
                var items = InstantiatedItems.Select(e => e.GetComponent<ICollectionViewItem>()).ToList();

                foreach (var item in items)
                    item?.SetSelected(item.Model == obj);
            }

            public override void SetVisibility(Visibility visibility)
            {
                switch (visibility)
                {
                    case Visibility.Visible:
                        UpdateChildVisibilities(visibility);
                        gameObject.SetActive(true);
                        this.CancelAnimation();
                        this.FadeIn(fadeTime: _fadeTime);
                        break;
                    case Visibility.Hidden:
                        UpdateChildVisibilities(visibility);
                        gameObject.SetActive(true);
                        this.CancelAnimation();
                        this.FadeOut(fadeTime: _fadeTime);
                        break;
                    case Visibility.Collapsed:
                        this.FadeOut(() =>
                        {
                            UpdateChildVisibilities(visibility);
                            gameObject.SetActive(false);
                        }, fadeTime: _fadeTime);
                        break;
                    default:
                        break;
                }

            }

            void UpdateChildVisibilities(Visibility v)
            {
                foreach (var item in InstantiatedItems)
                {
                    item.SetActive(v.Equals(Visibility.Collapsed) ? false : true);
                }
            }


            protected void OnDestroy()
            {
                if (_src != null)
                {
                    _src.OnElementAdded -= AddElement;
                    _src.OnElementRemoved -= RemoveElement;
                    _src.OnElementReplaced -= ReplaceElement;
                    _src.OnElementMove -= MoveElement;
                    _src.OnCollectionReset -= ResetView;
                }
            }

            protected virtual void InitItem(GameObject go, object item, int index)
            {
                var model = (item as IModel);
                if (model != null)
                {
                    var it = go.GetComponent<ICollectionViewItem>() as ICollectionViewItem;
                    if (it != null)
                    {
                        it.Model = model;
                        it.Init(model);
                    }
                }
            }


            protected virtual void ResetView()
            {
                foreach (Transform t in transform)
                    GameObject.Destroy(t.gameObject);

                InstantiatedItems.Clear();
            }

            //
            // Override this method to create the gameobject that will spawn in your CollectionView
            //
            protected virtual GameObject CreateCollectionItem(object ListItem, Transform parent)
            {
                var go = GameObject.Instantiate(_listItemPrefab, transform);

                return go;
            }

            protected virtual void AddElement(int index, object newItem)
            {
                var go = CreateCollectionItem(newItem, transform);
                go.transform.SetSiblingIndex(index);

                InitItem(go, newItem, index);

                InstantiatedItems.Insert(index, go);
            }

            protected virtual void RemoveElement(int index, object oldItem)
            {
                if (index > InstantiatedItems.Count) throw new IndexOutOfRangeException();
                Destroy(InstantiatedItems[index]);
                InstantiatedItems[index] = null;
            }

            private void OnValidate()
            {
                _src = GetComponent<CollectionViewSource>();
            }
        }
    }
}


