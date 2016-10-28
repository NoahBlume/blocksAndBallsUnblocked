//Created by Noah Blume and Caleb McGennis

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[RequireComponent(typeof(Camera))]
public class gameController : MonoBehaviour
{

    // Public constants
    public const float NATIVE_WIDTH = 900f;
    public const float NATIVE_HEIGHT = 1600f;
    public const float UNBLOCKING_TIMER = 3.0f;

    // Public New Variables
    public new Camera camera;

    // Public Static Variables
    public static gameController controller;
    public static bool hasNeverBeenRun;
    public static bool hasNeverHadTutorial;

    // Public Variables
    public bool isSelectingPrimary = false;
    public bool isSelectingSecondary = false;
    public bool yellowUnblocked = false;
    public bool orangeUnblocked = false;
    public bool redUnblocked = false;
    public bool greenUnblocked = false;
    public bool blueUnblocked = false;
    public bool purpleUnblocked = true;
    public bool whiteUnblocked = true;
    public bool blackUnblocked = true;
    public bool unblockingATM = false;
    public bool isMobile;
    public bool isConsole;
    public bool isPC;       
    public bool hasNewUnlocks;
    public float fadeDuration = 1.0f;
    public float fadEnd;
    public float timer;
    public float tmpTimer = 3;
    public GameObject background;
    public GameObject ballPrimary;
    public GameObject ballSecondary;
    public GameObject currentBall;
    public GameObject blockPrimary;
    public GameObject blockSecondary;
	public GameObject blockFlashing;
    public GameObject dr4g0nbyt3Logo;
    public GameObject flashingLights;
    public GUISkin guiSkin;
    public static int ballsLeft;
    public static int currentScore;
    public static int highScore;
    public static int score;
    public int blockSpeed;
    public int ballSpeed;
    public int unblockablesPage;
    public static string primaryColor = "white";
    public static string secondaryColor = "black";
    public string skin = "customLight";

    // Public stat variables
    public int totalAdsClicked;
    public int totalAdsViewed;
    public int totalBallsGained; 
    public int totalBallsLost;
    public int totalBallLostRatio;
    public int totalBlocksHit;
    public int totalColorChanges;

    // Private Variables
    private Vector3 topSpawnLocation;
    private Vector3 bottomSpawnLocation;
    private Vector3 ballSpawnLocation;

    // Clear this 
    private List<List<GameObject>> blockLanes = new List<List<GameObject>> { new List<GameObject>(), new List<GameObject>() };
    private List<GameObject> balls = new List<GameObject>();

    // Do not clear these
    private static List<string> spriteGroups = new List<string> { "balls", "blocks", "dr4g0nbyt3" };
	public static List<Dictionary<string, Sprite>> sprites = new List<Dictionary<string, Sprite>> { new Dictionary<string, Sprite>(), new Dictionary<string, Sprite>(), new Dictionary<string, Sprite>() };
    private static Dictionary<string, Color32> colors = new Dictionary<string, Color32>(); 
    private static Dictionary<string, GUISkin> guiSkins = new Dictionary<string, GUISkin>();
    private static UnityEngine.Object[] temp;

	//Stuff noah did in a hurry, not really knowing what effect it would have
	public Rigidbody2D primaryBall;
	public Rigidbody2D secondaryBall;
	//ballShouldSpawn is used to fix a weird bug the ball spawning had
	public static bool ballShouldSpawn = false;
	public bool upPressed = false;
	public bool downPressed = false;
	public bool gotAHighScore = false;
	public bool hideMenuContent = false;
	public bool hasLoaded = false;
	public bool hasSaved = false;

    void Awake()
    {

    }

    void Start()
    { 
        LoadAllColors();
        SceneController();
        ballsLeft = 3;
        unblockablesPage = 1;
        if(Application.loadedLevelName == "game")
        {
            StartGame();
			Instantiate (primaryBall, ballSpawnLocation, Quaternion.identity);
            score = 0;
            currentScore = 0;
        }
    }

    void FixedUpdate()
    {

    }

    void Update()
    {
        if (ballsLeft <= 0)
        {
			guiSkin.label.normal.textColor = colors[primaryColor];
            LoadGameOverScene();
			ballController.killBall = true;
        }
		if (ballShouldSpawn && ballsLeft >= 1) {
			Instantiate (primaryBall, ballSpawnLocation, Quaternion.identity);
			ballShouldSpawn = false;
		} else if (ballShouldSpawn) 
		{
			ballShouldSpawn = false;
		}
    }

    void OnGUI()
    {
        // Scaling
        float rx = Screen.width / NATIVE_WIDTH;
        float ry = Screen.height / NATIVE_HEIGHT;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(rx, ry, 1));
        GUI.skin = guiSkin;

        // Scene styling
	    // Gui for game scene.
        if (Application.loadedLevelName == "game")
        {
            
			guiSkin.label.fontSize = 50;
			GUI.Label(new Rect(0, 742, 450, 128), "BALLS: " + ballsLeft.ToString());
			GUI.Label(new Rect(450, 742, 450, 128), "SCORE: " + score.ToString());
			guiSkin.label.fontSize = 72;

			if (GUI.RepeatButton(new Rect(0, 0, 900, 800), "")) 
			{
					Debug.Log ("repeat up");
					upPressed = true;
					ballController.ballSpeedAndOrDirection = 1;
			}

			if (GUI.RepeatButton(new Rect(0, 800, 900, 800), "")) 
			{
					Debug.Log ("repeat down");
					downPressed = true;
					ballController.ballSpeedAndOrDirection = 2;
			}

            if (currentScore != score || unblockingATM)
            {
                currentScore = score;
                
                // Banner for unblocking yellow.
                if (score == 50 && !yellowUnblocked)
                {
                    unblockingATM = true;
                    Invoke("UnblockYellow", UNBLOCKING_TIMER);
                    Invoke("NoLongerUnblocking", 3.0f);
                    tmpTimer -= Time.deltaTime;
                    GUI.Label(new Rect(0, 500, 900, 128), "YELLOW UNBLOCKED!");
                    if (tmpTimer < 0)
                    {
                        Debug.Log("successfully unblocked yellow!");
                    }
                }

                // Banner for unblocking orange.
                else if (score == 100 && !orangeUnblocked)
                {
                    unblockingATM = true;
                    Invoke("UnblockOrange", UNBLOCKING_TIMER);
                    Invoke("NoLongerUnblocking", 3.0f);
                    tmpTimer -= Time.deltaTime;
                    GUI.Label(new Rect(0, 500, 900, 128), "ORANGE UNBLOCKED!");
                    if (tmpTimer < 0)
                    {
                        Debug.Log("successfully unblocked orange!");
                    }
                    
                }

                // Banner for unblocking red.
                else if (score == 150 && !redUnblocked)
                {
                    unblockingATM = true;
                    Invoke("UnblockRed", UNBLOCKING_TIMER);
                    Invoke("NoLongerUnblocking", 3.0f);
                    tmpTimer -= Time.deltaTime;
                    GUI.Label(new Rect(0, 500, 900, 128), "RED UNBLOCKED!");
                    if (tmpTimer < 0)
                    {
                        Debug.Log("successfully unblocked red!");
                    }
                }

                // Banner for unblocking green.
                else if (score == 200 && !greenUnblocked)
                {
                    unblockingATM = true;
                    Invoke("UnblockGreen", UNBLOCKING_TIMER);
                    Invoke("NoLongerUnblocking", 3.0f);
                    tmpTimer -= Time.deltaTime;
                    GUI.Label(new Rect(0, 500, 900, 128), "GREEN UNBLOCKED!");
                    if (tmpTimer < 0)
                    {
                        Debug.Log("successfully unblocked green!");
                    }
                }

                // Banner for unblocking blue.
                else if (score == 250 && !blueUnblocked)
                {
                    unblockingATM = true;
                    Invoke("UnblockBlue", UNBLOCKING_TIMER);
                    Invoke("NoLongerUnblocking", 3.0f);
                    tmpTimer -= Time.deltaTime;
                    GUI.Label(new Rect(0, 500, 900, 128), "BLUE UNBLOCKED!");
                    if (tmpTimer < 0)
                    {
                        Debug.Log("successfully unblocked blue!");
                    }
                    GUI.Label(new Rect(0, 500, 900, 128), "BLUE UNBLOCKED!");
                }

                // Banner for unblocking purple.
                else if (score == 300 && !purpleUnblocked)
                {
                    unblockingATM = true;
                    Invoke("UnblockPurple", UNBLOCKING_TIMER);
                    Invoke("NoLongerUnblocking", 3.0f);
                    tmpTimer -= Time.deltaTime;
                    GUI.Label(new Rect(0, 500, 900, 128), "PURPLE UNBLOCKED!");
                    if (tmpTimer < 0)
                    {
                        Debug.Log("successfully unblocked purple!");
                    }
                }
            }
        }

        // Gui for main menu scene.
		else if (Application.loadedLevelName == "menu")
        {
			if (!hasLoaded) 
			{
				Load();
				hasLoaded = true;
			}
			if (GUI.Button(new Rect(150, 960, 600, 128), ""))
			{
				//LoadUnblockablesScene();
				StartCoroutine(GoToUnblockables());
				isSelectingPrimary = true;
			}
			if (!hideMenuContent) {
				// Main Menu Logo.
				guiSkin.label.fontSize = 128;
				guiSkin.label.fontStyle = FontStyle.Bold;
				// Change color of font on main menu.
				guiSkin.label.normal.textColor = colors [primaryColor];
				GUI.Label (new Rect (0, 64, 900, 256), "BLOCKS");
				guiSkin.label.fontSize = 72;
				GUI.Label (new Rect (0, 288, 900, 256), "AND");
				guiSkin.label.fontSize = 128;
				GUI.Label (new Rect (0, 512, 900, 256), " BALLS ");
				guiSkin.label.fontSize = 72;
				guiSkin.label.fontStyle = FontStyle.Normal;

				// Main Menu Buttons.
				if (GUI.Button (new Rect (225, 800, 450, 128), "")) {
					LoadGameScene ();
					Save ();
					Destroy (gameObject);
					ballPrimary.GetComponent<SpriteRenderer> ().sprite = sprites [0] [primaryColor];
					ballSecondary.GetComponent<SpriteRenderer> ().sprite = sprites [0] [secondaryColor];
					Instantiate (primaryBall, ballSpawnLocation, Quaternion.identity);
				}
				guiSkin.label.normal.textColor = colors [secondaryColor];
				GUI.Label (new Rect (0, 800, 900, 128), "PLAY");


				if (hasNewUnlocks) {
					GUI.Label (new Rect (0, 960, 900, 128), "!UNBLOCKABLES!");
				} else {
					GUI.Label (new Rect (0, 960, 900, 128), "UNBLOCKABLES");
				}
				guiSkin.label.normal.textColor = colors [primaryColor];
				guiSkin.label.fontSize = 54;
				GUI.Label (new Rect (0, 1200, 900, 128), "High Score: " + highScore);
				guiSkin.label.fontSize = 72;
			}
        }
//THIS IS THE BEGINNING OF THE BEAUTIFUL UNBLOCKABLES PAGE THAT MESSES EVERYTHING UP. PLEASE BE GENTLE WHEN HANDLING IT.
        // Gui for unblockables scene.
		else if (Application.loadedLevelName == "unblockables")
        {
			//the beautiful back button (no graphics)
			if (GUI.Button(new Rect(0, 0, 144, 192), ""))
			{
				isSelectingPrimary = false;
				isSelectingSecondary = false;
				unblockablesPage = 1;
				Save();
				StartCoroutine(GoToMenu());
			}
			if (!hideMenuContent) {
	            // Unblockable header formatting.
	            GUI.Box(new Rect(0, 0, 900, 192), "UNBLOCKABLES", "unblockablesHeader");


					if (GUI.Button (new Rect (100, 192, 250, 128), "", "unblockableSection0")) {
						isSelectingPrimary = true;
						isSelectingSecondary = false;
					}
	            
					if (isSelectingPrimary) {
						guiSkin.label.normal.textColor = colors [primaryColor];
						GUI.Label (new Rect (100, 192, 290, 142), "PRIMARY", "unblockableSection2");
					} else {
						GUI.Label (new Rect (100, 192, 290, 142), "PRIMARY", "unblockableSection0");
					}
					if (GUI.Button (new Rect (510, 192, 290, 128), "", "unblockableSection0")) {
						isSelectingSecondary = true;
						isSelectingPrimary = false;
					}
					if (isSelectingSecondary) {
						guiSkin.label.normal.textColor = colors [secondaryColor];
						GUI.Label (new Rect (510, 192, 290, 142), "SECONDARY", "unblockableSection2");
					} else {
						GUI.Label (new Rect (510, 192, 290, 142), "SECONDARY", "unblockableSection0");
					}

					GUI.Label (new Rect (0, 200, 900, 128), unblockablesPage.ToString (), "label");
					guiSkin.label.normal.textColor = colors [primaryColor];

					if (GUI.Button (new Rect (-8, 732, 128, 128), "", "leftArrow")) {
						if (unblockablesPage != 1)
							unblockablesPage--;
					}
					if (GUI.Button (new Rect (780, 732, 128, 128), "", "rightArrow")) {
						if (unblockablesPage != 3)
							unblockablesPage++;
					}
					//The Beautiful back button graphic
					GUI.Box (new Rect (32, 60, 64, 64), "", "backButton");

				// Selections for page 0. (this is a hidden page that is not normally accessible in the game.
	            if (unblockablesPage == 0)
	            {
	                // Place hackers theme here.
	                // Unblock selection for Yellow.
	                if (GUI.Button(new Rect(100, 458, 700, 256), "", "leetSoup"))
	                {
	                    if (isSelectingPrimary)
	                    {
	                        primaryColor = "l33ts0up";
	                        Debug.Log("Primary Color should be l33t now.");
	                    }
	                    if (isSelectingSecondary)
	                    {
	                        secondaryColor = "l33ts0up";
	                        Debug.Log("Secondary Color should be l33t now.");
	                    }
	                }
	                GUI.Label(new Rect(100, 330, 700, 128), "l33t s0up", "unblockablesHeader");
	                Texture tmpTex = (Texture2D)Resources.Load("sprites/blocks/green");
	                GUI.Box(new Rect(494, 512, 256, 256), tmpTex, "box");
	                tmpTex = (Texture2D)Resources.Load("sprites/balls/green");
	                GUI.Box(new Rect(466, 372, 312, 312), tmpTex, "box");
	                tmpTex = (Texture2D)Resources.Load("sprites/blocks/green");
	                GUI.Box(new Rect(150, 512, 256, 256), tmpTex, "box");
	                tmpTex = (Texture2D)Resources.Load("sprites/balls/green");
	                GUI.Box(new Rect(122, 372, 312, 312), tmpTex, "box");
	            }
					// Selections for page 1.
					if (unblockablesPage == 1) {

						// Unblock selection for Yellow.
						if (GUI.Button (new Rect (100, 458, 700, 256), "", "unblockableSection0")) {
							if (isSelectingPrimary && secondaryColor != "yellow" && yellowUnblocked) {
								primaryColor = "yellow";
								guiSkin.label.normal.textColor = colors [primaryColor];
								Debug.Log ("Primary Color should be yellow now.");
							} else if (isSelectingSecondary && primaryColor != "yellow" && yellowUnblocked) {
								secondaryColor = "yellow";
								Debug.Log ("Secondary Color should be yellow now.");
							}
						}
						GUI.Label (new Rect (100, 330, 700, 128), "YELLOW", "unblockablesHeader");
						Texture tmpTex = (Texture2D)Resources.Load ("sprites/blocks/yellow");
						GUI.Box (new Rect (450, 512, 256, 256), tmpTex, "box");
						tmpTex = (Texture2D)Resources.Load ("sprites/balls/yellow");
						GUI.Box (new Rect (422, 372, 312, 312), tmpTex, "box");

						if (!yellowUnblocked) {
							GUI.Label (new Rect (100, 510, 350, 128), "50", "unblockableSection0");
							GUI.Label (new Rect (100, 560, 350, 128), "Points", "unblockableSection0");
						} else {
							GUI.Label (new Rect (100, 530, 350, 128), "UNBLOCKED", "unblockableSection0");
						}

						// Unblock selection for Orange.
						if (GUI.Button (new Rect (100, 860, 700, 256), "", "unblockableSection0")) {
							if (isSelectingPrimary && secondaryColor != "orange" && orangeUnblocked) {
								primaryColor = "orange";
								guiSkin.label.normal.textColor = colors [primaryColor];
								Debug.Log ("Primary Color should be orange now");
							} else if (isSelectingSecondary && primaryColor != "orange" && orangeUnblocked) {
								secondaryColor = "orange";
								Debug.Log ("Secondary Color should be orange now");
							}
						}
						GUI.Label (new Rect (100, 732, 700, 128), "ORANGE", "unblockablesHeader");
						tmpTex = (Texture2D)Resources.Load ("sprites/blocks/orange");
						GUI.Box (new Rect (450, 928, 256, 256), tmpTex, "box");
						tmpTex = (Texture2D)Resources.Load ("sprites/balls/orange");
						GUI.Box (new Rect (422, 788, 312, 312), tmpTex, "box");
						if (!orangeUnblocked) {
							GUI.Label (new Rect (100, 926, 350, 128), "100", "unblockableSection0");
							GUI.Label (new Rect (100, 976, 350, 128), "Points", "unblockableSection0");
						} else {
							GUI.Label (new Rect (100, 946, 350, 128), "UNBLOCKED", "unblockableSection0");
						}

						// Unblock selection for Red.
						if (GUI.Button (new Rect (100, 1260, 700, 256), "", "unblockableSection0")) {
							if (isSelectingPrimary && secondaryColor != "red" && redUnblocked) {
								primaryColor = "red";
								guiSkin.label.normal.textColor = colors [primaryColor];
								Debug.Log ("Primary Color should be red now");
							} else if (isSelectingSecondary && primaryColor != "red" && redUnblocked) {
								secondaryColor = "red";
								Debug.Log ("Secondary Color should be red now");
							} else if (!isSelectingPrimary && !isSelectingSecondary) {
								Debug.Log ("Abort missions both negative should be happening if no choice selected for main or secondary");
							}
						}
						GUI.Label (new Rect (100, 1132, 700, 128), "RED", "unblockablesHeader");
						tmpTex = (Texture2D)Resources.Load ("sprites/blocks/red");
						GUI.Box (new Rect (450, 1328, 256, 256), tmpTex, "box");
						tmpTex = (Texture2D)Resources.Load ("sprites/balls/red");
						GUI.Box (new Rect (422, 1188, 312, 312), tmpTex, "box");
						if (!redUnblocked) {
							GUI.Label (new Rect (100, 1326, 350, 128), "150", "unblockableSection0");
							GUI.Label (new Rect (100, 1376, 350, 128), "Points", "unblockableSection0");
						} else {
							GUI.Label (new Rect (100, 1346, 350, 128), "UNBLOCKED", "unblockableSection0");
						}
					}

	            // Selections for page 2.
	            else if (unblockablesPage == 2) {
					// Unblock selection for Green.
					if (GUI.Button (new Rect (100, 458, 700, 256), "", "unblockableSection0")) {
						if (isSelectingPrimary && secondaryColor != "green" && greenUnblocked) {
							primaryColor = "green";
							guiSkin.label.normal.textColor = colors [primaryColor];
							Debug.Log ("Primary Color should be green now.");
						}
						if (isSelectingSecondary && secondaryColor != "green" && greenUnblocked) {
							secondaryColor = "green";
							Debug.Log ("Secondary Color should be green now.");
						}
					}
					GUI.Label (new Rect (100, 330, 700, 128), "GREEN", "unblockablesHeader");
					Texture tmpTex = (Texture2D)Resources.Load ("sprites/blocks/green");
					GUI.Box (new Rect (450, 512, 256, 256), tmpTex, "box");
					tmpTex = (Texture2D)Resources.Load ("sprites/balls/green");
					GUI.Box (new Rect (422, 372, 312, 312), tmpTex, "box");
					if (!greenUnblocked) {
						GUI.Label (new Rect (100, 510, 350, 128), "200", "unblockableSection0");
						GUI.Label (new Rect (100, 560, 350, 128), "Points", "unblockableSection0");
					} else {
						GUI.Label (new Rect (100, 530, 350, 128), "UNBLOCKED", "unblockableSection0");
					}

					// Unblock selection for Blue.
					if (GUI.Button (new Rect (100, 860, 700, 256), "", "unblockableSection0")) {
						if (isSelectingPrimary && secondaryColor != "blue" && blueUnblocked) {
							primaryColor = "blue";
							guiSkin.label.normal.textColor = colors [primaryColor];
							Debug.Log ("Primary Color should be blue now");
						} else if (isSelectingSecondary && primaryColor != "blue" && blueUnblocked) {
							secondaryColor = "blue";
							Debug.Log ("Secondary Color should be blue now");
						}
					}
					GUI.Label (new Rect (100, 732, 700, 128), "BLUE", "unblockablesHeader");
					tmpTex = (Texture2D)Resources.Load ("sprites/blocks/blue");
					GUI.Box (new Rect (450, 928, 256, 256), tmpTex, "box");
					tmpTex = (Texture2D)Resources.Load ("sprites/balls/blue");
					GUI.Box (new Rect (422, 788, 312, 312), tmpTex, "box");
					if (!blueUnblocked) {
						GUI.Label (new Rect (100, 926, 350, 128), "250", "unblockableSection0");
						GUI.Label (new Rect (100, 976, 350, 128), "Points", "unblockableSection0");
					} else {
						GUI.Label (new Rect (100, 946, 350, 128), "UNBLOCKED", "unblockables0");
					}

					// Unblock selection for Purple.
					if (GUI.Button (new Rect (100, 1260, 700, 256), "", "unblockableSection0")) {
						if (isSelectingPrimary && secondaryColor != "purple" && purpleUnblocked) {
							primaryColor = "purple";
							guiSkin.label.normal.textColor = colors [primaryColor];
							Debug.Log ("Primary Color should be purple now");
						} else if (isSelectingSecondary && primaryColor != "purple" && purpleUnblocked) {
							secondaryColor = "purple";
							Debug.Log ("Secondary Color should be purple now");
						} else if (!isSelectingPrimary && !isSelectingSecondary) {
							Debug.Log ("Abort missions both negative should be happening if no choice selected for main or secondary");
						}
					}
					GUI.Label (new Rect (100, 1132, 700, 128), "PURPLE", "unblockablesHeader");
					tmpTex = (Texture2D)Resources.Load ("sprites/blocks/purple");
					GUI.Box (new Rect (450, 1328, 256, 256), tmpTex, "box");
					tmpTex = (Texture2D)Resources.Load ("sprites/balls/purple");
					GUI.Box (new Rect (422, 1188, 312, 312), tmpTex, "box");
					if (!purpleUnblocked) {
						GUI.Label (new Rect (100, 1326, 350, 128), "300", "unblockableSection0");
						GUI.Label (new Rect (100, 1376, 350, 128), "Points", "unblockableSection0");
					} else {
						GUI.Label (new Rect (100, 1346, 350, 128), "UNBLOCKED", "unblockableSection0");
					}
				}

	            // Unblock selections for page 3.
	            else if (unblockablesPage == 3) {
					// Unblock selection for White.
					if (GUI.Button (new Rect (100, 458, 700, 256), "", "unblockableSection0")) {
						if (isSelectingPrimary && secondaryColor != "white" && whiteUnblocked) {
							primaryColor = "white";
							guiSkin.label.normal.textColor = colors [primaryColor];
							Debug.Log ("Primary Color should be white now.");
						}
						if (isSelectingSecondary && primaryColor != "white" && whiteUnblocked) {
							secondaryColor = "white";
							Debug.Log ("Secondary Color should be white now.");
						}
					}
					GUI.Label (new Rect (100, 330, 700, 128), "WHITE", "unblockablesHeader");
					Texture tmpTex = (Texture2D)Resources.Load ("sprites/blocks/white");
					GUI.Box (new Rect (450, 512, 256, 256), tmpTex, "box");
					tmpTex = (Texture2D)Resources.Load ("sprites/balls/white");
					GUI.Box (new Rect (422, 372, 312, 312), tmpTex, "box");
					GUI.Label (new Rect (100, 530, 350, 128), "UNBLOCKED", "unblockableSection0");

					// Unblock selection for Black.
					if (GUI.Button (new Rect (100, 860, 700, 256), "", "unblockableSection0")) {
						if (isSelectingPrimary && secondaryColor != "black" && blackUnblocked) {
							primaryColor = "black";
							guiSkin.label.normal.textColor = colors [primaryColor];
							Debug.Log ("Primary Color should be black now");
						} else if (isSelectingSecondary && primaryColor != "black" && blackUnblocked) {
							secondaryColor = "black";
							Debug.Log ("Secondary Color should be black now");
						}
					}
					GUI.Label (new Rect (100, 732, 700, 128), "BLACK", "unblockablesHeader");
					tmpTex = (Texture2D)Resources.Load ("sprites/blocks/black");
					GUI.Box (new Rect (450, 928, 256, 256), tmpTex, "box");
					tmpTex = (Texture2D)Resources.Load ("sprites/balls/black");
					GUI.Box (new Rect (422, 788, 312, 312), tmpTex, "box");
					GUI.Label (new Rect (100, 946, 350, 128), "UNBLOCKED", "unblockableSection0");
				}
			}
        }

        // Gui for credits scene.
        else if (Application.loadedLevelName == "credits")
        {
            GUI.Box(new Rect(0, 0, 900, 192), "CREDITS", "unblockablesHeader");
            if (GUI.Button(new Rect(32, 60, 64, 64), "", "backButton"))
            {
                LoadMenuScene();
            }
            guiSkin.label.fontSize = 72;
            GUI.Label(new Rect(0,220,900, 128), "Programmers");
            guiSkin.label.fontSize = 48;
            GUI.Label(new Rect(0, 348, 900, 128), "dr4g0nbyt3");
            GUI.Label(new Rect(0, 412, 900, 128), "PantslessGrandpa");

            guiSkin.label.fontSize = 72;
            GUI.Label(new Rect(0, 602, 900, 128), "Marketing");
            guiSkin.label.fontSize = 48;
            GUI.Label(new Rect(0, 730, 900, 128), "Rodney Cooper");

            guiSkin.label.fontSize = 72;
            GUI.Label(new Rect(0, 910, 900, 128), "Backers");

            Texture tex = (Texture2D)Resources.Load("sprites/p4ntsl3ssgr4ndp4/pantslessgrandpaicon");
            GUI.Box(new Rect(736, 1400, 256, 256), tex, "box");
            tex = (Texture2D)Resources.Load("sprites/dr4g0nbyt3/icon");
            GUI.Box(new Rect(64, 1400, 256, 256), tex, "box");
        }
        // Gui for game over scene.
        else if (Application.loadedLevelName == "g")
        {
			// Main Menu Logo.
			guiSkin.label.fontSize = 128;
			guiSkin.label.fontStyle = FontStyle.Bold;
			// Change color of font on main menu.
			guiSkin.label.normal.textColor = colors[primaryColor];
			GUI.Label(new Rect(0, 64, 900, 256), "BLOCKS");
			guiSkin.label.fontSize = 72;
			GUI.Label(new Rect(0, 288,900,256 ), "AND");
			guiSkin.label.fontSize = 128;
			GUI.Label(new Rect(0, 512, 900, 256), " BALLS ");
			guiSkin.label.fontSize = 72;
			guiSkin.label.fontStyle = FontStyle.Normal;

			// Main Menu Buttons.
			if (GUI.Button(new Rect(225, 800, 450, 128), ""))
			{
				gotAHighScore = false;
				hasSaved = false;
				LoadGameScene();
				Destroy(gameObject);
				ballPrimary.GetComponent<SpriteRenderer>().sprite = sprites[0][primaryColor];
				ballSecondary.GetComponent<SpriteRenderer>().sprite = sprites[0][secondaryColor];
				Instantiate (primaryBall, ballSpawnLocation, Quaternion.identity);
			}
			guiSkin.label.normal.textColor = colors[secondaryColor];
			GUI.Label(new Rect(0, 800, 900, 128), "PLAY");
			if (GUI.Button(new Rect(150, 960, 600, 128), ""))
			{
				gotAHighScore = false;
				hasSaved = false;
				LoadUnblockablesScene();
				isSelectingPrimary = true;
			}
			if (hasNewUnlocks)
			{
				GUI.Label(new Rect(0, 960, 900, 128), "!UNBLOCKABLES!");
			}
			else
			{
				GUI.Label(new Rect(0, 960, 900, 128), "UNBLOCKABLES");
			}
			if (score > highScore) {
				gotAHighScore = true;
				highScore = score;
			} 
			guiSkin.label.normal.textColor = colors[primaryColor];
			if (gotAHighScore) 
			{
				guiSkin.label.fontSize = 54;
				GUI.Label (new Rect (0, 1120, 900, 128), "New High Score!");
				GUI.Label (new Rect (0, 1280, 900, 128), "High Score: " + highScore);
				GUI.Label (new Rect (0, 1440, 900, 128), "Your Score: " + score);
				guiSkin.label.fontSize = 72;
			}
			else 
			{
				guiSkin.label.fontSize = 54;
				GUI.Label (new Rect (0, 1120, 900, 128), "High Score: " + highScore);
				GUI.Label (new Rect (0, 1280, 900, 128), "Your Score: " + score);
				guiSkin.label.fontSize = 72;
			}
			if (!hasSaved) 
			{
				Save();
				hasSaved = true;
			}
        }
        else
        {
            LoadMenuScene();
        }
    }

    // Loads game scene.
    public void LoadGameScene()
    {
        Application.LoadLevel("game");
    }

    // Loads menu scene.
    public void LoadMenuScene()
    {
        Application.LoadLevel("menu");
    }

    // Loads options scene.
    public void LoadOptionsScene()
    {
        Application.LoadLevel("options");
    }

    // Loads unblockables scene.
    public void LoadUnblockablesScene()
    {
        Application.LoadLevel("unblockables");
    }

    // Loads game over scene
    // Change this to game over
    public void LoadGameOverScene()
    {
        Application.LoadLevel("g");
		ballController.killBall = true;
    }

    // Loads credits scene
    public void LoadCreditsScene()
    {
        Application.LoadLevel("credits");
    }

    // Exits game or editor
    public void QuitGame()
    {
        Application.Quit();
    }

    public void UnblockYellow()
    {
        yellowUnblocked = true;
    }

    public void UnblockOrange()
    {
        orangeUnblocked = true;
    }

    public void UnblockRed()
    {
        redUnblocked = true;
    }

    public void UnblockGreen()
    {
        greenUnblocked = true;
    }

    public void UnblockBlue()
    {
        blueUnblocked = true;
    }

    public void UnblockPurple()
    {
        purpleUnblocked = true;
    }

    public void DestroyGameController()
    {
        Destroy(gameObject);
    }

    public void resetTmpTimer()
    {
        tmpTimer = 3;
    }

    public void NoLongerUnblocking()
    {
        unblockingATM = false;
    }

    // Makes sure there is always a gameController.
    void GameControllerRules()
    {
        if (controller == null)
        {
            DontDestroyOnLoad(gameObject);
            controller = this;
        }
        else if (controller != this)
        {
            Destroy(gameObject);
        }
    }

    // Handles all of my magic.
    void SceneController()
    {
        GameControllerRules();
        camera = GetComponent<Camera>();
        LoadAllSprites();
        LoadAllGuiSkins();
        camera.backgroundColor = new Color32(32, 32, 32, 32);
        if (Application.loadedLevelName == "menu")
        {
            camera.backgroundColor = new Color32(32, 32, 32, 32);
        }
        if (Application.loadedLevelName == "options")
        {
            camera.backgroundColor = new Color32(32, 32, 32, 32);
        }
        if (Application.loadedLevelName == "game")
        {
            camera.backgroundColor = new Color32(32, 32, 32, 32);
        }
        if (Application.loadedLevelName == "g")
        {
            camera.backgroundColor = new Color32(32, 32, 32, 32);
        }
    }

    void StartGame()
    {
        // Handles spawning of block spawn locations based on screen size.
        var block2DCollider = blockPrimary.GetComponent<Collider2D>();
        topSpawnLocation = new Vector3(Screen.width + Screen.width, Screen.height - Screen.height / 32, 11);
        topSpawnLocation = camera.ScreenToWorldPoint(topSpawnLocation);
        bottomSpawnLocation = new Vector3(-Screen.width, Screen.height / 32, 11);
        bottomSpawnLocation = camera.ScreenToWorldPoint(bottomSpawnLocation);
        bottomSpawnLocation.x += block2DCollider.bounds.size.x * 2;
        bottomSpawnLocation.y -= block2DCollider.bounds.size.y * 2;
        ballSpawnLocation = new Vector3(0,0, 11);
        blockPrimary.GetComponent<SpriteRenderer>().sprite = sprites[1][primaryColor];
        blockSecondary.GetComponent<SpriteRenderer>().sprite = sprites[1][secondaryColor];
		blockFlashing.GetComponent<SpriteRenderer>().sprite = sprites[1][primaryColor];

        // Invoke methods
        Invoke("SpawnBall", 2.0f);
        InvokeRepeating("SpawnBlocks", 0.1f, 0.6f);
        InvokeRepeating("SwapPrimaryAndSecondary", 10.0f, 10.0f);
        InvokeRepeating("SpawnFlashingLights", 9.1f, 10.0f);
    }

	public static void HitRightColor()
	{
		gameController.score++;

	}

	public static void HitWrongColor()
	{
		gameController.ballsLeft--;
		if (score >= 1) 
		{
			gameController.score--;
		}
	}
	public static void HitFlashing()
	{
		gameController.ballsLeft++;
		gameController.score++;
	}

	// Begin Kanye reference if need be. (Just in case this was confusing, Kanye has a song called Flashing Lights, and we find joy in making dumb jokes in our code.)
    void SpawnFlashingLights()
    {
		if (Application.loadedLevelName == "game") 
		{
			StartCoroutine (FlashTextColor ());
		}
    }

	IEnumerator FlashTextColor()
	{
		if (guiSkin.label.normal.textColor == colors [primaryColor]) 
		{
			guiSkin.label.normal.textColor = colors [secondaryColor];
			yield return new WaitForSeconds (.2f);
			guiSkin.label.normal.textColor = colors [primaryColor];
			yield return new WaitForSeconds (.2f);
			guiSkin.label.normal.textColor = colors [secondaryColor];
			yield return new WaitForSeconds (.2f);
			guiSkin.label.normal.textColor = colors [primaryColor];
			yield return new WaitForSeconds (.2f);
			guiSkin.label.normal.textColor = colors [secondaryColor];
		}
		else if (guiSkin.label.normal.textColor == colors [secondaryColor]) 
		{
			guiSkin.label.normal.textColor = colors [primaryColor];
			yield return new WaitForSeconds (.2f);
			guiSkin.label.normal.textColor = colors [secondaryColor];
			yield return new WaitForSeconds (.2f);
			guiSkin.label.normal.textColor = colors [primaryColor];
			yield return new WaitForSeconds (.2f);
			guiSkin.label.normal.textColor = colors [secondaryColor];
			yield return new WaitForSeconds (.2f);
			guiSkin.label.normal.textColor = colors [primaryColor];
		}
	}
	IEnumerator GoToMenu() 
	{
		hideMenuContent = true;
		yield return new WaitForSeconds(0.1f);
		Application.LoadLevel ("menu");
		yield return new WaitForSeconds (0.1f);
		hideMenuContent = false;
	}
	IEnumerator GoToUnblockables() 
	{
		hideMenuContent = true;
		yield return new WaitForSeconds(0.1f);
		Application.LoadLevel("unblockables");
		yield return new WaitForSeconds (0.1f);
		hideMenuContent = false;
	}
		

    // Handles spawning and making primary, secondary, or exclusive blocks.
    void SpawnBlocks()
    {
		if (Application.loadedLevelName == "game") 
		{
			int topRandom = UnityEngine.Random.Range (0, 41);
			int bottomRandom = UnityEngine.Random.Range (0, 41);
			GameObject topSpawn;
			GameObject bottomSpawn;
			if (topRandom < 20) {
				topSpawn = blockSecondary;
			} else if (topRandom > 20) {
				topSpawn = blockPrimary;
			} else {
				topSpawn = blockFlashing;
			}

			if (bottomRandom < 20) {
				bottomSpawn = blockSecondary;
			} else if (bottomRandom > 20) {
				bottomSpawn = blockPrimary;
			} else {
				bottomSpawn = blockFlashing;
			}

			blockLanes [0].Add ((GameObject)Instantiate (topSpawn, topSpawnLocation, Quaternion.identity));
			blockLanes [0] [blockLanes [0].Count - 1].GetComponent<Rigidbody2D> ().velocity = new Vector2 (-3, 0);
			blockLanes [1].Add ((GameObject)Instantiate (bottomSpawn, bottomSpawnLocation, Quaternion.identity));
			blockLanes [1] [blockLanes [1].Count - 1].GetComponent<Rigidbody2D> ().velocity = new Vector2 (3, 0);
		}
	}

    // Swaps colors
    public void SwapPrimaryAndSecondary()
    {
		ballController.swapBallColor = true;
        var temp = blockPrimary.GetComponent<SpriteRenderer>().sprite;
        temp = ballPrimary.GetComponent<SpriteRenderer>().sprite;
        ballPrimary.GetComponent<SpriteRenderer>().sprite = ballSecondary.GetComponent<SpriteRenderer>().sprite;
        ballSecondary.GetComponent<SpriteRenderer>().sprite = temp;
    }

    // Creates rules based on the platform of the game.
    void EstablishControls()
    {
        if (hasNeverBeenRun)
        {
            if (Application.isMobilePlatform)
            {
                // Enable touch controls
                isMobile = true;
            }

            else if (Application.isConsolePlatform)
            {
                // Enable controler controls
                isConsole = true;
            }
            else if (Application.isEditor)
            {
                // Enable pc controls
                isPC = true;
                Debug.Log("Houston we appear to be working");
            }
        }
    }

	// Guess what this does. (It loads all of the sprites)
    private static void LoadAllSprites()
    {
        for (int i = 0; i < spriteGroups.Count; ++i)
        {
            if (sprites[i].Count == 0)
            {
                temp = Resources.LoadAll<Sprite>("sprites/" + spriteGroups[i] + "/");
                for (int j = 0; j < temp.Length; ++j)
                {
                    sprites[i].Add((temp[j]).name, (Sprite)temp[j]);
                }
            }
        }
    }

    // Idek what this does
    private static void LoadAllColors()
    {
        if (colors.Count == 0)
        {
            colors.Add("yellow", new Color32(255, 234, 0, 255));
            colors.Add("orange", new Color32(255, 150, 0, 255));
            colors.Add("red", new Color32(234, 27, 27, 255));
            colors.Add("green", new Color32(32, 184, 16, 255));
            colors.Add("blue", new Color32(0, 78, 220, 255));
            colors.Add("purple", new Color32(130, 0, 205, 255));
            colors.Add("white", new Color32(255, 255, 255, 255));
            colors.Add("black", new Color32(0, 0, 0, 255));
        }
    }

    // How about this one?
    private static void LoadAllGuiSkins()
    {
        if (guiSkins.Count == 0)
        {
            temp = Resources.LoadAll<GUISkin>("skins/");
            for (int j = 0; j < temp.Length; ++j)
            {
                guiSkins.Add((temp[j]).name, (GUISkin)temp[j]);
            }
        }

    }

    // The almighty save function
    void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.Open);

        GameData data = new GameData(hasNeverBeenRun, hasNeverHadTutorial, highScore, primaryColor, secondaryColor, skin);

        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Saved File to " + Application.persistentDataPath + "/gameData.dat");
    }

    // The messed up load function (only upon new loading)
    void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/gameData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file);
            file.Close();
            hasNeverBeenRun = data.m_hasNeverBeenRun;
            //guiSkin = guiSkins[data.m_skin];
            skin = data.m_skin;
            highScore = data.m_highScore;
            primaryColor = data.m_primaryColor;
            secondaryColor = data.m_secondaryColor;

            Debug.Log("Loaded Save File");
        }
        else
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/gameData.dat");
            GameData data = new GameData();
            bf.Serialize(file, data);
            file.Close();
            Debug.Log("Loaded New Save file");
        }
    }
}

[Serializable]
class GameData
{

    public bool m_hasNeverBeenRun;
    public bool m_hasNeverHadTutorial;
    public int m_highScore;
    public string m_primaryColor;
    public string m_secondaryColor;
    public string m_skin;
    public bool m_yellowUnblocked = false;
    public bool m_orangeUnblocked = false;
    public bool m_redUnblocked = false;
    public bool m_greenUnblocked = false;
    public bool m_blueUnblocked = false;
    public bool m_purpleUnblocked = false;

    public GameData()
    {
        m_hasNeverBeenRun = false;
        m_hasNeverHadTutorial = true;
        m_highScore = 0;
        m_primaryColor = "white";
        m_secondaryColor = "black";
        m_skin = "customLight";
    }

    public GameData(bool hasNeverBeenRun, bool hasNeverHadTutorial, int highScore, string primaryColor, string secondaryColor, string skin)
    {
        m_hasNeverBeenRun = hasNeverBeenRun;
        m_hasNeverHadTutorial = hasNeverHadTutorial;
        m_highScore = highScore;
        m_primaryColor = primaryColor;
        m_secondaryColor = secondaryColor;
        m_skin = skin;
    }
}
