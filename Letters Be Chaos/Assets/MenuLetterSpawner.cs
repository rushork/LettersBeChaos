using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLetterSpawner : MonoBehaviour
{
    public List<Sprite> letterSprites;
    public List<Transform> locations;
    public Transform prefabMenuLetterComponent;


    private float spawnDelay;
    public float spawnDelayMax;

    private void Update()
    {
        spawnDelay -= Time.deltaTime;
        if(spawnDelay <= 0)
        {
            spawnDelay = spawnDelayMax;
            GenRandomLetter();
        }
    }


    private void GenRandomLetter()
    {
        float chanceCalculation = Random.Range(0, 1f);
        if(chanceCalculation <= 0.2f)
        {
            //special
        }
        else
        {
            int chanceCalculation2 = Random.Range(0, letterSprites.Count);
            Sprite toSpawn = null;

            for (int i = 0; i < locations.Count; i++)
            {
                if(i == chanceCalculation2)
                {
                    toSpawn = letterSprites[i];
                }
            }

            chanceCalculation2 = Random.Range(0, locations.Count);
            Transform toSpawnLocation = null;

            for(int i = 0; i < locations.Count; i++)
            {
                if(i == chanceCalculation2)
                {
                    toSpawnLocation = locations[i];
                }

            }


            if(toSpawnLocation != null && toSpawn != null)
            {
                MenuBackgroundLetter letter = Instantiate(prefabMenuLetterComponent, toSpawnLocation.position, Quaternion.AngleAxis(Random.Range(-20, 20), Vector3.forward),transform).GetComponent<MenuBackgroundLetter>();
                letter.SetSprite(toSpawn);
            }
        }
    }
}
