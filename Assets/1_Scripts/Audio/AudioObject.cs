using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObject : MonoBehaviour
{

    private float time = 0;

    private void OnEnable()
    {
        time = GetComponent<AudioSource>().clip.length;
    }

    public void Init(float time)
    {
        StopAllCoroutines();
        this.time = time;
        Destroy();
    }

    public void Destroy()
    {
        StartCoroutine(DestroyGameObject());
    }
    
    private IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}