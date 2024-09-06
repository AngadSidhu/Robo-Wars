using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadSounds : MonoBehaviour
{
    [SerializeField] AudioSource reload;

    [SerializeField] AudioSource click;

    [SerializeField] TrailRenderer tr;
    
    public void Reload()
    {
        reload.Play();
    }

    public void Click()
    {
        click.Play();
    }

    public void TrailStart()
    {
        tr.emitting = true;
    }

    public void TrailEnd()
    {
        tr.emitting = false;
    }
}
