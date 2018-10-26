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
        public DataLevel level;
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

        private PlayerStats playerStats;

        void Awake()
        {
            instance = this;
            PlayerSave.CreatePlayerStats();

            totalCoin = GameObject.Find("TotalCoinUI");
            levelUIName = GameObject.Find("LevelNameUI");
            starAnimator = GameObject.Find("StarUI").GetComponent<Animator>();
            parentUIItem = GameObject.Find("PanelItemsUI").transform;

            GameObject shieldUI = GameObject.Find("ShieldUI");
            shieldSprite = shieldUI.GetComponentInChildren<Image>();
            shieldUI.SetActive(level.haveShield);

            levelUIName.GetComponent<Text>().text = level.levelName;

            parentUIGameOver = GameObject.Find("PanelEndGameUI");
            parentUIGameScore = GameObject.Find("PanelSuccessGameUI");

            playerStats = PlayerSave.GetPlayerStats();
            totalCoin.GetComponent<Text>().text = playerStats.playerCoin.ToString();
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
            if (level.itemSize <= 0)
                return;

            for (int x=0; x < level.itemSize; x++)
            {
                GameObject itm = Instantiate(level.prefabItem, transform.position, Quaternion.identity);
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
                    itemImages[currentItem].sprite = level.itemSprite;
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
            playerStats = PlayerSave.GetPlayerStats();

            yield return new WaitForSeconds(0.25f);
            float persenScore = (float)(itemPickSize) / (float)(level.itemSize);
            float starPersen = persenScore * 100f;

            Debug.Log(itemPickSize + " | " + level.itemSize + " | " + starPersen);

            if (starPersen <= 33.4f)
            {
                starAnimator.SetTrigger("1Star");
                playerStats.playerCoin += 10;
                playerStats.playerStar += 1;
            }
            else if (starPersen > 33.4f && starPersen <= 66.7f)
            {
                starAnimator.SetTrigger("2Star");
                playerStats.playerCoin += 35;
                playerStats.playerStar += 2;
            }
            else if (starPersen > 66.7f)
            {
                starAnimator.SetTrigger("3Star");
                playerStats.playerCoin += 85;
                playerStats.playerStar += 3;
            }

            totalCoin.GetComponent<Text>().text = playerStats.playerCoin.ToString();
            PlayerSave.SavePlayerStats(playerStats);
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
