using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NaStd
{
    public class Player : MonoBehaviour
    {
        public LayerMask hostageLayers;

        private CharacterController characterController;

        void Start()
        {
            characterController = GetComponent<CharacterController>();
        }

        void Update()
        {
            Vector3 posOrigin = transform.position + characterController.center;
            RaycastHit hit;
            if (Physics.SphereCast(posOrigin, 0.5f, transform.forward,  out hit, 0.5f, hostageLayers))
            {
                hit.transform.SendMessage("PlayerSaveMe", SendMessageOptions.DontRequireReceiver);
            }
            
        }

        public void ZombieCatchMe()
        {
            Debug.Log("Zombie Catch Me!");
        }

    }
}
