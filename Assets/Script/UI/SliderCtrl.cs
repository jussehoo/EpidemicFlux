using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderCtrl : MonoBehaviour
{
	public Slider slider;
	public TextMeshProUGUI inputField; // TODO: TMP_InputField
	public TextMeshProUGUI title, minText, maxText, timeText;
	public float minValue, maxValue;

	public delegate void OnSliderValueChanged(float? newValue);
	public OnSliderValueChanged onValueChanged;
	private float timeScale = -1f;
	private bool textEditing = false;
	Type type;

	public enum Type
	{
	DEFAULT,
	TIME,
	PERCENTAGE
	}

	public void Setup(string titleText, float min, float max, float current, Type _type, OnSliderValueChanged _onValueChanged)
	{
		Debug.Assert(min <= current && current <= max);
		minValue = min;
		maxValue = max;
		
		type = _type;
		onValueChanged = _onValueChanged;

		title.text = titleText;
		minText.text = numToString(min);
		maxText.text = numToString(max);
		inputField.text = current.ToString();
		timeText.text = "";
		slider.value = (current - min) / (max - min);
	}

	private string numToString(float f)
	{
		if (type == Type.PERCENTAGE)
		{
			f *= 100f;
			if (f>=10f) return "" + ((int)f) + "%";
			return String.Format("{0:0.00}", f) + "%";
		}
		else return String.Format("{0:0.00}", f);
	}

	private void Update()
	{
		if (type == Type.TIME && timeScale != EF.cfg.timeScale)
		{
			timeScale = EF.cfg.timeScale;

			RefreshTime();
		}
	}

	private void RefreshTime()
	{
		float simTime = (minValue + ((maxValue-minValue) * slider.value));
		timeText.text = EF.simTimeToString(simTime);
	}

	public void ValueChanged()
	{
		float newValue = minValue + (slider.value * (maxValue - minValue));
		if (!textEditing) inputField.text = numToString(newValue);
		onValueChanged(newValue);
		if (type == Type.TIME) RefreshTime();
	}

	public void ResetValue()
	{
		slider.value = .5f;
		inputField.text = "-";
		onValueChanged(null);
	}

	public void InputTextChanged()
	{
		try
		{
			// parse
			float f = float.Parse(inputField.text);
			// clamp
			float current = Mathf.Clamp(f, minValue, maxValue);
			// slider
			textEditing = true;
			slider.value = (current - minValue) / (maxValue - minValue);
			textEditing = false;
		}
		catch (Exception) { return; }
	}

	public void IncreaseValue()
	{
		slider.value = (float)Math.Round(slider.value + .01f, 2);
	}

	public void DecreaseValue()
	{
		slider.value = (float)Math.Round(slider.value - .01f, 2);
	}
}
