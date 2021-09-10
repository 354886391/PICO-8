using UnityEngine;

public class FPSConsole : MonoBehaviour
{
    private int _frames;
    private float _accum;
    private float _nextFPSUpdate;
    private string _fpsDisplay;

    public bool FPSEnabled;
    public float FPSUpdateInterval = 0.25f;
    public TextMesh FPSTextMesh;

    private void Start()
    {
        _nextFPSUpdate = FPSUpdateInterval;
    }

    private void Update()
    {
        if (FPSEnabled)
        {
            _nextFPSUpdate -= Time.deltaTime / Time.timeScale;
            _accum += Time.timeScale / Time.deltaTime;
            ++_frames;
            if (_nextFPSUpdate <= 0.0f)
            {
                float num = _accum / _frames;
                _frames = 0;
                _accum = 0.0f;
                _fpsDisplay = num.ToString("N0");
                _nextFPSUpdate = FPSUpdateInterval;
                FPSTextMesh.color = num < 40.0f ? (num < 20.0f ? Color.red : Color.yellow) : Color.green;
                FPSTextMesh.text = _fpsDisplay;

            }
        }

    }
}
