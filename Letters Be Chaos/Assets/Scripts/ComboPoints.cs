using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// A clone of PointsText, this is a larger version of that script which supports combo points.
/// </summary>
public class ComboPoints : MonoBehaviour
{

    public static ComboPoints Create(Vector3 pos, float pointAmount, string message)
    {
        Transform textTransform = Instantiate(GameSettings.Instance.pfComboPoints, pos, Quaternion.identity);
        ComboPoints pointsText = textTransform.GetComponent<ComboPoints>();
        pointsText.Setup(pointAmount, message);

        return pointsText;
    }


    [SerializeField] private TextMeshPro text;
    [SerializeField] private TextMeshPro headerText;

    private float disappearTimer;
    private Color textColor;

    private Color originalTextColor;
    private float originalFontSize;
    private const float DISAPPEAR_TIMER_MAX = 0.9f;
    


    public void Setup(float pointsValue, string message)
    {
        originalFontSize = text.fontSize;
        originalTextColor = text.color;
        //assign an error icon.

        text.fontSize = originalFontSize;
        textColor = originalTextColor;

        headerText.SetText(message);

        text.SetText(pointsValue.ToString() + "x");

        text.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;
    }


    private void Update()
    {

        float moveYSpeed = 0.2f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX * 0.5f)
        {
            //first half of popup
            float increaseScaleAmount = 2f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            //second half of popup
            float decreaseScaleAmount = 2f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }




        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            //start fade
            float disappearSpeed = 1.5f;

            textColor.a -= disappearSpeed * Time.deltaTime;
           
            text.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
