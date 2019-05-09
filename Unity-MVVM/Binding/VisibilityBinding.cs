using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityMVVM.Binding;

namespace UnityMVVM.Binding
{
    public class VisibilityBinding : OneWayDataBinding
    {
        public override bool KeepConnectionAliveOnDisable => true;

        protected override IEnumerable<BindablePropertyInfo> GetExtraViewModelProperties(FieldInfo[] fields)
        {
            return new BindablePropertyInfo[0];
        }
    }
}
