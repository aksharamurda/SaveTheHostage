using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NaStd
{
    public class Player : MonoBehaviour
    {

        [HideInInspector]
        public Transform head;

        public List<Hostage> segments = new List<Hostage>();
        public List<Vector3> breadcrumbs = new List<Vector3>();

        public float segmentSpacing;

        void Start()
        {
            head = transform;

            breadcrumbs = new List<Vector3>();
            breadcrumbs.Add(head.position);
            for (int i = 0; i < segments.Count; i++)
                breadcrumbs.Add(segments[i].transform.position);
        }

        void Update()
        {
            if (breadcrumbs.Count == 1)
            {
                head.position = transform.position;
                breadcrumbs[0] = transform.position;
            }

            FollowHostage();
        }

        void FollowHostage()
        {
            if (segments.Count < 1)
                return;

            float headDisplacement = (head.position - breadcrumbs[0]).magnitude;

            if (headDisplacement >= segmentSpacing)
            {
                breadcrumbs.RemoveAt(breadcrumbs.Count - 1);
                breadcrumbs.Insert(0, head.position);
                headDisplacement = headDisplacement % segmentSpacing;
            }



            if (headDisplacement != 0)
            {
                Vector3 pos = Vector3.Lerp(breadcrumbs[1], breadcrumbs[0], headDisplacement / segmentSpacing);
                segments[0].transform.position = pos;
                segments[0].transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(breadcrumbs[0] - breadcrumbs[1]), Quaternion.LookRotation(head.position - breadcrumbs[0]), headDisplacement / segmentSpacing);
                for (int i = 1; i < segments.Count; i++)
                {
                    pos = Vector3.Lerp(breadcrumbs[i + 1], breadcrumbs[i], headDisplacement / segmentSpacing);
                    segments[i].transform.position = pos;
                    segments[i].transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(breadcrumbs[i] - breadcrumbs[i + 1]), Quaternion.LookRotation(breadcrumbs[i - 1] - breadcrumbs[i]), headDisplacement / segmentSpacing);
                }
            }
        }

        public void isRun(bool isrun)
        {
            for (int i = 0; i < segments.Count; i++)
            {
                //segments[i].GetComponent<Animator>().SetBool("isRun", isrun);
            }
        }

        /*
        //MAYBE CHANGE
        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Hostage")
            {
                Hostage hostage = other.GetComponent<Hostage>();
                if (!segments.Contains(hostage))
                {
                    segments.Add(hostage);
                    breadcrumbs.Add(other.transform.position);
                }
            }
        }
        */
    }
}
