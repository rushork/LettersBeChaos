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

    public List<Transform> spawnLocations;
    private BoxCollider2D areaToSpawn;
    private float currentZ = 0f;

    private float timerValue = 0.2f;

    private void Awake()
    {
        areaToSpawn = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {


        timerValue -= Time.deltaTime;
        if(timerValue <= 0)
        {
            timerValue = 0.2f;
            SpawnLetter();
        }
    }

    private void SpawnLetter()
    {
        
        InteractableLetter.CreateLetter(GetRandomSpawn(), GetRandomTarget(), currentZ);
        currentZ -= 0.5f;
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
