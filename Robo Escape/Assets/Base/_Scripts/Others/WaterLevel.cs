using UnityEngine.UI;
using UnityEngine;

public class WaterLevel : MonoBehaviour
{
    [SerializeField] private Image _waterFill;

    private float _timeToFill;
    private float _currentFillAmount = 0f;
    private bool _canFill;

    void Awake()
    {
        _canFill = GameManager.Instance.waterLevel;
        _timeToFill = LevelManager.Instance.levelData.waterFillTime;
    }

    void Update()
    {
        if(!_canFill) return;

        if(_waterFill.fillAmount >= 1f)
        {
            GameManager.Instance.ChangeGameState(GameState.Lose);
            _canFill = false;
            return;
        }
        
        _currentFillAmount += Time.deltaTime / _timeToFill;
        SetWaterLevel(_currentFillAmount);
    }

    public void SetWaterLevel(float value) =>  _waterFill.fillAmount = Mathf.Clamp01(value);

    private void Fillable(GameState gameState)
    {
        if(gameState != GameState.Intro) _canFill = true;
        else _canFill = false;
    }

    void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += Fillable;
    }

    void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= Fillable;
    }
}
