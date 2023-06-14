using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Spectator spectator;
    public GameObject[] models;

    private bool isHide;
    public Rigidbody rigidbody;
    public Collider collider;

    public LayerMask layer;
    public LayerMask layerBrick;

    public BallType ballType;

    public GameObject[] elementModel;

    public enum BallType
    {
        red,
        yellow
    }

    public Color paintColor;
    public bool isMoving;

    public Collider colliderBrick;

    public bool isRemoveEvent;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        StopAllCoroutines();

        isRemoveEvent = false;

        GameManager.enableCollider += ActiveColliderBrick;
        GameManager.disableCollider += DisableColliderBrick;
        GameManager.touchSwipe += Move;
    }

    private void OnDisable()
    {
        if(isRemoveEvent == false)
        {
            isRemoveEvent = true;
            GameManager.enableCollider -= ActiveColliderBrick;
            GameManager.disableCollider -= DisableColliderBrick;
            GameManager.touchSwipe -= Move;
        }
    }

    private void LateUpdate()
    {
        if(ballType == BallType.red)
        {
            GameManager.instance.emoji.transform.position = transform.position;
        }
    }

    public void Init(Vector3 _position)
    {
        if (ballType == BallType.red)
        {
            elementModel[0].SetActive(true);
            elementModel[1].SetActive(true);
            elementModel[2].SetActive(true);
        }
        else
        {
         
        }


        isCompleted = false;
        isHide = false;
        colliderBrick = null;
        transform.eulerAngles = Vector3.zero;
        rigidbody.useGravity = false;
        rigidbody.velocity = Vector3.zero;
        collider.isTrigger = true;

        collider.isTrigger = true;
        collider.enabled = true;
    
        transform.position = _position;
    }

    public void Move()
    {
        if (gameObject.activeSelf == false || isHide) return;

        StartCoroutine(C_Move(GameManager.instance.directionSwipe));
    }

    public Collider[] colliders;

    private IEnumerator C_Move(Vector3 direction)
    {
        GameManager.instance._maxD++;

        yield return null;

        isMoving = true;

        Ray ray = new Ray(transform.position, direction);
        RaycastHit[] hits = Physics.RaycastAll(ray, 20.0f, layer);
        colliders = new Collider[hits.Length];
        var multiHitInfo = hits;
        System.Array.Sort(multiHitInfo, (x, y) => x.distance.CompareTo(y.distance));
        int n = 0;
        bool isContactHole = false;

        foreach (var hit in multiHitInfo)
        {
            if (hit.collider.CompareTag("Brick") || hit.collider.CompareTag("Hole"))
            {
                colliders[n] = hit.collider;

                //  n++;

                if (hit.collider.CompareTag("Hole"))
                {
                    isContactHole = true;

                    Vector3 _a = transform.position;
                    Vector3 _b = hit.collider.gameObject.transform.position;
                    _a.y = _b.y = 0.0f;
                    float _d = Vector3.Distance(_a, _b);
                    n = (int)_d - 1;

                    break;
                }
                else
                {
                    n++;
                }
            }
            else if (hit.collider.CompareTag("Tile"))
            {
                break;
            }
        }

        if (n > 0)
        {

            float t = 0.0f;

            Vector3 fromPosition = transform.position;
            Vector3 toPosition = fromPosition + direction * n;

            while (t < 1)
            {
                t += (0.4f) / n;
                transform.position = Vector3.Lerp(fromPosition, toPosition, t);

                yield return null;
            }

            transform.position = toPosition;
        }

        if (isContactHole)
        {
            //    GameManager.instance.disk.SetAnimPlate();
            GameManager.instance.count++;

            if (ballType == BallType.red)
            {
                if (GameManager.instance.count < GameManager.instance.maxCount)
                {
                    GameManager.instance.Fail();
                }
                else
                {
                    GameManager.instance.CheckComplete();
                }
            }
            else
            {

            }

            Vector3 _startPos = transform.position;
            Vector3 _targetPos = GameManager.instance.holeObject.transform.position;
            _targetPos.y = GameManager.instance.maxHeightSandWich;

            GameManager.instance.maxHeightSandWich += transform.localScale.y;

            Vector3 _targetAngle = AngleFixed(direction) * 179.0f + Vector3.up * Random.Range(-3, 4);

            if (ballType == BallType.red) _targetAngle *= 2.0f;

            float force = 3.5f;

            if (ballType == BallType.red)
            {
                GameManager.instance.SlowMotion();
                force = 3.5f * 1.25f;
                transform.DORotate(_targetAngle, 0.5f).SetRelative();
                yield return JumpCoroutine(_startPos, _targetPos, force, 2.0f,2);
            }
            else
            {
                transform.DORotate(_targetAngle, 0.25f).SetRelative();
                yield return JumpCoroutine(_startPos, _targetPos, force, 2.0f,4);
            }


            //   StartCoroutine(C_Rotate(_targetAngle));




            Hide();
            GameManager.instance.UpdateLevelBar();

     

            yield break;
        }

        if (isMoving == false) yield break;

        isMoving = false;

        GameManager.instance.ActiveColliderBrick();
    }

    private IEnumerator C_Rotate(Vector3 targetAngle)
    {
        float t = 0.0f;
        while(t < 1.0f)
        {
            t += Time.deltaTime * 5.0f;

            transform.eulerAngles = Vector3.Lerp(Vector3.zero, targetAngle, t);

            yield return null;
        }
    }

    public void ActiveColliderBrick()
    {
        if (colliderBrick != null)
        {
            colliderBrick.enabled = true;

            if (isHide)
            {
                isHide = false;

                if (isRemoveEvent == false)
                {
                    isRemoveEvent = true;
                    GameManager.enableCollider -= ActiveColliderBrick;
                    GameManager.disableCollider -= DisableColliderBrick;
                    GameManager.touchSwipe -= Move;
                }
            }
        }
    }

    public void DisableColliderBrick()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up * 10, Vector3.down);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerBrick))
        {
            if (hit.collider != null)
            {
                colliderBrick = hit.collider.gameObject.GetComponent<Collider>();
                colliderBrick.enabled = false;
            }
        }
    }

    private bool isCompleted;

    public void Completed()
    {
        isCompleted = true;

        GameManager.enableCollider -= ActiveColliderBrick;
        GameManager.disableCollider -= DisableColliderBrick;
        GameManager.touchSwipe -= Move;
    }

    public void Hide()
    {
        int currentPoint = (int)Mathf.Pow(2f, GameManager.instance.combo);

        GameManager.instance.InHoleEffect(transform.position + Vector3.up * 1.0f);
        GameManager.instance.scores.Add(currentPoint);

        GameManager.instance.SetPoint(currentPoint);
        GameManager.instance.combo++;
        GameManager.instance.DisplayCombo(transform.position + Vector3.up * 4.0f);

        isHide = true;
        isMoving = false;

        // StartCoroutine(C_ActivePhysic());
        GameManager.instance.ActiveColliderBrick();
        transform.SetParent(GameManager.instance.holeObject.transform.GetChild(0).GetChild(0).GetChild(0));
    }

    private IEnumerator C_ActivePhysic()
    {
        yield return new WaitForSeconds(1.0f);
        collider.isTrigger = false;
        rigidbody.useGravity = true;
    }

    private IEnumerator C_ActiveEmoji() 
    {
        GameManager.instance.emoji.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        GameManager.instance.emoji.SetActive(false);
    }

    public void ChangeModel(int index)
    {       //    if (ballType == BallType.red) return;

        for(int i = 0; i < models.Length; i++)
        {    
            models[i].SetActive(false);
        }

        models[index].SetActive(true);
    }

    private bool isJump;
    private float JumpProgress;

    private IEnumerator JumpCoroutine(Vector3 startPosition ,Vector3 destination, float maxHeight, float time,float timefixed)
    {
        JumpProgress = 0.0f;

        isJump = true;
        var startPos = startPosition;


        float distance_xyz = Vector3.Distance(startPos, destination);

        float he = distance_xyz / 5;
        maxHeight *= he;

        float ti = distance_xyz / 8;
        time *= ti;

        while (JumpProgress <= 1.0)
        {
            JumpProgress += Time.deltaTime * timefixed;
            var height = Mathf.Sin(Mathf.PI * JumpProgress) * maxHeight;
            if (height < 0f)
            {
                height = 0f;
            }

            if(ballType == BallType.red)
            {
                if (JumpProgress >= 0.6f)
                {
                    // GameManager.instance.SlowMotion();
                }
            }
        
            transform.position = Vector3.Lerp(startPos, destination, JumpProgress) + Vector3.up * height;
            yield return null;
        }

        transform.position = destination;
        isJump = false;
    }

    private Vector3 AngleFixed(Vector3 _direction)
    {
        if (_direction == Vector3.left)
        {
            return Vector3.forward;
        }
        else if (_direction == Vector3.right)
        {
            return Vector3.back;
        }
        else if (_direction == Vector3.forward)
        {
            return Vector3.right;
        }
        else
        {
            return Vector3.left;
        }
    }

    public void Jump()
    {
        Vector3 _pos = transform.position;
        float _force = transform.position.y / 2.0f;
        float _duration = 0.25f + transform.position.y / 10.0f;

        transform.DOJump(_pos, _force, 1, _duration);
    }

    public void EatElement(int index)
    {
        Vector3 a = transform.localEulerAngles;

        if(index == 0)
        {
            a.y = 0;
        }
        else
        {
            a.y = 0;    
        }
 
        a.x = a.z = 0;
        transform.localEulerAngles = a;

        elementModel[index].SetActive(false);
    }

    public void SetModel(int index)
    {
        if (ballType == BallType.red) return;

        if(index >= models.Length)
        {
            index = Random.Range(0, models.Length);
        }

        for(int i = 0; i < models.Length; i++)
        {
            models[i].SetActive(false);

            //if (i == index)
            //{
            //    elementModel = new GameObject[models[i].transform.childCount];

            //    for(int k = 0; k < elementModel.Length; k++)
            //    {
            //        elementModel[k] = models[i].transform.GetChild(k).gameObject;
            //        elementModel[k].SetActive(true);
            //    }

            //    models[i].SetActive(true);
            //}
            //else
            //{
            //    models[i].SetActive(false);
            //}
        }

        elementModel = new GameObject[models[index].transform.childCount];

        for (int k = 0; k < elementModel.Length; k++)
        {
            elementModel[k] = models[index].transform.GetChild(k).gameObject;
            elementModel[k].SetActive(true);
        }

        models[index].SetActive(true);
    }
}
