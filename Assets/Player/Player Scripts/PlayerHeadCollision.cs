using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHeadCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            //plays death sound if audio manager exists
            AudioManager.Instance?.PlayDeathSound();
            
            SaveManager.Instance.RestoreSavedSnapshot();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
