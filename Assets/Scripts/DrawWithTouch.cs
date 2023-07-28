using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DrawWithTouch : MonoBehaviour
{
    private LineRenderer currentLine; // Şu anki çizgi
    private List<Vector3> positions; // Çizginin noktaları
    private bool isDrawing; // Çizim yapılıyor mu?
    [SerializeField]
    public Material lineMaterial;

    [SerializeField]
    private float minDistance = 0f;
    public Color lineColor;
    public List<GameObject> lineObjectList;
    public bool held;

    public Slider widthSlider; // Slider nesnesi
    private float minLineWidth = 0.05f; // Minimum çizgi kalınlığı



    void Start()
    {
        positions = new List<Vector3>();
        isDrawing = false;
        lineColor = Color.black;

        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if (held && !isDrawing) // Sol fare düğmesine tıklandığında
        {
            isDrawing = true;

            positions.Clear();
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentPosition.z = 0f;
            positions.Add(currentPosition);

            if (isDrawing) // Eğer çizim yapılıyorsa
            {
                CreateNewLine(); // Yeni bir çizgi oluştur
            }
        }

        if (held && isDrawing) // Sol fare düğmesi basılıysa ve çizim yapılıyorsa
        {
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentPosition.z = 0f;

            if (positions.Count > 0 && Vector3.Distance(currentPosition, positions[positions.Count - 1]) >= minDistance)
            {
                positions.Add(currentPosition);
                UpdateLineRenderer();
            }
        }
        else if (!held && isDrawing) // Sol fare düğmesi bırakıldığında ve çizim yapılıyorsa
        {
            isDrawing = false;
            positions.Clear();

        }

        if (isDrawing)
        {
            UpdateLineRenderer();
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (lineObjectList.Count > 0)
            {
                GameObject lastLineObject = lineObjectList[lineObjectList.Count - 1];
                LineRenderer lastLineRenderer = lastLineObject.GetComponent<LineRenderer>();
                Destroy(lastLineObject);
                lineObjectList.RemoveAt(lineObjectList.Count - 1);
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void SmoothLineRenderer(LineRenderer lineRenderer, List<Vector3> positions, int smoothness)
    {
        List<Vector3> smoothedPositions = new List<Vector3>();

        for (int i = 0; i < positions.Count - 1; i++)
        {
            Vector3 startPoint = positions[i];
            Vector3 endPoint = positions[i + 1];

            for (int j = 0; j < smoothness; j++)
            {
                float t = j / (float)smoothness;
                Vector3 point = Vector3.Lerp(startPoint, endPoint, t);
                smoothedPositions.Add(point);
            }
        }

        lineRenderer.positionCount = smoothedPositions.Count;
        lineRenderer.SetPositions(smoothedPositions.ToArray());
    }

    public void Click()
    {
        held = true;
    }

    public void Release()
    {
        held = false;
        isDrawing = false;
    }

        void CreateNewLine()
        {
            GameObject lineObject = new GameObject("Line");
            LineRenderer newLine = lineObject.AddComponent<LineRenderer>();

            // Set line properties
            newLine.positionCount = positions.Count;
            newLine.startWidth = Mathf.Max(widthSlider.value, minLineWidth); // Kalınlığı slider değeri veya minimum kalınlık değeri olarak ayarla
            newLine.endWidth = Mathf.Max(widthSlider.value, minLineWidth); // Kalınlığı slider değeri veya minimum kalınlık değeri olarak ayarla
            newLine.useWorldSpace = true;
            newLine.numCornerVertices = 5;
            newLine.numCapVertices = 5;

            // Set line color
            Material tempMaterial = Instantiate(lineMaterial);
            tempMaterial.color = lineColor;
            newLine.material = tempMaterial;

            // Set line positions
            newLine.SetPositions(positions.ToArray());

            // Set line sorting order
            Renderer lineRenderer = lineObject.GetComponent<Renderer>();
            lineRenderer.sortingOrder = lineObjectList.Count;

            // Add the line object to the list
            lineObjectList.Add(lineObject);
            currentLine = newLine;

        }

    void UpdateLineRenderer()
    {
        if (currentLine != null && positions.Count > 0)
        {
            

            
            if (currentLine != null && positions.Count > 0)
            {
                currentLine.positionCount = positions.Count;
                if (currentLine.positionCount <= 1)
                {
                    currentLine.positionCount++;
                    currentLine.SetPosition(currentLine.positionCount - 1, currentLine.GetPosition(0));
                }
                currentLine.SetPositions(positions.ToArray());
            }

            int smoothness = 25; // Pürüzsüzlük değerini isteğe bağlı olarak ayarlayın
            SmoothLineRenderer(currentLine, positions, smoothness);

        }
    }
    public void RemoveLine()
       {
        if (lineObjectList.Count > 0)
        {
            GameObject lastLineObject = lineObjectList[lineObjectList.Count - 1];
            LineRenderer lastLineRenderer = lastLineObject.GetComponent<LineRenderer>();
            Destroy(lastLineObject);
            lineObjectList.RemoveAt(lineObjectList.Count - 1);
        }
       }

        public void SetLineColorBlue()
        {  
            lineColor = Color.blue;
        }

    public void SetLineColorRed()
    {
        lineColor = Color.red;
    }

    public void SetLineColorGreen()
    {
        lineColor = Color.green;
    }

        public void SetLineColorYellow()
        {
            lineColor = Color.yellow;
        }

        public void SetLineColorBlack()
        {
            lineColor = Color.black ;
        }

        public void SetLineColorWhite()
        {
            lineColor = Color.white ;
        }

    public void SetLineColorOrange()
    {
          lineColor = new Color(1f, 0.5f, 0f);
    }

    public void SetLineColorPink()
    {
        lineColor = new Color(1f, 0.5f, 0.5f);
    }

    public void SetLineColorPurple()
    {
        lineColor = new Color(0.5f, 0f, 0.5f);
    }

    public void SetLineColorBrown()
    {
        lineColor = new Color(0.6f, 0.4f, 0.2f);
    }
}