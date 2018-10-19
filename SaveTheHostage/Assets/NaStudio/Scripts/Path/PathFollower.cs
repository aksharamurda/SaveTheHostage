using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NaStd
{
    [RequireComponent(typeof(CharacterController))]
    public class PathFollower : MonoBehaviour
    {
        public bool hasPath { get; private set; }
        public GameObject destObject { get; set; }

        public Material pathMaterial;
        public Color colorMaterial = new Color(0, 0, 0, 255);
        public float lineWidth = 0.5f;

        private List<Vector3> currentPath = new List<Vector3>();
        private int currentNode;

        private LineRenderer pathLine;
        [HideInInspector]
        public Animator m_Animator;
        [HideInInspector]
        public Path path;

        private bool pathLineEnabled
        {
            get
            {
                return pathLine.enabled;
            }

            set
            {
                pathLine.enabled = value;
            }
        }


        void Awake()
        {
            pathLine = gameObject.AddComponent<LineRenderer>();
            pathLine.material = pathMaterial; // new Material( Shader.Find( "Unlit/Texture" ) );
            pathLine.material.color = colorMaterial;
            //pathLine.SetWidth( 0.05f, 0.05f );
            pathLine.startWidth = lineWidth;
            pathLine.endWidth = lineWidth;

            hasPath = false;
            pathLineEnabled = false;

            m_Animator = GetComponent<Animator>();
            path = GetComponent<Path>();
        }


        public void isDead()
        {
            currentNode = 0;
            pathLineEnabled = false;
            hasPath = false;
            Destroy(destObject);

            SendMessage("PathComplete", SendMessageOptions.DontRequireReceiver);
            currentPath.Clear();
            path.SetHasDestination();
        }

        void Update()
        {
            m_Animator.SetBool("isRun", pathLineEnabled);
        }

        void OnDisable()
        {
            pathLineEnabled = false;
        }


        void OnDestroy()
        {
            Destroy(pathLine);
            Destroy(destObject);
        }

        private void UpdateLine()
        {
            if (pathLineEnabled)
            {
                pathLine.positionCount = (currentPath.Count - currentNode);

                for (int i = currentNode; i < currentPath.Count; i++)
                {
                    pathLine.SetPosition(i - currentNode, currentPath[i] + 0.1f * Vector3.up);
                }

                pathLine.material.SetTextureScale("_MainTex", new Vector2(pathLine.positionCount, 1));

            }

        }

        public void MoverMoveComplete()
        {
            if (currentNode == currentPath.Count - 1)
            {
                currentPath.Clear();
                currentNode = 0;
                pathLineEnabled = false;
                hasPath = false;
                Destroy(destObject);

                SendMessage("PathComplete", SendMessageOptions.DontRequireReceiver);
                //Debug.Log(name + ": Path complete!");

                return;
            }

            currentNode++;
            SendMessage("SetDestination", currentPath[currentNode], SendMessageOptions.RequireReceiver);
            UpdateLine();
        }

        public void ClearPath()
        {
            pathLineEnabled = false;
            currentPath.Clear();
            hasPath = false;
        }

        public bool AddPathNode(Vector3 newNode)
        {
            hasPath = true;

            currentPath.Add(newNode);

            if (!pathLineEnabled)
            {
                currentNode = 0;
                SendMessage("SetDestination", currentPath[currentNode], SendMessageOptions.RequireReceiver);

                pathLineEnabled = true;

                SendMessage("PathStarted", SendMessageOptions.DontRequireReceiver);
                Debug.Log(name + ": Path started!");
            }

            UpdateLine();

            return true;
        }
    }
}
