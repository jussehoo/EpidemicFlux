using System;
using UnityEngine;

public class EFCtrl : MonoBehaviour
{
	[HideInInspector]
	public Population population;
	public MenuCtrl menuCtrl;
	private CameraCtrl camCtrl;
	private PlayerCtrl playerCtrl;

	public const float MIN_PACE = .2f;
	public const float MAX_PACE = .04f;
	public const float DEFAULT_PACE = MAX_PACE;
	internal float pace = DEFAULT_PACE;

	private float stepTime, simTime = 0f;
	const float SIM_STEP = .04f;
	
	internal bool stepped = false;

	public bool skipDrawingOnSteppingFrame = true;

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

		menuCtrl.ShowInfo();
	}
	
	//private bool gcNext = false;

	// Update is called once per frame
	void Update()
    {
		stepped = false;
		bool skipStep = false;

		// equalizing workload per frame

		//if (gcNext) // alternative solution but very slow if frame rate is low
		//{
		//	System.GC.Collect();
		//	gcNext = false;
		//	skipStep = true;
		//}

		if (skipDrawingOnSteppingFrame && !camCtrl.thisCamera.enabled)
		{
			camCtrl.thisCamera.enabled = true;
			
			System.GC.Collect();

			//gcNext = true;
			skipStep = true;
		}

		if (!skipDrawingOnSteppingFrame) System.GC.Collect();

        if (population != null && simState == SimState.RUNNING)
		{
			stepTime -= Time.deltaTime;
			if (!skipStep && stepTime < 0f)
			{
				simTime += SIM_STEP;
				population.Step(SIM_STEP);
				stepped = true;
				if (EndCondition())
				{
					simState = SimState.FINISHED;
					playerCtrl.RefreshButtons();
				}
				playerCtrl.RefreshRunStats();
				
				if (skipDrawingOnSteppingFrame) camCtrl.thisCamera.enabled = false;

				stepTime = pace + stepTime; // add overflow from previous time limit
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
		playerCtrl.RefreshRunStats();
		playerCtrl.CreateGenStats();
		EF.stats.dem.RemoveAll();
	}
	
	internal void PauseSim()
	{
        if (simState == SimState.RUNNING)
		{
			simState = SimState.PAUSE;
		}
		playerCtrl.RefreshButtons();
	}

	internal void PlayPauseSim()
	{
	
        if (simState == SimState.RUNNING)
		{
			simState = SimState.PAUSE;
		}
		else if (simState == SimState.INITIALIZED)
		{
			RunSim();
		}
		else if (simState == SimState.FINISHED)
		{
			Generate();
			RunSim();
		}
		else if (simState == SimState.PAUSE)
		{
			simState = SimState.RUNNING;
		}
		playerCtrl.RefreshButtons();
	}

	void RunSim()
	{
		pace = DEFAULT_PACE;
		stepTime = pace;
		simTime = 0f;
		simState = SimState.RUNNING;
		playerCtrl.CreateRunStats();
		EF.stats.dem.RemoveAll();
		playerCtrl.graphTex.SetActive(true);
	}

	internal void SpeedUpOrDown()
	{
		if (pace == MAX_PACE) pace = MIN_PACE;
		else pace = MAX_PACE;
		playerCtrl.RefreshButtons();
	}

	internal void RestartSim()
	{
		Generate();
		playerCtrl.RefreshButtons();
	}

	internal void OpenConfigMenu()
	{
		menuCtrl.ShowConfig();
	}
}
