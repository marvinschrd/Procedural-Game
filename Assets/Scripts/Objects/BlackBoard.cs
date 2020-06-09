using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BlackBoard : MonoBehaviour
{
    int amountOfDeath = 0;
    void Update()
    {
        if(amountOfDeath>=2)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }

    public void UpdateDeath()
    {
        amountOfDeath++;
    }
}
