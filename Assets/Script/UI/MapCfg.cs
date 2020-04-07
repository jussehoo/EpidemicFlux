using TMPro;
using UnityEngine;

public class MapCfg : MonoBehaviour
{
	public TMP_Dropdown dropdown;

	private void Start()
	{
		dropdown.value = (int)EF.cfg.mapType;
	}
	public void ValueChanged()
	{
		EF.cfg.mapType = (SceneConfig.Map)dropdown.value;
	}
}
