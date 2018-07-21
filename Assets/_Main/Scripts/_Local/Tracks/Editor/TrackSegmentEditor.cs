using UnityEditor;
using UnityEngine;

namespace LineRunner
{
    [CustomEditor(typeof(TrackSegment))]
    class TrackSegmentEditor : Editor
    {

        protected SerializedProperty startPointProp;
        protected SerializedProperty endPointProp;
        protected SerializedProperty minObstacleProp;
        protected SerializedProperty maxObstacleProp;
        protected SerializedProperty possibleObstaclesProp;
        protected SerializedProperty possibleObstaclePositionsProp;

        public void OnEnable()
        {
            startPointProp = serializedObject.FindProperty("startPoint");
            endPointProp = serializedObject.FindProperty("endPoint");
            minObstacleProp = serializedObject.FindProperty("minObstacleCount");
            maxObstacleProp = serializedObject.FindProperty("maxObstacleCount");
            possibleObstaclesProp = serializedObject.FindProperty("possibleObstacles");
            possibleObstaclePositionsProp = serializedObject.FindProperty("possibleObstaclePositions");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(startPointProp);
            EditorGUILayout.PropertyField(endPointProp);
            EditorGUILayout.PropertyField(minObstacleProp);
            EditorGUILayout.PropertyField(maxObstacleProp);
            EditorGUILayout.PropertyField(possibleObstaclesProp, true);

            EditorGUILayout.Space();

            if (GUILayout.Button("Add obstacle position"))
            {
                possibleObstaclePositionsProp.arraySize++;
                possibleObstaclePositionsProp.GetArrayElementAtIndex(possibleObstaclePositionsProp.arraySize - 1).vector2Value = Vector2.zero;
            }

            int toremove = -1;

            for (int i = 0; i < possibleObstaclePositionsProp.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.PropertyField(possibleObstaclePositionsProp.GetArrayElementAtIndex(i));
                if (GUILayout.Button("-", GUILayout.MaxWidth(32)))
                    toremove = i;

                EditorGUILayout.EndHorizontal();
            }

            if (toremove != -1)
                possibleObstaclePositionsProp.DeleteArrayElementAtIndex(toremove);



            serializedObject.ApplyModifiedProperties();

            if (minObstacleProp.intValue > possibleObstaclePositionsProp.arraySize || maxObstacleProp.intValue > possibleObstaclePositionsProp.arraySize)
            {
                EditorGUILayout.HelpBox("the maximum of obstacles is being driven by the size of array of possible positions", MessageType.Warning);
            }

        }
    }
}