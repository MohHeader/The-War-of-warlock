using UnityEngine;
using System.Collections;

public class ShowPanel : MonoBehaviour {

    public GameObject ServerListPanel;
    //public GameObject Cam1,cam2,cam3,cam4,cam5;MarketPanelSellContainer
    public GameObject Cam;
	public int level;
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Show() {
        ServerListPanel.SetActive(true);
    }

    public void Hide()
    {
        ServerListPanel.SetActive(false);
    }

	public void LoadScene()
	{
		Application.LoadLevel (level);
	}

	//public void ShowCam()
	//{
	//	Cam1.SetActive (true);
	//	cam2.SetActive (false);
	//	cam3.SetActive (false);
	//	cam4.SetActive (false);
	//	cam5.SetActive (false);
	//}

	public void ShowCam()
	{
		Cam.SetActive (true);
	}
	public void HideCam()
	{
		Cam.SetActive (false);
	}

}
