using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnnemyGenerator : MonoBehaviour
{
    [Serializable]
    public class EnnemyRelativeProbability
    {
        public EnnemyTypes ennemyType;
        public int relativeProbability;
    }

    [Serializable]
    public class EnnemyProbabilitiesOnTimePeriod
    {
        public float endPeriodTime;
        public List<EnnemyRelativeProbability> ennemyRelativeProbabilities;
        public float averageEnnemiesPerSecond;
    }

    public List<EnnemyProbabilitiesOnTimePeriod> ennemyProbabilitiesOnTimePeriods;
    public float defaultAverageEnnemiesIncrementPerSecond;
    

    private float _totalTimeElapsed;

    private static EnnemyGenerator _instance = null;
    public static EnnemyGenerator Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        _totalTimeElapsed = 0;
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

    private void Update()
    {
        _totalTimeElapsed += Time.deltaTime;

        EnnemyProbabilitiesOnTimePeriod probabilityOnTimePeriodToUse = null;

        foreach(var timePeriod in ennemyProbabilitiesOnTimePeriods)
        {
            if (timePeriod.endPeriodTime > _totalTimeElapsed)
            {
                probabilityOnTimePeriodToUse = timePeriod;
                break;
            }
        }

        //We're after the last planned period, we use last period with default increment
        if(probabilityOnTimePeriodToUse == null)
        {
            probabilityOnTimePeriodToUse = new EnnemyProbabilitiesOnTimePeriod()
            {
                ennemyRelativeProbabilities = ennemyProbabilitiesOnTimePeriods.Last().ennemyRelativeProbabilities,
                averageEnnemiesPerSecond = ennemyProbabilitiesOnTimePeriods.Last().averageEnnemiesPerSecond + ((_totalTimeElapsed - ennemyProbabilitiesOnTimePeriods.Last().endPeriodTime) * defaultAverageEnnemiesIncrementPerSecond)
            };
        }

        bool mustGenerateEnnemy = UnityEngine.Random.Range(0f, 1f) < (Time.deltaTime * probabilityOnTimePeriodToUse.averageEnnemiesPerSecond);
        if(mustGenerateEnnemy)
        {
            var typesProbabilities = new List<EnnemyTypes>();
            foreach(var relativeProbability in probabilityOnTimePeriodToUse.ennemyRelativeProbabilities)
            {
                for(int i = 0; i < relativeProbability.relativeProbability; ++i)
                {
                    typesProbabilities.Add(relativeProbability.ennemyType);
                }
            }
            GenerateEnemy(typesProbabilities[UnityEngine.Random.Range(0, typesProbabilities.Count)]);
        }
    }
    
    public void GenerateEnemy(EnnemyTypes ennemyType)
    {
        switch(ennemyType)
        {
            case EnnemyTypes.NORMAL:
                if (UnityEngine.Random.Range(1,6) % 2 == 1)
                    WaveManager.Instance.AliveEnnemies.Add(((GameObject)Instantiate(Resources.Load("Prefabs/Ennemies/NormalEnnemy"))).GetComponent<Ennemi>());
                else
                    WaveManager.Instance.AliveEnnemies.Add(((GameObject)Instantiate(Resources.Load("Prefabs/Ennemies/TrashEnnemy"))).GetComponent<Ennemi>());
                break;

            case EnnemyTypes.FLYING:
                WaveManager.Instance.AliveEnnemies.Add(((GameObject)Instantiate(Resources.Load("Prefabs/Ennemies/FlyingEnnemy"))).GetComponent<Ennemi>());
                break;
        }
    }

    public void NotifyDeadEnnemy(Ennemi ennemy)
    {
        WaveManager.Instance.AliveEnnemies.Remove(ennemy);
    }
}