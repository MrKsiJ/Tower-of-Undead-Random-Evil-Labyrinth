using UnityEngine;
using Firebase;
using Firebase.Extensions;

public class Analytics : MonoBehaviour
{
    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var app = FirebaseApp.DefaultInstance;
            Debug.Log("Installed!");
        });
    }
   
}
