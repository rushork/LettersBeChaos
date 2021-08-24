using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotStatsScript : MonoBehaviour {


    // Usage stats
    private int CPU;
    private int RAM;
    private int HYDRAULIC;

    private bool isRunning = false;

    void Update() {
        if (!isRunning) {
            StartCoroutine(decreaseHighest());
        }
    }


    IEnumerator decreaseHighest() {
        isRunning = true;
        yield return new WaitForSeconds(2f);

        CPU = GameSettings.Instance.CPU.getUsage();
        RAM = GameSettings.Instance.RAM.getUsage();
        HYDRAULIC = GameSettings.Instance.HYDRAULIC.getUsage();

        int[] usages = new int[] {CPU, RAM, HYDRAULIC};

        int highestIndex = 0;

        if (RAM > CPU && RAM > HYDRAULIC) {
            highestIndex = 2;
        } else if ( HYDRAULIC > CPU && HYDRAULIC > RAM) {
            highestIndex = 3;
        } else if (CPU > RAM && CPU > HYDRAULIC) {
            highestIndex = 1;
        } else {
            int r = Random.Range(0, 3);
            switch (r) {
                case 0:
                    highestIndex = 1;
                    break;
                case 1:
                    highestIndex = 2;
                    break;
                case 2:
                    highestIndex = 3;
                    break;
                default:
                    break;
            }
        }

        if (highestIndex == 1 && CPU > 0) {
            GameSettings.Instance.CPU.setUsage(CPU-1);
        } else if (highestIndex == 2 && RAM > 0) {
            GameSettings.Instance.RAM.setUsage(RAM-1);
        } else if (highestIndex == 3 && HYDRAULIC > 0) {
            GameSettings.Instance.HYDRAULIC.setUsage(HYDRAULIC-1);
        }

        isRunning = false;
    }

}
