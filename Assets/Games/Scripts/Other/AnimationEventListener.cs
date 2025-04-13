using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TowerDefence
{
    [Serializable]
    public class AnimationEvent : UnityEvent<string>
    {

    }

    public class AnimationEventListener : MonoBehaviour
    {
        public AnimationEvent animationEvent;

        public void OnAnimationEvent(string name)
        {
            animationEvent?.Invoke(name);
        }
    }
}

