using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointsText : MonoBehaviour
{

    public static PointsText Create(Vector3 pos, int pointAmount, bool isNegative)
    {
        Transform textTransform = Instantiate(GameSettings.Instance.pfPointsText, pos, Quaternion.identity);
        PointsText pointsText = textTransform.GetComponent<PointsText>();
        pointsText.Setup(pointAmount, isNegative);

        return pointsText;
    }


    [SerializeField]private TextMeshPro text;
    private float disappearTimer;
    private Color textColor;
    private Color originalTextColor;
    private float originalFontSize;
    private const float DISAPPEAR_TIMER_MAX = 0.6f;


    public void Setup(int pointsValue, bool isNegative)
    {
        originalFontSize = text.fontSize;
        originalTextColor = text.color;



        if (!isNegative)
        {
            text.fontSize = originalFontSize;
            textColor = originalTextColor;
        }
        else
        {
            text.fontSize = originalFontSize + 0.5f;
            textColor = Color.red;
        }


        if (isNegative)
        {
            text.SetText(pointsValue.ToString());
        }
        else
        {
            text.SetText("+" + pointsValue.ToString());
        }

       

        text.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;
    }


    private void Update()
    {
        float moveYSpeed = 0.7f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
        

        if(disappearTimer > DISAPPEAR_TIMER_MAX * 0.5f)
        {
            //first half of popup
            float increaseScaleAmount = 2f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            //second half of popup
            float decreaseScaleAmount = 0.5f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }


        disappearTimer -= Time.deltaTime;
        if(disappearTimer < 0)
        {
            //start fade
            float disappearSpeed = 2f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            text.color = textColor;
            if(textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
