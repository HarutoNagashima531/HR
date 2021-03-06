// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEditor;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.UI
{
    [CustomEditor(typeof(InteractableReceiverList))]
    public class InteractableReceiverListInspector : UnityEditor.Editor
    {
        private static readonly GUIContent InteractableLabel = new GUIContent("Interactable","The Interactable that will be monitored");
        private static readonly GUIContent SearchScopeLabel = new GUIContent("Search Scope", "Where to look for an Interactable if one is not assigned");

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            RenderInspectorHeader();

            SerializedProperty events = serializedObject.FindProperty("Events");

            if(events.arraySize < 1)
            {
                AddEvent(0);
            }
            else
            {
                for (int i = 0; i < events.arraySize; i++)
                {
                    SerializedProperty eventItem = events.GetArrayElementAtIndex(i);

                    bool canRemove = i > 0;
                    if (InteractableEventInspector.RenderEvent(eventItem, canRemove))
                    {
                        events.DeleteArrayElementAtIndex(i);
                        // If removed, skip rendering rest of list till next redraw
                        break;
                    }
                }

                if (GUILayout.Button(new GUIContent("Add Event")))
                {
                    AddEvent(events.arraySize);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void RenderInspectorHeader()
        {
            SerializedProperty interactable = serializedObject.FindProperty("Interactable");
            SerializedProperty searchScope = serializedObject.FindProperty("InteractableSearchScope");

            EditorGUILayout.PropertyField(interactable, InteractableLabel);
            EditorGUILayout.PropertyField(searchScope, SearchScopeLabel);
        }

        protected virtual void AddEvent(int index)
        {
            SerializedProperty events = serializedObject.FindProperty("Events");
            events.InsertArrayElementAtIndex(events.arraySize);
        }
    }
}
