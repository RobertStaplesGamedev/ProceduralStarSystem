using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace {
    Mesh mesh;
    int resolution;
    Vector3 localUp;
    
    Vector3 axisA;
    Vector3 axisB;

    ShapeGenerator shapeGenerator;

    public TerrainFace (ShapeGenerator _shapeGenerator, Mesh _mesh, int _resolution, Vector3 _localUp) {
        this.shapeGenerator = _shapeGenerator;
        this.mesh = _mesh;
        this.resolution = _resolution;
        this.localUp = _localUp;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructMesh() {
        Vector3[] verticies = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1)* (resolution - 1) * 6];
        int triIndex = 0;

        for (int y = 0; y < resolution; y++) {
            for (int x = 0; x < resolution; x++) {
                int i = x + y * resolution;
                Vector2 percent = new Vector2 (x,y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                verticies[i] = shapeGenerator.CalculatePointOnPlanet(pointOnUnitSphere);

                if (x != resolution-1 && y != resolution-1) {
                    triangles[triIndex] = i;
                    triangles[triIndex+1] = i+resolution+1;
                    triangles[triIndex+2] = i+resolution;

                    triangles[triIndex+3] = i;
                    triangles[triIndex+4] = i+1;
                    triangles[triIndex+5] = i+resolution+1;

                    triIndex = triIndex + 6;
                }
            }
        }
        mesh.Clear();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}