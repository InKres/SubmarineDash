using System;
using UnityEngine;

namespace RuntimeDebugSystem
{
    [Serializable]
    public class PhysicsVisualizer2D : DebugFunction
    {
        [SerializeField]
        private bool showVelocity = true;
        [SerializeField]
        private Color velocityColor = Color.red;

        private Rigidbody2D targetRigidbody;

        public override void Enable()
        {
            base.Enable();
            targetRigidbody = GameObject.FindObjectOfType<Rigidbody2D>();
            Debug.Log("[PhysicsVisualizer] Визуализация физики включена");
        }

        public override void OnGUI()
        {
            if (targetRigidbody && showVelocity && IsEnabled)
            {
                GUIStyle style = new GUIStyle();
                style.fontSize = 14;
                style.normal.textColor = velocityColor;

                string velocityInfo = $"Velocity: {targetRigidbody.velocity}\n" +
                                     $"Angular Vel: {targetRigidbody.angularVelocity}\n" +
                                     $"Position: {targetRigidbody.position}";

                GUI.Label(new Rect(10, 60, 300, 60), velocityInfo, style);
            }
        }

        public override void OnUpdate() { }
        public override void OnFixedUpdate() { }
    }
}