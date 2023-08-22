using FrameworkDesign;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * 创建人：杜
 * 功能说明：基础UI控制
 * 创建时间：
 */

namespace Dungeon_3DRPG_Demo
{
    public class BaseUI : MonoBehaviour, IController, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("PlayerInfo")]
        private GameObject mPlayerInfoBar;
        private Slider mHealthSlider;
        private Slider mExpSlider;
        private Text mHealthText;
        private Text mLevelText;
        private Text mPlayerName;

        private IPlayerModel mPlayerModel;

        [Header("Settings")]
        private Button mSettingsBtn;
        private GameObject mSettingsPanel;
        private bool isSettingsPanelOpen = false;
        private Slider mMusicSlider;
        private Slider mFXSlider;
        private Button mQuitGameBtn;

        public IArchitecture GetArchiteccture()
        {
            return DungeonRPG.Interface;
        }

        private void Awake()
        {
            mPlayerInfoBar = transform.Find("PlayuerInfo Bar").gameObject;
            mHealthSlider = mPlayerInfoBar.transform.Find("Slider05_UserInfo_White1").GetComponent<Slider>();
            mExpSlider = mPlayerInfoBar.transform.Find("Slider05_UserInfo_White2").GetComponent<Slider>();
            mHealthText = mPlayerInfoBar.transform.Find("Slider05_UserInfo_White1/Health Text").GetComponent<Text>();
            mLevelText = mPlayerInfoBar.transform.Find("UserLevel_White/Level Text").GetComponent<Text>();
            mPlayerName = mPlayerInfoBar.transform.Find("UserLevel_White/Name Text").GetComponent<Text>();

            mSettingsBtn = transform.Find("Settings Btn").GetComponent<Button>();
            mSettingsBtn.onClick.AddListener(OpenOrCloseSettingsPanel);

            mSettingsPanel = transform.Find("Settings Panel").gameObject;
            mMusicSlider = mSettingsPanel.transform.Find("Settings Board/BGM Slider").GetComponent<Slider>();
            mMusicSlider.onValueChanged.AddListener(OnBGMSliderChanged);

            mFXSlider = mSettingsPanel.transform.Find("Settings Board/SFX Slider").GetComponent<Slider>();
            mFXSlider.onValueChanged.AddListener(OnFXSliderChanged);

            mQuitGameBtn = mSettingsPanel.transform.Find("Settings Board/Quit Btn").GetComponent<Button>();
            mQuitGameBtn.onClick.AddListener(OnQuitGame);

            mPlayerModel = this.GetModel<IPlayerModel>();

            // 注册事件
            mPlayerModel.MaxHP.RegisterOnValueChanged(UpdateMaxHP)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
            mPlayerModel.CurrentHP.RegisterOnValueChanged(UpdateHP)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
            mPlayerModel.BaseEXP.RegisterOnValueChanged(UpdateBaseEXP)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
            mPlayerModel.CurrentEXP.RegisterOnValueChanged(UpdateEXP)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
            mPlayerModel.CurrentLV.RegisterOnValueChanged(UpdateLevel)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void Start()
        {
            UpdateMaxHP(mPlayerModel.MaxHP.Value);
            UpdateHP(mPlayerModel.CurrentHP.Value);
            UpdateBaseEXP(mPlayerModel.BaseEXP.Value);
            UpdateEXP(mPlayerModel.CurrentHP.Value);
            UpdateLevel(mPlayerModel.CurrentLV.Value);
        }

        private void UpdateMaxHP(int maxHp)
        {
            mHealthSlider.maxValue = maxHp;
            mHealthText.text = mHealthSlider.value + "/" + mHealthSlider.maxValue;
        }

        private void UpdateHP(int hp)
        {
            mHealthSlider.value = hp;
            mHealthText.text = mHealthSlider.value + "/" + mHealthSlider.maxValue;
        }

        private void UpdateBaseEXP(int baseExp)
        {
            mExpSlider.maxValue = baseExp;
        }

        private void UpdateEXP(int point)
        {
            mExpSlider.value += point;
        }

        private void UpdateLevel(int level)
        {
            mLevelText.text = level.ToString();
        }

        #region 设置面板
        /// <summary>
        /// 开启或关闭设置面板
        /// </summary>
        public void OpenOrCloseSettingsPanel()
        {
            if (isSettingsPanelOpen)
            {
                Time.timeScale = 1f;
                mSettingsPanel.SetActive(false);
            }
            else
            {
                Time.timeScale = 0f;
                mSettingsPanel.SetActive(true);
            }
            isSettingsPanelOpen = !isSettingsPanelOpen;
        }

        /// <summary>
        /// 退出游戏
        /// </summary>
        public void OnQuitGame()
        {
            this.SendCommand<QuitGameCommand>();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>
        /// 背景音乐控制
        /// </summary>
        /// <param name="value">Slider传入值</param>
        private void OnBGMSliderChanged(float value)
        {
            if (value == 0)
                value = 0.001f;
            AudioManager.Instance.mixer.SetFloat("BGM", 10 * Mathf.Log(value * 0.1f));
        }

        /// <summary>
        /// 音效控制
        /// </summary>
        /// <param name="value">Slider传入值</param>
        private void OnFXSliderChanged(float value)
        {
            if (value == 0)
                value = 0.001f;
            AudioManager.Instance.mixer.SetFloat("SystemFX", 10 * Mathf.Log(value * 0.1f));
            AudioManager.Instance.mixer.SetFloat("PlayerFX", 10 * Mathf.Log(value * 0.1f));
            AudioManager.Instance.mixer.SetFloat("EnvironmentFX", 10 * Mathf.Log(value * 0.1f));
        }
        #endregion

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (CheckSettingsBtnUI(eventData.position) && CheckSettingsPanelUI(eventData.position))
                this.SendCommand<PointerEnterUICommand>();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (CheckSettingsBtnUI(eventData.position) && CheckSettingsPanelUI(eventData.position))
                this.SendCommand<PointerExitUICommand>();
        }

        private bool CheckSettingsBtnUI(Vector3 mousePos)
        {
            RectTransform rectTrans = (RectTransform)mSettingsBtn.transform;
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTrans, mousePos))
                return true;
            return false;
        }

        private bool CheckSettingsPanelUI(Vector3 mousePos)
        {
            RectTransform rectTrans = (RectTransform)mSettingsPanel.transform;
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTrans, mousePos))
                return true;
            return false;
        }
    }
}
