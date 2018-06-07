using Assets.Noyau.Entities.Scripts;
using UnityEngine;

namespace Assets.Noyau.Entities.Samples.Scripts
{
    public sealed class Rotator : AbstractEntity
    {
        [SerializeField]
        private float m_speed = 1F;

        public float Speed => m_speed;

        [SerializeField]
        private Vector3 m_axis = Vector3.up;

        public Vector3 Axis => m_axis;

        public override IGroup Group => base.Group ?? (m_group = new RotatorGroup(this));

        protected override void OnValidate()
        {
            base.OnValidate();

            m_speed = Mathf.Max(m_speed, 0F);
        }

        protected override void OnRegister()
        {
            RotatorSystem.Register(this);
        }
        protected override void OnUnregister()
        {
            RotatorSystem.Unregister(this);
        }

        [ContextMenu("Randomize Speed")]
        public void RandomizeSpeed() => m_speed = Random.value * 100F;
        [ContextMenu("Randomize Axis")]
        public void RandomizeAxis() => m_axis = Random.onUnitSphere;
    } // class: Rotator
} // namespace