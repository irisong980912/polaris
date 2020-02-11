using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public GameObject star;

    public List<GameObject> usedStardust = new List<GameObject>();

    public bool active;

    public Material shader;

    public float speed = 1.0f;

    private float startTime;

    [SerializeField] private Animator StarAnimationController;
    [SerializeField] private Animator StarVFX;

    private void OnTriggerStay(Collider collision)
    {
        ThirdPersonPlayer player = collision.GetComponent<ThirdPersonPlayer>();

        if (Input.GetKeyDown("joystick button 0")) //activate
        {
            if (active == false)
            {
                if (player.stardust > 0)
                {
                    active = !active;
                    GameObject stardust = player.inventory[0];
                    usedStardust.Add(stardust);
                    player.inventory.RemoveAt(0);
                    player.stardust -= 1;
                    if (shader.GetFloat("Vector1_D5C0D32A") == 0)
                    {
                        startTime = Time.time;
                    }

                    ActivateAnimations();
                    GetComponent<Orbit>().enabled = true;
                }


            }
            else if (active == true) // destroy
            {
                GameObject stardust = usedStardust[0];
                stardust.SetActive(true);
                usedStardust.RemoveAt(0);
                active = !active;
                if (shader.GetFloat("Vector1_D5C0D32A") == 1)
                {
                    startTime = Time.time;
                }

                DeactivateAnimations();
                GetComponent<Orbit>().enabled = false;
            }
        }
    }

    void ActivateAnimations()
    {
        StarAnimationController.SetTrigger("playCreateStar");
        StarVFX.SetTrigger("playActivateStarVFX");
    }

    void DeactivateAnimations()
    {
        StarAnimationController.SetTrigger("playDeactivateStar");
        StarVFX.SetTrigger("playDeactivateStarVFX");
    }

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        if (active == true)
        {
            shader.SetFloat("Vector1_D5C0D32A", 1);
        }
        if (active == false)
        {
            shader.SetFloat("Vector1_D5C0D32A", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (active == true)
        {
            if (shader.GetFloat("Vector1_D5C0D32A") == 1)
            {
                star.GetComponent<Gravity>().enabled = true;
            }
            else
            {
                float t = (Time.time - startTime) * speed;
                shader.SetFloat("Vector1_D5C0D32A", Mathf.Lerp(0, 1, t));
            }
        }
        if (active == false)
        {
            if (shader.GetFloat("Vector1_D5C0D32A") == 0)
            {
                star.GetComponent<Gravity>().enabled = false;
            }
            else
            {
                float t = (Time.time - startTime) * speed;
                shader.SetFloat("Vector1_D5C0D32A", Mathf.Lerp(1, 0, t));
            }
        }
    }
}
