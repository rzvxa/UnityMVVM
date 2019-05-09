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
        public object PropertyOwner;

        public BindablePropertyInfo()
        {
        }

        public BindablePropertyInfo(string propertyName)
        {
            PropertyName = propertyName;
        }

        public BindablePropertyInfo(string propertyName, string propertyType)
        {
            PropertyName = propertyName;
            PropertyType = propertyType;
        }

        public BindablePropertyInfo(string propertyName, string propertyType, object propertyOwner)
        {
            PropertyName = propertyName;
            PropertyType = propertyType;
            PropertyOwner = propertyOwner;
        }

        public BindTarget ToBindTarget(object owner, bool isReactive = false, string path = null,
            UnityEngine.Events.UnityEvent dstChangedEvent = null) =>
            new BindTarget(owner, PropertyName, path, dstChangedEvent, isReactive,
                PropertyType.Replace(" ", "").ToLower().Equals("canexecute"));
    }
}
