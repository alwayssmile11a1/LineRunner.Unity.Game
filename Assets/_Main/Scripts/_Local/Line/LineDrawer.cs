using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineRunner
{
    public class LineDrawer : MonoBehaviour
    {

        #region Static
        public static LineDrawer Instance
        {
            get
            {
                if (m_Instance != null) return m_Instance;

                m_Instance = FindObjectOfType<LineDrawer>();

                if (m_Instance != null) return m_Instance;

                //create new
                GameObject gameObject = new GameObject("LineDrawer");
                gameObject = Instantiate(gameObject);
                m_Instance = gameObject.AddComponent<LineDrawer>();

                return m_Instance;
            }

        }

        private static LineDrawer m_Instance;
        #endregion

        public Line linePrefab;

        private Line m_CurrentLine;
        private List<Line> m_Lines = new List<Line>();
        private Camera mainCam;



        // Use this for initialization
        void Awake()
        {

            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            mainCam = Camera.main;
        }


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartDrawing();
            }

            if (Input.GetMouseButton(0))
            {
                AddDrawingPoint();
            }

            if (Input.GetMouseButtonUp(0))
            {
                EndDrawing();
            }


        }


        public void StartDrawing()
        {
            if (m_CurrentLine == null)
            {
                m_CurrentLine = Instantiate(linePrefab);
                m_Lines.Add(m_CurrentLine);
            }
            else
            {
                m_CurrentLine.lineRenderer.positionCount = 0;
            }

        }


        public void AddDrawingPoint()
        {

            if (m_CurrentLine == null) return;


            Vector3 newPosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = 0;
            m_CurrentLine.AddDrawingPoint(newPosition);

        }

        public void EndDrawing()
        {
            switch (m_CurrentLine.lineType)
            {
                case Line.LineType.Dynamic:
                    {
                        m_CurrentLine.rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                        break;
                    }
                case Line.LineType.Kinematic:
                    {
                        m_CurrentLine.rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
                        break;
                    }
                case Line.LineType.Static:
                    {
                        m_CurrentLine.rigidbody2D.bodyType = RigidbodyType2D.Static;
                        break;
                    }

            }

            m_CurrentLine = null;
        }

        //Freeze all lines by setting those lines to static
        public void FreezeAllLines()
        {
            for (int i = 0; i < m_Lines.Count; i++)
            {
                m_Lines[i].rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            }
        }

        public bool IsAllLinesSleeping()
        {
            for (int i = 0; i < m_Lines.Count; i++)
            {
                if (!m_Lines[i].rigidbody2D.IsSleeping())
                {
                    return false;
                }
            }

            return true;
        }

        public List<Line> GetLines()
        {
            return m_Lines;
        }

        public void Clear()
        {
            m_Lines.Clear();
        }

        public Line GetCurrentLine()
        {
            return m_CurrentLine;
        }

        public Vector3 GetHighestPoint()
        {
            if (m_Lines.Count == 0 || m_Lines[0].lineRenderer.positionCount == 0)
            {
                Debug.LogError("Nothing in the list");
            }

            Vector3 max = m_Lines[0].transform.TransformPoint(m_Lines[0].lineRenderer.GetPosition(0));

            for (int i = 0; i < m_Lines.Count; i++)
            {

                for (int j = 0; j < m_Lines[i].lineRenderer.positionCount; j++)
                {

                    Vector3 currentPoint = m_Lines[i].transform.TransformPoint(m_Lines[i].lineRenderer.GetPosition(j));

                    if (max.y < currentPoint.y)
                    {
                        max = currentPoint;
                    }
                }

            }

            return max;
        }
    }

}
