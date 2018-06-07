using UnityEngine;

namespace Assets.Noyau.Entities.Scripts
{
    public abstract class AbstractGroup<T> : IGroup
        where T : AbstractEntity
    {
        IEntity IGroup.Entity => Entity;

        public T Entity { get; }
        public Transform Transform { get; }

        public AbstractGroup(T target)
        {
            Entity = target;
            Transform = target.GetComponent<Transform>();
        }
    } // class: AbstractGroup<T>
} // namespace