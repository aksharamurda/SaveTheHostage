using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NaStd
{

    public enum EnemyType
    {
        STAY,
        PATROL
    }

    [RequireComponent(typeof(NavMeshAgent))]
    public class Zombie : MonoBehaviour
    {
        public float m_ViewRadius = 8;
        [Range(0, 360)]
        public float m_ViewAngle = 60;

        public LayerMask m_TargetMask;
        public LayerMask m_ObstacleMask;

        public bool m_CanSeePlayer;

        [HideInInspector]
        public List<Transform> m_VisibleTargets = new List<Transform>();

        public float m_MeshResolution = 1;
        public int m_EdgeResolveIteration = 4;
        public float m_EdgeDstThreshold = 0.5f;

        public MeshFilter m_ViewMeshFilter;
        Mesh viewMesh;

        [Range(-360, 360)]
        public float m_RotateAngleStart = 0f;
        [Range(-360, 360)]
        public float m_RotateAngleEnd = 45f;
        public float m_TimeToSpin = 1f;
        public float m_TimeToWait = 2f;

        public bool m_UseFov;
        public EnemyType m_EnemyType = EnemyType.STAY;


        public Transform m_PathRoot;
        public float m_PathSpeed = 5;
        public float m_PathTurnSpeed = 90;
        public float m_PathWaitTime = .3f;
        public float m_PathFindTime = 5f;
        public Color m_PathColorLine = new Color(0, 0, 0, 255);


        private Animator m_Animator;


        private NavMeshAgent navAgent;


        void Start()
        {
            m_Animator = GetComponent<Animator>();
            navAgent = GetComponent<NavMeshAgent>();

            if (m_UseFov)
            {

                viewMesh = new Mesh();
                viewMesh.name = "FOV";
                m_ViewMeshFilter.mesh = viewMesh;

                StartCoroutine("FindTargetWithDelay", m_PathFindTime);
            }

            if (m_EnemyType == EnemyType.PATROL)
            {
                m_Animator.SetBool("isPatrol", true);

                if (m_PathRoot != null)
                {
                    Vector3[] mWaypoints = new Vector3[m_PathRoot.childCount];
                    for (int i = 0; i < mWaypoints.Length; i++)
                    {
                        mWaypoints[i] = m_PathRoot.GetChild(i).position;
                        mWaypoints[i] = new Vector3(mWaypoints[i].x, transform.position.y, mWaypoints[i].z);
                    }

                    StartCoroutine("FollowPath", mWaypoints);
                }
            }
            else
            {
                //m_Animator.SetBool("isPatrol", false);
                StartCoroutine(LoopRotation(m_RotateAngleStart, m_RotateAngleEnd, m_TimeToWait));
            }
        }


        void LateUpdate()
        {
            if (m_UseFov)
            {
                DrawFieldOfView();

                if (m_EnemyType == EnemyType.PATROL)
                {
                    if (m_CanSeePlayer)
                    {
                        StopAllCoroutines();
                        StartCoroutine(TurnToFace(m_VisibleTargets[0].position));
                        navAgent.SetDestination(m_VisibleTargets[0].position);
                    }
                    else
                    {

                    }
                }
                else
                {
                    if (m_CanSeePlayer)
                    {

                        StopAllCoroutines();
                        StartCoroutine(TurnToFace(m_VisibleTargets[0].position));
                        navAgent.SetDestination(m_VisibleTargets[0].position);

                    }
                    else
                    {

                    }
                }

            }

            if (m_CanSeePlayer)
            {
                float dist = Vector3.Distance(transform.position, m_VisibleTargets[0].position);
                //Debug.Log(dist);
                if (dist < 1f)
                {
                    m_Animator.SetTrigger("isAttack");
                    m_Animator.SetBool("isMove", false);
                }
                else
                {
                    m_Animator.SetBool("isMove", true);
                }
            }

        }

        IEnumerator LoopRotation(float angleStart, float angleEnd, float waitTime)
        {
            var degreesPerRotation = 0f;
            var rotationAxis = Vector3.up;

            var rotateTimeStamp = 0.0f;
            Quaternion rotationStart;
            Quaternion rotationEnd = new Quaternion();

            var startRotation = transform.rotation;

            while (true)
            {
                rotationStart = transform.rotation;
                if (degreesPerRotation == angleStart)
                    degreesPerRotation = angleEnd;
                else
                    degreesPerRotation = angleStart;

                rotationEnd = Quaternion.AngleAxis(degreesPerRotation, rotationAxis) * startRotation;
                rotateTimeStamp = Time.time;

                while (Time.time - rotateTimeStamp < m_TimeToSpin)
                {
                    transform.rotation = Quaternion.Lerp(rotationStart, rotationEnd, (Time.time - rotateTimeStamp) / m_TimeToSpin);
                    yield return null;
                }

                transform.rotation = rotationEnd;
                yield return new WaitForSeconds(waitTime);

            }
        }

        IEnumerator FindTargetWithDelay(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                FindVisibleTargets();
            }
        }
        IEnumerator FollowPath(Vector3[] waypoints)
        {
            transform.position = waypoints[0];

            int targetWaypointIndex = 1;
            Vector3 targetWaypoint = waypoints[targetWaypointIndex];
            transform.LookAt(targetWaypoint);

            while (true)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, m_PathSpeed * Time.deltaTime);
                if (transform.position == targetWaypoint)
                {
                    //m_Animator.SetBool("isPatrol", false);
                    targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                    targetWaypoint = waypoints[targetWaypointIndex];

                    yield return new WaitForSeconds(m_PathWaitTime);

                    yield return StartCoroutine(TurnToFace(targetWaypoint));
                }
                yield return null;
            }
        }

        IEnumerator TurnToFace(Vector3 lookTarget)
        {
            Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
            float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

            while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
            {
                float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, m_PathTurnSpeed * Time.deltaTime);
                transform.eulerAngles = Vector3.up * angle;
                yield return null;
            }

            //m_Animator.SetBool("isMove", true);
        }

        void FindVisibleTargets()
        {
            m_VisibleTargets.Clear();
            m_CanSeePlayer = false;
            Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, m_ViewRadius, m_TargetMask);
            for (int i = 0; i < targetInViewRadius.Length; i++)
            {
                Transform target = targetInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, dirToTarget) < m_ViewAngle / 2)
                {
                    float distToTarget = Vector3.Distance(transform.position, target.position);
                    if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, m_ObstacleMask))
                    {
                        m_VisibleTargets.Add(target);
                        m_CanSeePlayer = true;
                    }
                }
            }
        }

        void DrawFieldOfView()
        {
            int stepCount = Mathf.RoundToInt(m_ViewAngle * m_MeshResolution);
            float stepAngleSize = m_ViewAngle / stepCount;
            List<Vector3> viewPoints = new List<Vector3>();

            ViewCastInfo oldViewCastInfo = new ViewCastInfo();

            for (int i = 0; i <= stepCount; i++)
            {
                float angle = transform.eulerAngles.y - m_ViewAngle / 2 + stepAngleSize * i;
                //Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * m_ViewRadius, Color.red);
                ViewCastInfo newViewCastInfo = ViewCast(angle);

                if (i > 0)
                {
                    bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCastInfo.mDistance - newViewCastInfo.mDistance) > m_EdgeDstThreshold;

                    if (oldViewCastInfo.mHit != newViewCastInfo.mHit || (oldViewCastInfo.mHit && newViewCastInfo.mHit && edgeDstThresholdExceeded))
                    {
                        EdgeInfo edge = FindEdge(oldViewCastInfo, newViewCastInfo);
                        if (edge.mPointA != Vector3.zero)
                        {
                            viewPoints.Add(edge.mPointA);
                        }
                        if (edge.mPointB != Vector3.zero)
                        {
                            viewPoints.Add(edge.mPointB);
                        }
                    }
                }

                viewPoints.Add(newViewCastInfo.mPoint);
                oldViewCastInfo = newViewCastInfo;
            }

            int vertexCount = viewPoints.Count + 1;
            Vector3[] verticles = new Vector3[vertexCount];
            int[] triangles = new int[(vertexCount - 2) * 3];

            verticles[0] = Vector3.zero;
            for (int i = 0; i < vertexCount - 1; i++)
            {
                verticles[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

                if (i < vertexCount - 2)
                {
                    triangles[i * 3] = 0;
                    triangles[i * 3 + 1] = i + 1;
                    triangles[i * 3 + 2] = i + 2;
                }

            }

            viewMesh.Clear();
            viewMesh.vertices = verticles;
            viewMesh.triangles = triangles;
            viewMesh.RecalculateNormals();
        }

        EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
        {
            float minAngle = minViewCast.mAngle;
            float maxAngle = maxViewCast.mAngle;

            Vector3 minPoint = Vector3.zero;
            Vector3 maxPoint = Vector3.zero;

            for (int i = 0; i < m_EdgeResolveIteration; i++)
            {
                float angle = (minAngle + maxAngle) / 2;
                ViewCastInfo newViewCast = ViewCast(angle);
                bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.mDistance - newViewCast.mDistance) > m_EdgeDstThreshold;

                if (newViewCast.mHit == minViewCast.mHit && !edgeDstThresholdExceeded)
                {
                    minAngle = angle;
                    minPoint = newViewCast.mPoint;
                }
                else
                {
                    maxAngle = angle;
                    maxPoint = newViewCast.mPoint;
                }
            }

            return new EdgeInfo(minPoint, maxPoint);
        }

        ViewCastInfo ViewCast(float globalAngle)
        {
            Vector3 dir = DirFromAngle(globalAngle, true);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, m_ViewRadius, m_ObstacleMask))
            {
                return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
            }
            else
            {
                return new ViewCastInfo(false, transform.position + dir * m_ViewRadius, m_ViewRadius, globalAngle);
            }
        }



        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += transform.eulerAngles.y;
            }

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        void OnDrawGizmos()
        {
            if (m_PathRoot != null && m_EnemyType == EnemyType.PATROL)
            {
                Vector3 startPosition = m_PathRoot.GetChild(0).position;
                Vector3 previousPosition = startPosition;

                foreach (Transform waypoint in m_PathRoot)
                {
                    Gizmos.color = m_PathColorLine;
                    Gizmos.DrawSphere(waypoint.position, .3f);
                    Gizmos.DrawLine(previousPosition, waypoint.position);
                    previousPosition = waypoint.position;
                }

                Gizmos.DrawLine(previousPosition, startPosition);
            }

        }

        public void CreatePath()
        {
            GameObject mRootPath = new GameObject("Path" + transform.name);
            m_PathRoot = mRootPath.transform;
            m_PathRoot.transform.position = new Vector3(transform.position.x, 0, transform.position.z);

            for (int i = 0; i < 3; i++)
            {
                GameObject mChildPath = new GameObject("Path (" + i + ")");
                mChildPath.transform.SetParent(m_PathRoot);
                mChildPath.transform.localPosition = Vector3.zero;
            }

        }

        public void CreateFov()
        {
            GameObject mMeshFilter = Instantiate(Resources.Load("Prefabs/FOV Visualisation")) as GameObject;
            mMeshFilter.name = "FOV Visualisation";
            mMeshFilter.transform.SetParent(transform);
            mMeshFilter.transform.localPosition = new Vector3(0, 1, 0);
            m_ViewMeshFilter = mMeshFilter.GetComponent<MeshFilter>();
        }

        public struct ViewCastInfo
        {
            public bool mHit;
            public Vector3 mPoint;
            public float mDistance;
            public float mAngle;

            public ViewCastInfo(bool hit, Vector3 point, float dst, float angle)
            {
                mHit = hit;
                mPoint = point;
                mDistance = dst;
                mAngle = angle;
            }
        }

        public struct EdgeInfo
        {
            public Vector3 mPointA;
            public Vector3 mPointB;

            public EdgeInfo(Vector3 pointA, Vector3 pointB)
            {
                mPointA = pointA;
                mPointB = pointB;
            }
        }
    }
}
