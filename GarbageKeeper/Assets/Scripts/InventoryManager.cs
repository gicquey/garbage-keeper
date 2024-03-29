﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    public List<Resource> ResourceInventory;
    public List<Ammo> AmmoInventory;

    [Header("Basic resources quantity text")]
    public Text Organic;
    public Text Solid;
    public Text Chemical;
    public Text Fabric;

    [Header("Crafted ammo quantity text")]
    public Text Poison;
    public Text Explosive;
    public Text Battery;
    public Text Puddle;
    public Text Monney;
    public Text Clothes;

    private static InventoryManager _instance = null;
    public static InventoryManager Instance
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
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (DebugManager.Instance.enabled && DebugManager.Instance.canCheatToCraftAmmo && Input.GetKeyDown(KeyCode.G))
        {
            UpdateResourceQuantity(Settings.Elements.chimical, 10);
            UpdateResourceQuantity(Settings.Elements.organic, 10);
            UpdateResourceQuantity(Settings.Elements.fabric, 10);
            UpdateResourceQuantity(Settings.Elements.solid, 10);

            UpdateAmmoQuantity(Settings.AmmoType.battery, 10);
            UpdateAmmoQuantity(Settings.AmmoType.explosive, 10);
            UpdateAmmoQuantity(Settings.AmmoType.puddle, 10);
            UpdateAmmoQuantity(Settings.AmmoType.clothes, 10);
            UpdateAmmoQuantity(Settings.AmmoType.poison, 10);
            GameManager.Instance.UpdateMoney(100);
        }
        UpdateTextValue();
    }

    public void ObtainResourcesForEnnemy(EnnemyTypes ennemyType)
    {
        foreach(var resourceType in Enum.GetValues(typeof(Settings.Elements)).Cast<Settings.Elements>())
        {
            var resourceMinMax = Settings.Instance.ResourcesGivenByEnemies[ennemyType][resourceType];
            UpdateResourceQuantity(resourceType, UnityEngine.Random.Range(resourceMinMax.Min, resourceMinMax.Max));
        }
        SoundHelper.Instance.play(AudioConfig.Instance.GetClipForSoundType(SoundTypes.LOOT));
    }

    private void UpdateTextValue()
    {
        foreach (var resource in ResourceInventory)
        {
            resource.quantityText.text = resource.quantity.ToString();
        }

        foreach (var ammo in AmmoInventory)
        {
            ammo.quantityText.text = ammo.quantity.ToString();
        }

        Monney.text = GameManager.Instance.CurrentMoney.ToString();
    }

    public void UpdateResourceQuantity(Settings.Elements resourceType, int quantity)
    {
        foreach (var resource in ResourceInventory)
        {
            if (resource.resource == resourceType)
            {
                resource.quantity += quantity;
            }
        }
        UpdateTextValue();
        FeedbackManager.Instance.OrderPushFeedback(resourceType, quantity);
    }

    public void UpdateAmmoQuantity(Settings.AmmoType ammoType, int quantity)
    {
        foreach (var ammo in AmmoInventory)
        {
            if (ammo.ammoType == ammoType)
            {
                ammo.quantity += quantity;
            }
        }
        UpdateTextValue();
    }

    public int getQuantityForGivenResource(Settings.Elements resourceType)
    {
        foreach (var resource in ResourceInventory)
        {
            if (resource.resource == resourceType)
            {
                return resource.quantity;
            }
        }
        return -1;
    }

    public int getQuantityForGivenAmmo(Settings.AmmoType ammoType)
    {
        foreach (var ammo in AmmoInventory)
        {
            if (ammo.ammoType == ammoType)
            {
                return ammo.quantity;
            }
        }
        return -1;
    }
}
