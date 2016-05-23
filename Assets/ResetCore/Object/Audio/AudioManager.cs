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
        [SerializeField]
        private List<AudioMixerGroup> groupList;

        public Dictionary<string, AudioMixerGroup> groupDictionary
        {
            get;
            private set;
        }

        [SerializeField]
        private AudioMixerGroup BGMGroup;

        private Transform BGMPool;
        private Transform SEPool;


        public override void Init()
        {
            base.Init();
            groupDictionary = new Dictionary<string, AudioMixerGroup>();
            foreach (AudioMixerGroup group in groupList)
            {
                groupDictionary.Add(group.name, group);
            }
            GameObject bgmPool = new GameObject("BGMPool");
            BGMPool = bgmPool.transform;
            BGMPool.SetParent(transform);
            GameObject sePool = new GameObject("SEPool");
            SEPool = sePool.transform;
            SEPool.SetParent(transform);
            DontDestroyOnLoad(gameObject);
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
                PlayObject(BGMObject.gameObject, clipName, BGMGroup, true, true);
            }
        }

        public void PlayObjectSE(GameObject go, string clipName, string mixerGroup = null)
        {
            AudioMixerGroup group = groupDictionary.ContainsKey(mixerGroup) ? groupDictionary[mixerGroup] : null;
            PlayObject(go, clipName, group, false, false);
        }

        

        public void PlayGlobalSE(string clipName, string mixerGroup = null)
        {
            AudioMixerGroup group = groupDictionary.ContainsKey(mixerGroup) ? groupDictionary[mixerGroup] : null;
            PlayObject(FindOrCreateSEClipObject(clipName, SEPool), clipName, group, false, false);
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
