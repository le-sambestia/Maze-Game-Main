using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeypadPuzzle : MonoBehaviour
{
    public Text _cardCode;
    public Text _inputCode;
    public int _codeLength = 5;
    public float _codeResetTime = 0.5f;
    private bool _isReset = false;
    public GameObject Door;
    public GameObject puzzlePanel;
    public GameObject maincam;
    public GameObject secondcam;

    private void OnEnable()
    {
        string code = string.Empty;

        for(int i = 0; i < _codeLength; i++)
        {
            code += Random.Range(1, 10);
        }
        _cardCode.text = code;
        _inputCode.text = string.Empty;
    }

    public void ButtonClick(int number)
    {
        if (_isReset) { return; }

        _inputCode.text += number;

        if (_inputCode.text == _cardCode.text)
        {
            _inputCode.text = "Correct";
            StartCoroutine(ResetCode());
            Door.SetActive(false);
            puzzlePanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            maincam.SetActive(true);
            secondcam.SetActive(false);
        }
        else if (_inputCode.text.Length >= _codeLength)
        {
            _inputCode.text = "Code too long";
            StartCoroutine(ResetCode());
        }
    }

    private IEnumerator ResetCode()
    {
        _isReset = true;

        yield return new WaitForSeconds(_codeResetTime);
        _inputCode.text = string.Empty;
        _isReset = false;
    }
}
