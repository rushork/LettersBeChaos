using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointsText : MonoBehaviour
{

    public static PointsText Create(Vector3 pos, int pointAmount)
    {
        Transform textTransform = Instantiate(GameSettings.Instance.pfPointsText, pos, Quaternion.identity);
        PointsText pointsText = textTransform.GetComponent<PointsText>();
        pointsText.Setup(pointAmount);

        return pointsText;
    }


    [SerializeField]private TextMeshPro text;

    public void Setup(int pointsValue)
    {
        text.SetText(pointsValue.ToString());
    }
}
