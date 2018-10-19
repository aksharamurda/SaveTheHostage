using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NaStd
{
    public class Player : MonoBehaviour
    {
        public float healthAmount = 100;
        public bool isDead;
        public LayerMask hostageLayers;
        public LayerMask doorLayers;
        private CharacterController characterController;
        private PathFollower pathFollower;
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            pathFollower = GetComponent<PathFollower>();
        }

        void Update()
        {
            Vector3 posOrigin = transform.position + characterController.center;
            RaycastHit hit;
            if (Physics.SphereCast(posOrigin, 0.5f, transform.forward,  out hit, 0.5f, hostageLayers))
            {
                hit.transform.SendMessage("PlayerSaveMe", SendMessageOptions.DontRequireReceiver);
            }

            if (Physics.SphereCast(posOrigin, 0.5f, transform.forward, out hit, 0.5f, doorLayers))
            {
                hit.transform.SendMessage("OnFinish", SendMessageOptions.DontRequireReceiver);
            }
        }

        public void TakeDamage(float damage)
        {
            if (isDead)
                return;

            if (healthAmount > 0)
            {
                pathFollower.m_Animator.SetTrigger("isHit");
                healthAmount -= damage;
            }
            else
            {
                isDead = true;
                PathInputManager.instance.enabled = false;
                pathFollower.m_Animator.SetTrigger("isDead");
                pathFollower.isDead();
            }


            Debug.Log("Script Player : Zombie Take Damage!");
        }
    }
}
