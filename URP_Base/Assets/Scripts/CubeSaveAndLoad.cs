using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class CubeSaveAndLoad : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject CubePrefab;
    
    private List<Transform> cubes = new List<Transform>();
    
    public struct TransformData
    {
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        
        public float AngleX { get; set; }
        public float AngleY { get; set; }
        public float AngleZ { get; set; }

        public Vector3 Position => new Vector3(PositionX, PositionY, PositionZ);
        public Quaternion Rotation => Quaternion.Euler(new Vector3(AngleX, AngleY, AngleZ));
        
        public TransformData(Transform transform)
        {
            PositionX = transform.position.x;
            PositionY = transform.position.y;
            PositionZ = transform.position.z;
            
            AngleX = transform.eulerAngles.x;
            AngleY = transform.eulerAngles.y;
            AngleZ = transform.eulerAngles.z;
        }
        
        public TransformData(string rawData)
        {
            string[] values = rawData.Split(',');
            PositionX = float.Parse(values[0]);
            PositionY = float.Parse(values[1]);
            PositionZ = float.Parse(values[2]);
            
            AngleX = float.Parse(values[3]);
            AngleY = float.Parse(values[4]);
            AngleZ = float.Parse(values[5]);
        }
        
        public string ToCSV()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(PositionX.ToString("F3"));
            sb.Append(",");
            sb.Append(PositionY.ToString("F3"));
            sb.Append(",");
            sb.Append(PositionZ.ToString("F3"));
            sb.Append(",");
            
            sb.Append(AngleX.ToString("F3"));
            sb.Append(",");
            sb.Append(AngleY.ToString("F3"));
            sb.Append(",");
            sb.Append(AngleZ.ToString("F3"));
            
            return sb.ToString();
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 randEuler = 
                    new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
                var cube = Instantiate(CubePrefab, hit.point, Quaternion.Euler(randEuler));
                cubes.Add(cube.transform);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            List<TransformData> datas = new List<TransformData>();
            for (int i = 0; i < cubes.Count; i++)
            {
                TransformData data = new TransformData(cubes[i].transform);
                datas.Add(data);
            }
          
            StreamWriter sw = new StreamWriter("CubeSaveAndLoad.csv");
            for (int i = 0; i < datas.Count; i++)
            {
                sw.WriteLine(datas[i].ToCSV());
            }
            sw.Close();
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach (var cube in cubes)
            {
                Destroy(cube.gameObject);
            }
            cubes.Clear();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StreamReader sr = new StreamReader("CubeSaveAndLoad.csv");

            while (sr.EndOfStream == false)
            {
                string rawData = sr.ReadLine();
                TransformData data = new TransformData(rawData);
                
                var cube = 
                    Instantiate(CubePrefab, data.Position, data.Rotation);
                cubes.Add(cube.transform);
            }
            
            sr.Close();
        }
    }
}
