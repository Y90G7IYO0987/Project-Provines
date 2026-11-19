using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectProvines.Unity.Entities
{
    public class EntitySliders : MonoBehaviour
    {
        [SerializeField] private Image bar;
        [SerializeField] private Image fill;

        private EntityHealthManager _healthManager;

        private void Awake()
        {
            _healthManager = GetComponent<EntityHealthManager>();
        }

        private void Update()
        {
            var procent = _healthManager.CurrentHealth / _healthManager.MaxHealth;
            var newSize = Math.Min(bar.transform.localScale.x * procent, bar.transform.localScale.x);

            fill.transform.localScale = new Vector2(newSize, fill.transform.localScale.y);
        }
    }
}