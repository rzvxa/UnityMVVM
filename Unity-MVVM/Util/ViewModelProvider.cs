using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityMVVM.ViewModel;

namespace UnityMVVM.Util
{
    public class ViewModelProvider : Singleton<ViewModelProvider>
    {
        public static List<string> Viewmodels
        {
            get
            {
                if (_viewModels == null)
                    _viewModels = GetViewModels();

                return _viewModels;

            }
        }
        static List<string> _viewModels = null;

        public static List<string> GetViewModels()
        {
            //Assembly mscorlib = Assembly.GetExecutingAssembly();
            Type t = typeof(ViewModelBase);


            return t.Assembly.GetTypes().Where(e => e.IsSubclassOf(t)).Select(e => e.ToString()).ToList();
        }

        public static Type GetViewModelType(string typeString)
        {
            Assembly asm = typeof(ViewModelBase).Assembly;
            return asm.GetType(typeString);
        }

        public static PropertyInfo[] GetViewModelProperties(string viewModelTypeString)
        {
            Assembly asm = typeof(ViewModelBase).Assembly;
            return asm.GetType(viewModelTypeString).GetProperties();
        }

        public static FieldInfo[] GetViewModelFields(string viewModelTypeString)
        {
            Assembly asm = typeof(ViewModelBase).Assembly;
            return asm.GetType(viewModelTypeString).GetFields();
        }

        internal ViewModelBase GetViewModelBehaviour(string viewModelName)
        {

            var vm = GetComponent(ViewModelProvider.GetViewModelType(viewModelName));
            if(vm == null)
                vm = FindObjectOfType(ViewModelProvider.GetViewModelType(viewModelName)) as ViewModelBase;

            if (vm == null)
                return gameObject.AddComponent(ViewModelProvider.GetViewModelType(viewModelName)) as ViewModelBase;

            return vm as ViewModelBase;

        }

        public static PropertyInfo[] GetViewModelProperties(string viewModelTypeString, BindingFlags bindingAttr = BindingFlags.Default)
        {
            Assembly asm = typeof(ViewModelBase).Assembly;
            return asm.GetType(viewModelTypeString).GetProperties(bindingAttr);
        }

        public static bool IsViewModelTypeNameValid(string typeString)
        {
            return !string.IsNullOrEmpty(typeString) && Viewmodels.Contains(typeString);
        }

        internal static MethodInfo[] GetViewModelMethods(string viewModelName, BindingFlags bindingFlags = 0)
        {
            Assembly asm = typeof(ViewModelBase).Assembly;
            return asm.GetType(viewModelName).GetMethods(bindingFlags);
        }

        internal static MethodInfo[] GetViewModelBindEventMethods(string viewModelName)
        {
            var asm = typeof(ViewModelBase).Assembly;
            return asm.GetType(viewModelName).GetMethods();
        }

    }
}
