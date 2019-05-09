using System.Linq;
using UnityEditor;
using UnityMVVM.Binding;
using System.Collections.Generic;

namespace UnityMVVM.Editor
{
    [CustomEditor(typeof(CollectionViewSource), true)]
    public class CollectionViewSourceEditor : DataBindingBaseEditor
    {
        public int _srcIndex = 0;

        private List<string> _srcNameProp;

        protected override void CollectSerializedProperties()
        {
            base.CollectSerializedProperties();
            _srcNameProp = serializedObject.FindProperty("SrcCollections").GetPropertiesArray();
        }

        protected override void DrawChangeableElements()
        {
            base.DrawChangeableElements();

            var myClass = target as CollectionViewSource;
            EditorGUILayout.LabelField("Source Collection");

            _srcIndex = EditorGUILayout.Popup(_srcIndex, _srcNameProp.ToArray());

        }

        protected override void UpdateSerializedProperties()
        {
            base.UpdateSerializedProperties();
            var myClass = target as CollectionViewSource;
            myClass.SrcCollectionName = _srcIndex > -1 ?
                myClass.SrcCollections[_srcIndex] : null;
        }

        public override void OnInspectorGUI()
        {

            if(!(target is CollectionViewSource myClass)) return;

            _srcIndex = myClass.SrcCollectionName is null
                ? -1
                : myClass.SrcCollections.FindIndex(c => c.PropertyName == myClass.SrcCollectionName.PropertyName);

            if (_srcIndex < 0 && myClass.SrcCollections.Count > 0)
            {
                _srcIndex = 0;
                myClass.SrcCollectionName = myClass.SrcCollections.FirstOrDefault();
            }
            base.OnInspectorGUI();

        }

    }
}
