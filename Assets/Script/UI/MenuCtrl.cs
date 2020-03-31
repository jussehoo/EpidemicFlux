using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using static SliderCtrl;

public class MenuCtrl : MonoBehaviour
{
	public PlayerButton iconTmp, buttonTmp;
	public TextMeshProUGUI titleTmp, textTmp;
	public SliderCtrl sliderTmp;

	public GameObject panelTmp;
	private Transform buttonPanel;

	void Awake()
	{
		iconTmp.gameObject.SetActive(false);
		buttonTmp.gameObject.SetActive(false);
		titleTmp.gameObject.SetActive(false);
		textTmp.gameObject.SetActive(false);
		sliderTmp.gameObject.SetActive(false);

		panelTmp.SetActive(false);
		//Hide();
	}

    // Start is called before the first frame update
    void Start()
    {

    }

	public void CreateConfigMenu()
	{    
		CreateTitle("Scene configuration");
				
		createSlider(
			"Density", .6f, 1f, EF.cfg.density, SliderCtrl.Type.PERCENTAGE,
			(f) => { if (f != null) EF.cfg.density = f.Value; });

		createSlider(
			"Neighbor Infectivity", 0f, 1f, EF.cfg.infectionOnContact, SliderCtrl.Type.PERCENTAGE,
			(f) => { if (f != null) EF.cfg.infectionOnContact = f.Value; });

		createSlider(
			"Random Infectivity", 0f, .1f, EF.cfg.randomInfection, SliderCtrl.Type.PERCENTAGE,
			(f) => { if (f != null) EF.cfg.randomInfection = f.Value; });

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
			"Death rate", 0f, .5f, EF.cfg.deathRate, SliderCtrl.Type.PERCENTAGE,
			(f) => { if (f != null) EF.cfg.deathRate = f.Value; });

			
        CreateTitle("");
		
		createSlider(
			"Time scale (hours)", 0f, 90 * 24f, EF.cfg.timeScale, SliderCtrl.Type.DEFAULT,
			(f) => { if (f != null) EF.cfg.timeScale = f.Value; });

			
		CreateIcon(EF.img.iconClose, "", () => { EF.efCtrl.CloseConfigMenu(); });
	}

	private void createSlider(string titleText, float min, float max, float current, SliderCtrl.Type type, OnSliderValueChanged _onValueChanged)
	{
		var slider = Instantiate(sliderTmp);
		slider.Setup(titleText, min, max, current, type, _onValueChanged);
		addMenuObject(slider.gameObject);
	}

	internal void Hide()
	{
		if (buttonPanel != null)
		{
			Destroy(buttonPanel.gameObject);
			buttonPanel = null;
		}
	}
	
	internal void ShowConfig()
	{
		CreatePanel();
		CreateConfigMenu();
	}

	internal void ShowPresets()
	{
		CreatePanel();
		
		CreateTitle("Scene Presets");
		
		CreateButton(EF.img.iconBasic, "Basic", () => {
			EF.cfg = new SceneConfig();
			EF.efCtrl.Generate();
			Hide();
		});
		CreateText("Basic settings.");

		CreateButton(EF.img.iconAviators, "Aviators", () => {
			EF.cfg = new SceneConfig();
			EF.cfg.density = .65f;
			EF.cfg.randomInfection = .08f;
			EF.cfg.infectionOnContact = .65f;
			EF.efCtrl.Generate();
			Hide();
		});
		CreateText("Aviating causes more random infections in distant places. Neighbor infections are not so common here.");

		CreateButton(EF.img.iconCity, "Overcrowded", () => {
			EF.cfg = new SceneConfig();
			EF.cfg.density = .95f;
			EF.cfg.randomInfection = .001f;
			EF.cfg.infectionOnContact = .65f;
			EF.cfg.immunityRate = .20f;
			EF.efCtrl.Generate();
			Hide();
		});
		CreateText("Less distant infections. Dense population.");

		CreateIcon(EF.img.iconClose, "", () => { EF.efCtrl.CloseConfigMenu(); });
	}

	private void CreatePanel()
	{
		Hide();
		
		var bp = Instantiate(panelTmp);
		bp.SetActive(true);

		buttonPanel = bp.transform;
		buttonPanel.SetParent(transform);
		buttonPanel.localScale = Vector3.one;
		buttonPanel.localPosition = Vector3.zero;
	}
	
	private void CreateIcon(Sprite icon, string s, System.Action act)
	{
		var button = Instantiate(iconTmp);
		button.icon.sprite = icon;
		button.text.text = s;
		button.act = act;
		addMenuObject(button.gameObject);
	}

	private void CreateButton(Sprite icon, string s, System.Action act)
	{
		var button = Instantiate(buttonTmp);
		button.icon.sprite = icon;
		button.text.text = s;
		button.act = act;
		addMenuObject(button.gameObject);
	}
	
	private void CreateTitle(string s)
	{
		var title = Instantiate(titleTmp);
		title.text = s;
		addMenuObject(title.gameObject);
	}
	private void CreateText(string s)
	{
		var txt = Instantiate(textTmp);
		txt.text = s;
		addMenuObject(txt.gameObject);
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
