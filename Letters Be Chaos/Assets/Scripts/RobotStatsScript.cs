using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotStatsScript : MonoBehaviour {


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
        yield return new WaitForSeconds(1f);

        CPU = GameSettings.Instance.CPU.getUsage();
        RAM = GameSettings.Instance.RAM.getUsage();
        HYDRAULIC = GameSettings.Instance.HYDRAULIC.getUsage();

        int[] usages = new int[] {CPU, RAM, HYDRAULIC};

        int highestIndex;

        if (RAM > CPU && RAM > HYDRAULIC) {
            highestIndex = 2;
        } else if ( HYDRAULIC > CPU && HYDRAULIC > RAM) {
            highestIndex = 3;
        } else {
            highestIndex = 1;
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
