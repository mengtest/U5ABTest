using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using ResetCore.Util;
using ResetCore.Asset;
using System.Collections.Generic;

namespace ResetCore.Util
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        private Transform BGMPool;
        private Transform SEPool;

        
        void Awake()
        {
            GameObject bgmPool = new GameObject("BGMPool");
            BGMPool = bgmPool.transform;
            GameObject sePool = new GameObject("SEPool");
            SEPool = sePool.transform;
        }

        public void PlayBGM(string clipName)
        {
            GameObject BGMObject = null;
            BGMPool.DoToAllChildren((tran) =>
            {
                AudioSource source = tran.GetComponent<AudioSource>();
                if (source.clip.name == clipName && !source.isPlaying)
                {
                    BGMObject = source.gameObject;
                    PlayObject(BGMObject, clipName, null, true, true);
                }
                if (source.isPlaying)
                {
                    source.volume = 0;
                }
            });
            if (BGMObject == null)
            {
                BGMObject = new GameObject(clipName);
                BGMObject.transform.SetParent(BGMPool);
                PlayObject(BGMObject.gameObject, clipName, null, true, true);
            }
        }

        public void PlayObjectSE(GameObject go, string clipName, AudioMixerGroup mixerGroup = null)
        {
            PlayObject(go, clipName, mixerGroup, false, false);
        }

        

        public void PlayGlobalSE(string clipName, AudioMixerGroup mixerGroup = null)
        {
            PlayObject(FindOrCreateSEClipObject(clipName, SEPool), clipName, mixerGroup, false, false);
        }

        public void PlayObject(GameObject go, string clipName, AudioMixerGroup mixerGroup = null, bool isLoop = false, bool playOnAwake = false, bool fadeIn = false)
        {
            AudioSource audioSource = go.GetOrCreateComponent<AudioSource>();
            audioSource.clip = ResourcesLoaderHelper.Instance.LoadResource<AudioClip>(clipName);
            audioSource.playOnAwake = playOnAwake;
            audioSource.outputAudioMixerGroup = mixerGroup;
            audioSource.loop = isLoop;
            audioSource.volume = 1;
            audioSource.Play();
        }

        private GameObject FindOrCreateSEClipObject(string clipName, Transform pool)
        {
            List<AudioSource> fitSource = new List<AudioSource>();
            pool.DoToAllChildren((tran) =>
            {
                AudioSource source = tran.GetComponent<AudioSource>();
                if (source.clip.name == clipName && !source.isPlaying)
                {
                    fitSource.Add(source);
                }
            });
            if (fitSource.Count > 0)
            {
                return fitSource[0].gameObject;
            }
            else
            {
                GameObject newSource = new GameObject(clipName);
                newSource.transform.SetParent(pool);
                return newSource;
            }
        }
    }

}
