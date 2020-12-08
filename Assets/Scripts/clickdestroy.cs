using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickdestroy : MonoBehaviour
{
    public bool puzzlePanelUp = true;
    public GameObject puzzlePanel;
    public GameObject playermaincam;
    public GameObject playersecondcam;


    void OnMouseDown()
    {
        puzzlePanelUp = true;
    }
    public void togglePuzzlePanel()
    {
        puzzlePanelUp = !puzzlePanelUp;
    }
    void Update()
    {
        if (puzzlePanelUp == false)
        {
            puzzlePanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            playermaincam.SetActive(true);
            playersecondcam.SetActive(false);

        }
        else if (puzzlePanelUp == true)
        {
            puzzlePanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            playermaincam.SetActive(false);
            playersecondcam.SetActive(true);
        }
    }
}
