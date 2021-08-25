using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will manage where letters spawn and how they move.
/// For now, they can be spawned by clicking.
/// </summary>
public class LetterSpawner : MonoBehaviour
{
    [SerializeField] Transform letterPrefab_Basic;

    [Header("Bombs")]
    [SerializeField] Transform letterPrefab_Bomb;
    [SerializeField] Transform letterPrefab_BombR;
    [SerializeField] Transform letterPrefab_BombG;
    [SerializeField] Transform letterPrefab_BombB;
    [SerializeField] private float chanceForDangerBomb; //danger bombs send letters absolutely anywhere with a 5X MULTIPLIER! 

    public List<Transform> spawnLocations;
    private BoxCollider2D areaToSpawn;
    private float currentZ = 0f;

    private float originalTimeBeforeSpawnIntervalChange = 2.5f;
    private float timeBeforeSpawnIntervalChange;
    private float initialTimerValue = 5f;
    private float currentTimerValue;

    private void Awake()
    {
        areaToSpawn = GetComponent<BoxCollider2D>();
        currentTimerValue = initialTimerValue;
        timeBeforeSpawnIntervalChange = originalTimeBeforeSpawnIntervalChange;
    }

    private void Update()
    {


        //Count down and spawn a letter.
        currentTimerValue -= Time.deltaTime;
        if(currentTimerValue <= 0)
        {
            currentTimerValue = initialTimerValue;
            SpawnLetter();
        }

        timeBeforeSpawnIntervalChange -= Time.deltaTime;
        if(timeBeforeSpawnIntervalChange <= 0)
        {
            //increase the interval by 5 seconds each time
            timeBeforeSpawnIntervalChange = (originalTimeBeforeSpawnIntervalChange += 3.5f);
            //Spawn letters 15% faster every 10 seconds ish.
            if(initialTimerValue > 0.25f)
            {
                initialTimerValue *= 0.85f;
                initialTimerValue = Mathf.Round(initialTimerValue * 100) / 100f;
            }
            
        }
    }

    private void SpawnLetter()
    {
        
        //5% chance of a special letter.
        InteractableLetter letter = InteractableLetter.CreateLetter(RollForSpecialLetter(0.05f),GetRandomSpawn(), GetRandomTarget(), currentZ);


        currentZ -= 0.5f;
        if(currentZ >= 500)
        {
            currentZ = 0;
        }

        //dont add letters if they're special, they dont count.
        if (!letter.letterScriptable.isSpecial)
        {
            GameSettings.Instance.AddLetter(); // Adds letter to letter count.
        }
        
    }

    //sometimes a letter can roll as a special one instead of an ordinary one.
    //chance is the chance of this letter changing
    private Transform RollForSpecialLetter(float chance)
    {
        float random = Random.Range(0f, 1f);
        random = Mathf.Round(random * 1000) / 1000;
        if(random <= chance)
        {
            //for now, theres only bombs.
            random = Random.Range(0f, 1f);
            if(random <= chanceForDangerBomb)
            {
                int random2 = Random.Range(1, 4);
                if(random2 == 1)
                {
                    return letterPrefab_BombR;
                }
                else if (random2 == 2)
                {
                    return letterPrefab_BombG;
                }
                else
                {
                    return letterPrefab_BombB;
                }
            }
            else
            {
                return letterPrefab_Bomb;
            }
            
            
        }
        else
        {
            return letterPrefab_Basic;
        }
    }

    private Transform GetRandomSpawn()
    {
       
        int rand = Random.Range(0, spawnLocations.Count);

        return spawnLocations[rand];
    }

    private Vector2 GetRandomTarget()
    {

        Vector2 extents = areaToSpawn.size / 2f;
        Vector2 point = new Vector2(Random.Range(-extents.x, extents.x), Random.Range(-extents.y, extents.y) + areaToSpawn.bounds.center.y);

        return point;
    }
}
