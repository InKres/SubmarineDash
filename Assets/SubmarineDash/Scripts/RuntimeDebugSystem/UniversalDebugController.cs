using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RuntimeDebugSystem
{
    public class UniversalDebugController : MonoBehaviour
    {
        [Serializable]
        public class DebugFunctionContainer
        {
            public DebugFunction function;
            public bool useFunction;
            public bool isEnabled;
        }

        [Header("Debug Functions")]
        [SerializeField]
        public FPSLimiter fpsLimiter = new FPSLimiter();
        [SerializeField]
        public PhysicsVisualizer2D physicsVisualizer = new PhysicsVisualizer2D();

        private List<DebugFunction> activeFunctions = new List<DebugFunction>();

        void Start()
        {
            InitializeFunctions();
        }

        void InitializeFunctions()
        {
            AddFunctionIfUsed(fpsLimiter);
            AddFunctionIfUsed(physicsVisualizer);
        }

        void AddFunctionIfUsed(DebugFunction function)
        {
            if (function.UseFunction)
            {
                activeFunctions.Add(function);
                if (function.IsEnabled)
                {
                    function.Enable();
                }
            }
        }

        void Update()
        {
            foreach (var function in activeFunctions)
            {
                if (function.IsEnabled)
                {
                    function.OnUpdate();
                }
            }
        }

        void FixedUpdate()
        {
            foreach (var function in activeFunctions)
            {
                if (function.IsEnabled)
                {
                    function.OnFixedUpdate();
                }
            }
        }

        void OnGUI()
        {
            foreach (var function in activeFunctions)
            {
                if (function.IsEnabled)
                {
                    function.OnGUI();
                }
            }
        }

        // Public methods for editor
        public void ToggleFunction(DebugFunction function)
        {
            if (function.IsEnabled)
            {
                function.Disable();
            }
            else
            {
                function.Enable();
                if (!activeFunctions.Contains(function) && function.UseFunction)
                {
                    activeFunctions.Add(function);
                }
            }
        }

        void OnDestroy()
        {
            // Отключаем все функции при уничтожении
            foreach (var function in activeFunctions)
            {
                if (function.IsEnabled)
                {
                    function.Disable();
                }
            }
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(UniversalDebugController))]
    public class UniversalDebugControllerEditor : Editor
    {
        private SerializedProperty fpsLimiterProp;
        private SerializedProperty physicsVisualizerProp;

        private void OnEnable()
        {
            fpsLimiterProp = serializedObject.FindProperty("fpsLimiter");
            physicsVisualizerProp = serializedObject.FindProperty("physicsVisualizer");
        }

        public override void OnInspectorGUI()
        {
            UniversalDebugController controller = (UniversalDebugController)target;

            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Universal Debug Controller", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            DrawFunctionSection("FPS Limiter", fpsLimiterProp, controller.fpsLimiter, controller);
            DrawFunctionSection("Physics Visualizer", physicsVisualizerProp, controller.physicsVisualizer, controller);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawFunctionSection(string title, SerializedProperty property, DebugFunction function, UniversalDebugController controller)
        {
            EditorGUILayout.BeginVertical("box");

            // Use Function toggle
            SerializedProperty useFunctionProp = property.FindPropertyRelative("useFunction");
            EditorGUILayout.PropertyField(useFunctionProp, new GUIContent($"Use {title}"));

            if (useFunctionProp.boolValue)
            {
                EditorGUILayout.Space();

                // Draw all properties except useFunction and isEnabled
                DrawChildPropertiesExcluding(property, "useFunction", "isEnabled");

                EditorGUILayout.Space();

                // Enable/Disable button
                GUIContent buttonContent = function.IsEnabled ?
                    new GUIContent("Disable", "Отключить функцию") :
                    new GUIContent("Enable", "Включить функцию");

                Color originalColor = GUI.color;
                GUI.color = function.IsEnabled ? Color.red : Color.green;

                if (GUILayout.Button(buttonContent, GUILayout.Height(25)))
                {
                    controller.ToggleFunction(function);
                }

                GUI.color = originalColor;

                // Status indicator
                EditorGUILayout.LabelField("Status:", function.IsEnabled ? "Enabled" : "Disabled",
                                          function.IsEnabled ? EditorStyles.boldLabel : EditorStyles.label);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        private void DrawChildPropertiesExcluding(SerializedProperty property, params string[] propertiesToExclude)
        {
            SerializedProperty iterator = property.Copy();
            SerializedProperty end = property.GetEndProperty();
            bool enterChildren = true;

            while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, end))
            {
                enterChildren = false;

                bool shouldExclude = false;
                foreach (string exclude in propertiesToExclude)
                {
                    if (iterator.name == exclude)
                    {
                        shouldExclude = true;
                        break;
                    }
                }

                if (!shouldExclude)
                {
                    EditorGUILayout.PropertyField(iterator, true);
                }
            }
        }
    }
#endif
}