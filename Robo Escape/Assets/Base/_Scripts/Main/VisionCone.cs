using Unity.VisualScripting;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    public enum VisionType {Turret, Drone}
    [HideInInspector] public bool playerSpotted;
    [SerializeField] private VisionType _visionType;
    [SerializeField] private Color _spottedColor;
    [SerializeField] private Color _normalColor;
    public Material VisionConeMaterial;
    public float VisionRange;
    public float VisionAngle;
    public LayerMask VisionObstructingLayer;//layer with objects that obstruct the enemy view, like walls, for example
    public int VisionConeResolution = 120;//the vision cone will be made up of triangles, the higher this value is the pretier the vision cone will be
    Mesh VisionConeMesh;
    MeshFilter MeshFilter_;

    private Turret _turret;
    private Drone _drone;

    //Create all of these variables, most of them are self explanatory, but for the ones that aren't i've added a comment to clue you in on what they do
    //for the ones that you dont understand dont worry, just follow along
    void Start()
    {
        transform.AddComponent<MeshRenderer>().material = VisionConeMaterial;
        MeshFilter_ = transform.AddComponent<MeshFilter>();
        VisionConeMesh = new Mesh();
        VisionAngle *= Mathf.Deg2Rad;

        if (_visionType == VisionType.Turret)
            _turret = transform.parent.GetComponent<Turret>();
        else if (_visionType == VisionType.Drone)
            _drone = transform.parent.GetComponent<Drone>();
    }

    
    void Update()
    {
        playerSpotted = false;
        DrawVisionCone();
        ChangeVisionColor(playerSpotted);
    }

    void DrawVisionCone()
    {
        int[] triangles = new int[(VisionConeResolution - 1) * 3];
        Vector3[] Vertices = new Vector3[VisionConeResolution + 1];
        Vertices[0] = Vector3.zero;
        float Currentangle = -VisionAngle / 2;
        float angleIcrement = VisionAngle / (VisionConeResolution - 1);
        float Sine;
        float Cosine;
    
        for (int i = 0; i < VisionConeResolution; i++)
        {
            Sine = Mathf.Sin(Currentangle);
            Cosine = Mathf.Cos(Currentangle);
            Vector3 RaycastDirection = (transform.forward * Cosine) + (transform.right * Sine);
            Vector3 VertForward = (Vector3.forward * Cosine) + (Vector3.right * Sine);
    
            // Önce duvar var mı diye kontrol et Önemli!: VisionObstructingLayer da player ve obstacle seçili hata varsa player ı çıkart.
            if (Physics.Raycast(transform.position, RaycastDirection, out RaycastHit hit, VisionRange, VisionObstructingLayer))
            {
                // Duvar varsa, bu mesafede bir şey göremez
                Vertices[i + 1] = VertForward * hit.distance;
    
                // Duvarın arkasında oyuncu var mı diye kontrol et
                if (Physics.Raycast(transform.position, RaycastDirection, out RaycastHit playerHit, hit.distance, LayerMask.GetMask("Player")))
                {
                    playerSpotted = true;
    
                    DoSomething(playerHit.transform);
                }
            }
            else
            {
                // Duvar yoksa, direkt oyuncu var mı diye kontrol et
                if (Physics.Raycast(transform.position, RaycastDirection, out RaycastHit playerHit, VisionRange, LayerMask.GetMask("Player")))
                {
                    playerSpotted = true;
                    Vertices[i + 1] = VertForward * playerHit.distance;
    
                    DoSomething(playerHit.transform);
                }
                else
                {
                    Vertices[i + 1] = VertForward * VisionRange;
                }
            }
    
            Currentangle += angleIcrement;
        }
    
        for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j + 2;
        }
    
        VisionConeMesh.Clear();
        VisionConeMesh.vertices = Vertices;
        VisionConeMesh.triangles = triangles;
        MeshFilter_.mesh = VisionConeMesh;
    }

    public void ChangeVisionColor(bool playerSpotted)
    {
        if (playerSpotted)
            VisionConeMaterial.color = _spottedColor;
        else
            VisionConeMaterial.color = _normalColor;
    }

    private void DoSomething(Transform info)
    {
        if(_visionType == VisionType.Turret)
            _turret.PlayerSpotted(info.transform);
        else if (_visionType == VisionType.Drone)
            _drone.PlayerSpotted();
    }
}
