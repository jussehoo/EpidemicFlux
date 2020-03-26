using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerButton : MonoBehaviour
{
	public Image icon;
	public TextMeshProUGUI text;
	public System.Action act;

    public void Click()
	{
		if (act != null) act.Invoke();
	}
}
