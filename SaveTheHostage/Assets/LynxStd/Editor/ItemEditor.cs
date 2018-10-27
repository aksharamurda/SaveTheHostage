using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace NaStd
{
    [CustomEditor(typeof(Item))]
    public class ItemEditor : Editor
    {
        private Item item;

        void OnSceneGUI()
        {
            item = (Item)target;
        }

        public override void OnInspectorGUI()
        {
            item = (Item)target;

            EditorGUILayout.BeginVertical("Box");
            EditorStyles.label.fontStyle = FontStyle.Normal;
            item.itemType = (ItemType)EditorGUILayout.EnumPopup("Item Type", item.itemType);

            item.itemTrans = EditorGUILayout.ObjectField("Item Transform", item.itemTrans, typeof(Transform), true) as Transform;
            item.rotateRight = EditorGUILayout.Toggle("Rotate Right", item.rotateRight, EditorStyles.toggle);
            item.speed = EditorGUILayout.FloatField("Rotate Speed", item.speed);

            if (item.itemType == ItemType.Item)
            {
                item.FxItem = EditorGUILayout.ObjectField("Fx Item", item.FxItem, typeof(GameObject), true) as GameObject;
            }else if (item.itemType == ItemType.Shield)
            {
                item.FxShield = EditorGUILayout.ObjectField("Fx Shield", item.FxShield, typeof(GameObject), true) as GameObject;
                item.timeShieldActive = EditorGUILayout.FloatField("Time Shield (s)", item.timeShieldActive);
            }




            EditorGUILayout.EndVertical();
            EditorUtility.SetDirty(target);
        }
    }
}
