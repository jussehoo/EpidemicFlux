using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using static SliderCtrl;
using UnityEngine.UI;

public class MenuCtrl : MonoBehaviour
{
	public PlayerButton iconTmp, buttonTmp;
	public TextMeshProUGUI titleTmp, textTmp;
	public SliderCtrl sliderTmp;

	public GameObject panelTmp, infoTmp, backgroundBlocker;
	private Transform buttonPanel;

	void Awake()
	{
		iconTmp.gameObject.SetActive(false);
		buttonTmp.gameObject.SetActive(false);
		titleTmp.gameObject.SetActive(false);
		textTmp.gameObject.SetActive(false);
		sliderTmp.gameObject.SetActive(false);
		infoTmp.gameObject.SetActive(false);
		backgroundBlocker.SetActive(false);
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

			
		CreateIcon(EF.img.iconClose, "", () => { CloseMenu(); });
	}

	private void createSlider(string titleText, float min, float max, float current, SliderCtrl.Type type, OnSliderValueChanged _onValueChanged)
	{
		var slider = Instantiate(sliderTmp);
		slider.Setup(titleText, min, max, current, type, _onValueChanged);
		AddMenuObject(slider.gameObject);
	}

	public void BackgroundClicked()
	{
		CloseMenu();
	}

	internal void CloseMenu()
	{
		if (buttonPanel != null)
		{
			Destroy(buttonPanel.gameObject);
			buttonPanel = null;
			
			backgroundBlocker.SetActive(false);
		}
	}

	internal void ShowInfo()
	{
		CreatePanel();

		buttonPanel.GetComponent<Image>().color = new Color(.1f,.5f,.1f,.9f);

		CreateTitle("GVIDS info");

		var info = Instantiate(infoTmp);
		
		CreateButton(EF.img.iconScenarios, "Load a scenario preset", () => ShowPresets());
		CreateButton(EF.img.iconMenu, "Adjust simulation configurations", () => ShowConfig());
		CreateButton(EF.img.iconInfo, "Show this screen", () => {});
		CreateButton(EF.img.iconPlay, "Run simulation", () => {CloseMenu(); EF.efCtrl.PlayPauseSim(); });
		
		AddMenuObject(info);
		
		CreateText(
			"Infection flow: (a) Units get infected by a neighbor or other sick unit according to certain odds [Neighbor/random infectivity]. (b) For immune units, it stops here [Immunity rate]," +
			"(c) others get sick. (d, e) Death rate determines how many units recovers.");

		CreateIcon(EF.img.iconClose, "", () => { CloseMenu(); });
		
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
			CloseMenu();
		});
		CreateText("Basic settings.");

		CreateButton(EF.img.iconAviators, "Aviators", () => {
			EF.cfg = new SceneConfig();
			EF.cfg.density = .65f;
			EF.cfg.randomInfection = .08f;
			EF.cfg.infectionOnContact = .65f;
			EF.efCtrl.Generate();
			CloseMenu();
		});
		CreateText("Aviating causes more random infections in distant places. Neighbor infections are not so common here.");

		CreateButton(EF.img.iconCity, "Overcrowded", () => {
			EF.cfg = new SceneConfig();
			EF.cfg.density = .95f;
			EF.cfg.randomInfection = .001f;
			EF.cfg.infectionOnContact = .65f;
			EF.cfg.immunityRate = .20f;
			EF.efCtrl.Generate();
			CloseMenu();
		});
		CreateText("Less distant infections. Dense population.");

		CreateIcon(EF.img.iconClose, "", () => { CloseMenu(); });
	}

	private void CreatePanel()
	{
		CloseMenu();
		
		EF.efCtrl.PauseSim();

		backgroundBlocker.SetActive(true);

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
		AddMenuObject(button.gameObject);
	}

	private void CreateButton(Sprite icon, string s, System.Action act)
	{
		var button = Instantiate(buttonTmp);
		button.icon.sprite = icon;
		button.text.text = s;
		button.act = act;
		AddMenuObject(button.gameObject);
	}
	
	private void CreateTitle(string s)
	{
		var title = Instantiate(titleTmp);
		title.text = s;
		AddMenuObject(title.gameObject);
	}
	private void CreateText(string s)
	{
		var txt = Instantiate(textTmp);
		txt.text = s;
		AddMenuObject(txt.gameObject);
	}

	private void AddMenuObject(GameObject go)
	{
		go.gameObject.SetActive(true);
		go.transform.SetParent(buttonPanel);
	}
}
