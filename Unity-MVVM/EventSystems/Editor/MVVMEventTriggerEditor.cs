using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityMVVM.EventSystems;

namespace UnityEditor.EventSystems.Editor
{
    /// <summary>
    ///   <para>Custom Editor for the EventTrigger Component.</para>
    /// </summary>
    [CustomEditor(typeof(MVVMEventTrigger), true)]
    public class EventTriggerEditor : UnityEditor.Editor
    {
        private SerializedProperty m_DelegatesProperty;
        private GUIContent m_IconToolbarMinus;
        private GUIContent m_EventIDName;
        private GUIContent[] m_EventTypes;
        private GUIContent m_AddButonContent;

        protected virtual void OnEnable()
        {
            this.m_DelegatesProperty = this.serializedObject.FindProperty("m_Delegates");
            this.m_AddButonContent = EditorGUIUtility.TrTextContent("Add New Event Type", (string)null, (Texture)null);
            this.m_EventIDName = new GUIContent("");
            this.m_IconToolbarMinus = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"));
            this.m_IconToolbarMinus.tooltip = "Remove all events in this list.";
            string[] names = Enum.GetNames(typeof(EventTriggerType));
            this.m_EventTypes = new GUIContent[names.Length];
            for (int index = 0; index < names.Length; ++index)
                this.m_EventTypes[index] = new GUIContent(names[index]);
        }

        /// <summary>
        ///   <para>Implement specific EventTrigger inspector GUI code here. If you want to simply extend the existing editor call the base OnInspectorGUI () before doing any custom GUI code.</para>
        /// </summary>
        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            int toBeRemovedEntry = -1;
            EditorGUILayout.Space();
            Vector2 vector2 = GUIStyle.none.CalcSize(this.m_IconToolbarMinus);
            for (int index = 0; index < this.m_DelegatesProperty.arraySize; ++index)
            {
                SerializedProperty arrayElementAtIndex = this.m_DelegatesProperty.GetArrayElementAtIndex(index);
                SerializedProperty propertyRelative1 = arrayElementAtIndex.FindPropertyRelative("eventID");
                SerializedProperty propertyRelative2 = arrayElementAtIndex.FindPropertyRelative("callback");
                this.m_EventIDName.text = propertyRelative1.enumDisplayNames[propertyRelative1.enumValueIndex];
                EditorGUILayout.PropertyField(propertyRelative2, this.m_EventIDName);
                Rect lastRect = GUILayoutUtility.GetLastRect();
                if (GUI.Button(new Rect((float)((double)lastRect.xMax - (double)vector2.x - 8.0), lastRect.y + 1f, vector2.x, vector2.y), this.m_IconToolbarMinus, GUIStyle.none))
                    toBeRemovedEntry = index;
                EditorGUILayout.Space();
            }
            if (toBeRemovedEntry > -1)
                this.RemoveEntry(toBeRemovedEntry);
            Rect rect = GUILayoutUtility.GetRect(this.m_AddButonContent, GUI.skin.button);
            rect.x += (float)(((double)rect.width - 200.0) / 2.0);
            rect.width = 200f;
            if (GUI.Button(rect, this.m_AddButonContent))
                this.ShowAddTriggermenu();
            this.serializedObject.ApplyModifiedProperties();
        }

        private void RemoveEntry(int toBeRemovedEntry)
        {
            this.m_DelegatesProperty.DeleteArrayElementAtIndex(toBeRemovedEntry);
        }

        private void ShowAddTriggermenu()
        {
            GenericMenu genericMenu = new GenericMenu();
            for (int index1 = 0; index1 < this.m_EventTypes.Length; ++index1)
            {
                bool flag = true;
                for (int index2 = 0; index2 < this.m_DelegatesProperty.arraySize; ++index2)
                {
                    if (this.m_DelegatesProperty.GetArrayElementAtIndex(index2).FindPropertyRelative("eventID").enumValueIndex == index1)
                        flag = false;
                }
                if (flag)
                    genericMenu.AddItem(this.m_EventTypes[index1], false, new GenericMenu.MenuFunction2(this.OnAddNewSelected), (object)index1);
                else
                    genericMenu.AddDisabledItem(this.m_EventTypes[index1]);
            }
            genericMenu.ShowAsContext();
            Event.current.Use();
        }

        private void OnAddNewSelected(object index)
        {
            int num = (int)index;
            ++this.m_DelegatesProperty.arraySize;
            var arrayElementAtIndex = this.m_DelegatesProperty.GetArrayElementAtIndex(this.m_DelegatesProperty.arraySize - 1);
            arrayElementAtIndex.FindPropertyRelative("callback").FindPropertyRelative("_eventId").enumValueIndex =
                arrayElementAtIndex.FindPropertyRelative("eventID").enumValueIndex = num;
            this.serializedObject.ApplyModifiedProperties();
        }
    }
}
