using System;
using UnityEngine;
using System.Collections;

namespace UnityMVVM.Binding
{
    [Serializable]
    public class BindablePropertyInfo
    {
        public string PropertyName;
        public string PropertyType;
        public bool IsStatic;

        public BindablePropertyInfo()
        {
        }

        public BindablePropertyInfo(string propertyName)
        {
            PropertyName = propertyName;
        }

        public BindablePropertyInfo(string propertyName, string propertyType, bool isStatic = false)
        {
            PropertyName = propertyName;
            PropertyType = propertyType;
            IsStatic = isStatic;
        }

        public BindTarget ToBindTarget(object owner, bool isReactive = false, string path = null,
            UnityEngine.Events.UnityEvent dstChangedEvent = null) =>
            new BindTarget(owner, PropertyName, path, dstChangedEvent, isReactive && !IsStatic,
                PropertyType.Replace(" ", "").ToLower().Equals("canexecute"));
    }
}
