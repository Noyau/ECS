using System.Collections.Generic;
using UnityEngine;

namespace Assets.Noyau.Entities.Scripts
{
    [DisallowMultipleComponent, SelectionBase]
    public abstract class AbstractSystem : MonoBehaviour, ISystem
    {
        public abstract List<T> GetEntities<T>() where T : IGroup;
        public abstract void OnUpdate();

        protected virtual void Reset() { }
        protected virtual void OnValidate() { }

        protected virtual void Awake() { }

        protected void Update() => OnUpdate();
    } // class: AbstractSystem

    [DisallowMultipleComponent, SelectionBase]
    public abstract class AbstractSystem<T> : AbstractSystem
        where T : IEntity
    {
        public delegate void RegisterEvent(T target);

        public static event RegisterEvent onRegister = _tgt => { };
        public static event RegisterEvent onUnregister = _tgt => { };

        protected static List<T> s_entities = new List<T>();

        public static void Register(T target)
        {
            if (target != null && !s_entities.Contains(target))
            {
                s_entities.Add(target);
                onRegister(target);
            }
        }
        public static void Unregister(T target)
        {
            if (s_entities.Remove(target))
                onUnregister(target);
        }

        public override List<U> GetEntities<U>()
        {
            List<U> _output = new List<U>();
            for (int i = 0; i < s_entities.Count; ++i)
            {
                if (s_entities[i].Group is U)
                    _output.Add((U)s_entities[i].Group);
            }
            return _output;
        }
    }
} // namespace