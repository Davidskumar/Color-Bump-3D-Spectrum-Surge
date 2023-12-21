using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private new Rigidbody rigidbody;
    private Vector3 lastpos;
    public float sensi = .18f, clamp = 42f, bound = 5f;

    public bool canMove, gameover, finish,ad;
    public GameObject BreakablePlayer;
    GameObject Restart;
    GameObject restartcanvas;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]

    public void LoadMain()
    {
        int level = PlayerPrefs.GetInt("Level", 1);
        SceneManager.LoadScene("Level"+level);
    }
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        Restart = GameObject.FindGameObjectWithTag("Restart");
        restartcanvas = Restart.transform.GetChild(0).gameObject;
    }
    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        if(Input.GetMouseButtonDown(0))
        {
            lastpos=Input.mousePosition;

        }
        if(canMove)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 newpos = lastpos - Input.mousePosition;
                lastpos = Input.mousePosition;
                newpos = new Vector3(newpos.x, 0, newpos.y);
                Vector3 moveforce = Vector3.ClampMagnitude(newpos, clamp);
                rigidbody.AddForce((-moveforce * sensi - rigidbody.velocity / 5f), ForceMode.VelocityChange);

            }
        }
        
        rigidbody.velocity.Normalize();
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x,-bound,bound),transform.position.y,transform.position.z);
        if(canMove)
        {
            transform.position += FindObjectOfType<CameraMove>().camVelocity;
        }
        if(!canMove && gameover)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Time.timeScale = 1;
                Time.fixedDeltaTime = Time.timeScale*0.02f;
                restartcanvas.SetActive(false);
            }

        }
        else if(!canMove && !finish)
        {
            if(Input.GetMouseButtonDown(0))
            {
                FindObjectOfType<GameManager>().removeui();
                canMove = true;
            }
        }
        
    }
    private void GameOver()
    {
        restartcanvas.SetActive(true);
        GameObject shatter = Instantiate(BreakablePlayer,transform.position,Quaternion.identity);
        foreach(Transform obj in shatter.transform)
        {
            obj.GetComponent<Rigidbody>().AddForce(Vector3.forward * 5, ForceMode.Impulse);

        }
        canMove= false;
        gameover = false;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled=false;
        Time.timeScale = .3f;
        Time.fixedDeltaTime = Time.timeScale*0.02f;

        Invoke("LoadMain", 1.5f);
    }
    IEnumerator NextLevel()
    {
        finish = true;
        canMove = false;
        int currentlevel = PlayerPrefs.GetInt("Level", 1);
        int nextLevel = currentlevel + 1;
        PlayerPrefs.SetInt("Level", nextLevel);
        yield return new WaitForSeconds(1f);
        if(Application.CanStreamedLevelBeLoaded("Level"+nextLevel))
        {
            SceneManager.LoadScene("Level"+nextLevel);
        }
        else
        {
            
            nextLevel = 1;
            ad = true;
            new WaitForSeconds(10);
            PlayerPrefs.SetInt("Level", nextLevel);
            SceneManager.LoadScene("Level" + nextLevel);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (!gameover)
            {
                GameOver();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Finish")
        {
            StartCoroutine(NextLevel());
        }
    }

}
