using System;
using UnityEngine;

public class EFCtrl : MonoBehaviour
{
	[HideInInspector]
	public Population population;
	public MenuCtrl menuCtrl;
	private CameraCtrl camCtrl;
	private PlayerCtrl playerCtrl;
	const float DEFAULT_PACE = .1f;
	float pace = DEFAULT_PACE;
	private float stepTime, simTime = 0f;
	const float SIM_STEP = .06f;
	
	public enum SimState
	{
		EMPTY,
		INITIALIZED,
		RUNNING,
		PAUSE,
		FINISHED
	}

	public SimState simState;

    // Start is called before the first frame update
    void Awake()
    {
		EF.img = FindObjectOfType<ImageCollection>();
		menuCtrl = FindObjectOfType<MenuCtrl>();
		camCtrl = FindObjectOfType<CameraCtrl>();
		playerCtrl = FindObjectOfType<PlayerCtrl>();
		simState = SimState.EMPTY;
    }

	private void Start()
	{
		Generate();
	}

	// Update is called once per frame
	void Update()
    {
        if (population != null && simState == SimState.RUNNING)
		{
			stepTime -= Time.deltaTime;
			if (stepTime < 0f)
			{
				simTime += SIM_STEP;
				population.Step(SIM_STEP);
				if (EndCondition()) simState = SimState.FINISHED;
				playerCtrl.RefreshStats();
				camCtrl.drawNext = true;
				stepTime = pace;
			}
		}
    }
	
	public bool EndCondition()
	{
		return
			population.unitStateNum[(int)Unit.State.INFECTED] == 0 &&
			population.unitStateNum[(int)Unit.State.SICK] == 0;
	}

	public bool isRunning()
	{
		return simState == SimState.RUNNING;
	}

	public float getSimTime()
	{
		return simTime;
	}

	public void Generate()
	{
		population = new Population(EF.cfg);
		simState = SimState.INITIALIZED;
		camCtrl.ResetView();
		playerCtrl.RefreshStats();
	}

	internal void PlayPauseSim()
	{
	
        if (simState == SimState.RUNNING)
		{
			simState = SimState.PAUSE;
		}
		else if (simState == SimState.INITIALIZED)
		{
			pace = DEFAULT_PACE;
			stepTime = pace;
			simTime = 0f;
			simState = SimState.RUNNING;
		}
		else if (simState == SimState.PAUSE)
		{
			simState = SimState.RUNNING;
		}
		playerCtrl.RefreshPlayPauseButton();
	}

	internal void PauseSim()
	{
        if (simState == SimState.RUNNING)
		{
			simState = SimState.PAUSE;
		}
		playerCtrl.RefreshPlayPauseButton();
	}

	internal void SpeedUp()
	{
		pace *= 0.66f;
	}

	internal void SpeedDown()
	{
		pace *= 1.33f;
	}

	internal void RestartSim()
	{
		Generate();
	}

	internal void CloseConfigMenu()
	{
		menuCtrl.Hide();
	}

	internal void OpenConfigMenu()
	{
		menuCtrl.Show();
	}
}
