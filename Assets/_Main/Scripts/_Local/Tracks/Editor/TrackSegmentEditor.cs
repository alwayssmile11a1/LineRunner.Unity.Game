using UnityEditor;
using UnityEngine;

namespace LineRunner
{
    [CustomEditor(typeof(TrackSegment))]
    class TrackSegmentEditor : Editor
    {

        protected SerializedProperty startPointProp;
        protected SerializedProperty endPointProp;
        protected SerializedProperty possibleObstaclesProp;
        protected SerializedProperty possibleObstaclePositionsProp;

        public void OnEnable()
        {
            startPointProp = serializedObject.FindProperty("startPoint");
            endPointProp = serializedObject.FindProperty("endPoint");
            possibleObstaclesProp = serializedObject.FindProperty("possibleObstacles");
            possibleObstaclePositionsProp = serializedObject.FindProperty("possibleObstaclePositions");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(startPointProp);
            EditorGUILayout.PropertyField(endPointProp);
            EditorGUILayout.PropertyField(possibleObstaclesProp, true);



            if (GUILayout.Button("Add obstacle position"))
            {
                possibleObstaclePositionsProp.arraySize++;
                possibleObstaclePositionsProp.GetArrayElementAtIndex(possibleObstaclePositionsProp.arraySize - 1).floatValue = 0.0f;
            }

            int toremove = -1;

            for (int i = 0; i < possibleObstaclePositionsProp.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.Slider(possibleObstaclePositionsProp.GetArrayElementAtIndex(i), 0.0f, 1.0f);
                if (GUILayout.Button("-", GUILayout.MaxWidth(32)))
                    toremove = i;

                EditorGUILayout.EndHorizontal();
            }

            if (toremove != -1)
                possibleObstaclePositionsProp.DeleteArrayElementAtIndex(toremove);



            serializedObject.ApplyModifiedProperties();

        }
    }
}