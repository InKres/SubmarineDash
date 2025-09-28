using System;
using UnityEngine;

namespace RuntimeDebugSystem
{
    [Serializable]
    public class FPSLimiter : DebugFunction
    {
        [SerializeField]
        private int targetFPS = 60;
        [SerializeField]
        private bool showCurrentFPS = true;

        private float deltaTime = 0.0f;
        private int originalFPS;

        public override void Enable()
        {
            base.Enable();
            originalFPS = Application.targetFrameRate;
            Application.targetFrameRate = targetFPS;
            Debug.Log($"[FPSLimiter] Ограничение FPS до: {targetFPS}");
        }

        public override void Disable()
        {
            base.Disable();
            Application.targetFrameRate = originalFPS;
            Debug.Log("[FPSLimiter] Ограничение FPS снято");
        }

        public override void OnUpdate()
        {
            if (showCurrentFPS)
            {
                deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            }
        }

        public override void OnFixedUpdate() { }

        public override void OnGUI()
        {
            if (showCurrentFPS && IsEnabled)
            {
                float msec = deltaTime * 1000.0f;
                float fps = 1.0f / deltaTime;

                GUIStyle style = new GUIStyle();
                style.fontSize = 16;
                style.normal.textColor = Color.green;

                GUI.Label(new Rect(10, 30, 300, 20),
                         $"FPS: {fps:0.} (Target: {targetFPS})", style);
            }
        }
    }
}