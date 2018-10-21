using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace NaStd
{
    [CustomEditor(typeof(Zombie))]
    public class ZombieEditor : Editor
    {
        private Zombie zombie;

        void OnSceneGUI()
        {
            zombie = (Zombie)target;
            Handles.color = Color.white;

            Handles.DrawWireArc(zombie.m_ViewMeshFilter.transform.position, Vector3.up, Vector3.forward, 360, zombie.m_ViewRadius);
            Vector3 viewAngleA = zombie.DirFromAngle(-zombie.m_ViewAngle / 2, false);
            Vector3 viewAngleB = zombie.DirFromAngle(zombie.m_ViewAngle / 2, false);

            Handles.DrawLine(zombie.m_ViewMeshFilter.transform.position, zombie.m_ViewMeshFilter.transform.position + viewAngleA * zombie.m_ViewRadius);
            Handles.DrawLine(zombie.m_ViewMeshFilter.transform.position, zombie.m_ViewMeshFilter.transform.position + viewAngleB * zombie.m_ViewRadius);

            Handles.color = Color.red;
            foreach (Transform visibleTarget in zombie.m_VisibleTargets)
            {
                Handles.DrawLine(zombie.transform.position, visibleTarget.position);
            }


        }

        public override void OnInspectorGUI()
        {
            zombie = (Zombie)target;

            zombie.damageZombie = EditorGUILayout.FloatField("Damage", zombie.damageZombie);

            #region TOGGLE_USE_FOV
            //TOGGLE USE FOV
            EditorGUILayout.BeginVertical("Box");
            var origFontStyle = EditorStyles.label.fontStyle;
            EditorStyles.label.fontStyle = FontStyle.Normal;
            zombie.m_UseFov = EditorGUILayout.Toggle("Use Field Of View", zombie.m_UseFov, EditorStyles.toggle);
            EditorStyles.label.fontStyle = origFontStyle;


            if (zombie.m_UseFov)
            {
                zombie.m_ViewMeshFilter = EditorGUILayout.ObjectField("FOV Mesh", zombie.m_ViewMeshFilter, typeof(MeshFilter), true) as MeshFilter;
                if (zombie.m_ViewMeshFilter == null)
                {
                    EditorGUILayout.HelpBox("FOV Mesh not set.", MessageType.Error);
                    if (GUILayout.Button("Create FOV Mesh"))
                    {
                        zombie.CreateFov();
                    }
                }

                //VIEW RADIUS
                zombie.m_ViewRadius = EditorGUILayout.FloatField("View Radius", zombie.m_ViewRadius);

                //VIEW ANGLE
                GUILayout.BeginHorizontal();
                GUILayout.Label("View Angle", GUILayout.Width(75));
                GUILayout.FlexibleSpace();
                zombie.m_ViewAngle = EditorGUILayout.Slider(zombie.m_ViewAngle, 0, 360);
                GUILayout.EndHorizontal();

                //LAYERMASK OBSTACLE
                zombie.m_ObstacleMask = EditorTools.LayerMaskField("Obstacles Layer", zombie.m_ObstacleMask);

                //LAYERMASK TARGET
                zombie.m_TargetMask = EditorTools.LayerMaskField("Targets Layer", zombie.m_TargetMask);

                //MESH RESOLUTION
                zombie.m_MeshResolution = EditorGUILayout.FloatField("FOV Resolution", zombie.m_MeshResolution);

                //EDGE RESOLVE ITERATION
                zombie.m_EdgeResolveIteration = EditorGUILayout.IntField("Edge Resolve", zombie.m_EdgeResolveIteration);

                //EDGE DISTANCE THRESHOLD
                zombie.m_EdgeDstThreshold = EditorGUILayout.FloatField("Edge Distance", zombie.m_EdgeDstThreshold);

            }

            EditorGUILayout.EndVertical();
            #endregion

            #region TOGGLE_USE_PATROL_OR_ROTATE
            //TOGGLE USE PATROL
            EditorGUILayout.BeginVertical("Box");
            EditorStyles.label.fontStyle = FontStyle.Normal;
            //zombie.m_UsePatrol = EditorGUILayout.Toggle("Use Patrol", zombie.m_UsePatrol, EditorStyles.toggle);
            zombie.m_EnemyType = (EnemyType)EditorGUILayout.EnumPopup("Enemy Type", zombie.m_EnemyType);
            EditorStyles.label.fontStyle = origFontStyle;


            if (zombie.m_EnemyType == EnemyType.PATROL)
            {
                zombie.m_PathRoot = EditorGUILayout.ObjectField("Path", zombie.m_PathRoot, typeof(Transform), true) as Transform;

                if (zombie.m_PathRoot == null)
                {
                    EditorGUILayout.HelpBox("Path not set.", MessageType.Error);
                    if (GUILayout.Button("Create Path"))
                    {
                        zombie.CreatePath();
                    }
                }

                zombie.m_PathColorLine = EditorGUILayout.ColorField("Line Color", zombie.m_PathColorLine);
                zombie.m_PathFindTime = EditorGUILayout.FloatField("Find Target Time", zombie.m_PathFindTime);
                zombie.m_PathSpeed = EditorGUILayout.FloatField("Move Speed", zombie.m_PathSpeed);
                zombie.m_PathTurnSpeed = EditorGUILayout.FloatField("Turn Speed", zombie.m_PathTurnSpeed);
                zombie.m_PathWaitTime = EditorGUILayout.FloatField("Turn Time", zombie.m_PathWaitTime);

            }
            else
            {
                //EditorGUILayout.HelpBox("Nilai sudut -360 ~ 360, nilai 0 selalu merepresentasikan dari nilai rotasi y object.", MessageType.Info);

                //ANGLE ROTATE START
                GUILayout.BeginHorizontal();
                GUILayout.Label("Angle Start", GUILayout.Width(75));
                GUILayout.FlexibleSpace();
                zombie.m_RotateAngleStart = EditorGUILayout.Slider(zombie.m_RotateAngleStart, -360, 360);
                GUILayout.EndHorizontal();

                //ANGLE ROTATE START
                GUILayout.BeginHorizontal();
                GUILayout.Label("Angle End", GUILayout.Width(75));
                GUILayout.FlexibleSpace();
                zombie.m_RotateAngleEnd = EditorGUILayout.Slider(zombie.m_RotateAngleEnd, -360, 360);
                GUILayout.EndHorizontal();

                zombie.m_PathFindTime = EditorGUILayout.FloatField("Find Target Time", zombie.m_PathFindTime);
                //TIME ROTATE
                zombie.m_TimeToSpin = EditorGUILayout.FloatField("Rotate Time", zombie.m_TimeToSpin);

                //WAIT ROTATE
                zombie.m_TimeToWait = EditorGUILayout.FloatField("Wait Time", zombie.m_TimeToWait);
            }

            EditorGUILayout.EndVertical();

            EditorUtility.SetDirty(target);
            #endregion
        }
    }
}
