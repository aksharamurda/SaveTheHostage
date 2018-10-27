using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace NaStd
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        private Transform parentUIItem;
        
        [Header("Level Data")]
        public LevelSettings levelSettings;
        private Zone zone;
        private List<Image> itemImages = new List<Image>();
        private int currentItem = -1;
        private int itemPickSize;

        private Image shieldSprite;

        private GameObject parentUIGameOver;
        private GameObject parentUIGameScore;
        private GameObject levelUIName;
        private GameObject totalCoin;

        private Animator starAnimator;

        public Image starA;
        public Image starB;
        public Image starC;

        [HideInInspector]
        public bool isDone;
        [HideInInspector]
        public bool isShieldActive;

        private float timeShield;
        private float timeStamp;


        void Awake()
        {
            instance = this;
            //PlayerSave.CreatePlayerProfile();

            totalCoin = GameObject.Find("TotalCoinUI");
            levelUIName = GameObject.Find("LevelNameUI");
            starAnimator = GameObject.Find("StarUI").GetComponent<Animator>();
            parentUIItem = GameObject.Find("PanelItemsUI").transform;

            GameObject shieldUI = GameObject.Find("ShieldUI");
            shieldSprite = shieldUI.GetComponentInChildren<Image>();
            shieldUI.SetActive(levelSettings.haveShield);

            //levelUIName.GetComponent<Text>().text = level.levelName;

            parentUIGameOver = GameObject.Find("PanelEndGameUI");
            parentUIGameScore = GameObject.Find("PanelSuccessGameUI");

            //playerProfile = PlayerSave.GetPlayerProfile();
            //totalCoin.GetComponent<Text>().text = playerProfile.playerCoin.ToString();
            if (ZoneData.GetZoneData(levelSettings.zone) != null)
                zone = (ZoneData.GetZoneData(levelSettings.zone));

            foreach (Level lvl in zone.levels)
            {
                if (lvl.levelName == levelSettings.refLevelScene)
                {
                    levelSettings.level = lvl;
                }
            }
        }

        void Start()
        {
            parentUIGameOver.SetActive(false);
            parentUIGameScore.SetActive(false);
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
            if (levelSettings.itemSize <= 0)
                return;

            for (int x=0; x < levelSettings.itemSize; x++)
            {
                GameObject itm = Instantiate(levelSettings.prefabItem, transform.position, Quaternion.identity);
                itm.transform.SetParent(parentUIItem);
                itm.transform.localScale = Vector3.one;
                itemImages.Add(itm.GetComponent<Image>());
            }

        }

        public void SetPickItem(ItemType itemType, float time = 0)
        {
            
            switch (itemType)
            {
                case ItemType.Item:
                    currentItem++;
                    itemImages[currentItem].sprite = levelSettings.itemSprite;
                    itemImages[currentItem].color = Color.white;
                    itemPickSize++;
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
                parentUIGameScore.SetActive(true);
                
                StartCoroutine(WaitShowGameScore());
                isDone = true;
            }
        }

        IEnumerator WaitShowGameScore()
        {
            yield return new WaitForSeconds(0.25f);
            //float persenScore = (float)(itemPickSize) / (float)(level.itemSize);
            //float starPersen = persenScore * 100f;

            //Debug.Log(itemPickSize + " | " + level.itemSize + " | " + starPersen);

            switch (itemPickSize)
            {
                case 1:
                    starAnimator.SetTrigger("1Star");
                    if(!levelSettings.level.levelComplete && (itemPickSize > levelSettings.level.findItem))
                        levelSettings.level.findItem = 1;

                    break;
                case 2:
                    starAnimator.SetTrigger("2Star");
                    if (!levelSettings.level.levelComplete && (itemPickSize > levelSettings.level.findItem))
                        levelSettings.level.findItem = 2;
                    break;
                case 3:
                    starAnimator.SetTrigger("3Star");
                    levelSettings.level.findItem = 3;
                    levelSettings.level.levelComplete = true;
                    break;
            }

            for(int x=0; x < zone.levels.Count; x++)
            {
                if (zone.levels[x].levelName == levelSettings.refLevelScene)
                {
                    zone.levels[x] = levelSettings.level;

                    if(itemPickSize > 0)
                        if((x + 1) < zone.levels.Count)
                        {
                            zone.levels[x + 1].Unlocked = true;
                        }
                }

                int sumFindItem = 0;
                sumFindItem += zone.levels[x].findItem;

                if (sumFindItem >= zone.itemGoal)
                {
                    zone.missionComplete = true;
                }
            }

            ZoneData.UpdateZoneData(zone);
            //totalCoin.GetComponent<Text>().text = playerProfile.playerCoin.ToString();

        }

        public void OnGameOver()
        {
            StartCoroutine(WaitShowGameOver());
        }

        IEnumerator WaitShowGameOver()
        {
            yield return new WaitForSeconds(0.5f);
            parentUIGameOver.SetActive(true);
        }

        public void OnButtonPause()
        {
            Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        }

        public void OnButtonRetry()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void OnButtonNextLevel(string levelname)
        {
            SceneManager.LoadScene(levelname);
        }

        public void OnButtonBackToMenu(string levelname)
        {
            SceneManager.LoadScene(levelname);
        }
    }
}
