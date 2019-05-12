using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityMVVM.Binding;
using Rotorz.Games.Collections;
using UnityEngine;

namespace UnityMVVM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(OneWayDataBinding), true)]
    public class OneWayDataBindingEditor : DataBindingBaseEditor
    {
        public int _srcIndex = 0;
        public int _dstIndex = 0;

        SerializedProperty _srcNameProp;
        SerializedProperty _dstNameProp;

        SerializedProperty _srcProps;
        SerializedProperty _dstProps;

        List<(string, bool)> _srcPropNames;
        List<string> _dstPropNames;

        private SerializedProperty _convertersProp;

        protected override void CollectSerializedProperties()
        {
            base.CollectSerializedProperties();
            _srcNameProp = serializedObject.FindProperty("SrcPropertyName");
            _dstNameProp = serializedObject.FindProperty("DstPropertyName");

            _srcProps = serializedObject.FindProperty("SrcProps");
            _dstProps = serializedObject.FindProperty("DstProps");

            _srcPropNames = _srcProps.GetSrcPropertiesArray();
            _dstPropNames = _dstProps.GetPropertiesArray();

            _convertersProp = serializedObject.FindProperty("Converters");

        }

        protected override void DrawChangeableElements()
        {
            ReorderableListGUI.Title("Converters");
            ReorderableListGUI.ListField(_convertersProp,
                () => EditorGUILayout.LabelField("There is no converter in use.", EditorStyles.miniLabel));

            base.DrawChangeableElements();

            var myClass = target as OneWayDataBinding;
            var srcGenericMenu = new GenericMenu();
//            srcGenericMenu.AddItem(new GUIContent("1"),true, () => { });
            for (var index = 0; index < _srcPropNames.Count; index++)
            {
                var (item1, item2) = _srcPropNames[index];
                var index1 = index;
                srcGenericMenu.AddItem(new GUIContent((item2 ? "Static/" : "Reactive/") + item1),
                    index == _srcIndex, () =>
                    {
                        _srcIndex = index1;
                        UpdateSerializedProperties();
                    });
            }

            var srcBtnContent = new GUIContent(_srcPropNames[_srcIndex].Item1);
            EditorGUILayout.LabelField("Source Property");
            var theRect = GUILayoutUtility.GetRect(srcBtnContent, EditorStyles.popup);
            if (GUI.Button(theRect, srcBtnContent, EditorStyles.popup))
                srcGenericMenu.DropDown(theRect);
//            _srcIndex = EditorGUILayout.Popup(_srcIndex, _srcPropNames.ToArray());

            EditorGUILayout.LabelField("Destination Property");
            _dstIndex = EditorGUILayout.Popup(_dstIndex, _dstPropNames.ToArray());
        }

        protected override void UpdateSerializedProperties()
        {
            base.UpdateSerializedProperties();
            if (!(target is OneWayDataBinding myClass)) return;

            myClass.SrcPropertyName = _srcIndex > -1 ?
                   myClass.SrcProps[_srcIndex] : null;

            //myClass.DstPropertyName = _dstIndex > -1 ?
            //     _dstPropNames[_dstIndex] : null;

            myClass.DstPropertyName = _dstIndex > -1 ?
                myClass.DstProps[_dstIndex] : null;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (!(target is OneWayDataBinding myClass)) return;

            _srcIndex = myClass.SrcPropertyName is null
                ? -1
                : myClass.SrcProps.FindIndex(p => p.PropertyName == myClass.SrcPropertyName.PropertyName);
            if (_srcIndex < 0 && _srcPropNames.Count > 0)
            {
                _srcIndex = 0;
                myClass.SrcPropertyName = myClass.SrcProps.FirstOrDefault();
            }

            _dstIndex = myClass.DstPropertyName is null
                ? -1
                : myClass.DstProps.FindIndex(p => p.PropertyName == myClass.DstPropertyName.PropertyName);
            if (_dstIndex < 0 && _dstPropNames.Count > 0)
            {
                _dstIndex = 0;
                myClass.DstPropertyName = myClass.DstProps.FirstOrDefault();
            }
        }
    }
}

public static class SerializedPropertyExt
{

    public static List<string> GetStringArray(this SerializedProperty prop)
    {
        List<string> list = new List<string>(prop.arraySize);

        for (int i = 0; i < prop.arraySize; i++)
        {
            list.Add(prop.GetArrayElementAtIndex(i).stringValue);
        }

        return list;
    }

    public static List<string> GetPropertiesArray(this SerializedProperty prop)
    {
        List<string> list = new List<string>(prop.arraySize);

        for (int i = 0; i < prop.arraySize; i++)
        {
            var arrayElementAtIndex = prop.GetArrayElementAtIndex(i);
            list.Add(
                $"{arrayElementAtIndex.FindPropertyRelative("PropertyName").stringValue}({arrayElementAtIndex.FindPropertyRelative("PropertyType").stringValue})");
        }

        return list;
    }

    public static List<(string, bool)> GetSrcPropertiesArray(this SerializedProperty prop)
    {
        var list = new List<(string, bool)>();
        for (int i = 0; i < prop.arraySize; ++i)
        {
            var arrayElementAtIndex = prop.GetArrayElementAtIndex(i);
            list.Add((
                $"{arrayElementAtIndex.FindPropertyRelative("PropertyName").stringValue}({arrayElementAtIndex.FindPropertyRelative("PropertyType").stringValue})",
                arrayElementAtIndex.FindPropertyRelative("IsStatic").boolValue));
        }

        return list;
    }

}
