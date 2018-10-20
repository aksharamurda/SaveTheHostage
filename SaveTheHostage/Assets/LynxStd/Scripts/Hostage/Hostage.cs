using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NaStd
{
    public class Hostage : MonoBehaviour
    {
        public void ZombieCatchMe()
        {
            Debug.Log("Zombie Catch Me!");
        }

        public void PlayerSaveMe()
        {
            Debug.Log("Player Save Me!");
            Destroy(gameObject, 1f);
        }
    }
}
