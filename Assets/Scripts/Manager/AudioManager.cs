using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/*
 * 创建人：杜
 * 功能说明：音频控制
 * 创建时间：
 */

namespace Dungeon_3DRPG_Demo
{
    public class AudioManager : Singleton<AudioManager>
    {
        [Header("Audio Mixer")]
        public AudioMixer mixer;

        [Header("Base Audio Source")]
        private AudioSource bgmMusic;
        private AudioSource systemFX;

        [Header("Player Audio Source")]
        private AudioSource playerWalkFX;
        private AudioSource playerJumpFX;
        private AudioSource playerPunchFX;
        private AudioSource playerEquipFX;
        private AudioSource playerAxeFX;

        [Header("Base Audio Clips")]
        public AudioClip bgmClip;
        public AudioClip systemFXClip;

        [Header("Player Audio Clips")]
        public List<AudioClip> walkClips;
        public AudioClip jumpClip;
        public List<AudioClip> punchClips;
        public AudioClip equipClip;
        public List<AudioClip> axeClips;

        protected override void Awake()
        {
            base.Awake();
            bgmMusic = transform.Find("BGMMusic").GetComponent<AudioSource>();
            systemFX = transform.Find("SystemFX").GetComponent<AudioSource>();

            playerWalkFX = transform.Find("Player/WalkFX").GetComponent<AudioSource>();
            playerJumpFX = transform.Find("Player/JumpFX").GetComponent<AudioSource>();
            playerPunchFX = transform.Find("Player/PunchFX").GetComponent<AudioSource>();
            playerEquipFX = transform.Find("Player/EquipFX").GetComponent<AudioSource>();
            playerAxeFX = transform.Find("Player/AxeFX").GetComponent<AudioSource>();
        }

        private void Start()
        {
            bgmMusic.clip = bgmClip;
            bgmMusic.Play();
        }

        private void OnEnable()
        {
            PlayBGMMusic();
        }

        public void PlayBGMMusic()
        {
            if (!bgmMusic.isPlaying)
            {
                bgmMusic.Play();
            }
        }

        public void PlayWalkSFX()
        {
            playerWalkFX.clip = walkClips[Random.Range(0, walkClips.Count)];
            playerWalkFX.Play();
        }

        public void PlayJumpSFX()
        {
            playerJumpFX.Play();
        }

        public void PlayPunchSFX()
        {
            playerPunchFX.clip = punchClips[Random.Range(0, punchClips.Count)];
            playerPunchFX.Play();
        }

        public void PlayEquipSFX()
        {
            playerEquipFX.Play();

        }

        public void PlayAxeSFX()
        {
            playerAxeFX.clip = axeClips[Random.Range(0, axeClips.Count)];
            playerAxeFX.Play();
        }

        public void PlaySystemSFX()
        {
            //TODO
        }
    }
}
