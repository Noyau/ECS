using Assets.Noyau.Entities.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Noyau.Entities.Samples.Scripts
{
    public sealed class RotatorSystem : AbstractSystem<Rotator>
    {
        [System.Serializable]
        public sealed class LoadingEvent : UnityEvent<float>
        { } // class: LoadingEvent

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

        [Space, SerializeField]
        private GameObject m_template = null;
        [SerializeField]
        private float m_templateSize = 5F;
        [SerializeField]
        private int m_gridSize = 3;
        [SerializeField, Range(0F, 1F)]
        private float m_chunkSize = 1e-2F;

        [Space, SerializeField]
        private LoadingEvent m_onBeginLoading = new LoadingEvent();
        [SerializeField]
        private LoadingEvent m_onLoading = new LoadingEvent();
        [SerializeField]
        private LoadingEvent m_onEndLoading = new LoadingEvent();

        private List<RotatorGroup> m_entities = new List<RotatorGroup>();
        private bool m_entitiesAreDirty = true;

        protected override void OnValidate()
        {
            base.OnValidate();
            m_randomSpeed = Mathf.Max(m_randomSpeed, 0F);
            m_randomAngle = Mathf.Max(m_randomAngle, 0F);
            m_templateSize = Mathf.Max(m_templateSize, 0F);
            m_gridSize = Mathf.Max(m_gridSize, 0);
        }

        protected override void Awake()
        {
            base.Awake();
            onRegister += OnEntitiesUpdate;
            onUnregister += OnEntitiesUpdate;
        }

        private IEnumerator Start()
        {
            if (m_template == null || m_templateSize < 1e-3F || m_gridSize < 1)
                yield break;

            enabled = false;

            m_template.SetActive(false);

            int _count = m_gridSize * m_gridSize * m_gridSize;

            m_onBeginLoading.Invoke(_count);

            yield return null;

            Transform _root = new GameObject("Rotators").transform;

            Vector3 _size = m_templateSize * Vector3.one;
            Vector3 _offset = m_gridSize * _size * .5F;

            int _chunkSize = m_chunkSize > 0F
                ? Mathf.RoundToInt(_count * m_chunkSize)
                : 0;

            for (int i = 0; i < _count; ++i)
            {
                int _x = i % m_gridSize;
                int _y = (i / m_gridSize) % m_gridSize;
                int _z = i / (m_gridSize * m_gridSize);

                Vector3 _position = new Vector3
                {
                    x = _size.x * _x - _offset.x,
                    y = _size.y * _y - _offset.y,
                    z = _size.z * _z - _offset.z,
                };

                Transform _chunk = Instantiate(m_template).transform;
                _chunk.name = $"{m_template.name} ({_x}x{_y}x{_z})";
                _chunk.SetParent(_root, false);
                _chunk.localPosition = _position;
                _chunk.gameObject.SetActive(true);

                if (_chunkSize == 0 || (i > 0 && (i % _chunkSize) == 0))
                {
                    m_onLoading.Invoke(i);
                    yield return null;
                }
            }

            m_onEndLoading.Invoke(_count);

            Destroy(m_template);

            enabled = true;
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