using Assets.Noyau.Entities.Scripts;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Noyau.Entities.Samples.Scripts
{
    public sealed class RotatorSystem : AbstractSystem<Rotator>
    {
        private static void Rotate(RotatorGroup target, float deltaTime, float randomSpeed, float randomAngle)
        {
            float _angle = target.Entity.Speed * deltaTime;

            if (randomSpeed > 0F)
                _angle += Random.value * randomSpeed * deltaTime;

            Vector3 _axis = target.Entity.Axis;

            if (randomAngle > 0F)
                _axis += Random.onUnitSphere * randomAngle;

            target.Transform.Rotate(_axis, _angle, Space.Self);
        }

        [SerializeField]
        private float m_timeScale = 1F;
        [SerializeField]
        private float m_randomSpeed = 0F;
        [SerializeField]
        private float m_randomAngle = 0F;

        private List<RotatorGroup> m_entities = new List<RotatorGroup>();
        private bool m_entitiesAreDirty = true;

        protected override void OnValidate()
        {
            base.OnValidate();
            m_randomSpeed = Mathf.Max(m_randomSpeed, 0F);
            m_randomAngle = Mathf.Max(m_randomAngle, 0F);
        }

        protected override void Awake()
        {
            base.Awake();
            onRegister += OnEntitiesUpdate;
            onUnregister += OnEntitiesUpdate;
        }

        private void OnEntitiesUpdate(Rotator target)
        {
            m_entitiesAreDirty = true;
        }

        public List<RotatorGroup> GetEntities()
        {
            if (!m_entitiesAreDirty)
                return m_entities;

            if (m_entities.Count > 0)
                m_entities.Clear();

            m_entities.AddRange(GetEntities<RotatorGroup>());

            m_entitiesAreDirty = false;

            return m_entities;
        }

        public override void OnUpdate()
        {
            float _dt = m_timeScale * Time.deltaTime;

            //List<RotatorGroup> _entities = GetEntities<RotatorGroup>();
            List<RotatorGroup> _entities = GetEntities();

            for (int i = 0; i < _entities.Count; ++i)
                Rotate(_entities[i], _dt, m_randomSpeed, m_randomAngle);
        }
    } // class: RotatorSystem
} // namespace