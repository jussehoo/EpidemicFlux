using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using static SliderCtrl;

public class MenuCtrl : MonoBehaviour
{
	public PlayerButton buttonTmp;
	public TextMeshProUGUI titleTmp;
	public SliderCtrl sliderTmp;

	public Transform buttonPanel;

	void Awake()
	{
		buttonTmp.gameObject.SetActive(false);
		titleTmp.gameObject.SetActive(false);
		sliderTmp.gameObject.SetActive(false);
		Hide();
	}

    // Start is called before the first frame update
    void Start()
    {
        createTitle("Scene configuration");
				
		createSlider(
			"Density", 0f, 1f, EF.cfg.density, SliderCtrl.Type.PERCENTAGE,
			(f) => { if (f != null) EF.cfg.density = f.Value; });

		createSlider(
			"Infectivity", 0f, 1f, EF.cfg.infectionOnContact, SliderCtrl.Type.PERCENTAGE,
			(f) => { if (f != null) EF.cfg.infectionOnContact = f.Value; });

		createSlider(
			"Infection time", 0.01f, 1f, EF.cfg.infectionTime, SliderCtrl.Type.TIME,
			(f) => { if (f != null) EF.cfg.infectionTime = f.Value; });

		createSlider(
			"Duration of illness", 0.01f, 1f, EF.cfg.sickTime, SliderCtrl.Type.TIME,
			(f) => { if (f != null) EF.cfg.sickTime = f.Value; });

		createSlider(
			"Immunity rate", 0f, 1f, EF.cfg.immunityRate, SliderCtrl.Type.PERCENTAGE,
			(f) => { if (f != null) EF.cfg.immunityRate = f.Value; });

		createSlider(
			"Death rate", 0f, 1f, EF.cfg.deathRate, SliderCtrl.Type.PERCENTAGE,
			(f) => { if (f != null) EF.cfg.deathRate = f.Value; });

			
        createTitle("");
		
		createSlider(
			"Time scale", 0f, 90 * 24f, EF.cfg.timeScale, SliderCtrl.Type.DEFAULT,
			(f) => { if (f != null) EF.cfg.timeScale = f.Value; });

			
		createButton(EF.img.iconClose, "", () => { EF.efCtrl.CloseConfigMenu(); });
    }

	private void createSlider(string titleText, float min, float max, float current, SliderCtrl.Type type, OnSliderValueChanged _onValueChanged)
	{
		var slider = Instantiate(sliderTmp);
		slider.Setup(titleText, min, max, current, type, _onValueChanged);
		addMenuObject(slider.gameObject);
	}

	internal void Hide()
	{
		buttonPanel.gameObject.SetActive(false);
	}
	
	internal void Show()
	{
		buttonPanel.gameObject.SetActive(true);
	}

	private void createButton(Sprite iconPlay, string s, System.Action act)
	{
		var button = Instantiate(buttonTmp);
		button.icon.sprite = iconPlay;
		button.text.text = s;
		button.act = act;
		addMenuObject(button.gameObject);
	}

	private void createTitle(string s)
	{
		var title = Instantiate(titleTmp);
		title.text = s;
		addMenuObject(title.gameObject);
	}

	private void addMenuObject(GameObject go)
	{
		go.gameObject.SetActive(true);
		go.transform.SetParent(buttonPanel);
	}

	// Update is called once per frame
	void Update()
    {
        
    }

}
