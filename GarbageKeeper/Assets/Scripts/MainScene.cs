using System;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    public Transform tilesRoot;
    public Transform topTilesTopCorner;

    public TextAsset mapGenerator;

    public List<Vector3> Checkpoints { get; private set; }

    public void Awake()
    {
        GameManager.Instance.mainScene = this;
        Checkpoints = new List<Vector3>();
        foreach (Transform child in tilesRoot)
            Destroy(child.gameObject);
        GenerateMap();
    }

    private void GenerateMap()
    {
        var generatorLines = mapGenerator.text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        var sortedCheckpoints = new SortedDictionary<char, Vector3>();
        for(int i = 0; i < generatorLines.Length; ++i)
        {
            var line = generatorLines[i];
            for(int j = 0; j < line.Length; ++j)
            {
                Vector2 currentXZ = new Vector3(topTilesTopCorner.transform.localPosition.x + j, topTilesTopCorner.transform.localPosition.z - i);
                if (line[j].Equals('@'))
                {
                    var newBuildableTile = (GameObject)Instantiate(Resources.Load("Prefabs/BuildableTile"));
                    newBuildableTile.transform.SetParent(tilesRoot.transform);
                    newBuildableTile.transform.localPosition = new Vector3(currentXZ.x, topTilesTopCorner.transform.localPosition.y, currentXZ.y);
                }
                else if(line[j].Equals('-'))
                {
                    //Add generate empty tile ?
                }
                else
                {
                    sortedCheckpoints[line[j]] = new Vector3(currentXZ.x, topTilesTopCorner.transform.position.y, currentXZ.y);
                }
            }
        }
        Checkpoints.AddRange(sortedCheckpoints.Values);
    }

    public void RefreshLifeIndicator()
    {

    }
}
