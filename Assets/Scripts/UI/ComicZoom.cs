using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ComicZoom : MonoBehaviour
{
    [SerializeField]
    private GameObject image; // The image to be scaled
    [SerializeField]
    private float zoomSpeed = 0.5f; // The rate of change of the canvas scale factor
    [SerializeField]
    private float minScale = 0.6f; // The minimum scale the image will scale to
    [SerializeField]
    private float maxScale = 2.0f; // The maximum scale the image will scale to
    [SerializeField]
    private Sprite[] comics;

    private int currentComic = 0;

    private ScrollRect scrollRect;

    private Vector2 startPos;
    private Vector2 direction;
    private bool directionChosen;
    private bool resizing = false;

    public Button prevButton;
    public Button nextButton;
    public Button playLevel;

    void Start()
    {
        if(comics.Length == 0)
        {
            StartLevel();
            return;
        }
        //find all buttons in scene or just attach them
        prevButton.onClick.AddListener(PrevPage);
        nextButton.onClick.AddListener(NextPage);
        playLevel.onClick.AddListener(StartLevel);
        prevButton.gameObject.SetActive(false);
        if (comics.Length == 1)
        {
            playLevel.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(false);
        }
        else
        {
            playLevel.gameObject.SetActive(false);
        }
        image.GetComponent<Image>().sprite = comics[currentComic];
        image.transform.localScale = new Vector3(minScale, minScale, 1);
        scrollRect = GetComponent<ScrollRect>();
    }

    void Update()
    {
        if(comics.Length == 0)
        {
            return;
        }
        // If there is one touch on the device
        if(Input.touchCount == 1)
        {
            if (!resizing)
            {
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    // Record initial touch position.
                    case TouchPhase.Began:
                        startPos = touch.position;
                        directionChosen = false;
                        break;

                    // Determine direction by comparing the current touch position with the initial one.
                    case TouchPhase.Moved:
                        direction = touch.position - startPos;
                        break;

                    // Report that a direction has been chosen when the finger is lifted.
                    case TouchPhase.Ended:
                        directionChosen = true;
                        break;
                }

                /*if(directionChosen && !resizing)
                {
                    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                    {
                        scrollRect.vertical = false;
                        // Check if the swipe if left and update comic page
                        if (direction.x < -50)
                        {
                            if (currentComic < comics.Length - 1)
                            {
                                NextPage();
                            }
                        }
                        // Check if the swipe is right and update comic page
                        else if (direction.x > 50)
                        {
                            if (currentComic > 0)
                            {
                                PrevPage();
                            }
                        }
                    }
                }*/
                directionChosen = false;
            }
        }
        // If there are two touches on the device
        else if (Input.touchCount == 2)
        {
            resizing = true;
            //Turn off vertical scrolling of the scroll rect
            scrollRect.vertical = false;

            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // ... change the image size based on the change in distance between the touches.
            image.transform.localScale += new Vector3(-deltaMagnitudeDiff * zoomSpeed, -deltaMagnitudeDiff * zoomSpeed);

            // Clamp the image to the maximum size
            image.transform.localScale = new Vector3(Mathf.Clamp(image.transform.localScale.x, minScale, maxScale), Mathf.Clamp(image.transform.localScale.y, minScale, maxScale));
            StartCoroutine(PauseSwipe());
        }
        else
        {
            // Restore Vertical Scrolling
            scrollRect.vertical = true;
        }
    }

    public void NextPage()
    {
        if (currentComic < comics.Length - 1)
        { 
            currentComic++;
            if (currentComic == comics.Length - 1)
            {
                nextButton.gameObject.SetActive(false);
                playLevel.gameObject.SetActive(true);
            }
            if (prevButton.gameObject.activeSelf == false)
            {
                prevButton.gameObject.SetActive(true);
            }
            image.transform.localScale = new Vector3(minScale, minScale, 1);
            image.GetComponent<Image>().sprite = comics[currentComic];
        }
    }

    public void PrevPage()
    {
        if (currentComic > 0)
        {
            currentComic--;
            nextButton.gameObject.SetActive(true);

            if (currentComic == 0)
            {
                prevButton.gameObject.SetActive(false);
            }
            if (playLevel.gameObject.activeSelf == true)
            {
                playLevel.gameObject.SetActive(false);
            }
            image.transform.localScale = new Vector3(minScale, minScale, 1);
            image.GetComponent<Image>().sprite = comics[currentComic];
        }
    }

    public void StartLevel()
    {
        SceneManager.LoadScene("World1Level1");
    }

    IEnumerator PauseSwipe()
    {
        yield return new WaitForSeconds(1.0f);
        resizing = false;
    }
}
