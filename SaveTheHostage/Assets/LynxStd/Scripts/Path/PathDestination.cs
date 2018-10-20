using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NaStd
{
    [RequireComponent(typeof(CharacterController))]
    public class PathDestination : MonoBehaviour
    {
        public GameObject objectOnPath { get; set; }
    }
}
