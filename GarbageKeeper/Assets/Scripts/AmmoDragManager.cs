using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDragManager : MonoBehaviour
{
    public bool isDragging;
    public bool canDrop;
    public Tourelle dropZone;

    private static AmmoDragManager _instance = null;
    public static AmmoDragManager Instance
    {
        get { return _instance; }
    }
    

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void setDropZone(Tourelle t)
    {
        canDrop = true;
        dropZone = t;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void dropAmmo(Settings.AmmoType ammoType, int droppedQuantity)
    {
        Debug.Log("Je drop");
        dropZone.AddAmmo(droppedQuantity, ammoType);
        InventoryManager.Instance.UpdateAmmoQuantity(ammoType, -droppedQuantity);
    }
}
