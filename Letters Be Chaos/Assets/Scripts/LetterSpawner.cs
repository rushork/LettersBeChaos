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
    [SerializeField] Transform letterPrefab_Trash;
    [SerializeField] Transform letterPrefab_Summon;
    [SerializeField] Transform letterPrefab_Column;
    [Header("Two Categories. Add to 1.0")]
    [SerializeField] private float chanceForDangerBomb; //danger bombs send letters absolutely anywhere with a 5X MULTIPLIER! 
    [SerializeField] private float chanceForSuperBomb; //any of the three below bombs have an equal chance of spawning.
    [Space]
    [SerializeField] private float chanceForSummonBomb;
    [SerializeField] private float chanceForColumnBomb;
    [SerializeField] private float chanceForTrashBomb;

    public List<Transform> spawnLocations;
    private BoxCollider2D areaToSpawn;
    private float currentZ = 0f;

    private float originalTimeBeforeSpawnIntervalChange = 2.5f;
    private float timeBeforeSpawnIntervalChange;
    private float initialTimerValue = 5f;
    private float currentTimerValue;

    private int summonBombAmount; //the amount that will spawn this round
    private bool summoning;
    private bool allowSummonBombSpawn = true;
    private float summonDelay = 0.15f;

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


        if (summoning)
        {
            summonDelay -= Time.deltaTime;
            if(summonDelay <= 0)
            {
                if(summonBombAmount > 0)
                {
                    
                    SpawnLetter();
                    summonBombAmount--;
                }
                else
                {
                    summonBombAmount = 0;
                    summoning = false;
                    allowSummonBombSpawn = true;
                }

                summonDelay = 0.15f;

            }
        }
    }

    private void SpawnLetter()
    {

        //5% chance of a special letter.
        if (GameSettings.Instance.specialLettersAllowed)
        {
            InteractableLetter letter = InteractableLetter.CreateLetter(RollForSpecialLetter(0.05f), GetRandomSpawn(), GetRandomTarget(), currentZ);

            //dont add letters if they're special, they dont count.
            if (!letter.letterScriptable.isSpecial)
            {
                GameSettings.Instance.AddLetter(); // Adds letter to letter count.
            }
        }
        else
        {
            InteractableLetter letter = InteractableLetter.CreateLetter(letterPrefab_Basic, GetRandomSpawn(), GetRandomTarget(), currentZ);

            //dont add letters if they're special, they dont count.
            if (!letter.letterScriptable.isSpecial)
            {
                GameSettings.Instance.AddLetter(); // Adds letter to letter count.
            }
        }
        


        currentZ -= 0.5f;
        if(currentZ >= 500)
        {
            currentZ = 0;
        }

        
        
    }

    //sometimes a letter can roll as a special one instead of an ordinary one.
    //chance is the chance of this letter changing
    private Transform RollForSpecialLetter(float chance)
    {
        float random = Random.Range(0f, 1f);
        random = Mathf.Round(random * 1000) / 1000;

        if (GameSettings.Instance.autoBombAllowed && GameSettings.Instance.colourBombAllowed
            && GameSettings.Instance.trashAllowed && GameSettings.Instance.columnBombAllowed && GameSettings.Instance.summonBombAllowed)
        {
            if (random <= chance)
            {
                //for now, theres only bombs.
                random = Random.Range(0f, 1f);
                if (random <= chanceForDangerBomb)
                {
                    int random2 = Random.Range(1, 6);
                    if (random2 == 1)
                    {
                        return letterPrefab_BombR;
                    }
                    else if (random2 == 2)
                    {
                        return letterPrefab_BombG;
                    }
                    else if (random2 == 3)
                    {
                        return letterPrefab_BombB;
                    }
                    else
                    {
                        return letterPrefab_Bomb;
                    }
                }
                else if (random > chanceForDangerBomb)
                {

                    random = Random.Range(0f, 1f);

                    if (random <= chanceForColumnBomb)
                    {
                        //to avoid multiple summon bombs.
                        return letterPrefab_Column;

                    }
                    else if (random > chanceForColumnBomb && random <= chanceForSummonBomb)
                    {
                        //to avoid multiple summon bombs.
                        if (allowSummonBombSpawn)
                        {
                            return letterPrefab_Summon;
                        }
                        else
                        {
                            return letterPrefab_Basic;
                        }

                    }
                    else if (random > chanceForSummonBomb)
                    {
                        //to avoid multiple summon bombs.
                        return letterPrefab_Trash;

                    }
                    else
                    {
                        return letterPrefab_Basic;
                    }

                }

                else
                {
                    return letterPrefab_Basic;
                }


            }
            else
            {
                return letterPrefab_Basic;
            }
        }
        else
        {

            if (GameSettings.Instance.autoBombAllowed && GameSettings.Instance.colourBombAllowed && GameSettings.Instance.columnBombAllowed && GameSettings.Instance.summonBombAllowed)
            {
                if (random <= chance)
                {

                    random = Random.Range(0f, 1f);
                    if (random <= chanceForDangerBomb)
                    {
                        int random2 = Random.Range(1, 6);
                        if (random2 == 1)
                        {
                            return letterPrefab_BombR;
                        }
                        else if (random2 == 2)
                        {
                            return letterPrefab_BombG;
                        }
                        else if (random2 == 3)
                        {
                            return letterPrefab_BombB;
                        }
                        else
                        {
                            return letterPrefab_Bomb;
                        }
                    }
                    else if (random > chanceForDangerBomb)
                    {

                        random = Random.Range(0f, 1f);

                        if (random <= chanceForColumnBomb)
                        {
                            //to avoid multiple summon bombs.
                            return letterPrefab_Column;

                        }
                        else if (random > chanceForColumnBomb)
                        {
                            //to avoid multiple summon bombs.
                            if (allowSummonBombSpawn)
                            {
                                return letterPrefab_Summon;
                            }
                            else
                            {
                                return letterPrefab_Basic;
                            }

                        }
                        else
                        {
                            return letterPrefab_Basic;
                        }

                    }
                    else
                    {
                        return letterPrefab_Basic;
                    }


                }
                else
                {
                    return letterPrefab_Basic;
                }
            }
            else if (GameSettings.Instance.autoBombAllowed && GameSettings.Instance.colourBombAllowed && GameSettings.Instance.columnBombAllowed)
            {
                if (random <= chance)
                {

                    random = Random.Range(0f, 1f);
                    if (random <= chanceForDangerBomb)
                    {
                        int random2 = Random.Range(1, 6);
                        if (random2 == 1)
                        {
                            return letterPrefab_BombR;
                        }
                        else if (random2 == 2)
                        {
                            return letterPrefab_BombG;
                        }
                        else if (random2 == 3)
                        {
                            return letterPrefab_BombB;
                        }
                        else
                        {
                            return letterPrefab_Bomb;
                        }
                    }
                    else if (random > chanceForDangerBomb)
                    {

                        return letterPrefab_Column;

                    }
                    else
                    {
                        return letterPrefab_Bomb;
                    }
                }
            }
            else if (GameSettings.Instance.autoBombAllowed && GameSettings.Instance.colourBombAllowed)
            {
                if (random <= chance)
                {

                    random = Random.Range(0f, 1f);
                    if (random <= chanceForDangerBomb)
                    {
                        int random2 = Random.Range(1, 4);
                        if (random2 == 1)
                        {
                            return letterPrefab_BombR;
                        }
                        else if (random2 == 2)
                        {
                            return letterPrefab_BombG;
                        }
                        else if (random2 == 3)
                        {
                            return letterPrefab_BombB;
                        }
                        else
                        {
                            return letterPrefab_Bomb;
                        }
                    }
                    else
                    {
                        return letterPrefab_Bomb;
                    }
                }
            }
            else if (GameSettings.Instance.autoBombAllowed)
            {
                return letterPrefab_Bomb;
            }
            else
            {
                return letterPrefab_Basic;
            }

            return letterPrefab_Basic;
            
        }
        
    }

    public void SummonBomb()
    {
        if (!summoning)
        {
            //releases 30 - 60 letters within 5 seconds.
            summonBombAmount = Random.Range(25, 50);
            summoning = true;
            allowSummonBombSpawn = false;
        }
        else
        {
            summonBombAmount += 10;
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

    private IEnumerator ResetSummonBomb()
    {
        yield return new WaitForSeconds(10f);
    }
}
