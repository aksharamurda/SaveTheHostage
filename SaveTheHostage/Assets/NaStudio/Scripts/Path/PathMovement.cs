using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NaStd
{
    public abstract class PathMovement : MonoBehaviour
    {
        protected bool hasDestination = false;
        protected Vector3 destination;

        public float moveSpeed = 1.0f;
        public abstract float distToDestination { get; }
        public float timeToDestination
        {
            get
            {
                return distToDestination / moveSpeed;
            }
        }

        public virtual void SetDestination(Vector3 newDest)
        {
            destination = newDest;
            hasDestination = true;
        }
    }
}
