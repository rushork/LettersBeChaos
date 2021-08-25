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
    [SerializeField] Transform letterPrefab_Bomb;

    public List<Transform> spawnLocations;
    private BoxCollider2D areaToSpawn;
    private float currentZ = 0f;

    private float originalTimeBeforeSpawnIntervalChange = 5;
    private float timeBeforeSpawnIntervalChange;
    private float initialTimerValue = 0.5f;
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
            timeBeforeSpawnIntervalChange = (originalTimeBeforeSpawnIntervalChange += 2.5f);
            //Spawn letters 15% faster every 10 seconds ish.
            initialTimerValue *= 0.85f;
        }
    }

    private void SpawnLetter()
    {
        
        //5% chance of a special letter.
        InteractableLetter.CreateLetter(RollForSpecialLetter(0.4f),GetRandomSpawn(), GetRandomTarget(), currentZ);
        currentZ -= 0.5f;
        if(currentZ >= 500)
        {
            currentZ = 0;
        }
        GameSettings.Instance.addLetter(); // Adds letter to letter count.
    }

    //sometimes a letter can roll as a special one instead of an ordinary one.
    //chance is the chance of this letter changing
    private Transform RollForSpecialLetter(float chance)
    {
        float random = Random.Range(0, 1f);
        if(random <= chance)
        {
            //for now, theres only bombs.
            return letterPrefab_Bomb;
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
