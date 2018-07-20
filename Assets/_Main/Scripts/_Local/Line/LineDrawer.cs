using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

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

        protected float m_ScreenOffsetError = 0.1f;


        protected Line m_CurrentLine;
        protected List<Line> m_Lines = new List<Line>();
        protected DefaultObjectPool m_LinePool;

        private Camera mainCamera;


        // Use this for initialization
        void Awake()
        {

            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            mainCamera = Camera.main;

            m_LinePool = DefaultObjectPool.GetObjectPool(linePrefab.gameObject, 2);

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

            CheckOutOfViewLine();

        }


        protected void CheckOutOfViewLine()
        {
            for (int i = 0; i < m_Lines.Count; i++)
            {
                //Return to pool when out of view
                if (mainCamera.transform.position.x - mainCamera.GetHalfCameraWidth() - m_Lines[i].CalculateRightestPoint().x > m_ScreenOffsetError)
                {
                    m_Lines[i].RemoveSelf();
                    m_Lines.Remove(m_Lines[i]);
                }
            }

        }


        public void StartDrawing()
        {
            if (m_CurrentLine == null)
            {

                DefaultPoolObject poolObject = m_LinePool.Pop(Vector3.zero);
                m_CurrentLine = poolObject.transform.GetComponent<Line>();
                m_CurrentLine.linePoolObject = poolObject;
                m_Lines.Add(m_CurrentLine);
            }
            else
            {
                m_CurrentLine.ReInitialize();
            }

        }


        public void AddDrawingPoint()
        {

            if (m_CurrentLine == null) return;


            Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = 0;
            m_CurrentLine.AddDrawingPoint(newPosition);

        }

        public void EndDrawing()
        {
            if (m_CurrentLine == null) return;

            m_CurrentLine.EndDrawing();

            m_CurrentLine = null;
        }

        //Freeze all lines by setting those lines to static
        public void FreezeAllLines()
        {
            for (int i = 0; i < m_Lines.Count; i++)
            {
                m_Lines[i].LineBody2D.bodyType = RigidbodyType2D.Kinematic;
            }
        }

        public bool IsAllLinesSleeping()
        {
            for (int i = 0; i < m_Lines.Count; i++)
            {
                if (!m_Lines[i].LineBody2D.IsSleeping())
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

        public Vector3 CalculateHighestPoint()
        {
            if (m_Lines.Count == 0 || m_Lines[0].RendereredLine.positionCount == 0)
            {
                Debug.LogError("Nothing in the list");
            }

            Vector3 max = m_Lines[0].transform.TransformPoint(m_Lines[0].RendereredLine.GetPosition(0));

            for (int i = 0; i < m_Lines.Count; i++)
            {

                for (int j = 0; j < m_Lines[i].RendereredLine.positionCount; j++)
                {

                    Vector3 currentPoint = m_Lines[i].transform.TransformPoint(m_Lines[i].RendereredLine.GetPosition(j));

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
