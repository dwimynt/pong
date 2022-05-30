using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Pemain 1
    public PlayerControl player1; // skrip
    private Rigidbody2D player1Rigidbody;
    
    // Pemain 2
    public PlayerControl player2; // skrip
    private Rigidbody2D player2Rigidbody;
    
    // Bola
    public BallControl ball; // skrip
    private Rigidbody2D ballRigidbody;
    private CircleCollider2D ballCollider;
    
    // Skor maksimal
    public int maxScore;


    // Start is called before the first frame update
    void Start()
    {
        // Inisialisasi rigidbody dan collider
        player1Rigidbody = player1.GetComponent<Rigidbody2D>();
        player2Rigidbody = player2.GetComponent<Rigidbody2D>();
        ballRigidbody = ball.GetComponent<Rigidbody2D>();
        ballCollider = ball.GetComponent<CircleCollider2D>();
        
        SpawnBoxes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject ObjectToSpawn; // skrip
    public int numOfBox;

    private Color[] colors = {new Color (1, 0, 0, 1), new Color (0, 0, 1, 1), new Color (1, 0.92f, 0.016f, 1)};
    private int[] points = {10, 8, 5};

    void SpawnBoxes()
    {
        for(int i=0; i< numOfBox; i++){
            // Random position within this transform
            Vector3 rndPosWithin;
            rndPosWithin = new Vector3(Random.Range(-15f, 15f), Random.Range(-5f, 5f), Random.Range(-1f, 1f));
            //rndPosWithin = transform.TransformPoint(rndPosWithin * .5f);
            rndPosWithin = transform.TransformPoint(rndPosWithin * .9f);
            GameObject box = Instantiate(ObjectToSpawn, rndPosWithin, transform.rotation); 
            SpriteRenderer sprite = box.GetComponent<SpriteRenderer>();
            int rand = Random.Range(0, 3);
            sprite.color = colors[rand]; 
            BoxControl boxControl = box.GetComponent<BoxControl>();
            boxControl.point = points[rand];
        }
    }

    // Untuk menampilkan GUI
    void OnGUI()
    {
        // Tampilkan skor pemain 1 di kiri atas dan pemain 2 di kanan atas
        GUI.Label(new Rect(Screen.width / 2 - 150 - 12, 20, 100, 100), "" +player1.Score);
        GUI.Label(new Rect(Screen.width / 2 + 150 + 12, 20, 100, 100), "" + player2.Score);
        // Tombol restart untuk memulai game dari awal
        if (GUI.Button(new Rect(Screen.width / 2 - 60, 35, 120, 53),"RESTART"))
        {
            // Ketika tombol restart ditekan, reset skor kedua pemain...
            player1.ResetScore();
            player2.ResetScore();
            // ...dan restart game.
            ball.SendMessage("RestartGame", 0.5f, SendMessageOptions.RequireReceiver);
        }
        // Jika pemain 1 menang (mencapai skor maksimal), ...
        if (player1.Score == maxScore)
        {
            // ...tampilkan teks "PLAYER ONE WINS" di bagian kiri layar...
            GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 -10, 2000, 1000), "PLAYER ONE WINS");
            // ...dan kembalikan bola ke tengah.
            ball.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        }
        // Sebaliknya, jika pemain 2 menang (mencapai skor maksimal), ...
        else if (player2.Score == maxScore)
        {
            // ...tampilkan teks "PLAYER TWO WINS" di bagian kanan layar...
            GUI.Label(new Rect(Screen.width / 2 + 30, Screen.height / 2 - 10, 2000, 1000), "PLAYER TWO WINS");
            // ...dan kembalikan bola ke tengah.
            ball.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        } 


        //if (GUI.Button(new Rect(Screen.width / 3 - 60, 35, 120, 53),"Add BALL"))
        //{
        //    ball.SendMessage("AddBall", 0.5f, SendMessageOptions.RequireReceiver);
        //}
        
        // Toggle nilai debug window ketika pemain mengklik tombol ini.
        if (GUI.Button(new Rect(Screen.width/2 - 60, Screen.height - 73, 120, 53), "TOGGLE\nDEBUG INFO"))
        {
            isDebugWindowShown = !isDebugWindowShown;
            trajectory.enabled = !trajectory.enabled;
        }
        
        // Jika isDebugWindowShown == true, tampilkan text area untuk debug window.
        if (isDebugWindowShown)
        {
            // Simpan nilai warna lama GUI
            Color oldColor = GUI.backgroundColor;
            // Beri warna baru
            GUI.backgroundColor = Color.red;

            // Simpan variabel-variabel fisika yang akan ditampilkan.
            float ballMass = ballRigidbody.mass;
            Vector2 ballVelocity = ballRigidbody.velocity;
            float ballSpeed = ballRigidbody.velocity.magnitude;
            Vector2 ballMomentum = ballMass * ballVelocity;
            float ballFriction = ballCollider.friction;
            float impulsePlayer1X = player1.LastContactPoint.normalImpulse;
            float impulsePlayer1Y =
            player1.LastContactPoint.tangentImpulse;
            float impulsePlayer2X = player2.LastContactPoint.normalImpulse;
            float impulsePlayer2Y =
            player2.LastContactPoint.tangentImpulse;
            // Tentukan debug text-nya
            string debugText =
            "Ball mass = " + ballMass+ "\n" +
            "Ball velocity = " + ballVelocity + "\n" +
            "Ball speed = " + ballSpeed + "\n" +
            "Ball momentum = " + ballMomentum + "\n" +
            "Ball friction = " + ballFriction + "\n" +
            "Last impulse from player 1 = (" + impulsePlayer1X + ", " +
            impulsePlayer1Y + ")\n" +
            "Last impulse from player 2 = (" + impulsePlayer2X + ", "
            + impulsePlayer2Y + ")\n";

            // Tampilkan debug window
            GUIStyle guiStyle = new GUIStyle(GUI.skin.textArea);
            guiStyle.alignment = TextAnchor.UpperCenter;
            GUI.TextArea(new Rect(Screen.width/2 - 200, Screen.height - 200, 400, 110), debugText, guiStyle);

            // Kembalikan warna lama GUI
            GUI.backgroundColor = oldColor;
        }
    }

    // Apakah debug window ditampilkan?
    private bool isDebugWindowShown = false;

    // Objek untuk menggambar prediksi lintasan bola
    public Trajectory trajectory;


}
