using System;
using UnityEngine;

namespace Assets.Scripts.GravityBox
{
    public class ZoneChecker : MonoBehaviour
    {
        [SerializeField] public string Tag;

        private int _count = 0;

        public int Count {  get => _count; set {_count = value;}  }

        public event Action CountChanged;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Tag))
            {
                _count++;
                CountChanged?.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(Tag))
            {
                _count--;
                CountChanged?.Invoke();
            }
        }
    }
}
