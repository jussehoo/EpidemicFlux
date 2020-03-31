using System;
using TMPro;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
	public PlayerButton buttonTmp;
	public Transform buttonPanel;

	public Transform statsPanel;
	public StatItem statTmp;

	private EFCtrl efCtrl;
	private CameraCtrl camCtrl;
	private MenuCtrl menuCtrl;
	private PlayerButton playPauseButton;
	private StatItem
		statTime,
		statNeutral	,
		statInfected,
		statSick	,
		statImmune	,
		statRecovered,
		statDead,
		statState;

	void Awake()
	{
		buttonTmp.gameObject.SetActive(false);
		statTmp.gameObject.SetActive(false);

		efCtrl = FindObjectOfType<EFCtrl>();
		camCtrl = FindObjectOfType<CameraCtrl>();
		menuCtrl = FindObjectOfType<MenuCtrl>();
	}

    // Start is called before the first frame update
    void Start()
    {
        playPauseButton = CreateButton(EF.img.iconPlay, "Play", () => efCtrl.PlayPauseSim() );
        CreateButton(EF.img.iconReset, "Reset", () => efCtrl.RestartSim() );
		CreateButton(EF.img.iconSpeedUp, "Faster", () => efCtrl.SpeedUp() );
        CreateButton(EF.img.iconSpeedDown, "Slower", () => efCtrl.SpeedDown() );
        CreateButton(EF.img.iconMenu, "Config.", () => { efCtrl.PauseSim(); menuCtrl.ShowConfig(); } );
        CreateButton(EF.img.iconScenarios, "Presets", () => { efCtrl.PauseSim(); menuCtrl.ShowPresets(); } );
        CreateButton(EF.img.iconInfo, "Info", () => { ; } );

		RefreshPlayPauseButton();
		
		statTime		= CreateStat("Time", null, EF.img.iconTime);

		statNeutral		= CreateStat("Neutral",		EF.img.unitNeutral.color);
		statInfected	= CreateStat("Infected",	EF.img.unitInfected.color);
		statSick		= CreateStat("Sick",		EF.img.unitSick.color);
		statImmune		= CreateStat("Immune",		EF.img.unitImmune.color);
		statRecovered	= CreateStat("Recovered",	EF.img.unitRecovered.color);
		statDead		= CreateStat("Dead",		EF.img.unitDead.color);
		statState		= CreateStat("State", null);
    }

	internal void RefreshStats()
	{
		statTime		.value.text = EF.simTimeToString(efCtrl.getSimTime());
		statNeutral		.value.text = EF.efCtrl.population.unitStateNum[(int)Unit.State.NEUTRAL].ToString();
		statInfected	.value.text = EF.efCtrl.population.unitStateNum[(int)Unit.State.INFECTED].ToString();
		statSick		.value.text = EF.efCtrl.population.unitStateNum[(int)Unit.State.SICK].ToString();
		statImmune		.value.text = EF.efCtrl.population.unitStateNum[(int)Unit.State.IMMUNE].ToString();
		statRecovered	.value.text = EF.efCtrl.population.unitStateNum[(int)Unit.State.RECOVERED].ToString();
		statDead		.value.text = EF.efCtrl.population.unitStateNum[(int)Unit.State.DEAD].ToString();
		statState		.value.text = efCtrl.simState.ToString();
	}

	private PlayerButton CreateButton(Sprite icon, string s, System.Action act)
	{
		var button = Instantiate(buttonTmp);
		button.gameObject.SetActive(true);
		button.transform.SetParent(buttonPanel);
		button.transform.localScale = Vector3.one;
		button.icon.sprite = icon;
		button.text.text = s;
		button.act = act;
		return button;
	}
	private StatItem CreateStat(string s, Color? color, Sprite icon = null)
	{
		var stat = Instantiate(statTmp);
		stat.gameObject.SetActive(true);
		stat.transform.SetParent(statsPanel);
		stat.transform.localScale = Vector3.one;
		stat.title.text = s;
		stat.value.text = "";

		if (color == null && icon == null) stat.img.gameObject.SetActive(false);
		if (color != null) stat.img.color = color.Value;
		if (icon != null) stat.img.sprite = icon;

		return stat;
	}

	internal void RefreshPlayPauseButton()
	{
        if (efCtrl.simState == EFCtrl.SimState.RUNNING)
		{
			playPauseButton.icon.sprite = EF.img.iconPause;
			playPauseButton.text.text = "Pause";
		}
		else
		{
			playPauseButton.icon.sprite = EF.img.iconPlay;
			playPauseButton.text.text = "Play";
		}
	}

	// Update is called once per frame
	void Update()
    {
        //if (efCtrl.isRunning())
		//{
		//	statTime.value.text 
		//}
    }
}
