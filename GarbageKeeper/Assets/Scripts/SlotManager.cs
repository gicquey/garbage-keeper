using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{


    private static SlotManager _instance = null;
    public static SlotManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new SlotManager();
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
