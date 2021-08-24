using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointsText : MonoBehaviour
{

    public static PointsText Create(Vector3 pos, int pointAmount, bool isNegative, Sprite errorIcon)
    {
        Transform textTransform = Instantiate(GameSettings.Instance.pfPointsText, pos, Quaternion.identity);
        PointsText pointsText = textTransform.GetComponent<PointsText>();
        pointsText.Setup(pointAmount, isNegative, errorIcon);

        return pointsText;
    }


    [SerializeField]private TextMeshPro text;
    [SerializeField]private SpriteRenderer iconRenderer;
    private float disappearTimer;
    private Color textColor;
    private Color iconColor;
    private Color originalTextColor;
    private float originalFontSize;
    private const float DISAPPEAR_TIMER_MAX = 0.6f;


    public void Setup(int pointsValue, bool isNegative, Sprite icon)
    {
        originalFontSize = text.fontSize;
        originalTextColor = text.color;
        //assign an error icon.
        iconRenderer.sprite = icon;
        iconColor = iconRenderer.color;

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
        float moveYSpeed = 0.4f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
        
        if(iconRenderer.sprite == null)
        {
            if (disappearTimer > DISAPPEAR_TIMER_MAX * 0.5f)
            {
                //first half of popup
                float increaseScaleAmount = 2f;
                transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
            }
            else
            {
                //second half of popup
                float decreaseScaleAmount = 1f;
                transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
            }
        }
       


        disappearTimer -= Time.deltaTime;
        if(disappearTimer < 0)
        {
            //start fade
            float disappearSpeed = 1.5f;
            
            textColor.a -= disappearSpeed * Time.deltaTime;
            iconColor.a -= disappearSpeed * Time.deltaTime;
            iconRenderer.color = iconColor;
            text.color = textColor;
            if(textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
