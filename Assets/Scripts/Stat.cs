using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private BarScript bar;

    public BarScript Bar
    {
        get
        {
            return bar;
        }
    }

    [SerializeField] private float maxVal;

    [SerializeField] private float currentVal;

    public float CurrentValue
    {
        get
        {
            return currentVal;
        }

        set
        {
            this.currentVal = Mathf.Clamp(value, 0, MaxValue);
            Bar.Value = currentVal;
        }
    }

    public float MaxValue
    {
        get
        {
            return maxVal;
        }

        set
        {
           this.maxVal = value;
            Bar.MaxValue = maxVal;
        }
    }

	public void Initialize()
    {
        this.MaxValue = maxVal;
        this.CurrentValue = currentVal;
    }
}
