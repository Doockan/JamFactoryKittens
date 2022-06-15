using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using LevelMenu;
using MainMenu;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Core_Logic
{
    // todo: Заменить на DI после джема
    public class MonoBehaviourSingleton : MonoBehaviour
    {
        [Header("Timers")]
        [SerializeField] private int profitTimeout;
        [SerializeField] private float roomMaxTime;
        [SerializeField] private float levelMaxTime;
        [SerializeField] private float escapeMaxTime;

        [Header("Mint Settings")]
        [SerializeField] private float mintDamageTimeout;
        [SerializeField] private float mintFirstDamage;
        [SerializeField] private float mintRegularDamage;
        [SerializeField] private float mintReduce;
        [SerializeField] private float mintFirstGrowTime;
        [SerializeField] private float mintRegularGrowTime;

        [Header("Score Settings")]
        [SerializeField] private int maxScore;
        [SerializeField] private int penalty25;
        [SerializeField] private int penalty50;
        [SerializeField] private int penalty75;
        [SerializeField] private int penlaty100;

        [Header("Music and Sound")] 
        [SerializeField] private AudioClip menuMusic;
        [SerializeField] private AudioClip levelMusic;
        [SerializeField] private AudioClip clickSound;
        [SerializeField] private AudioClip toLevelSound;
        [SerializeField] private AudioClip toRoomSound;
        [SerializeField] private AudioClip winSound;
        [SerializeField] private AudioClip[] mintSound;
        
        [Header("System Settings")] [SerializeField]
        private GameObject mintVfx;
        [SerializeField] private GameLevel home;
        [SerializeField] private List<GameLevel> levels;
        public Image progressSlider;
        [SerializeField] private Transform catRoomGuiContent;
        
        public Slider SoundSlider;
        public Slider MusicSlider;
        
        [SerializeField] private TextMeshProUGUI scoreText;
        
        public AudioSource musicAudioSource;
        public float SoundVolume;
        
        public DraggableObject CurrentDrag;
        public SpriteRenderer Renderer;

        public PlayerState State = PlayerState.Menu;
        public PlayerState DefaultState = PlayerState.Menu;
        
        private List<int> unasignedCats;
        private List<int> roomCats;
        
        private CatView levelCat;
        
        private DraggableObject[] levelDraggables;
        
        private List<CatView> levelCatViews;
        private List<CatView> roomCatViews;
        private List<CatRoomView> roomCatGuiViews;
        
        private List<MintView> levelMint;
        private List<MintView> inactiveMint;

        private float levelTime;
        public bool IsCountingHigh;

        private int score;

        private Coroutine setScoreCor;

        private float mintGrowTime;
        private float mintGrowTimer;

        private List<MintView> allMint;

        private void Awake()
        {
            unasignedCats = new List<int>();
            roomCats = new List<int>();
            roomCatViews = home.CatNode.GetComponentsInChildren<CatView>(true).ToList();
            roomCatGuiViews = catRoomGuiContent.GetComponentsInChildren<CatRoomView>(true).ToList();

            foreach (CatView cat in roomCatViews)
            {
                unasignedCats.Add(cat.Id);
            }

            MusicSlider.onValueChanged.AddListener(ChangeMusic);
            SoundSlider.onValueChanged.AddListener(ChangeSound);

            mintGrowTime = mintFirstGrowTime;
        }

        private void Start()
        {
            InitVolume();
            musicAudioSource.clip = menuMusic;
            musicAudioSource.Play();
        }

        private void Update()
        {
            if (IsCountingHigh && progressSlider.fillAmount < 1)
            {
                // Highing Cat
                levelTime += Time.deltaTime;
                
                // Damage Mint
                if (levelMint.Count > 0)
                {
                    foreach (MintView mintView in levelMint)
                    {
                        mintView.Time += Time.deltaTime;

                        if (mintView.Time >= mintDamageTimeout)
                        {
                            mintView.Time = 0;

                            DamageHighBar(mintRegularDamage);
                        }
                    }
                }
                
                // Mint Growing
                if (mintGrowTimer < mintGrowTime)
                {
                    mintGrowTimer += Time.deltaTime;
                }
                else
                {
                    if (inactiveMint.Count > 0)
                    {
                        DamageHighBar(mintFirstDamage);
                        int randomMint = UnityEngine.Random.Range(0, inactiveMint.Count);
                        MintView randomMintView = inactiveMint[randomMint];
                        inactiveMint.Remove(randomMintView);
                        levelMint.Add(randomMintView);
                        randomMintView.gameObject.SetActive(true);
                        randomMintView.Time = 0;
                        mintGrowTime = mintRegularGrowTime;
                        randomMintView.transform.localScale = Vector3.zero;
                        randomMintView.transform.DOScale(1f, 0.2f);
                        mintGrowTimer = 0;
                    }
                }

                // High filling
                progressSlider.fillAmount = levelTime / levelMaxTime;
            }

            foreach (CatRoomView catRoomView in roomCatGuiViews)
            {
                if (roomCats.Contains(catRoomView.Id))
                {
                    if (catRoomView.ProgressImage.fillAmount > 0)
                    {
                        catRoomView.HighTime += Time.deltaTime;
                        catRoomView.ProgressImage.fillAmount = 1 - catRoomView.HighTime / roomMaxTime;
                    }
                    else 
                    {
                        if (catRoomView.EscapeTime < escapeMaxTime)
                        {
                            catRoomView.EscapeTime += Time.deltaTime;
                        }
                        else
                        {
                            catRoomView.gameObject.SetActive(false);
                            catRoomView.EscapeTime = 0f;
                            roomCatViews.Find(x => x.Id == catRoomView.Id).gameObject.SetActive(false);
                            roomCats.Remove(catRoomView.Id);
                            unasignedCats.Add(catRoomView.Id);

                            if (roomCats.Count == 0)
                            {
                                if (setScoreCor != null)
                                {
                                    StopCoroutine(setScoreCor);
                                }
                            }
                        }
                    }
                }
            }
        }

        private IEnumerator CreateMintVfx(MintView mint)
        {
            Animator anim = Instantiate(mintVfx, mint.transform.position, quaternion.identity).GetComponent<Animator>();
            anim.Play("ChikChik");
            yield return new WaitForSeconds(1f);
            DestroyImmediate(anim.gameObject);
        }

        private void DamageHighBar(float damage)
        {
            if (levelTime + damage > levelMaxTime)
            {
                levelTime = levelMaxTime;
            }
            else
            {
                levelTime += damage;
            }
        }

        private void ReduceHighBar(float reduce)
        {
            if (levelTime - reduce < 0)
            {
                levelTime = 0;
            }
            else
            {
                levelTime -= reduce;
            }
        }

        public void RemoveMint(MintView mint)
        {
            StartCoroutine(CreateMintVfx(mint));
            levelMint.Remove(mint);
            inactiveMint.Add(mint);
            mint.gameObject.SetActive(false);
            mint.Time = 0;
            PlaySound(mintSound);
            ReduceHighBar(mintReduce);
        }

        public void ChangeSound(float value)
        {
            PlayerPrefs.SetFloat("SoundVolume", value);
            SoundVolume = value;
        }
        public void ChangeMusic(float value)
        {
            PlayerPrefs.SetFloat("MusicVolume", value);
            musicAudioSource.volume = value;
        }
        
        private void InitVolume()
        {
            if (!PlayerPrefs.HasKey("SoundVolume"))
            {
                PlayerPrefs.SetFloat("SoundVolume", 1);
            }
            if (!PlayerPrefs.HasKey("MusicVolume"))
            {
                PlayerPrefs.SetFloat("MusicVolume", 1);
            }

            SoundSlider.normalizedValue = PlayerPrefs.GetFloat("SoundVolume");
            MusicSlider.normalizedValue = PlayerPrefs.GetFloat("MusicVolume");
        
            musicAudioSource.volume = MusicSlider.normalizedValue;
            SoundVolume = SoundSlider.normalizedValue;
        }
        
        public void OnCatFound(CatView cat)
        {
            DefaultState = PlayerState.Menu;
            AppStartPoint.menuManager.GoToScreenOfType<WinState>();

            cat.gameObject.SetActive(false);
            
            roomCatViews.Find(x => x.Id == cat.Id).gameObject.SetActive(true);
            var view = roomCatGuiViews.Find(x => x.Id == cat.Id);
            view.HighTime = roomMaxTime * (1 - (levelTime / levelMaxTime));
            view.gameObject.SetActive(true);

            if (roomCats.Count == 0)
            {
                setScoreCor = StartCoroutine(SetScore());
            }
            
            roomCats.Add(cat.Id);
            levelCat = null;
            
            PlaySound(winSound);
            IsCountingHigh = false;
        }
        
        public void GoHome()
        {
            DisableAll();
            home.LevelObject.SetActive(true);
            Renderer = home.BackgroundRenderer;
            StartCoroutine(DragOffDelayed());
            DefaultState = PlayerState.Idling;
            State = PlayerState.Idling;
            IsCountingHigh = false;
            
            if (musicAudioSource.clip != menuMusic || !musicAudioSource.isPlaying)
            {
                musicAudioSource.clip = menuMusic;
                musicAudioSource.Play();
            }
        }
        
        public void SetLevel(int id)
        {
            DefaultState = PlayerState.Idling;
            DisableAll();
            levels[id].LevelObject.SetActive(true);
            Renderer = levels[id].BackgroundRenderer;
            levelDraggables = levels[id].AllDraggablesParent.GetComponentsInChildren<DraggableObject>(true);
            levelCatViews = levels[id].CatNode.GetComponentsInChildren<CatView>(true).ToList();
            allMint = levels[id].MintNode.GetComponentsInChildren<MintView>(true).ToList();
            
            if (levelCat == null)
            {
                foreach (DraggableObject draggableObject in levelDraggables)
                {
                    draggableObject.cat = null;
                }

                if (unasignedCats.Count > 0)
                {
                    int cat = Random.Range(0, unasignedCats.Count);
                    levelCat = levelCatViews.ToList().Find(x => x.Id == unasignedCats[cat]);
                    unasignedCats.Remove(unasignedCats[cat]);
                    
                    foreach (DraggableObject draggable in levelCat.Draggables)
                    {
                        draggable.cat = levelCat;
                    }
                    
                    levelCat.gameObject.SetActive(true);
                }

                levelTime = 0f;
                progressSlider.fillAmount = 0f;

                inactiveMint = new List<MintView>();
                levelMint = new List<MintView>();
                mintGrowTime = mintFirstGrowTime;
                
                foreach (MintView mintView in allMint)
                {
                    mintView.gameObject.SetActive(false);
                    mintView.Time = 0;
                    inactiveMint.Add(mintView);
                }
                
                State = PlayerState.Idling;
            }

            IsCountingHigh = true;
            musicAudioSource.clip = levelMusic;
            musicAudioSource.Play();
            
            PlaySound(toLevelSound);
        }

        private void DisableAll()
        {
            foreach (GameLevel level in levels)
            {
                level.LevelObject.SetActive(false);
            }
            
            home.LevelObject.SetActive(false);
        }

        public enum PlayerState
        {
            Idling = 0,
            DraggingItem = 1,
            Scrolling = 2,
            Menu = 3
        }

        private IEnumerator SetStateDelayed(PlayerState state, float delay)
        {
            yield return new WaitForSeconds(delay);
            State = state;
        }
        
        private IEnumerator DragOffDelayed()
        {
            yield return null;

            if (levelDraggables != null)
            {
                foreach (DraggableObject draggable in levelDraggables)
                {
                    draggable.SetDragOff();
                }
            }
        }

        private IEnumerator SetScore()
        {
            while (true)
            {
                int finalScore = maxScore;
                
                foreach (CatRoomView catRoomView in roomCatGuiViews)
                {
                    if (roomCats.Contains(catRoomView.Id))
                    {
                        if (catRoomView.ProgressImage.fillAmount > 0.75 && catRoomView.ProgressImage.fillAmount <= 1)
                        {
                            finalScore -= penlaty100;
                        }

                        if (catRoomView.ProgressImage.fillAmount > 0.5 && catRoomView.ProgressImage.fillAmount <= 0.75)
                        {
                            finalScore -= penalty75;
                        }

                        if (catRoomView.ProgressImage.fillAmount > 0.25 && catRoomView.ProgressImage.fillAmount <= 0.5)
                        {
                            finalScore -= penalty50;
                        }

                        if (catRoomView.ProgressImage.fillAmount >= 0 && catRoomView.ProgressImage.fillAmount <= 0.25)
                        {
                            finalScore -= penalty25;
                        }
                    }
                }

                score += finalScore;
                scoreText.text = KiloFormat(score);

                yield return new WaitForSeconds(profitTimeout);
            }
        }
        
        private string KiloFormat(int num)
        {
            if (num >= 100000000)
                return (num / 1000000).ToString("#,0M");

            if (num >= 10000000)
                return (num / 1000000).ToString("0.#") + "M";

            if (num >= 100000)
                return (num / 1000).ToString("#,0K");

            if (num >= 10000)
                return (num / 1000).ToString("0.#") + "K";

            return num.ToString("#,0");
        }
        
        public void PlayToRoom()
        {
            PlaySound(toRoomSound);    
        }
        
        public void PlayMenuClick()
        {
            PlaySound(clickSound);
        }
        
        public void PlaySound(AudioClip sound)
        {
            StartCoroutine(PlaySoundAsync(sound));
        }

        public void PlaySound(AudioClip[] sound)
        {
            int soundNumb = UnityEngine.Random.Range(0, sound.Length);
            PlaySound(sound[soundNumb]);
        }

        private IEnumerator PlaySoundAsync(AudioClip sound)
        {
            GameObject aso = new GameObject();
            aso.name = "SFX System";
            AudioSource source = aso.AddComponent<AudioSource>();
            source.spatialBlend = 0;
            source.volume = SoundVolume;
            source.loop = false;
            source.clip = sound;
            source.Play();
            yield return new WaitForSeconds(sound.length);
            source.Stop();
            DestroyImmediate(aso);
        }
    }
}