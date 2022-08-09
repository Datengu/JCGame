using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCanvasManager : MonoBehaviour
{
    public static DungeonCanvasManager instance;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
