using System;
using UnityEngine;

public class EFCtrl : MonoBehaviour
{
	[HideInInspector]
	public Population population;
	public MenuCtrl menuCtrl;
	private CameraCtrl camCtrl;
	const float DEFAULT_PACE = .1f;
	float pace = DEFAULT_PACE;
	private float stepTime, simTime = 0f;
	const float SIM_STEP = .2f;
	enum SimState
	{
		EMPTY,
		RUNNING,
		PAUSE,
		FINISHED
	}

	SimState simState;

    // Start is called before the first frame update
    void Awake()
    {
		EF.img = FindObjectOfType<ImageCollection>();
		menuCtrl = FindObjectOfType<MenuCtrl>();
		camCtrl = FindObjectOfType<CameraCtrl>();
		simState = SimState.EMPTY;

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
				camCtrl.DrawAll();
				stepTime = pace;
			}
		}
    }
	
	public bool isRunning()
	{
		return simState == SimState.RUNNING;
	}

	public float getSimTime()
	{
		return simTime;
	}

	internal void PlaySim()
	{
		if (simState == SimState.EMPTY)
		{
			pace = DEFAULT_PACE;
			stepTime = pace;
			simTime = 0f;

			population = new Population(EF.cfg);
			camCtrl.ResetView();
			simState = SimState.RUNNING;
		}
		else if (simState == SimState.PAUSE)
		{
			simState = SimState.RUNNING;
		}
	}

	internal void SpeedUp()
	{
		pace *= 0.66f;
	}

	internal void SpeedDown()
	{
		pace *= 1.33f;
	}

	internal void PauseSim()
	{
        if (simState == SimState.RUNNING)
		{
			simState = SimState.PAUSE;
		}
	}

	internal void RestartSim()
	{
		population = null;
		simState = SimState.EMPTY;
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
