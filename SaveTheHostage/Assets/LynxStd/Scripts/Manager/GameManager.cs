using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NaStd
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [Header("Items Manager")]
        public Transform parentUIItem;
        public GameObject prefabItem;
        [Header("Items Banana")]
        public int bananaSize = 1;
        public Sprite bananaSprite;
        private List<Image> bananaImage = new List<Image>();
        private int currentBanana = -1;

        [Header("Items Shield")]
        public Image shieldSprite;


        [Header("UI Game Over")]
        public GameObject parentUIGameOver;

        [HideInInspector]
        public bool isDone;
        [HideInInspector]
        public bool isShieldActive;

        private float timeShield;
        private float timeStamp;

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            CreateUIItems();
        }

        void Update()
        {
            if (isShieldActive)
            {
                timeStamp -= Time.deltaTime;
                shieldSprite.fillAmount = timeStamp / timeShield;

                if (timeStamp <= 0) {
                    timeStamp = 0;
                    isShieldActive = false;
                }
            }
        }

        public void CreateUIItems()
        {
            if (bananaSize <= 0)
                return;

            for (int x=0; x < bananaSize; x++)
            {
                GameObject itm = Instantiate(prefabItem, transform.position, Quaternion.identity);
                itm.transform.SetParent(parentUIItem);
                itm.transform.localScale = Vector3.one;
                bananaImage.Add(itm.GetComponent<Image>());
            }

        }

        public void SetPickItem(ItemType itemType, float time = 0)
        {
            
            switch (itemType)
            {
                case ItemType.Item:
                    currentBanana++;
                    bananaImage[currentBanana].sprite = bananaSprite;
                    bananaImage[currentBanana].color = Color.white;
                    break;

                case ItemType.Shield:
                    shieldSprite.fillAmount = 1;
                    isShieldActive = true;
                    timeShield = time;
                    timeStamp = time;
                    break;
            }
        }

        public void OnFinishGame()
        {
            if (!isDone)
            {
                Debug.Log("Show UI Game Score!");
                isDone = true;
            }
        }

        public void OnGameOver()
        {
            parentUIGameOver.SetActive(true);
        }

        public void OnPauseGame()
        {
            Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        }
    }
}
