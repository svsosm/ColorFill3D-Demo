using UnityEngine;


public enum PieceState
{
    EMPTY,
    FILLED,
    TRACK
}

public class PieceController : MonoBehaviour
{

    public PieceState CurrentPieceState
    {
        get
        {
            return _currentPieceState;
        }
    }

    public delegate void OnStateChangeAction();
    public OnStateChangeAction OnStateChange;


    [SerializeField] private Material emptyMaterial;
    [SerializeField] private Material trackMaterial;
    [SerializeField] private Material filledMaterial;

    public bool indexed;
    public int x;
    public int z;

    [SerializeField] private PieceState _currentPieceState;
    private MeshRenderer _renderer;

    private void Start()
    {
        x = (int) gameObject.transform.position.x;
        z = (int) gameObject.transform.position.z; 
    }

    //Change material based on piece states
    public void ChangePieceState(PieceState pieceState)
    {
        _currentPieceState = pieceState;
        _renderer = gameObject.GetComponent<MeshRenderer>();

        switch (pieceState)
        {
            case PieceState.EMPTY:
                _renderer.material = emptyMaterial;
                break;
            case PieceState.TRACK:
                if (_renderer != null)
                    _renderer.material = trackMaterial;
                break;
            case PieceState.FILLED:
                _renderer.material = filledMaterial;
                transform.Translate(0, 1, 0);
                OnStateChange?.Invoke();
                break;
        }
    }

}
