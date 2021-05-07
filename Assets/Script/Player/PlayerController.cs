using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MoveDirection
{
    RIGHT,
    LEFT,
    FORWARD,
    BACK,
    DEFAULT
}

public class PlayerController : MonoBehaviour
{
    public AreaController areaController;
    [SerializeField] private MoveDirection moveDirection;
    public PlayerScriptableObject player;

    private float speed;

    private bool _isMoving;
    private bool _isDead = false;
    private int _directionX;
    private int _directionZ;
    private PieceController _destinationPiece;
    private PieceController _currentPiece;
    private Vector3 _tempVector;
    private Vector3 rayDirection;


    private void Start()
    {
        moveDirection = MoveDirection.DEFAULT;
        speed = player.speed;
    }

    private void Update()
    {
        LevelTransition();

        if (!_isDead)
        {
            Movement();
        }

        if(areaController.isCreateArea)
        {
            Init();
            areaController.isCreateArea = false;
        }

    }

    
    private void FixedUpdate()
    {
        //player raycast direction
        switch(moveDirection)
        {
            case MoveDirection.RIGHT:
                rayDirection = transform.TransformDirection(Vector3.right);
                break;
            case MoveDirection.LEFT:
                rayDirection = transform.TransformDirection(Vector3.left);
                break;
            case MoveDirection.FORWARD:
                rayDirection = transform.TransformDirection(Vector3.forward);
                break;
            case MoveDirection.BACK:
                rayDirection = transform.TransformDirection(Vector3.back);
                break;
        }


        if(Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, .5f))
        {
            CheckRaycastSituation(hit.collider.tag);
        }
    }
    
    //initialize, create game area.
    public void Init()
    {
        _isMoving = false;
        _isDead = false;
        gameObject.transform.position = player.startPos;
        _destinationPiece = _currentPiece = areaController.GetPiece((int)transform.position.x, (int) transform.position.z);
    }

    private void Movement()
    {
        if (GameplayController.Instance.gamestate == GameState.GamePlay)
        {
            //Input System UP,DOWN,LEFT,RIGHT!
            #region Input 
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _directionX = 0;
                _directionZ = 1;
                _isMoving = true;
                moveDirection = MoveDirection.FORWARD;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _directionX = 0;
                _directionZ = -1;
                _isMoving = true;
                moveDirection = MoveDirection.BACK;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _directionX = -1;
                _directionZ = 0;
                _isMoving = true;
                moveDirection = MoveDirection.LEFT;

            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _directionX = 1;
                _directionZ = 0;
                _isMoving = true;
                moveDirection = MoveDirection.RIGHT;

            }
            #endregion

            if (_isMoving)

                if (_destinationPiece != null)
                {
                    float distX = Mathf.Abs(transform.position.x - _destinationPiece.gameObject.transform.position.x);
                    float distZ = Mathf.Abs(transform.position.z - _destinationPiece.gameObject.transform.position.z);
                    float step = speed * Time.deltaTime;

                    _tempVector = _destinationPiece.gameObject.transform.position;
                    _tempVector.y = 1;
                    _tempVector = Vector3.MoveTowards(transform.position, _tempVector, step);

                    transform.position = _tempVector;

                    if (distX < 0.1f && distZ < 0.1f) //check distance and after check piece situation.
                    {
                        CheckPiece();
                    }
                }
        }
    }

    private void CheckPiece()
    {
        if (!_isMoving)
            return;

        //it is checked when the player reaches the filled piece.
        if (_currentPiece.CurrentPieceState == PieceState.TRACK && _destinationPiece.CurrentPieceState == PieceState.FILLED)
        {
            _isMoving = false;

            areaController.FillTrack();
            areaController.FillArea();
        }
        else
        {
            //if the player hits the track
            if (_destinationPiece.CurrentPieceState == PieceState.TRACK && _destinationPiece != _currentPiece)
            {
                Die();
            }

            _currentPiece = _destinationPiece;

            //if the player hits the empty
            if (_currentPiece.CurrentPieceState == PieceState.EMPTY)
            {
                _currentPiece.ChangePieceState(PieceState.TRACK);
            }

            ChooseNextPiece(_directionX, _directionZ);
        }
    }

    private void ChooseNextPiece(int x, int z)
    {
        PieceController nextPiece = areaController.GetPiece(_currentPiece.x + x, _currentPiece.z + z);
        if (nextPiece != null && _isMoving)
            _destinationPiece = nextPiece;
    }

    public void Die()
    {
        Debug.Log("Player Died!");
        _isMoving = false;
        _isDead = true;
        GameplayController.Instance.LoseLevel();
    }



    private void LevelTransition()
    {
        if(GameplayController.Instance.gamestate == GameState.Transition && transform.position.z < player.targetPosForLevelTransition.z)
        {
            /*TODO:
             * -first check x coordinate, if x != 5, player move x=5 coordinate,
             * -second check z coordinate, if z < 20, player move z=19 coordinate.
             */
            StartCoroutine(LevelTransitionIEnumarator());
        }
    }

    IEnumerator LevelTransitionIEnumarator()
    {
        transform.position = Vector3.Lerp(transform.position, player.targetPosForLevelTransition, speed * Time.deltaTime);
        yield return new WaitForSeconds(1f);
        SceneController.Instance.NextLevel();
        GameplayController.Instance.gamestate = GameState.GamePlay;
    }

    private void CheckRaycastSituation(string tag)
    {
        moveDirection = MoveDirection.DEFAULT;

        if (tag == "Wall")  //If the player hits the wall, turn the track states to full and fill the area.
        {
            _isMoving = false;

            areaController.FillTrack();
            areaController.FillArea();

            if (_destinationPiece != null)
                if (_destinationPiece.CurrentPieceState == PieceState.TRACK)
                    _currentPiece = _destinationPiece;

            if (_currentPiece.CurrentPieceState == PieceState.EMPTY)
            {
                _currentPiece.ChangePieceState(PieceState.TRACK);
            }
            
        }

        if(tag == "Enemy") //If the player hits the enemy, player die.
        {
            Die();
        }

        
    }

   

}


