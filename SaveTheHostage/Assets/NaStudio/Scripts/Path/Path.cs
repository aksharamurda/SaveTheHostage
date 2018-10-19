using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NaStd
{
    public class Path : PathMovement
    {
        private float rotSpeed = 360.0f;
        private float currSpeed;

        // Properties
        public bool lookDownPath;
        public bool allowOffscreenMovement;

        public override float distToDestination
        {
            get
            {
                return PathVector.Distance(transform.position, destination);
            }
        }

        public override void SetDestination(Vector3 newDest)
        {
            Vector3 viewPos = Camera.main.WorldToViewportPoint(newDest);
            if (!allowOffscreenMovement && (viewPos.x < 0.0f || viewPos.x > 1.0f || viewPos.y < 0.0f || viewPos.y > 1.0f))
            {
                return;
            }

            base.SetDestination(newDest);
            currSpeed = moveSpeed;
        }

        public void PathFollowerTapped()
        {
            MeshRenderer mr = GetComponentInChildren<MeshRenderer>();
            if (mr.material.color == Color.blue)
            {
                mr.material.color = Color.green;
            }
            else
            {
                mr.material.color = Color.blue;
            }
            Debug.Log("Path Follower was tapped, path not modified");
        }


        void Update()
        {
            if (!hasDestination)
            {
                return;
            }

            if (timeToDestination < Time.deltaTime)
            {
                currSpeed = distToDestination / Time.deltaTime;
                hasDestination = false;

                SendMessage("MoverMoveComplete");
            }

            Vector3 moveVector = PathVector.VectorTo(transform.position, destination).normalized * currSpeed;

            if (lookDownPath)
            {
                transform.forward = Vector3.RotateTowards(transform.forward, moveVector, Time.deltaTime * rotSpeed * Mathf.Deg2Rad, 0.0f);
            }
            
            gameObject.GetComponent<CharacterController>().SimpleMove(moveVector);
        }
    }
}
