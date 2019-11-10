
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecetteManager : MonoBehaviour
{

    private static RecetteManager _instance = null;
    public static RecetteManager Instance
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

    public List<Recette> recettes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Recette CheckResult(Settings.Elements ingredient1, Settings.Elements ingredient2)
    {
        List<Settings.Elements> craft = new List<Settings.Elements>()
        {
            ingredient1,
            ingredient2
        };

        int index = 0;
        foreach (var recette in recettes)
        {
            if (recette.recette.Intersect(craft).Count() == 2)
            {
                return recette;
            }

            if(recettes[index].recette[0] == craft[0] && recettes[index].recette[1] == craft[1])
            {
                return recette;
            }

            index++;
        }

        

        return null;
    }

}
