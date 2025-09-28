using System;
using UnityEngine;

namespace RuntimeDebugSystem
{
    [Serializable]
    public abstract class DebugFunction
    {
        [SerializeField] protected bool useFunction = false;
        [SerializeField] protected bool isEnabled = false;

        public bool UseFunction => useFunction;
        public bool IsEnabled => isEnabled;

        public string FunctionName => GetType().Name;

        public virtual void Enable() => isEnabled = true;
        public virtual void Disable() => isEnabled = false;

        public abstract void OnUpdate();
        public abstract void OnFixedUpdate();
        public abstract void OnGUI();
    }
}