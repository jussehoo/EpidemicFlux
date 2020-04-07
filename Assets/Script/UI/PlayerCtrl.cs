using System;
using TMPro;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
	public PlayerButton buttonTmp;
	public Transform buttonPanel;

	public Transform statsPanel;
	public StatItem statTmp;

	public GameObject graphTex;

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
	private PlayerButton speedUpAndDownButton;

	void Awake()
	{
		buttonTmp.gameObject.SetActive(false);
		statTmp.gameObject.SetActive(false);
		//graphTex.SetActive(false);

		efCtrl = FindObjectOfType<EFCtrl>();
		camCtrl = FindObjectOfType<CameraCtrl>();
		menuCtrl = FindObjectOfType<MenuCtrl>();
	}

    // Start is called before the first frame update
    void Start()
    {
        playPauseButton = CreateButton(EF.img.iconPlay, "Run", () => efCtrl.PlayPauseSim() );
        CreateButton(EF.img.iconReset, "Reset", () => efCtrl.RestartSim() );
		speedUpAndDownButton = CreateButton(EF.img.iconSpeedUp, "Faster", () => efCtrl.SpeedUpOrDown() );
        //CreateButton(EF.img.iconSpeedDown, "Slower", () => efCtrl.SpeedDown() );
        CreateButton(EF.img.iconMenu, "Config.", () => { efCtrl.PauseSim(); menuCtrl.ShowConfig(); } );
        CreateButton(EF.img.iconScenarios, "Presets", () => { efCtrl.PauseSim(); menuCtrl.ShowPresets(); } );
        CreateButton(EF.img.iconInfo, "Info", () => { menuCtrl.ShowInfo(); } );

		RefreshButtons();

		CreateRunStats();
    }

	private void ClearStats()
	{
		foreach(Transform  st in statsPanel)
		{
			if (st.gameObject.activeSelf) Destroy(st.gameObject);
		}
	}

	internal void CreateGenStats()
	{
		ClearStats();
		CreateStat("Units", null, null, EF.efCtrl.population.numUnits.ToString());
		CreateStat("Links", null, null, EF.efCtrl.population.numLinks.ToString());
		CreateStat("Avg. links", null, null, String.Format("{0:0.00}", ((float)EF.efCtrl.population.numLinks/EF.efCtrl.population.numUnits)));
	}

	internal void CreateRunStats()
	{
		ClearStats();
		statTime		= CreateStat("Time", null, EF.img.iconTime);
		statNeutral		= CreateStat("Neutral",		EF.img.unitNeutral.color);
		statInfected	= CreateStat("Infected",	EF.img.unitInfected.color);
		statSick		= CreateStat("Sick",		EF.img.unitSick.color);
		statImmune		= CreateStat("Immune",		EF.img.unitImmune.color);
		statRecovered	= CreateStat("Recovered",	EF.img.unitRecovered.color);
		statDead		= CreateStat("Dead",		EF.img.unitDead.color);
		statState		= CreateStat("State", null);
	}

	internal void RefreshRunStats()
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
	private StatItem CreateStat(string s, Color? color, Sprite icon = null, string value = null)
	{
		var stat = Instantiate(statTmp);
		stat.gameObject.SetActive(true);
		stat.transform.SetParent(statsPanel);
		stat.transform.localScale = Vector3.one;
		stat.title.text = s;

		if (value != null) stat.value.text = value;
		else stat.value.text = "";

		if (color == null && icon == null) stat.img.gameObject.SetActive(false);
		if (color != null) stat.img.color = color.Value;
		if (icon != null) stat.img.sprite = icon;

		return stat;
	}

	internal void RefreshButtons()
	{
        if (efCtrl.simState == EFCtrl.SimState.RUNNING)
		{
			playPauseButton.icon.sprite = EF.img.iconPause;
			playPauseButton.text.text = "Pause";
		}
		else
		{
			playPauseButton.icon.sprite = EF.img.iconPlay;
			playPauseButton.text.text = "Run";
		}

		if (efCtrl.pace == EFCtrl.MAX_PACE)
		{
			speedUpAndDownButton.icon.sprite = EF.img.iconSpeedDown;
			speedUpAndDownButton.text.text = "Slower";
		}
		else
		{
			speedUpAndDownButton.icon.sprite = EF.img.iconSpeedUp;
			speedUpAndDownButton.text.text = "Faster";
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
