using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSpawner : MonoBehaviour {

    public AudioMixerGroup mixerGroup;
    public GameObject audioSourceObj;

    public GameObjectVariable spawnerVariable;

    private void Start()
    {
        spawnerVariable.value = gameObject;
    }

    public void PlayClip(AudioClip clipIN)
    {
        GameObject sourceInstance = Instantiate(audioSourceObj, transform);
        AudioSource source = sourceInstance.GetComponent<AudioSource>();
        source.clip = clipIN;
        source.outputAudioMixerGroup = mixerGroup;
        source.Play();
        StartCoroutine(TimedDeath(sourceInstance, clipIN.length));
    }

    IEnumerator TimedDeath(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj);
    }

}
