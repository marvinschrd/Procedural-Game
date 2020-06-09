using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio", fileName = "Clip")]
public class SO_Clip : ScriptableObject{
    [SerializeField] AudioClip clip_;
    [SerializeField] float volume_ = 1;

    public AudioClip Clip => clip_;
    public float Volume => volume_;
}    
