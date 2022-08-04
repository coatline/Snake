using UnityEngine;

public class GameAssets : MonoBehaviour {

    [SerializeField] AudioClip appleCrunch;
    public Sprite appleSprite;
    public Sprite snakeBodySprite;

    public static GameAssets I;
    private AudioSource ad;

    private void Awake()
    {
        I = this;
        ad = GetComponent<AudioSource>();
    }

    public void PlayAppleCrunch()
    {
        ad.PlayOneShot(appleCrunch);
    }

}
