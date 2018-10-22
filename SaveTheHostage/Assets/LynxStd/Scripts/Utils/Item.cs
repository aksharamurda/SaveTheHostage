using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NaStd
{
    public enum ItemType { Item, Shield }

    public class Item : MonoBehaviour
    {

        public ItemType itemType;
        public bool rotateRight;

        public Transform itemTrans;
        public float speed = 5;

        public GameObject FxItem;
        public GameObject FxShield;

        private float heightMax = 1;
        private bool isPickUp;

        public float timeShieldActive;

        void Start()
        {

        }

        void Update()
        {
            Vector3 rot = rotateRight ? Vector3.right : Vector3.up;
            itemTrans.Rotate(rot * Time.deltaTime * speed * 10, Space.Self);

            Vector3 itemPos = itemTrans.position;
            itemPos.y = Mathf.PingPong(Time.time / 2, heightMax);
            itemTrans.position = itemPos;
        }

        public void OnPickItem()
        {
            Debug.Log("Pick Item!");
            if (!isPickUp)
            {
                switch (itemType)
                {
                    case ItemType.Item:
                        Instantiate(FxItem, transform.position, Quaternion.identity);
                        break;
                    case ItemType.Shield:
                        Instantiate(FxShield, transform.position, Quaternion.identity);
                        break;
                }

                GameManager.instance.SetPickItem(itemType, timeShieldActive);
                isPickUp = true;
                Destroy(gameObject);
            }

        }
    }
}
