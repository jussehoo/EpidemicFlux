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
        playPauseButton = createButton(EF.img.iconPlay, "Play", () => efCtrl.PlayPauseSim() );
        //createButton(EF.img.iconPause, "Pause", () => efCtrl.PauseSim() );
        createButton(EF.img.iconReset, "Reset", () => efCtrl.RestartSim() );
        //createButton(EF.img.iconZoomIn, "Zoom in", () => camCtrl.ZoomIn() );
        //createButton(EF.img.iconZoomOut, "Zoom out", () => camCtrl.ZoomOut() );
		createButton(EF.img.iconSpeedUp, "Faster", () => efCtrl.SpeedUp() );
        createButton(EF.img.iconSpeedDown, "Slower", () => efCtrl.SpeedDown() );
        createButton(EF.img.iconMenu, "Config.", () => { efCtrl.PauseSim(); menuCtrl.Show(); } );

		RefreshPlayPauseButton();
		
		statTime		= createStat("Time");
		statNeutral		= createStat("Neutral");
		statInfected	= createStat("Infected");
		statSick		= createStat("Sick");
		statImmune		= createStat("Immune");
		statRecovered	= createStat("Recovered");
		statDead		= createStat("Dead");
		statState		= createStat("State");
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

	private PlayerButton createButton(Sprite icon, string s, System.Action act)
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
	private StatItem createStat(string s)
	{
		var stat = Instantiate(statTmp);
		stat.gameObject.SetActive(true);
		stat.transform.SetParent(statsPanel);
		stat.transform.localScale = Vector3.one;
		stat.title.text = s;
		stat.value.text = "";
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
