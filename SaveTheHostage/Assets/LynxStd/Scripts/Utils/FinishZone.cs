using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NaStd {
    public class FinishZone : MonoBehaviour {

        public void OnFinishZone()
        {
            StartCoroutine(WaitForEndGame());
        }

        IEnumerator WaitForEndGame()
        {
            foreach (Zombie zombie in FindObjectsOfType(typeof(Zombie)))
            {
                zombie.FreezeZombie();
            }

            yield return new WaitForSeconds(0.25f);

            Player player = FindObjectOfType(typeof(Player)) as Player;
            player.GetPathFollower.DisableController();
            GameManager.instance.OnFinishGame();
        }
    }
}
