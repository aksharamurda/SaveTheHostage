﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NaStd
{
    public class Player : MonoBehaviour
    {
        public float healthAmount = 100;
        public bool isShieldActive;
        public bool isDead;
        public LayerMask pickLayers;
        public LayerMask finishLayers;

        private CharacterController characterController;
        private PathFollower pathFollower;
        public PathFollower GetPathFollower { get { return pathFollower; } }
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            pathFollower = GetComponent<PathFollower>();
        }

        void Update()
        {
            Vector3 posOrigin = transform.position + characterController.center;
            RaycastHit hit;

            //ITEMS
            if (Physics.SphereCast(posOrigin, 0.5f, transform.forward,  out hit, 0.5f, pickLayers))
            {
                hit.transform.SendMessage("OnPickItem", SendMessageOptions.DontRequireReceiver);
            }

            //FINISH ZONE
            if (Physics.SphereCast(posOrigin, 0.5f, transform.forward, out hit, 0.5f, finishLayers))
            {
                hit.transform.SendMessage("OnFinishZone", SendMessageOptions.DontRequireReceiver);
            }

            isShieldActive = GameManager.instance.isShieldActive;
        }

        public void TakeDamage(float damage)
        {
            if (isDead)
                return;

            if (isShieldActive)
                return;

            if (GameManager.instance.isDone)
                return;

            if (healthAmount > 0)
            {
                pathFollower.m_Animator.SetTrigger("isHit");
                healthAmount -= damage;
            }
            else
            {
                isDead = true;
                pathFollower.m_Animator.SetTrigger("isDead");
                pathFollower.DisableController();
                GameManager.instance.OnGameOver();
            }


            Debug.Log("Script Player : Zombie Take Damage!");
        }
    }
}
