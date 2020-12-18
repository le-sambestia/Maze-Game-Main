using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class keyget : MonoBehaviour
{
    void OnMouseDown()
    {

        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("EndScene");

    }
}
