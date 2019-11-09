using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftManager : MonoBehaviour
{
    private Resource slotOne;
    private Resource slotTwo;

    public Button craftOne;
    public Button craftTen;
    public Button craftFifty;

    public Image result;
    public Sprite resultBasicSprite;

    public List<ResourceImages> resourceImages;

    private bool canCraft = false;

    private static CraftManager _instance = null;
    public static CraftManager Instance
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

    
    public void addResource(int slot, GameObject res)
    {

        Debug.Log(slot);
        switch (slot)
        {
            case 1:
                slotOne = res.GetComponent<Resource>();
                break;
            case 2:
                slotTwo = res.GetComponent<Resource>();
                break;

            default:
                break;
        }
    }

    public void removeResource(int slot)
    {
        switch (slot)
        {
            case 1:
                slotOne = null;
                break;
            case 2:
                slotTwo = null;
                break;

            default:
                break;
        }
    }

    private void Update()
    {

        if(slotOne == null || slotTwo == null)
        {
            result.sprite = resultBasicSprite;
        }

        if(slotOne != null && slotTwo != null)
        {
            Recette rec = RecetteManager.Instance.CheckResult(slotOne.resourceType, slotTwo.resourceType);
            Sprite sprite = GetSpriteFromAmmoType(rec.result);
            result.sprite = sprite;
            
        }
    }


    private Sprite GetSpriteFromAmmoType(Settings.AmmoType type)
    {
        foreach (var resImg in resourceImages)
        {
            if(resImg.ammoType == type)
            {
                return resImg.sprite;
            }
        }
        return null;
    }


}
