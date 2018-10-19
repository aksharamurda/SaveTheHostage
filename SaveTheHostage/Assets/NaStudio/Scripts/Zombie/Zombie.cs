using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NaStd
{
    public class Zombie : MonoBehaviour
    {

        public LayerMask attackLayers;
        public Transform targetHostage;

        void Start()
        {

        }

        void Update()
        {
            Vector3 posOrigin = transform.position;
            RaycastHit hit;
            if (Physics.SphereCast(posOrigin, 0.5f, transform.forward, out hit, 0.5f, attackLayers))
            {
                if(targetHostage == null)
                {
                    hit.transform.SendMessage("ZombieCatchMe", SendMessageOptions.DontRequireReceiver);
                    targetHostage = hit.transform;
                }

            }
        }

    }
}
