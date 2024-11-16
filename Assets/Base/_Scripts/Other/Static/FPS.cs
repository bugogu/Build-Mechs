using UnityEngine;

public class FPS : MonoBehaviour
{
    [SerializeField] private float _refereshTime = 0.3f;
    private float _timeCounter;
    private float _fps;
    private int _frameCounter;
    private TMPro.TMP_Text _fpsText;
    private void Awake()
    {
        _fpsText = GetComponent<TMPro.TMP_Text>();
    }
    void Update()
    {
        if (_timeCounter < _refereshTime)
        {
            _timeCounter += Time.deltaTime;
            _frameCounter++;
        }
        else
        {
            _fps = _frameCounter / _timeCounter;
            _frameCounter = 0;
            _timeCounter = 0;
        }
        _fpsText.text = _fps.ToString("F2");
    }
}
