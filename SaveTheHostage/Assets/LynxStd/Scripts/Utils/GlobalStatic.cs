using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStatic{

    public static PlayerStats GetPlayerStats {
        get { return Resources.Load("PlayerStats") as PlayerStats; }
    }
}
