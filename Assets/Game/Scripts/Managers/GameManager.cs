using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isResetData;
    public bool isSwapPlayerHole;
    public Text mapID;


    [Header("Status")]
    public int levelGame;
    public int levelFixed;
    public bool isComplete;
    private bool isChangeColor;
    private bool isSlowMotion;


    [Header("Level Controller")]
    public int count;
    public int maxCount;
    public bool isMoving;
    public bool isScored;
    public List<Ball> ballList;
    public Ball player;
    public List<Brick> brickList = new List<Brick>();
    private List<GameObject> obtacleList = new List<GameObject>();
    public List<GameObject> blankList = new List<GameObject>();
    public Texture2D[] textures;
    private Vector4 Yellow = new Vector4(255, 255, 0, 255);
    private Vector4 Red = new Vector4(255, 0, 0, 255);
    private Vector4 Black = new Vector4(0, 0, 0, 255);
    private Vector4 White = new Vector4(255, 255, 255, 255);
    private Vector4 Green = new Vector4(0, 255, 0, 255);
    public bool isBlack;
    public GameObject emoji;
    public Animator lineAnimator;
    public GameObject lineParent;
    public GameObject holeObject;
    public float maxHeightSandWich;
    public Transform parentRedBall;
    public Transform parentYellowBall;
    public Disk disk;
    public Transform left, right;
    public Transform parentWhite;

    [Header("Score Controller")]
    public int totalMoney;
    public int combo;
    public int swipeAmount;
    public int point;
    public List<int> scores = new List<int>();

    [Header("Camera Controller")]
    public GameObject pivotCamera;
    private Vector3 camPos;
    private float cameraMinX;
    private float cameraMaxX;
    private float cameraMinY;
    private float cameraMaxY;
    public float _horizontal;
    public float _pivotHorizontal;

    [Header("Effects")]
    public ParticleSystem[] fireWork;

    [Header("UI")]
    public Animator tutorialAnimator;
    public GameObject starControl;
    public Text currentLevelText;
    public Text nextLevelText;
    public Text swipeAmountText;
    public Text score;
    public Text comboText;
    public Animator comboAnimator;
    public Animator scoreAnimatior;
    public GameObject combo01;
    public Text combo01Text;
    public string[] congratulations;
    public Image levelFill;
    public GameObject eatTextObject;

    public delegate void DisableCollider();
    public static event TouchSwipe disableCollider;

    public delegate void EnableCollider();
    public static event TouchSwipe enableCollider;

    public delegate void TouchSwipe();
    public static event TouchSwipe touchSwipe;

    public Vector3 directionSwipe;

    [Header("Star Manager")]
    public int currentMove;
    public int maxMove;
    public Text currentMoveText;
    public Text maxMoveText;

    public Material brickMaterial;
    public Material obtacleMaterial;

    [Header("Tutorial")]

    public List<TutorialStep> tutorialStep = new List<TutorialStep>();

    [System.Serializable]
    public class TutorialStep
    {
        public Vector3[] directionArray;
    }

    private void Awake()
    {
        if (isResetData) PlayerPrefs.DeleteAll();

        if(PlayerPrefs.GetInt("first") == 0)
        {
            PlayerPrefs.SetInt("first", 1);
            PlayerPrefs.SetInt("model", 8);
        }

        Application.targetFrameRate = 60;
        MMVibrationManager.iOSInitializeHaptics();

        instance = this;

        ReadMovesTextAsset();

        for (int i = 0; i < fireWork.Length; i++)
        {
            fireWork[i].Stop();
        }
    }

    private void Start()
    {
        totalMoney = PlayerPrefs.GetInt("TotalMoney", 0);
        levelGame = PlayerPrefs.GetInt("levelGame", 0);
        currentLevelText.text = "" + (levelGame + 1);
        nextLevelText.text = "" + (levelGame + 2);
        levelFixed = levelGame;
        if (levelFixed >= textures.Length)
        {
            levelFixed = Random.Range(0, textures.Length);
        }

        //GenerateLevel();
        StartCoroutine(C_Score());
        GenerateLevel();
    }



    private void LevelUp()
    {

        var random = Random.value;
        if (random < 0.65)
        {
            random = 0.65f;
        }

        if (swipeAmount != 0)
            totalMoney += (((int)(point * maxCount * 1.5 * random)) / swipeAmount);

        levelGame++;

        PlayerPrefs.SetInt("levelGame", levelGame);

        levelFixed = levelGame;

        if (levelFixed >= textures.Length)
        {
            //levelFixed = Random.Range(0, textures.Length);
            levelFixed = levelGame % textures.Length;
        }

        isChangeColor = true;
    }

    public int _d;
    public int _maxD;

    public void ActiveColliderBrick()
    {
        _d++;
        if (_d >= _maxD)
        {
            enableCollider();
            _d = 0;
            _maxD = 0;
        }
    }

    private bool isToturial;
    private int stepIndex;

    private void CheckToturial()
    {
        if(levelGame < tutorialStep.Count)
        {
            stepIndex = 0;
            tutorialAnimator.gameObject.SetActive(true);
            tutorialAnimator.SetTrigger(NameToturialAnimation(tutorialStep[levelGame].directionArray[stepIndex]));
            isToturial = true;
        }
        else
        {
            tutorialAnimator.gameObject.SetActive(false);
            isToturial = false;
        }
    }

    private string NameToturialAnimation(Vector3 direction)
    {
        if (direction == Vector3.forward)
        {
            return "up";
        }
        else if (direction == Vector3.back)
        {
            return "down";
        }
        else if (direction == Vector3.right)
        {
            return "right";
        }
        else
        {
            return "left";
        }
    }

    public void Swipe(Vector3 direction)
    {
        if (isComplete) return;

        if (_d != 0) return;

        if (isToturial)
        {
            if (direction == tutorialStep[levelGame].directionArray[stepIndex])
            {
                stepIndex++;

                if (stepIndex >= tutorialStep[levelGame].directionArray.Length)
                {
                    isToturial = false;
                    tutorialAnimator.gameObject.SetActive(false);
                }
                else
                {
                    tutorialAnimator.SetTrigger(NameToturialAnimation(tutorialStep[levelGame].directionArray[stepIndex]));
                }
            }
            else
            {
                return;
            }
        }

        swipeAmount++;

        if (_d != _maxD) return;

        if (isComplete || isMoving)
        {
            return;
        }

        isMoving = true;

        SetCurrentMove();
        directionSwipe = direction;
        disableCollider();
        touchSwipe();
    }

    private bool CheckBallMoving()
    {
        for (int i = 0; i < ballList.Count; i++)
        {
            if (ballList[i].isMoving)
            {
                return true;
            }
        }
        return false;
    }

    private int _curCombo;
    private float _timeCombo;

    private void Update()
    {
        //if(_timeCombo > 0)
        //{
        //    _timeCombo -= Time.deltaTime;

        //    if(_timeCombo <= 0)
        //    {
        //        combo01.SetActive(true);
        //        _timeCombo = 0;
        //        combo01Text.text = "Combo x" + _curCombo;
        //        _curCombo = 0;
        //    }
        //}

        if (!CheckBallMoving())
        {
            isMoving = false;
            combo = 0;
        }
    }

    public void SetCurrentMove()
    {
        currentMove++;
        currentMoveText.text = currentMove.ToString();
    }

    private IEnumerator EnableMoving()
    {
        yield return new WaitForSeconds(0.25f);
        isMoving = false;
        combo = 0;

    }
    #region Map
    private void ClearMap()
    {
        currentMove = 0;
        currentMoveText.text = currentMove.ToString();

        maxMove = MaxStar();
        maxMoveText.text = "/" + maxMove.ToString();

      //  lineAnimator.SetTrigger("Idle");

        for(int i = 0; i < ballList.Count; i++)
        {
            if (ballList[i].ballType == Ball.BallType.red)
            {
                ballList[i].transform.SetParent(parentRedBall);
            }
            else
            {
                ballList[i].transform.SetParent(parentYellowBall);
            }
        }

        Vector3 _posCamMain = Camera.main.transform.localPosition;
        _posCamMain.y = 16;
        _posCamMain.z = -7f;
        Camera.main.transform.localPosition = _posCamMain;

        pivotCamera.transform.eulerAngles = Vector3.zero;
        _d = _maxD = 0;
        levelFill.fillAmount = 0;
        blankObjects.Clear();
        ballList.Clear();
        brickList.Clear();
        obtacleList.Clear();
        blankList.Clear();
        swipeAmount = 0;
        swipeAmountText.text = swipeAmount.ToString();
        isComplete = false;
        count = maxCount = point = 0;
        for (int i = 0; i < fireWork.Length; i++)
        {
            fireWork[i].Stop();
        }
        maxHeightSandWich = 0.8f;
        Time.timeScale = 1;
        isComplete = false;
        parentWhite.position = Vector3.zero;
        PoolManager.instance.RefreshItem(PoolManager.NameObject.brick);
        PoolManager.instance.RefreshItem(PoolManager.NameObject.tile);
        PoolManager.instance.RefreshItem(PoolManager.NameObject.redball);
        PoolManager.instance.RefreshItem(PoolManager.NameObject.yellowball);
        PoolManager.instance.RefreshItem(PoolManager.NameObject.hole);
        PoolManager.instance.RefreshItem(PoolManager.NameObject.obstacle);
        PoolManager.instance.RefreshItem(PoolManager.NameObject.blank);
        PoolManager.instance.RefreshItem(PoolManager.NameObject.white);
    }

    public void GenerateLevel()
    {
        ClearMap();

        if (isChangeColor)
        {
            isChangeColor = false;
            ColorManager.instance.ChangeColor();
        }

        levelFixed = levelGame % textures.Length;

        UIManager.instance.Show_InGameUI();
        //currentLevelText.text = "" + (levelGame + 1);
        //nextLevelText.text = "" + (levelGame + 2);
        currentLevelText.text = "" + (levelFixed + 1);
        nextLevelText.text = "" + (levelFixed + 2);
        GenerateMap(textures[levelFixed]);
        CheckToturial();
    }

    public void ResetLevel()
    {
        levelGame = 0;
        PlayerPrefs.SetInt("levelGame", levelGame);
        GenerateLevel();

    }

    private void GenerateMap(Texture2D texture)
    {
        mapID.text = texture.name;
        if (texture.height % 2 == 0)
        {
            for (int x = 0; x < texture.width; x++)
            {
                isBlack = !isBlack;
                for (int y = 0; y < texture.height; y++)
                {
                    GenerateTile(texture, x, y);
                    isBlack = !isBlack;
                }
            }

        }
        else if (texture.height % 2 != 0)
        {

            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    GenerateTile(texture, x, y);
                    isBlack = !isBlack;
                }
            }
        }

        SetCamera(texture);
        SetModelBall();

        for (int y = -10; y <= texture.height + 30; y++)
        {
            for (int x = -10; x <= texture.width + 10; x++)
            {
                bool _x = false;
                bool _y = false;

                if (x > 0 && x < texture.width - 1)
                {
                    _x = true;
                }

                if (y > 0 && y < texture.height - 1)
                {
                    _y = true;
                }

                GameObject _obj = PoolManager.instance.GetObject(PoolManager.NameObject.white);
                _obj.transform.position = new Vector3(x, 0, y);

                if (_x && _y)
                {
                    _obj.SetActive(true);
                    blankList.Add(_obj);
                }
                else
                {
                    _obj.SetActive(true);
                }

                if (y % 2 == 0)
                {
                    if (x % 2 == 0)
                    {
                        _obj.GetComponent<Renderer>().material = ColorManager.instance.plane1_material;
                    }
                    else
                    {
                        _obj.GetComponent<Renderer>().material = ColorManager.instance.plane2_material;
                    }
                }
                else
                {
                    if (x % 2 == 0)
                    {
                        _obj.GetComponent<Renderer>().material = ColorManager.instance.plane_material;
                    }
                    else
                    {
                        _obj.GetComponent<Renderer>().material = ColorManager.instance.plane1_material;
                    }
                }
            }
        }

        for(int i = 0; i < blankList.Count; i++)
        {
            blankList[i].gameObject.SetActive(false);
        }
    }


    public IEnumerator ResetTimeScale()
    {
        yield return new WaitForSeconds(0.25f);
        Time.timeScale = 1f;
    }
    private void GenerateTile(Texture2D texture, int x, int y)
    {
        Color32 pixelColor = texture.GetPixel(x, y);
        Vector4 color32 = new Vector4(pixelColor.r, pixelColor.g, pixelColor.b, pixelColor.a);
        Vector3 pos = new Vector3(x, 0, y);

        if (color32 == Red)
        {
            if (isSwapPlayerHole)
            {
                GenerateDropOut(pos);
            }
            else
            {
                GenerateRedBall(pos);
                GenerateBrick(pos);
            }
        }
        else if (color32 == Yellow)
        {
            GenerateYellowBall(pos);
            GenerateBrick(pos);
        }
        else if (color32 == White)
        {
            GenerateTile(pos);
        }
        else if (color32 == Black)
        {
            GenerateObstacle(pos);
        }

        else if (color32 == Green)
        {
            if (isSwapPlayerHole)
            {
                GenerateRedBall(pos);
                GenerateBrick(pos);
            }
            else
            {
                GenerateDropOut(pos);
            }
        }
    }

    private void SetModelBall()
    {
        int _m = PlayerPrefs.GetInt("model");
        int[] _array = SimpleMathf.RandomIntArray(_m + 1);
        List<int> _list = new List<int>();

        for (int i = 0; i < _array.Length; i++)
        {
            _list.Add(_array[i]);
          
            if(i == _array.Length -1)
            {
                i = 0;
                _array = SimpleMathf.RandomIntArray(_m);
            }

            if (_list.Count > ballList.Count)
            {
                i = _array.Length;
            }
        }

        for(int i = 0; i < ballList.Count; i++)
        {
            ballList[i].SetModel(_list[i]);
        }

    }

    private void GenerateYellowBall(Vector3 _position)
    {
        GameObject yellowBallObject = PoolManager.instance.GetObject(PoolManager.NameObject.yellowball);
        var ball = yellowBallObject.GetComponent<Ball>();
        _position.y = 0.6f;
        ball.Init(_position);
        ballList.Add(ball);
        maxCount++;
        yellowBallObject.SetActive(true);
    }

    private void GenerateRedBall(Vector3 _position)
    {
        GameObject redBallObject = PoolManager.instance.GetObject(PoolManager.NameObject.redball);
        player = redBallObject.GetComponent<Ball>();
        _position.y = 0.6f;
        player.Init(_position);
        ballList.Add(player);
        maxCount++;
        redBallObject.SetActive(true);
    }

    private void GenerateDropOut(Vector3 _position)
    {
        holeObject = PoolManager.instance.GetObject(PoolManager.NameObject.hole);
        _position.y = 0.0f;
        holeObject.transform.position = _position;
        //    holeObject.layer = 8;
        holeObject.GetComponent<Hole>().SetBrickColor(isBlack);

        holeObject.tag = "Hole";
        holeObject.GetComponent<Renderer>().enabled = true;
        holeObject.SetActive(true);
        disk = holeObject.transform.GetChild(0).gameObject.GetComponent<Disk>();
        disk.gameObject.SetActive(true);
    }
    private void GenerateTile(Vector3 _position)
    {
        GameObject tileObject = PoolManager.instance.GetObject(PoolManager.NameObject.tile);
        _position.y = tileObject.transform.position.y;
        tileObject.transform.position = _position;
        tileObject.SetActive(true);
    }

    private void GenerateObstacle(Vector3 _position)
    {
        GameObject obstacleObject = PoolManager.instance.GetObject(PoolManager.NameObject.obstacle);
        _position.y = 0f;
        obstacleObject.transform.localScale = new Vector3(1, 1.5f, 1);
        obstacleObject.transform.position = _position;
        obstacleObject.SetActive(true);
        obstacleObject.GetComponent<Renderer>().material = obtacleMaterial;
        obtacleList.Add(obstacleObject);
    }

    private void GenerateBrick(Vector3 _position)
    {
        GameObject brickObject = PoolManager.instance.GetObject(PoolManager.NameObject.brick);
        _position.y = 0.0f;
        brickObject.transform.position = _position;
        var brick = brickObject.GetComponent<Brick>();
        brick.SetBrickColor(isBlack);
        // brickObject.layer = 10;
        brickList.Add(brickObject.GetComponent<Brick>());
        brickObject.SetActive(true);
    }
    #endregion

    public void SetPoint(int ballPoint)
    {
        point += ballPoint;
        swipeAmountText.text = point.ToString();
    }

    public void UpdateLevelBar ()
    {
        levelFill.fillAmount = ((float)count / (float)maxCount);
    }

    public void CheckComplete()
    {
        if (count >= maxCount)
        {
            // SlowMotion();
            Complete();
        }
    }

    int index;
    public void ChangeModel()
    {
        index++;

        if (index >= 3) index = 0;

        for(int i = 0; i < ballList.Count; i++)
        {
            ballList[i].ChangeModel(index);
        }
    }

    private void ShowBlank()
    {
        for(int i = 0; i < brickList.Count; i++)
        {
            brickList[i].gameObject.SetActive(false);
        }

        for(int i = 0; i < obtacleList.Count; i++)
        {
            obtacleList[i].gameObject.SetActive(false);
        }

        for(int i = 0; i < blankList.Count; i++)
        {
            blankList[i].gameObject.SetActive(true);
        }
    }

    public void Complete()
    {
        if (isComplete) return;

        isComplete = true;
        StartCoroutine(C_Complete());
    }

    private IEnumerator C_Complete()
    {
        yield return new WaitForSeconds(1.0f);

        holeObject.GetComponent<Hole>().SetAnimationDisk(false);
        disk.Explosion();
        LevelUp();

        ColorManager.instance.ChangeColorBrick();


        yield return C_CameraSnap();
        ShowBlank();


        //  PoolManager.instance.RefreshItem(PoolManager.NameObject.brick);
        // PoolManager.instance.RefreshItem(PoolManager.NameObject.tile);
        // PoolManager.instance.RefreshItem(PoolManager.NameObject.obstacle);
        yield return C_EatSandWick();
        yield return C_UpgradeModel();

        // player.Jump(true);

        yield return new WaitForSeconds(0.25f);

        Vector3 _eulerAngle = lineParent.transform.localEulerAngles;

        int r = Random.Range(0, 4);

        if(r == 0)
        {
            _eulerAngle.y = 45;
        }
        else if ( r== 1)
        {
            _eulerAngle.y = 45 * 3;
        }
        else if (r == 2)
        {
            _eulerAngle.y = 45 * 5;
        }
        else
        {
            _eulerAngle.y = 45 * 7;
        }

   //     lineAnimator.SetTrigger("Move");

        for (int i = 0; i < fireWork.Length; i++)
        {
            fireWork[i].Play();
        }
        score.text = totalMoney.ToString();

        Debug.Log("Complete");


        yield return new WaitForSeconds(1.25f);

        // UnityAdsManager.Instance.UpdateCompleteToShowVideoAds();

        UIManager.instance.Show_CompleteUI();
      //  starControl.SetActive(true);
    }

    private IEnumerator C_CameraSnap()
    {
        float t = 0.0f;
        Vector3 _fromPosition = pivotCamera.transform.position;
        Vector3 _toPosition = holeObject.transform.position + new Vector3(0,0,4);
        _toPosition.y = maxHeightSandWich / 2.0f;

        Vector3 _fromAngle = Vector3.zero;
        Vector3 _toAngle = new Vector3(-35.0f, 0.0f, 0.0f);

        Vector3 _posCamMain = Camera.main.transform.localPosition;
        Vector3 _tarCamMain = _posCamMain;
        _tarCamMain.y = 10;
        _tarCamMain.z = -6.5f;



        for (int i = 0; i < brickList.Count;i++)
        {
         //   brickList[i].ChangeMaterial();
        }

        while(t < 1.0f)
        {
            t += Time.deltaTime * 2.0f;

            pivotCamera.transform.position = Vector3.Lerp(_fromPosition, _toPosition, t);
            pivotCamera.transform.eulerAngles = Vector3.Lerp(_fromAngle, _toAngle, t);
            Camera.main.transform.localPosition = Vector3.Lerp(_posCamMain, _tarCamMain, t);
            parentWhite.position = Vector3.Lerp(Vector3.zero, Vector3.up * -0.25f, t);

            for (int i = 0; i < obtacleList.Count; i++)
            {
                obtacleList[i].transform.localScale = Vector3.Lerp(new Vector3(1, 1.5f, 1), Vector3.one, t);
            }

            yield return null;
        }
    }


    private IEnumerator C_UpgradeModel()
    {
        if (PlayerPrefs.GetInt("model") >= 17) yield break;

        if (levelGame > 0 && levelGame % 5 == 0)
        {
            yield return new WaitForSeconds(1.25f);

            Debug.Log("NEW ELEMENT");
            int m = PlayerPrefs.GetInt("model");
            m++;
            PlayerPrefs.SetInt("model", m);
            UIManager.instance.newUnlockUI.SetActive(true);
            yield return new WaitForSeconds(1.2f);
        }
    }

    private IEnumerator C_EatSandWick()
    {
        yield return new WaitForSeconds(0.25f);

        eatTextObject.SetActive(true);

        Animator diskAnimator = disk.GetComponent<Animator>();
        Animator elementAnimator = disk.transform.GetChild(0).GetComponent<Animator>();

        bool isWait = true;
        int n = 0;

        while (isWait)
        {
            if (Input.GetMouseButtonDown(0))
            {
                iTween.ShakePosition(Camera.main.gameObject, Vector3.one * 0.1f, 0.2f);
                disk.DisableAnimator();
                elementAnimator.SetTrigger("Scale");
                MMVibrationManager.iOSTriggerHaptics(HapticTypes.MediumImpact);
                EatAnim(n);

                for(int i = 0; i < ballList.Count; i++)
                {
                    ballList[i].EatElement(n);
                }
                disk.EatCheese(n);

                n++;
            }

            if(n >= 3)
            {
                isWait = false;
            }

            yield return null;
        }

        disk.Hide();
    //    holeObject.GetComponent<Renderer>().enabled = false;
        EatCompleteEffect(holeObject.transform.position);
        eatTextObject.SetActive(false);
    }

    public void Fail()
    {
        if (isComplete) return;

        isComplete = true;
        StartCoroutine(C_Fail());
    }

    private IEnumerator C_Fail()
    {
        Debug.Log("Fail");

        yield return new WaitForSeconds(1.25f);

        // UnityAdsManager.Instance.UpdateDeadToShowVideoAds();
        UIManager.instance.Show_FailUI();
    }

    public void RedExplosion(Vector3 _pos)
    {
        return;

        GameObject obj = PoolManager.instance.GetObject(PoolManager.NameObject.redExplosion);
        obj.transform.position = _pos;
        obj.SetActive(true);
    }

    public void YellowExplosion(Vector3 _pos)
    {
        return;

        GameObject obj = PoolManager.instance.GetObject(PoolManager.NameObject.yellowExplosion);
        obj.transform.position = _pos;
        obj.SetActive(true);
    }

    private bool isVibrating;

    public void Vibration()
    {
        if (isVibrating) return;

        StartCoroutine(C_Vibration());
    }

    private IEnumerator C_Vibration()
    {
        // Debug.Log("vibration");

        MMVibrationManager.iOSTriggerHaptics(HapticTypes.LightImpact);

        isVibrating = true;

        yield return new WaitForSeconds(0.1f);

        isVibrating = false;
    }


    public void InHoleEffect(Vector3 _pos)
    {
        Vibration();

        GameObject _obj = PoolManager.instance.GetObject(PoolManager.NameObject.inHoleEffect);
        _obj.transform.position = _pos;
        _obj.SetActive(true);
    }

    public void DisplayCombo(Vector3 _pos)
    {
        _curCombo = combo;

        if (combo % 2 == 2 || combo < 2 || isComboAnimation)
        {
            return;
        }

        _timeCombo = 0.5f;

        comboAnimator.GetComponent<Text>().text = congratulations[Random.Range(0, congratulations.Length)];
        comboAnimator.SetTrigger("ShowUp");
        StartCoroutine(C_DelayCombo());
        //GameObject _obj = PoolManager.instance.GetObject(PoolManager.NameObject.combo);
        //_obj.transform.position = _pos;
        //_obj.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Combo x" + combo;
        //_obj.SetActive(true);
        //if (combo >= 5)
        //{
        //    comboText.SetText("Perfect");
        //    comboText.fontSize += Time.deltaTime * 10f;
        //}
        //else
        //    comboText.SetText("Combo x" + combo);

    }

   

    bool isComboAnimation;

    private IEnumerator C_DelayCombo()
    {
        isComboAnimation = true;
        yield return new WaitForSeconds(1.25f);
        isComboAnimation = false;
    }

    public IEnumerator HideCombo()
    {
        yield return new WaitForSeconds(0.25f);
        comboText.text = "";
    }

    public List<GameObject> blankObjects;
    public Vector3[][] blankArray;
   
    private void SetCamera(Texture2D texture)
    {
        float _posX = (texture.width-1)/ 2.0f;
        float _posY = (texture.height-1) / 2.0f;
        pivotCamera.transform.position = new Vector3(_posX, 0.0f, _posY);

        _horizontal = texture.width - 2;
        _pivotHorizontal = _horizontal / 2.0f;
        CameraControl.instance.SetFOVSizeMap();
    }

    private IEnumerator C_Score()
    {
        bool isLoop = true;

        while (isLoop)
        {
            if (scores.Count > 0)
            {
                //if (isComplete)
                //{
                //    scores.Clear();
                //}
                //else
                //{
                scoreAnimatior.SetTrigger("Awake");

                GameObject _score = PoolManager.instance.GetObject(PoolManager.NameObject.scoreup);
                _score.transform.GetChild(0).GetComponent<Text>().text = "+" + scores[0].ToString();
                _score.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-150,150), Random.Range(-100, 100));
                scores.RemoveAt(0);
                _score.SetActive(true);
                //}
                yield return new WaitForSeconds(0.1f);
            }

            yield return null;
        }
    }

    public void EffectInHole3(Vector3 _position)
    {
        GameObject _obj = PoolManager.instance.GetObject(PoolManager.NameObject.inHoleEffect3);
        _obj.transform.position = _position;
        _obj.SetActive(true);
    }

    private int MaxStar()
    {
        return 0;


        if(levelFixed <= movesArray.Length)
        {
            return movesArray[levelFixed] + 2;
        }
        else
        {
            return Random.Range(10,30) + 2;
        }
    }

    public void SlowMotion()
    {
        if (isSlowMotion) return;

        StartCoroutine(C_SLowMotion());
    }

    private IEnumerator C_SLowMotion()
    {
        isSlowMotion = true;

        float _n = 1.0f;

        while(_n > 0.2f)
        {
            _n -= Time.unscaledDeltaTime * 2.0f;
            Time.timeScale = _n;

            yield return null;
        }

        yield return new WaitForSeconds(0.15f);

        while (_n < 1.0f)
        {
            _n += Time.unscaledDeltaTime * 2.0f;
            Time.timeScale = _n;
            yield return null;  
        }

        isSlowMotion = false;
    }

    public TextAsset movesTextAsset;
    public int[] movesArray;

    private void ReadMovesTextAsset()
    {
        return;

        string _text = movesTextAsset.text;
        string[] _text2 = _text.Split('\n');
        movesArray = new int[_text2.Length];

        for (int i = 0; i < _text2.Length; i++)
        {
            string _a = _text2[i];
            string[] _b = _a.Split('.');

            if(_b.Length >= 1)
              _text2[i] = _b[1];

            movesArray[i] = int.Parse(_text2[i]);
        }
    }

    public void EatAnim(int n)
    {
        StartCoroutine(C_EatAnim(n));
    }


    private IEnumerator C_EatAnim(int n)
    {
        GameObject _obj = PoolManager.instance.GetObject(PoolManager.NameObject.eat);

        if(n == 0)
        {
            _obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300, 0);
            _obj.transform.GetChild(0).gameObject.GetComponent<Text>().text = "YUMMY!";
            _obj.GetComponent<EatAnim>().SetYummy(n);
            Vector3 _eulerAngle = _obj.transform.eulerAngles;
            _eulerAngle.z = 15;
            _obj.transform.eulerAngles = _eulerAngle;
        }
        else if (n ==1)
        {
            _obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(300, 150);
            _obj.transform.GetChild(0).gameObject.GetComponent<Text>().text = "YUMMY!!";
            _obj.GetComponent<EatAnim>().SetYummy(n);
            Vector3 _eulerAngle = _obj.transform.eulerAngles;
            _eulerAngle.z = -15;
            _obj.transform.eulerAngles = _eulerAngle;
        }
        else
        {
            _obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 300);
            _obj.transform.GetChild(0).gameObject.GetComponent<Text>().text = "YUMMY!!!";
            _obj.GetComponent<EatAnim>().SetYummy(n);
            Vector3 _eulerAngle = _obj.transform.eulerAngles;
            _eulerAngle.z = 15;
            _obj.transform.eulerAngles = _eulerAngle;
        }


        _obj.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        _obj.SetActive(false);
    }

    private void EatCompleteEffect(Vector3 _pos)
    {
        StartCoroutine(C_EatCompleteEffect(_pos));
    }

    private IEnumerator C_EatCompleteEffect(Vector3 _pos)
    {
        GameObject _obj = PoolManager.instance.GetObject(PoolManager.NameObject.eatCompleted);
        _obj.transform.position = _pos;

        _obj.SetActive(true);
        yield return new WaitForSeconds(3f);
        _obj.SetActive(false);
    }
}
