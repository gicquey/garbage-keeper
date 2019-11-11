using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftManager : MonoBehaviour
{
    private ResourceDropHandler slotOneDrop;
    private ResourceDropHandler slotTwoDrop;

    private ResourceItem slotOne;
    private ResourceItem slotTwo;

    public Button craftOne;
    public Button craftTen;
    public Button craftFifty;
    public Button craftMax;

    public Image resultImage;
    public Sprite resultBasicSprite;

    public List<ResourceImages> resourceImages;

    private Recette recette;
    private bool canCraft = false;
    private int firstSlotAvailableQuantity;
    private int secondSlotAvailableQuantity;

    private static CraftManager _instance = null;
    public static CraftManager Instance
    {
        get { return _instance; }
    }

    private int _maxAvailableQuantity
    {
        get
        {
            if (isSameResource)
            {
                if (firstSlotAvailableQuantity == secondSlotAvailableQuantity)
                {
                    return (int) firstSlotAvailableQuantity / 2;
                }

                if (firstSlotAvailableQuantity > secondSlotAvailableQuantity)
                {
                    return (int)firstSlotAvailableQuantity / secondSlotAvailableQuantity;
                }
                else
                {
                    return (int)secondSlotAvailableQuantity / firstSlotAvailableQuantity;
                }
            }

            if (firstSlotAvailableQuantity == secondSlotAvailableQuantity)
            {
                return firstSlotAvailableQuantity;
            }
            if (firstSlotAvailableQuantity > secondSlotAvailableQuantity)
            {
                return secondSlotAvailableQuantity % firstSlotAvailableQuantity;
            }
            else
            {
                return firstSlotAvailableQuantity % secondSlotAvailableQuantity;
            }
        }
    }

    private bool isSameResource
    {
        get
        {
            return slotOne.resourceType == slotTwo.resourceType && slotOne != null && slotTwo != null;
        }
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
    }

    private void Start()
    {
        craftOne.enabled = false;
        craftTen.enabled = false;
        craftFifty.enabled = false;
    }


    public void addResource(int slot, ResourceDropHandler res)
    {

        switch (slot)
        {
            case 1:
                slotOneDrop = res;
                slotOne = res.currentObjectIn;
                firstSlotAvailableQuantity = InventoryManager.Instance.getQuantityForGivenResource(slotOne.resourceType);
                break;
            case 2:
                slotTwoDrop = res;
                slotTwo = res.currentObjectIn;
                secondSlotAvailableQuantity = InventoryManager.Instance.getQuantityForGivenResource(slotTwo.resourceType);
                break;

            default:
                break;
        }
    }

    public void RemoveResource(int slot)
    {
        switch (slot)
        {
            case 1:
                slotOneDrop.RemoveResource();
                slotOne = null;
                firstSlotAvailableQuantity = 0;
                break;
            case 2:
                slotTwoDrop.RemoveResource();
                slotTwo = null;
                secondSlotAvailableQuantity = 0;
                break;

            default:
                break;
        }
    }

    private void Update()
    {
        if (slotOne == null || slotTwo == null)
        {
            resultImage.sprite = resultBasicSprite;
        }

        if (slotOne != null && slotTwo != null)
        {
            CheckRecipeAndUpdateResult();
            firstSlotAvailableQuantity = InventoryManager.Instance.getQuantityForGivenResource(slotOne.resourceType);
            secondSlotAvailableQuantity = InventoryManager.Instance.getQuantityForGivenResource(slotTwo.resourceType);
            CraftButtonManagement();
        }
    }

    private void CraftButtonManagement()
    {
        if (firstSlotAvailableQuantity > 0 && secondSlotAvailableQuantity > 0 && !isSameResource)
        {
            craftOne.enabled = true;
            craftMax.enabled = true;
        }
        else
        {
            craftOne.enabled = false;
            craftTen.enabled = false;
            craftFifty.enabled = false;
            craftMax.enabled = false;
        }
        if (firstSlotAvailableQuantity > 9 && secondSlotAvailableQuantity > 9 && !isSameResource)
        {
            craftTen.enabled = true;
        }
        else
        {
            craftTen.enabled = false;
            craftFifty.enabled = false;
        }

        if (firstSlotAvailableQuantity > 49 && secondSlotAvailableQuantity > 49 && !isSameResource)
        {
            craftFifty.enabled = true;
        }
        else
        {
            craftFifty.enabled = false;
        }

        if (isSameResource)
        {
            if (_maxAvailableQuantity > 0)
            {
                craftOne.enabled = true;
                craftMax.enabled = true;
            }
            else
            {
                craftOne.enabled = false;
                craftTen.enabled = false;
                craftFifty.enabled = false;
                craftMax.enabled = false;
            }

            if (_maxAvailableQuantity > 9)
            {
                craftTen.enabled = true;
            }
            else
            {
                craftTen.enabled = false;
                craftFifty.enabled = false;
            }

            if (_maxAvailableQuantity > 49)
            {
                craftFifty.enabled = true;
            }
            else
            {
                craftFifty.enabled = false;
            }
        }
    }

    private void CheckRecipeAndUpdateResult()
    {
        recette = RecetteManager.Instance.CheckResult(slotOne.resourceType, slotTwo.resourceType);
        Sprite sprite = GetSpriteFromAmmoType(recette.result);
        resultImage.sprite = sprite;
    }

    private Sprite GetSpriteFromAmmoType(Settings.AmmoType type)
    {
        foreach (var resImg in resourceImages)
        {
            if (resImg.ammoType == type)
            {
                return resImg.sprite;
            }
        }
        return null;
    }

    public void CraftOneHandler()
    {
        firstSlotAvailableQuantity -= 1;
        secondSlotAvailableQuantity -= 1;
        InventoryManager.Instance.UpdateResourceQuantity(slotOne.resourceType, -1);
        InventoryManager.Instance.UpdateResourceQuantity(slotTwo.resourceType, -1);
        InventoryManager.Instance.UpdateAmmoQuantity(recette.result, recette.resultAmount);
        if (!CanCraft())
        {
            ResetSlots();
        }
    }

    public void CraftTenHandler()
    {
        firstSlotAvailableQuantity -= 10;
        secondSlotAvailableQuantity -= 10;
        InventoryManager.Instance.UpdateResourceQuantity(slotOne.resourceType, -10);
        InventoryManager.Instance.UpdateResourceQuantity(slotTwo.resourceType, -10);
        InventoryManager.Instance.UpdateAmmoQuantity(recette.result, recette.resultAmount * 10);
        if (!CanCraft())
        {
            ResetSlots();
        }
    }

    public void CraftFiftyHandler()
    {
        firstSlotAvailableQuantity -= 50;
        secondSlotAvailableQuantity -= 50;
        InventoryManager.Instance.UpdateResourceQuantity(slotOne.resourceType, -50);
        InventoryManager.Instance.UpdateResourceQuantity(slotTwo.resourceType, -50);
        InventoryManager.Instance.UpdateAmmoQuantity(recette.result, recette.resultAmount * 50);
        if (!CanCraft())
        {
            ResetSlots();
        }
    }

    public void CraftMaxHandler()
    {

        int value = _maxAvailableQuantity;

        if (isSameResource)
        {
            firstSlotAvailableQuantity = 0;
            secondSlotAvailableQuantity = 0;
        }
        else
        {
            firstSlotAvailableQuantity -= value;
            secondSlotAvailableQuantity -= value;
        }
        InventoryManager.Instance.UpdateResourceQuantity(slotOne.resourceType, -value);
        InventoryManager.Instance.UpdateResourceQuantity(slotTwo.resourceType, -value);
        InventoryManager.Instance.UpdateAmmoQuantity(recette.result, recette.resultAmount * value);
        if (!CanCraft())
        {
            ResetSlots();
        }
    }


    private bool CanCraft()
    {
        return _maxAvailableQuantity > 0;
    }

    private void ResetSlots()
    {
        RemoveResource(1);
        RemoveResource(2);
    }
}
