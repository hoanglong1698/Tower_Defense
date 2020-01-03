using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    private float fillAmount;

    [SerializeField]
    private float lerpSpeed;

    [SerializeField]
    private Image content;

    [SerializeField]
    private Text valueText;

    [SerializeField]
    private Color fullColor;

    [SerializeField]
    private Color lowColor;

    [SerializeField]
    private bool lerpColors;

    public float MaxValue { get; set; }

    public float Value
    {
        set
        {
            //string[] tmp = valueText.text.Split(':');
            //valueText.text = tmp[0] + ": " + value;
            fillAmount = Map(value, 0, MaxValue, 0, 1);
        }
    }

    void Start ()
    {
        if (lerpColors)
        {
            content.color = fullColor;
        }
	}
	
	void Update ()
    {
        HandleBar();
	}

    private void HandleBar()
    {
        if (fillAmount != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
        }
        if (lerpColors)
        {
            content.color = Color.Lerp(lowColor, fullColor, fillAmount);
        }
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        /*For example the max health is 100. current health is 80. min health is 0
        (80-0) * (1 - 0 ) / (100-0) + 0
        80 * 1 / 100 + 0 
        80 / 100
        0.8
        For example the max health is 230. current 78 min 0
        (78 - 0) * (1 - 0 )/ ( 230 - 0) + 0
        78 * 1 / 230 + 0
        78 / 230 
        0.339*/
    }

	public void Reset()
	{
        Value = MaxValue;
        content.fillAmount = 1;
    }
}
