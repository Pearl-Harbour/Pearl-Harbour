using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {
    public GameObject[] Levels;
    public Camera [] Cameras;
    public GameObject IntroLevel;
    public GameObject IntroUi;
    public GameObject InGameUI;
    public Text LevelNo;
    public Image HealthSlider;
    public FPSPlayer fpsPlayer;
    public Text TutorialInstructions;
    public Text MapInfo;
    int TutorialId = 0;
    public GameObject InitialKnife;
    public GameObject MissionDonePanel;
    public GameObject MissionFailedPanel;
    public int ShotsFiredCount = 0;
    public GameObject RccCamera;
    public static bool IsDriving = false;
    public GameObject SelectMode;
    public static int EnemiesCount = 0;
    public GameObject FpsControls;
    public GameObject RccCanvas;
    public Text BulletLeft;
	void Awake () {
        Application.targetFrameRate = 70;
        ////for music on/off
        if (PlayerPrefs.GetInt("Music", 0) == 0)
        {
            AudioListener.volume = 1;
        }
        else
        {
            AudioListener.volume = 0;
        }
        ///for checking quality-settings
        if (PlayerPrefs.GetInt("Quality", 0) == 0)
        {
            QualitySettings.SetQualityLevel(6);
        }
        else
        {
            QualitySettings.SetQualityLevel(5);
        }
        /////////
        if (PlayerPrefs.GetInt("Introduction", 0) == 0)
        {
            BulletLeft.transform.parent.parent.gameObject.SetActive(false);
            IntroUi.SetActive(true);
            IntroLevel.SetActive(true);
        }
        else
        {
           // PlayerPrefs.SetInt("LevelNo", 1);
          //  Debug.Log(PlayerPrefs.GetInt("LevelNo", 1));
            if (PlayerPrefs.GetInt("LevelNo", 1) > Levels.Length)
            {
                PlayerPrefs.SetInt("LevelNo", 1);
            }
            Levels[PlayerPrefs.GetInt("LevelNo", 1) - 1].SetActive(true);
            InGameUI.SetActive(true);
            LevelNo.text = "MISSION " + PlayerPrefs.GetInt("LevelNo", 1);
            switch (PlayerPrefs.GetInt("LevelNo", 1))
            {
                case 1:
                    if (PlayerPrefs.GetInt("Part2", 0) == 0)
                    {
                        BulletLeft.transform.parent.parent.gameObject.SetActive(false);
                        InGameUI.SetActive(false);
                        Physics.gravity = new Vector3(0, -0.5f, 0);
                        Levels[0].transform.GetChild(0).gameObject.SetActive(true);
                    }
                    else
                    {
                        FpsControls.SetActive(true);
                        RccCanvas.SetActive(false);
                        TutorialInstructions.gameObject.SetActive(true);
                        Invoke("StartTutorial", 4f);
                        Physics.gravity = new Vector3(0, -9.5f, 0);
                        Levels[0].transform.GetChild(1).gameObject.SetActive(true);
                    }
                    ZoomOutCam();
                    break;
                case 2:
                    FpsControls.SetActive(true);
                    RccCanvas.SetActive(false);
                    TutorialInstructions.gameObject.SetActive(true);
                    TutorialId = 9;
                    Invoke("TutorialInstruction", 0f);
                    Invoke("StartTutorial", 0f);
                    break;
                case 3: 
                     FpsControls.SetActive(true);
                     RccCanvas.SetActive(false);
                    TutorialInstructions.gameObject.SetActive(true);
                    TutorialId = 10;
                    Invoke("TutorialInstruction", 0f);
                    Invoke("StartTutorial", 0f);
                    break;
                case 4:
                     FpsControls.SetActive(true);
                     RccCanvas.SetActive(false);
                    TutorialInstructions.gameObject.SetActive(true);
                    TutorialId = 11;
                    Invoke("TutorialInstruction", 0f);
                    Invoke("StartTutorial", 0f);
                    break;
                case 5:
                    FpsControls.SetActive(true);
                    RccCanvas.SetActive(false);
                    TutorialInstructions.gameObject.SetActive(true);
                    TutorialId = 12;
                    Invoke("TutorialInstruction", 0f);
                    Invoke("StartTutorial", 0f);
                    break;
            }
        }

        fpsPlayer = FindObjectOfType<FPSPlayer>();
	}
    void DisableHint()
    {
        TutorialInstructions.gameObject.SetActive(false);
    }
    public void EnterInCar()
    {
        if(TutorialId == 13)
        Invoke("DisableHint", 0f);
        FpsControls.SetActive(false);
        RccCanvas.SetActive(true);
        ControlFreak2.CFCursor.lockState = CursorLockMode.None;
        FindObjectOfType<CameraControl>().GetComponent<Camera>().farClipPlane = 0;
        FindObjectOfType<CameraControl>().GetComponent<AudioListener>().enabled = false;
        RccCamera.gameObject.SetActive(true);
        FindObjectOfType<InputControl>().enabled = false;
        fpsPlayer.crosshairEnabled = false;
       //    fpsPlayer.transform.parent.
    }
    public void ExitCar()
    {
        FpsControls.SetActive(true);
        RccCanvas.SetActive(false);
        ControlFreak2.CFCursor.lockState = CursorLockMode.Locked;
        FindObjectOfType<CameraControl>().GetComponent<Camera>().farClipPlane = 200;
        FindObjectOfType<CameraControl>().GetComponent<AudioListener>().enabled = true;
        fpsPlayer.crosshairEnabled = true;
        FindObjectOfType<InputControl>().enabled = true;
        fpsPlayer.transform.position = FindObjectOfType<RCC_CarControllerV3>().transform.position;
        RccCamera.gameObject.SetActive(false);
      //  fpsPlayer.transform.parent.gameObject.SetActive(true);
      
    }
    public void ExitMobile()
    {
        EnterExitCar EnterExit = FindObjectOfType<EnterExitCar>();
        EnterExit.ExitCar = true;
        TutorialInstructions.gameObject.SetActive(true);
    }
    public void StealthMode()
    {
        EnterExitCar EnterExit = FindObjectOfType<EnterExitCar>();
        EnterExit.ExitCar = true;
        EnterExit.whatToDo.text = "";
        TutorialInstructions.text = "Walk Slowly and attack enemies from behind";
        TutorialInstructions.gameObject.SetActive(true);
        EnterExit.enabled = false;
        PlayerWeapons weapons = FindObjectOfType<PlayerWeapons>();
        weapons.firstWeapon = 1;
        weapons.weaponOrder[1].GetComponent<WeaponBehavior>().haveWeapon = true;
        StartCoroutine(weapons.SelectWeapon(1));
        ExitCar();
    }
    public void ArcadeMode()
    {
        EnterExitCar EnterExit = FindObjectOfType<EnterExitCar>();
        EnterExit.PlayingArcadeMode = true;
        PlayerWeapons weapons = FindObjectOfType<PlayerWeapons>();
        TutorialInstructions.text = "Shoot at enemies with gun";
        weapons.firstWeapon = 5;
        weapons.weaponOrder[5].GetComponent<WeaponBehavior>().haveWeapon = true;
        StartCoroutine(weapons.SelectWeapon(5));
    }
    void StartTutorial()
    {
        PlayerPrefs.SetInt("InitialTutorial",1);
    }
	void TutorialInstruction()
    {
        TutorialId = TutorialId + 1;
        switch(TutorialId)
        {
            case 1:
                TutorialInstructions.text = "Move Joystic Forward to Move Forward";
                break;
            case 2:
                TutorialInstructions.text = "Move Joystic Backward to Move Backward";
                break;
            case 3:
                TutorialInstructions.text = "Move Joystic Right to Move Right";
                break;
            case 4:
                TutorialInstructions.text = "Move Joystic Left to Move Left";
                break;
            case 5:
                TutorialInstructions.text = "Press Crouch Button to Crouch";
                break;
            case 6:
                TutorialInstructions.text = "Press Jump Button to Jump";
                break;
            case 10:
                TutorialInstructions.text = "Practice some shooting by hitting dummy with knife";
                break;
            case 11:
                TutorialInstructions.text = "Pick Up Gun from bench and shoot 10 fires at board";
                break;
            case 12:
                EnemiesCount = 2;
                TutorialInstructions.text = "Look around for car by following car icon on map";
                break;
            case 13:
                EnemiesCount = 8;
                TutorialInstructions.text = "Pickup weapons & ammos then\n drive vehicle to main base to clear it!";
                break;
        }
    }
    void Update()
    {
        if(fpsPlayer!=null)
            BulletLeft.text = "" + fpsPlayer.WeaponBehaviorComponent.bulletsLeft + "/" + fpsPlayer.WeaponBehaviorComponent.ammo;
        if(PlayerPrefs.GetInt("InitialTutorial",0)==1)
        {
            switch(TutorialId)
            {
                case 0:
                    if (ControlFreak2.CF2Input.GetAxis("Mouse X") > 0.2 || ControlFreak2.CF2Input.GetAxis("Mouse X") < -0.2)
                    {
                        TutorialInstruction();
                    }
                    break;
                case 1:
                    if (ControlFreak2.CF2Input.GetAxis("Joystick Move Y") > 0.1f)
                    {
                        TutorialInstruction();
                    }
                    break;
                case 2:
                    if (ControlFreak2.CF2Input.GetAxis("Joystick Move Y") < -0.1f)
                    {
                        TutorialInstruction();
                    }
                    break;
                case 3:
                    if (ControlFreak2.CF2Input.GetAxis("Joystick Move X") > 0.1f)
                    {
                        TutorialInstruction();
                    }
                    break;
                case 4:
                    if (ControlFreak2.CF2Input.GetAxis("Joystick Move X") < -0.1f)
                    {
                        TutorialInstruction();
                    }
                    break;
                case 5:
                    if (ControlFreak2.CF2Input.GetKey(KeyCode.LeftControl) || ControlFreak2.CF2Input.GetKey(KeyCode.RightControl))
                    {
                        TutorialInstruction();
                    }
                    break;
                case 6:
                    fpsPlayer.crosshairEnabled = true;
                    if (ControlFreak2.CF2Input.GetKey(KeyCode.Space))
                    {
                        TutorialInstruction();
                        MapInfo.gameObject.SetActive(true);
                        TutorialInstructions.gameObject.SetActive(false);
                    }
                    break;
                case 7:
                    if(InitialKnife!=null&& Vector3.Distance(InitialKnife.transform.position,fpsPlayer.transform.position)<5)
                    {
                        MapInfo.gameObject.SetActive(false);
                        TutorialInstructions.gameObject.SetActive(true);
                        TutorialInstructions.text = "Look at knife and press use button to pickUp It";
                        TutorialInstruction();
                    }
                    break;
                case 8:
                    if (InitialKnife==null)
                    {
                        TutorialInstructions.text = "CLICK FIRE BUTTON TO FIRE";
                        TutorialInstruction();
                    }
                    break;
                case 9:
                    if (ControlFreak2.CF2Input.GetMouseButton(0))
                    {
                        Invoke("MissionCompleted", 3f);
                        PlayerPrefs.SetInt("InitialTutorial", 0);
                        TutorialInstructions.gameObject.SetActive(false);
                        TutorialInstruction();
                    }
                    break;
            }
        }
        if(fpsPlayer!=null)
        HealthSlider.fillAmount = fpsPlayer.hitPoints / 100;
    }
    public void GameIsOver()
    {
        fpsPlayer.crosshairEnabled = false;
        ControlFreak2.CFCursor.lockState = CursorLockMode.None;
        MissionFailedPanel.SetActive(true);
        FindObjectOfType<SmoothMouseLook>().enabled = false;
    }
    public void MissionCompleted()
    {
        fpsPlayer.crosshairEnabled = false;
        ControlFreak2.CFCursor.lockState = CursorLockMode.None;
        FindObjectOfType<SmoothMouseLook>().enabled = false;    
        PlayerPrefs.SetInt("LevelNo", PlayerPrefs.GetInt("LevelNo", 1) + 1);
        MissionDonePanel.SetActive(true);
    }
    public void Next()
    {
        fpsPlayer.levelLoadFadeObj.GetComponent<LevelLoadFade>().FadeAndLoadLevel(Color.black, 1.2f, false);
    }
	void ZoomOutCam()
    {
        Cameras[0].fieldOfView = Cameras[0].fieldOfView + .05f;
        if(Cameras[0].fieldOfView<50)
        {
            Invoke("ZoomOutCam", 0.01f);
        }
    }
}
