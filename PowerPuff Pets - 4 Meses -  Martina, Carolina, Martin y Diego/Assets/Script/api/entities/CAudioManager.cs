using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CAudioManager : MonoBehaviour
{

    #region SINGLETON
    private static CAudioManager _inst;
    public static CAudioManager Inst
    {
        get
        {
            //if (_inst == null)
            //{
            //    _inst = FindObjectOfType(typeof(AudioManager)) as AudioManager;
            //    if (_inst == null)
            //    {
            //        GameObject obj = new GameObject("AudioManager");
            //        _inst = obj.AddComponent<AudioManager>();
            //        //_inst.Init();
            //        //DontDestroyOnLoad(obj);
            //    }
            //    //else
            //    //{
            //    //    _inst.Init();
            //    //}
            //}
            return _inst;
        }
    }
    #endregion

    public float maxHearingDistance = 30f; //max distance in meters in which the sound dies off.

    #region EDITOR VARIABLES
    public List<AudioSerial> musicAssetList;
    public List<AudioSerial> sfxAssetList;
    #endregion

    #region PRIVATE VARIABLES
    private Dictionary<string, AudioClip> musicList;
    private Dictionary<string, AudioClip> sfxList;
    private AudioSource activeMusicAudioSource;
    private List<AudioSource> activeSFXAudioSources;
    private string activeMusicAudioHash;
    private bool sfxPaused = false;
    #endregion

    #region VOLUMES
    [SerializeField]
    private float musicVolume = 1f;
    [SerializeField]
    private float sfxVolume = 1f;
    #endregion


    private void Awake()
    {
        if (_inst != null)// && _inst != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _inst = this;
        DontDestroyOnLoad(gameObject);
        //TODO: load volume & music fx from save when implemented
        if (musicAssetList == null)
            musicAssetList = new List<AudioSerial>();

        if (sfxAssetList == null)
            sfxAssetList = new List<AudioSerial>();

        if (musicAssetList.Count == 0)
            Debug.LogWarning("No Music added in AudioManager. Playing a clip will resort in Exception!");
        if (sfxAssetList.Count == 0)
            Debug.LogWarning("No SFX added in AudioManager. Playing a clip will resort in Exception!");

        //creates music audio source
        BuildMusicSource();
        activeSFXAudioSources = new List<AudioSource>();

        //creates music&sfx audio reference sheet
        musicList = new Dictionary<string, AudioClip>();
        sfxList = new Dictionary<string, AudioClip>();

        for (int i = 0; i < musicAssetList.Count; i++)
        {
            musicList.Add(musicAssetList[i].hash, musicAssetList[i].source);
        }

        for (int j = 0; j < sfxAssetList.Count; j++)
        {
            sfxList.Add(sfxAssetList[j].hash, sfxAssetList[j].source);
        }

    }

    public static bool Exists()
    {
        return _inst != null;
    }

    private void BuildMusicSource()
    {
        GameObject source = new GameObject("MusicSource"); //create new object
        AudioListener listener = GameObject.FindObjectOfType<AudioListener>() as AudioListener; //gets listener

        source.transform.parent = listener.gameObject.transform; //set the audiolistener object as parent
        source.transform.localPosition = Vector3.zero; //set position to middle of object
        activeMusicAudioSource = source.AddComponent<AudioSource>(); //adds an audiosource & saves reference

        SetMusicVolume(musicVolume);
    }



    void LateUpdate()
    {
        //Debug.Log(activeSFXAudioSources);
        for (int i = activeSFXAudioSources.Count - 1; i >= 0; i--)
        {
            AudioSource auxSource = activeSFXAudioSources[i];
            if (auxSource == null || (!auxSource.loop && !auxSource.isPlaying)) //sfx has ended and not looping? or was deleted?
            {
                if (auxSource != null)
                {
                    auxSource.Stop();
                    Destroy(auxSource.gameObject); //TODO: once a pool is made, remove gameobject instead of destroying
                }

                activeSFXAudioSources.RemoveAt(i);
            }
        }
    }

    public bool IsMusicPlaying(string hash)
    {
        return activeMusicAudioHash == hash;
    }

    public void PlayMusic(string hash, bool loop = true, Vector3 position = default(Vector3))
    {
        if (activeMusicAudioSource == null)
            BuildMusicSource();
        //get music object, set the audioclip to it and save reference
        AudioClip audioToPlay = musicList[hash];
        activeMusicAudioSource.GetComponent<AudioSource>().Stop();
        activeMusicAudioSource.clip = audioToPlay;
        activeMusicAudioSource.priority = 0;
        activeMusicAudioSource.volume = musicVolume;
        activeMusicAudioSource.loop = loop;
        activeMusicAudioSource.maxDistance = maxHearingDistance;
        activeMusicAudioSource.Play();
        activeMusicAudioHash = hash;
    }

    public void PlaySFX(string hash, bool loop, Transform parent)
    {
        //get new audiosource object, set the clip and properties and save reference
        //Debug.Log(hash);
        AudioClip audioToPlay = sfxList[hash];

        GameObject sfxObj = new GameObject("sfx " + hash); //TODO: once a pool is made, get gameobject from pool instead of creating new
        sfxObj.transform.parent = parent;
        sfxObj.transform.localPosition = Vector3.zero;
        AudioSource source = sfxObj.AddComponent<AudioSource>();
        source.GetComponent<AudioSource>().clip = audioToPlay;
        source.volume = sfxVolume;
        source.loop = loop;
        source.minDistance = maxHearingDistance;
        source.maxDistance = maxHearingDistance;
        source.dopplerLevel = 0f; //sets doppler to 0 to cancel pitch distortions on movement

        source.Play();

        activeSFXAudioSources.Add(source);

    }

    public void SetMusicVolume(float aVolume)
    {
        musicVolume = aVolume;
        if (activeMusicAudioSource != null)
        {
            activeMusicAudioSource.volume = musicVolume;
        }
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public void SetSFXVolume(float aVolume)
    {
        sfxVolume = aVolume;
    }

    private void UpdateMusicVolume()
    {
        activeMusicAudioSource.volume = musicVolume;
    }

    private void UpdateSFXVolume()
    {
        for (int i = 0; i < activeSFXAudioSources.Count; i++)
        {
            activeSFXAudioSources[i].volume = sfxVolume;
        }
    }

    public void TogglePauseAll()
    {
        TogglePauseMusic();
        TogglePauseSFX();
    }

    public void TogglePauseMusic()
    {
        if (activeMusicAudioSource.GetComponent<AudioSource>().isPlaying)
        {
            activeMusicAudioSource.Pause();
        }
        else
        {
            activeMusicAudioSource.Play();
        }

    }

    public void TogglePauseSFX()
    {
        for (int i = 0; i < activeSFXAudioSources.Count; i++)
        {
            if (sfxPaused)
            {
                activeSFXAudioSources[i].Play();
            }
            else
            {
                activeSFXAudioSources[i].Pause();
            }
        }

        sfxPaused = !sfxPaused;
    }

    public void ResetCurrentMusic()
    {
        activeMusicAudioSource.GetComponent<AudioSource>().time = 0f;
    }

    public void StopAll()
    {
        StopMusic();
        StopSFX();
    }

    public void StopMusic()
    {
        if (activeMusicAudioSource == null)
            BuildMusicSource();
        activeMusicAudioSource.Stop();
    }

    public void StopSFX()
    {
        for (int i = 0; i < activeSFXAudioSources.Count; i++)
        {
            activeSFXAudioSources[i].Stop();
        }
    }
}
