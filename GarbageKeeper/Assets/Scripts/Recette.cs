using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class Recette 
{
    public List<Settings.Elements> recette;
    public Settings.AmmoType result;
    public int amount;
}