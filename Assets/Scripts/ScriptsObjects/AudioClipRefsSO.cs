using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
// this script is used to reference audio clips in our project
public class AudioClipRefsSO : ScriptableObject
{
    public AudioClip[] chop;
    public AudioClip[] deliverFail;
    public AudioClip[] deliverSuccess;
    public AudioClip[] footstep;
    public AudioClip[] objectDrop;
    public AudioClip[] objectPickup;
    public AudioClip[] stoveSizzle;
    public AudioClip[] trash;
    public AudioClip[] warning;
}
