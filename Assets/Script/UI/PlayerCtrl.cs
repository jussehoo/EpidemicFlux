using TMPro;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
	public PlayerButton buttonTmp;
	public Transform buttonPanel;

	public TextMeshProUGUI stats;

	private EFCtrl efCtrl;
	private CameraCtrl camCtrl;
	private MenuCtrl menuCtrl;

	void Awake()
	{
		buttonTmp.gameObject.SetActive(false);
		efCtrl = FindObjectOfType<EFCtrl>();
		camCtrl = FindObjectOfType<CameraCtrl>();
		menuCtrl = FindObjectOfType<MenuCtrl>();
		
		stats.text = "";
	}

    // Start is called before the first frame update
    void Start()
    {
        createButton(EF.img.iconPlay, "Play", () => efCtrl.PlaySim() );
        createButton(EF.img.iconPause, "Pause", () => efCtrl.PauseSim() );
        createButton(EF.img.iconReset, "Reset", () => efCtrl.RestartSim() );
        //createButton(EF.img.iconZoomIn, "Zoom in", () => camCtrl.ZoomIn() );
        //createButton(EF.img.iconZoomOut, "Zoom out", () => camCtrl.ZoomOut() );
		createButton(EF.img.iconSpeedUp, "Faster", () => efCtrl.SpeedUp() );
        createButton(EF.img.iconSpeedDown, "Slower", () => efCtrl.SpeedDown() );
        createButton(EF.img.iconMenu, "Config.", () => { efCtrl.PauseSim(); menuCtrl.Show(); } );
    }

	private void createButton(Sprite iconPlay, string s, System.Action act)
	{
		var button = Instantiate(buttonTmp);
		button.gameObject.SetActive(true);
		button.transform.SetParent(buttonPanel);
		button.icon.sprite = iconPlay;
		button.text.text = s;
		button.act = act;
	}

	// Update is called once per frame
	void Update()
    {
        if (efCtrl.isRunning())
		{
			stats.text = "Time: " + EF.simTimeToString(efCtrl.getSimTime());
		}
    }
}
