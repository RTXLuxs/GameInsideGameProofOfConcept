using UnityEngine;

public class UseTablet : MonoBehaviour
{
    public Animator tabletAnimator;
    public AudioSource audioSource;
    public AudioClip audioClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (UserInput.Instance.tabletPressed && PlayerState.Instance.canUseTablet)
        {
            if(tabletAnimator.GetBool("Watching") == true)
            {
                PlayerState.Instance.ExitPC();
                audioSource.PlayOneShot(audioClip);
                Debug.Log("exit");
            }
            else
            {
                PlayerState.Instance.EnterPC();
                audioSource.PlayOneShot(audioClip);
                Debug.Log("enter");
            }
            
            tabletAnimator.SetBool("Watching", !tabletAnimator.GetBool("Watching"));
        }
    }
}
