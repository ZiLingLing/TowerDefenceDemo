using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TowerDefence
{
    [Serializable]
    public class TriggerEvent : UnityEvent<Collider>
    {

    }

    [Serializable]
    public class TriggerEvent2D : UnityEvent<Collider2D>
    {

    }

    public class TriggerEventListener : MonoBehaviour
    {
        public TriggerEvent onTriggerEnter;
        public TriggerEvent onTriggerStay;
        public TriggerEvent onTriggerExit;

        public TriggerEvent2D onTriggerEnter2d;
        public TriggerEvent2D onTriggerStay2D;
        public TriggerEvent2D onTriggerExit2D;

        private void OnTriggerEnter(Collider other)
        {
            onTriggerEnter?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            onTriggerStay?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            onTriggerExit?.Invoke(other);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            onTriggerEnter2d?.Invoke(collision);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            onTriggerStay2D?.Invoke(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            onTriggerExit2D?.Invoke(collision);
        }
    }
}

