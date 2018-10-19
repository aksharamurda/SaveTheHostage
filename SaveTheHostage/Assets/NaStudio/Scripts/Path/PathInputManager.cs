using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NaStd
{
    public class PathInputManager : MonoBehaviour
    {

        private GameObject[] destObject;
        private GameObject[] objectOnPath;

        private bool[] mouseDownLastFrame;
        private Vector3[] lastPosOnPath;
        private Vector3[] initialClickPos;
        private float[] timeSwipeStarted;
        private bool[] isTap;

        private bool usingMouse;
        private int maxTouches;

        public GameObject destObjectPrefab;
        public LayerMask pathInputObjectLayer;
        public LayerMask pathFloorLayer;

        public float maxTapDuration = 0.15f;
        public float maxTapDistance = 0.1f;

        public float minPathSegmentLength = 0.2f;

        void Awake()
        {
#if UNITY_IPHONE
		usingMouse = false;
		maxTouches = 5;
#elif UNITY_ANDROID && !UNITY_EDITOR
		usingMouse = false;
		maxTouches = 5;
#else
            usingMouse = true;
            maxTouches = 1;
#endif
            destObject = new GameObject[maxTouches];
            objectOnPath = new GameObject[maxTouches];

            mouseDownLastFrame = new bool[maxTouches];
            lastPosOnPath = new Vector3[maxTouches];
            initialClickPos = new Vector3[maxTouches];
            timeSwipeStarted = new float[maxTouches];
            isTap = new bool[maxTouches];
            for (int i = 0; i < maxTouches; i++)
            {
                destObject[i] = null;
                objectOnPath[i] = null;
                mouseDownLastFrame[i] = false;
                lastPosOnPath[i] = Vector3.zero;
                initialClickPos[i] = Vector3.zero;
                timeSwipeStarted[i] = 0f;
                isTap[i] = false;
            }
            if (MaskToNames(pathInputObjectLayer).Length == 0)
            {
                Debug.LogError("No Path Input Object Layer defined! Please define a layer in the Path Input Manager component");
            }
            if (MaskToNames(pathFloorLayer).Length == 0)
            {
                Debug.LogError("No Path Floor Layer defined! Please define a layer in the Path Input Manager component");
            }
            if (!destObjectPrefab)
            {
                Debug.LogError("No Destination Object prefab defined! Please define a prefab in the Path Input Manager component");
            }
            if (!destObjectPrefab.GetComponent<PathDestination>())
            {
                Debug.LogError("Destination Object prefab (" + destObjectPrefab.name + ") must have the Destination Object component!");
            }
        }

        public static string[] MaskToNames(LayerMask original)
        {
            List<string> output = new List<string>();

            for (int i = 0; i < 32; i++)
            {
                int shifted = 1 << i;
                if ((original & shifted) == shifted)
                {
                    string layerName = LayerMask.LayerToName(i);
                    if (!string.IsNullOrEmpty(layerName))
                    {
                        output.Add(layerName);
                    }
                }
            }
            return output.ToArray();
        }

        private Vector3 GetPosition(int id)
        {
            if (usingMouse)
            {
                return Input.mousePosition;
            }
            else
            {
                return Input.GetTouch(id).position;
            }
        }

        void Update()
        {
            for (int id = 0; id < maxTouches; id++)
            {
                int idAsTouch = -1;

                if (usingMouse && Input.GetMouseButton(0))
                {
                    idAsTouch = 0;
                }
                else
                {
                    for (int i = 0; i < Input.touchCount; i++)
                    {
                        if (Input.GetTouch(i).fingerId == id)
                        {
                            idAsTouch = i;
                        }
                    }
                }

                if (idAsTouch >= 0)
                {
                    if (!mouseDownLastFrame[id])
                    {
                        RaycastHit hitInfo;
                        if (Physics.Raycast(Camera.main.ScreenPointToRay(GetPosition(idAsTouch)), out hitInfo, Mathf.Infinity, pathInputObjectLayer))
                        {
                            if (hitInfo.transform.parent == null)
                            {
                                Debug.LogError(hitInfo.transform.name + " is using the PathInputObject layer, but does not have a parent object with either a PathFollower or a DestinationObject component!");
                                return;
                            }

                            GameObject objectHit = hitInfo.transform.parent.gameObject;

                            timeSwipeStarted[id] = Time.time;
                            initialClickPos[id] = Camera.main.ScreenToViewportPoint(GetPosition(idAsTouch));

                            if (objectHit.GetComponent<PathFollower>())
                            {
                                objectOnPath[id] = objectHit;

                                PathFollower pf = objectOnPath[id].GetComponent<PathFollower>();
                                if (pf.destObject)
                                {
                                    destObject[id] = pf.destObject;
                                }
                                else
                                {
                                    destObject[id] = (GameObject)Instantiate(destObjectPrefab, objectOnPath[id].transform.position, objectOnPath[id].transform.rotation);
                                    pf.destObject = destObject[id];
                                    destObject[id].GetComponent<PathDestination>().objectOnPath = objectOnPath[id];

                                    CharacterController destObjCC = destObject[id].GetComponent<CharacterController>();
                                    CharacterController onPathCC = objectOnPath[id].GetComponent<CharacterController>();
                                    destObjCC.radius = onPathCC.radius;
                                    destObjCC.height = onPathCC.height;
                                    destObjCC.center = onPathCC.center;

                                    destObject[id].transform.Find("Marker").localScale *= destObjCC.radius / 0.5f;
                                }

                                lastPosOnPath[id] = objectOnPath[id].transform.position;
                                isTap[id] = true;
                            }
                            else if (objectHit.GetComponent<PathDestination>())
                            {
                                objectOnPath[id] = objectHit.GetComponent<PathDestination>().objectOnPath;
                                destObject[id] = objectHit;
                                lastPosOnPath[id] = destObject[id].transform.position;
                                isTap[id] = false;
                            }
                            else
                            {
                                Debug.LogError(objectHit.name + " has a PathInputObject child, but does not have either a PathFollower or a DestinationObject component!");
                            }
                        }
                    }
                    else
                    {
                        if (objectOnPath[id] && destObject[id])
                        {
                            RaycastHit hitInfo;
                            if (Physics.Raycast(Camera.main.ScreenPointToRay(GetPosition(idAsTouch)), out hitInfo, Mathf.Infinity, pathFloorLayer))
                            {

                                float dist = (Camera.main.ScreenToViewportPoint(GetPosition(idAsTouch)) - initialClickPos[id]).magnitude;
                                float duration = Time.time - timeSwipeStarted[id];

                                if (isTap[id] && (dist > maxTapDistance || duration > maxTapDuration))
                                {
                                    objectOnPath[id].GetComponent<PathFollower>().ClearPath();
                                    objectOnPath[id].GetComponent<PathFollower>().AddPathNode(objectOnPath[id].transform.position);
                                    destObject[id].transform.position = objectOnPath[id].transform.position;
                                    isTap[id] = false;
                                }

                                if (!isTap[id])
                                {
                                    Vector3 oldPos = destObject[id].transform.position;

                                    CharacterController cc = destObject[id].GetComponent<CharacterController>();
                                    Vector3 moveVector = hitInfo.point - destObject[id].transform.position;
                                    moveVector.y = Mathf.Min(moveVector.y, 0.0f);
                                    cc.Move(moveVector);

                                    if (destObject[id].transform.position.y > oldPos.y)
                                    {
                                        destObject[id].transform.position = oldPos;
                                    }

                                    if (Vector3.Distance(destObject[id].transform.position, lastPosOnPath[id]) > minPathSegmentLength)
                                    {
                                        if (!objectOnPath[id].GetComponent<PathFollower>().AddPathNode(destObject[id].transform.position))
                                        {
                                            objectOnPath[id] = null;
                                            destObject[id] = null;
                                        }
                                        lastPosOnPath[id] = destObject[id].transform.position;
                                    }
                                }
                            }
                        }
                    }
                    mouseDownLastFrame[id] = true;
                }
                else
                {
                    if (objectOnPath[id])
                    {
                        if (isTap[id])
                        {
                            objectOnPath[id].SendMessage("PathFollowerTapped", SendMessageOptions.DontRequireReceiver);
                        }
                    }

                    objectOnPath[id] = null;
                    destObject[id] = null;
                    mouseDownLastFrame[id] = false;
                }
            }
        }
    }
}
