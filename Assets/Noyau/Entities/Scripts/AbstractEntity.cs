using UnityEngine;

namespace Assets.Noyau.Entities.Scripts
{
    [DisallowMultipleComponent, SelectionBase]
    public abstract class AbstractEntity : MonoBehaviour, IEntity
    {
        protected IGroup m_group = default(IGroup);

        public virtual IGroup Group => m_group;

        protected abstract void OnRegister();
        protected abstract void OnUnregister();

        protected virtual void Reset() { }
        protected virtual void OnValidate() { }

        protected virtual void OnEnable() => OnRegister();
        protected virtual void OnDisable() => OnUnregister();
    } // class: AbstractEntity
} // namespace