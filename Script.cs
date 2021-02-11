using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script : MonoBehaviour
{

    
    // Start is called before the first frame update
    void Start()
    {
       //Colores a usar
       Color moradoOscuro = RGBToPercent(new Vector3(130,9,217));
       Color gris = RGBToPercent(new Vector3(93,92,97));
       Color grisClaro = RGBToPercent(new Vector3(115,149,174));
       Color negro = Color.black;
       Color azul = RGBToPercent(new Vector3(13,163,255));
       Color rojo = RGBToPercent(new Vector3(255,29,0));
       Color verde = RGBToPercent(new Vector3(20,166,43));
       Color amarillo = RGBToPercent(new Vector3(255,229,0));
       Color cafe = RGBToPercent(new Vector3(100,0,0));
       
       //Rotaciones
       Quaternion ejexM = Quaternion.Euler(90f,0f,0f);
       Quaternion ejexm = Quaternion.Euler(-90f,0f,0f);
       Quaternion ejezM = Quaternion.Euler(0f,0f,90f);
       Quaternion ejezm = Quaternion.Euler(0f,0f,-90f);
       Quaternion ejeyM = Quaternion.Euler(0f,90f,0f);
       Quaternion ejeym = Quaternion.Euler(0f,-90f,0f);

       //Vectores utiles
       Vector3 origen = new Vector3(0,0,0);
       Vector3 mxmz = new Vector3(-15,0,-15);
       Vector3 mxMz = new Vector3(-15,0,15);
       Vector3 Mxmz = new Vector3(15,0,-15);
       Vector3 MxMz = new Vector3(15,0,15);

       //Dibujo del Entorno
       GameObject cuarto = crearParedes(origen,60, gris);


       //Dibujo de los tapetes
       


       //Dibujo del altar
       VerticesAltar(origen, 1f, 1.5f, 2.25f, 3f, 1, 1, 128, grisClaro, moradoOscuro, negro, 8, 0);
       VerticesAltar(mxmz, 1f, 1.5f, 2.25f, 3f, 1, 1, 128, grisClaro, azul, negro, 5, 1);
       VerticesAltar(mxMz, 1f, 1.5f, 2.25f, 3f, 1, 1, 128, grisClaro, rojo, negro, 5, 2);
       VerticesAltar(Mxmz, 1f, 1.5f, 2.25f, 3f, 1, 1, 128, grisClaro, verde, negro, 5, 3);
       VerticesAltar(MxMz, 1f, 1.5f, 2.25f, 3f, 1, 1, 128, grisClaro, amarillo, negro, 5, 4); 
       

       
       //Dibujo del Planeta
       VerticesPlaneta(new Vector3(0,6,0), 2.5f, 90, 180, moradoOscuro);
       GameObject anillo1 = verticesAnillos(origen, 2.75f, 5f, negro, 180);
       GameObject anillo2 = verticesAnillos(origen, 2.75f, 5f, negro, 180);

       anillo1.transform.rotation = Quaternion.Slerp(anillo1.transform.rotation,ejezM,0.3f);
       anillo2.transform.rotation = Quaternion.Slerp(anillo2.transform.rotation,ejezm,0.3f);
       anillo1.transform.position += new Vector3(0,6,0);
       anillo2.transform.position += new Vector3(0,6,0);
    }

    GameObject crearParedes(Vector3 origen,float lado, Color color){
        Vector3[] vert = new Vector3[8];
        float coord = (float) lado/2f;
        vert[0] = new Vector3(coord,0,coord);
        vert[1] = new Vector3(coord,0,-coord);
        vert[2] = new Vector3(-coord,0,-coord);
        vert[3] = new Vector3(-coord,0,coord);
        vert[4] = new Vector3(coord,15,coord);
        vert[5] = new Vector3(coord,15,-coord);
        vert[6] = new Vector3(-coord,15,-coord);
        vert[7] = new Vector3(-coord,15,coord);

        int[] tri = new int[] {0,1,3,1,2,3,1,5,6,1,6,2,2,6,7,2,7,3,3,7,4,3,4,0,0,4,5,0,5,1,5,7,6,4,7,5};
        return dibujarMesh(vert, tri, color);
    }

    GameObject verticesCilindro(Vector3 origen, int n, float r, float h, Color color){
        Vector3[] vert = new Vector3[2+2*n];
        vert[0] = PolarToCartesian(new Vector3(0, 0, h), origen);
        vert[1] = PolarToCartesian(new Vector3(0, 0, 0), origen);
        float delta = (float)(2*System.Math.PI/n);
        for(int i = 2;i<n+2;i++){
            float theta = i*delta;
            vert[i] = PolarToCartesian(new Vector3(r, theta, h), origen);
        }
        for(int i = n+2;i<2*n+2;i++){
            float theta = i*delta;
            vert[i] = PolarToCartesian(new Vector3(r, theta, 0), origen);
        }
        return triCilindro(vert,n,color);
    }

    GameObject triCilindro(Vector3[] vert, int n, Color color){
        int[] tri = new int[12*n];

        int j = n+1;
        for(int i = 0;i<n*3;i++){
            if(i%3==0){
                tri[i]=0;
            }else if(i%3==1){
                tri[i] = j;
                j--;
            }else{
                if(j==1){
                    j=n+1;
                }
                tri[i] = j;
            }
        }
        
        j = n+2;
        for(int i=3*n;i<6*n;i++){
            if(i%3==0){
                tri[i] = 1;
            }else if(i%3==1){
                tri[i] = j;
                j++;
            }else{
                if(j==(2*n+2)){
                    j=n+2;
                }
                tri[i] = j;
            }
        }

        int inf = n+2;
        int sup = 2;
        for(int i = 6*n;i<9*n;i++){
            if(i%3==0){
                tri[i] = inf;
                inf++;
            }else if(i%3==1){
                tri[i] = sup;
                sup++;
            }else{
                if(inf == 2*n+2){
                    inf = n+2;
                }
                tri[i] = inf;
            }
        }

        sup = n+1;
        inf = 2*n+1;
        for(int i = 9*n;i<12*n;i++){
            if(i%3==0){
                tri[i] = sup;
                sup--;
            }else if(i%3==1){
                tri[i] = inf;
                inf--;
            }else{
                if(sup==1){
                    sup = n+1;
                }
                tri[i] = sup;
            }
        }
        return dibujarMesh(vert, tri, color);
    }
    GameObject verticesAnillos(Vector3 origen, float rin, float rout, Color color, int n){
        Vector3[] vert = new Vector3[2*n];
        float delta = (float)(2*System.Math.PI/n);
        for(int i = 0; i <n;i++){
            float theta = delta*i;
            vert[i] =PolarToCartesian(new Vector3(rout, theta, 0), origen);
        }
        for(int i = n;i<2*n;i++){
            float theta = delta*i;
            vert[i] = PolarToCartesian(new Vector3(rin, theta,0),origen);
        }
        //vertices = vert;
        return triAnillos(vert,color,n);
    }
    
    GameObject triAnillos(Vector3[] vert, Color color, int n){
        int[] tri = new int[12*n];
        int inf = n;
        int sup = 0;

        //Triangulos base hacia el centro
        for(int i=0;i<3*n;i++){
            if(i%3==0){
                tri[i] = inf;
                tri[12*n-i-1] = inf;
                inf++;
            }else if(i%3==1){
                tri[i] = sup;
                tri[12*n-i-1] = sup;
                sup++;
            }else {
                if(inf == (2*n)){
                    inf = n;
                }
                tri[i] = inf;
                tri[12*n-i-1] = inf;
            }
        }
        //Triangulos base hacia afuera

        inf = 2*n-1;
        sup = n-1;
        for(int i = 3*n;i<6*n;i++){
            if(i%3==0){
                tri[i] = sup;
                tri[12*n-i-1] = sup;
                sup--;
            }else if(i%3==1){
                tri[i] = inf;
                tri[12*n-i-1] = inf;
                inf--;
            }else{
                if(sup<0){
                    sup = n-1; 
                }
                tri[i] = sup;
                tri[12*n-i-1] = sup;
            }
        }
        return dibujarMesh(vert,tri,color);

    }
    GameObject VerticesPlaneta(Vector3 origen, float r, int np, int nt, Color color){
        Vector3[] vert = new Vector3[2+nt*(np-1)];
        
        vert[0] = SphericalToCartesian(new Vector3(r,0,0), origen);
        vert[1] = SphericalToCartesian(new Vector3(r,0,(float)System.Math.PI), origen);
        float dp = (float)((System.Math.PI)/np);
        float dt = (float)((2*System.Math.PI)/nt);
        int c = 2;
        for(int i = 0;i<np-1;i++){
            float phi = (i+1)*dp;
            for(int j = 0;j<nt;j++){
                float theta = j*dt;
                vert[c] = SphericalToCartesian(new Vector3(r,theta,phi), origen);
                c++;
            }
        }
        //
        
       return triPlaneta(vert,np, nt, color);
       //return null;
    }

    


    GameObject triPlaneta(Vector3[] vert, int np, int nt, Color color) {
        int[] tri = new int[6*nt*(np-1)];

        //Triangulos de la parte de arriba de la esfera
        int j = nt+1;
        for(int i=0;i<3*nt;i++){
            if(i%3==0){
                tri[i] = 0; 
            }else if(i%3==1){
                tri[i] = j;
                j--;
            }else{
                if(j==1){
                    j=nt+1;
                }
                tri[i] = j;
            }
        }
        int t = 3*nt;
        //Triangulos centrales
        for(int p = 0;p<(np-2);p++){
            //Base Abajo
            int inf = (p+1)*nt+2;
            int sup = p*nt+2;
            for(int i= 0; i<3*nt ;i++){
                if(t%3==0){
                    tri[t] = inf;
                    inf++;
                    t++;
                }else if(t%3==1){
                    tri[t] = sup;
                    sup++;
                    t++;
                }else{
                    if(inf==(2+nt*(p+2))){
                        inf = (p+1)*nt+2;
                    }
                    tri[t] = inf;
                    t++;
                }
            }
            sup = (p+1)*nt+1;
            inf = (p+2)*nt+1;
            for(int i = 0; i<3*nt;i++){
                if(t%3==0){
                    tri[t] = sup;
                    t++;
                    sup--;
                }else if(t%3==1){
                    tri[t] = inf;
                    inf--;
                    t++;
                }else{
                    if(sup == nt*p+1){
                        sup = (p+1)*nt+1;
                    }
                    tri[t] = sup;
                    t++;
                }
            }
            //Base Arriba
        }

        j = 2+nt*(np-2);
        for(int i = 3*nt*(2*np-3);i<(6*nt*(np-1));i++){
            if(i%3==0){
                tri[i] = 1;
            }else if(i%3==1){
                tri[i] = j;
                j++;
            }else{
                if(j==(2+nt*(np-1))){
                    j = 2+nt*(np-2);
                }
                tri[i] = j;
            }
        }
        return dibujarMesh(vert, tri, color);
    }
    /*
     *Convierte el Vector3 de coordenadas esfericas en un Vector3 de coordenadas cartesianas. esferica = (r, t, p).
     */
    Vector3 SphericalToCartesian(Vector3 sph, Vector3 orgn){
        float r1 =(float)((sph.x)*System.Math.Cos((System.Math.PI/2)-sph.z));
        float x = (float)((r1)*System.Math.Cos(sph.y));
        float y = (float)((sph.x)*System.Math.Cos(sph.z));
        float z = (float)((r1)*System.Math.Sin(sph.y));
        x += orgn.x;
        y += orgn.y;
        z += orgn.z;
        return new Vector3(x,y,z);
    }

    Color RGBToPercent(Vector3 colores){
        Vector3 p = new Vector3((float)(colores.x/255),(float)(colores.y/255),(float)(colores.z/255));
        return new Color(p.x,p.y,p.z);
    }

    

    /*
     * Convierte el vector 3 de coordenadas polares en un Vector3 de coordenadas cartesianas y mueve las coordenadas al nuevo origen
     */
    Vector3 PolarToCartesian(Vector3 polar, Vector3 origen){
        float x = (float)((polar.x)*(System.Math.Cos(polar.y)));
        float z = (float)((polar.x)*(System.Math.Sin(polar.y)));
        float y = (float)(polar.z);
        return new Vector3(x +origen.x,y + origen.y,z + origen.z);
    }

    /*
     * Cuadra los vertices de un tapete en el piso de la escena, en el centro, y los guarda en un arreglo de Vector3 llamado vert. Tambien pasa el color
     */
    GameObject verticesTapete(Vector3 origen, float lado, Color color, float elevacion){
        Vector3[] vert = new Vector3[4];
        vert[0] = (new Vector3(-lado/2,elevacion,-lado/2)) + origen;
        vert[1] = (new Vector3(lado/2,elevacion,-lado/2)) + origen;
        vert[2] = (new Vector3(-lado/2,elevacion,lado/2)) + origen;
        vert[3] = (new Vector3(lado/2,elevacion,lado/2)) + origen;
        return triangulosTapete(vert, color);
    }

    /*
     * Crea los vertices de un Segundo Tapete más grande que el anterior
     */
    GameObject verticesTapete2(Vector3 origen, float lado, Color color, float elevacion){
        Vector3[] vert = new Vector3[4];
        vert[0] = (new Vector3(-lado,elevacion,0)) + origen;
        vert[1] = (new Vector3(0,elevacion,-lado)) + origen;
        vert[2] = (new Vector3(0,elevacion,lado)) + origen;
        vert[3] = (new Vector3(lado,elevacion,0)) + origen;
        return triangulosTapete(vert, color);
    }
    
    /*
     * Crea los vertices de un tapete circular
     */
    GameObject verticesTapeteCircular(Vector3 origen, float radio, int n, Color color, float altura){
        Vector3[] vert = new Vector3[n+1];
        vert[0] = PolarToCartesian(new Vector3(0,0,altura), origen);
        float delta = (float)(2*System.Math.PI/n);
        for(int i = 1;i<=n;i++){
            vert[i] = PolarToCartesian(new Vector3(radio, i*delta, altura), origen);
        }
        return triTapeteCircular(vert, color);
    }


    /*
     * Crea los triangulos del tapete circular
     */
    GameObject triTapeteCircular(Vector3[] vert, Color color){
        int[] tri = new int[3*(vert.Length-1)];
        int j = vert.Length-1;
        int t = 1;
        for(int i = 0;i<3*(vert.Length-1);i++)
        {
            if(i%3==0){
                tri[i] = 0;
            }
            else if(i%3==1){
                tri[i] = j;
                j--;
            }
            else{
                if(j==(0)){
                    j = vert.Length-1;
                }
                tri[i] = j;
                //Debug.Log("Agregado triangulo " + t + ": " + tri[i-2] + "("+vert[tri[i-2]] +"), " + tri[i-1] + "("+vert[tri[i-1]] +"), " + tri[i] + "("+vert[tri[i]] +")");
                t++;
            }
        }
        return dibujarMesh(vert, tri, color);
    }

    /*
     * Toma los vertices del tapete y guarda en un arreglo de enteros, tri, los triangulos para formar el tapete. Tambien pasa el color
     */
    GameObject triangulosTapete(Vector3[] vert, Color color){
        int[] tri = new int[6];
        tri[0] = 0;
        tri[1] = 2;
        tri[2] = 1;
        tri[3] = 1;
        tri[4] = 2;
        tri[5] = 3;
        return dibujarMesh(vert, tri, color);
    }

    /*
     * Dibuja el mesh del objeto
     */
    GameObject dibujarMesh(Vector3[] vert, int[] tri, Color color) {
        GameObject obj = new GameObject("obj", typeof(MeshFilter), typeof(MeshRenderer));
        Mesh mesh = new Mesh();
        obj.GetComponent<Renderer>().material.color = color;
        obj.GetComponent<MeshFilter>().mesh = mesh;
        mesh.Clear();
        mesh.vertices = vert;
        mesh.triangles = tri;
        mesh.RecalculateNormals();
        return obj;
    }


    /*
     * Procedimiento de creación de unn mago
     */
    void crearMago(Vector3 origen, float r, float h, Color color, int n, int id){
        Vector3[] vert = new Vector3[2+2*n];
        vert[0] = PolarToCartesian(new Vector3(0, 0, h), origen);
        vert[1] = PolarToCartesian(new Vector3(0, 0, 0), origen);
        float delta = (float)(2*System.Math.PI/n);
        for(int i = 2;i<n+2;i++){
            float theta = i*delta;
            vert[i] = PolarToCartesian(new Vector3(r, theta, h), origen);
        }
        for(int i = n+2;i<2*n+2;i++){
            float theta = i*delta;
            vert[i] = PolarToCartesian(new Vector3(r, theta, 0), origen);
        }

        GameObject bd = verticesCilindro((new Vector3(0,0,0)),n,r/5,h, color);
        GameObject bi = verticesCilindro((new Vector3(0,0,0)),n,r/5,h, color);
        Vector3 dir = (new Vector3(0,0,0))-origen;
        Quaternion objetivo = darOrientacion(id);
        Quaternion act = Quaternion.Euler(0,90f,0);
        bd.transform.rotation = Quaternion.Slerp(act, objetivo,90f);
        bi.transform.rotation = Quaternion.Slerp(act, objetivo,90f);
        dir-= new Vector3(0,.9f*h,0);
        bd.transform.position -= dir;
        bi.transform.position -= dir;
        Vector3 mov = magnitudVector(new Vector3(dir.z,0,-dir.x), 1.2f*r);
        bi.transform.position +=mov;
        bd.transform.position -= mov;
        GameObject hom1 = VerticesPlaneta(origen+mov+new Vector3(0,.9f*h,0),r/5,n,n,color);
        GameObject hom2 = VerticesPlaneta(origen-mov+new Vector3(0,.9f*h,0),r/5,n,n,color);
        GameObject cab = VerticesPlaneta(origen + new Vector3(0,1.5f*h,0),r,n,n,Color.white);
        GameObject aureola = verticesAnillos(origen+ new Vector3(0,2f*h,0), r, 1.25f*r, color, n);
        GameObject ataque = VerticesPlaneta(origen+magnitudVector(dir,2f*h)+ new Vector3(0,h*1.25f,0),r, n, n, color);
        triPlaneta(vert,2,n, color);
    }

    /*
     * Método que dado un vector u, retorna un vector en su misma dirección pero con magnitud n
     */ 
    Vector3 magnitudVector(Vector3 u, float n){
        float m = (float) System.Math.Sqrt(u.x*u.x+u.y*u.y+u.z*u.z);
        return new Vector3(n*u.x/m,n*u.y/m,n*u.z/m);
    }

    /*
     * Retorna la orientación de giro para los brazos de los magos
     */
    Quaternion darOrientacion(int id){
        Quaternion respuesta;
        if(id == 1){
            respuesta = Quaternion.Euler(90f,45f,0);
        }else if(id == 2){
            respuesta = Quaternion.Euler(0,45f,-90f);
        }else if(id == 3){
            respuesta = Quaternion.Euler(0,45f,90f);
        }else{
            respuesta = Quaternion.Euler(-90f,45f,0);
        }
        return respuesta;
    }

    /*
     * Crea las velas de los altares
     */
    void crearVelas(Vector3 origen, float rp, float rv, float h, float elevacion, Color llama, int n, int cantidad){
        Color cera = RGBToPercent(new Vector3(228,235,241));
        float delta = (float)(2*System.Math.PI/cantidad);
        float rv2 = rv/5;
        float h2 = h/5;
        for(int i = 0;i<cantidad;i++){

            Vector3 posActual = PolarToCartesian(new Vector3(rp,delta*i,elevacion), origen);
            verticesCilindro(posActual,n, rv, h, cera);

            verticesCilindro((posActual+=new Vector3(0,h,0)),n,rv2,h2,new Color(0,0,0));
            VerticesPlaneta((posActual+=new Vector3(0,h2,0)), (rv+rv2)/2, n, n, llama);

        }
    }


    /*
     * Cuadra los vertices del altar
     */
    GameObject VerticesAltar(Vector3 origen, float r1, float r2, float r3, float r4, float h1, float h2, int n, Color color, Color colorPrincipal, Color colorSecundario, int velas, int id) {
        Vector3[] vert = new Vector3[4*n+2];
        vert[0] = PolarToCartesian(new Vector3(0,0,h1+h2),origen);
        vert[1] = PolarToCartesian(new Vector3(0,0,h2),origen);
        float delta = (float)(2*System.Math.PI/n);
        
        //Vertices r1
        for (int i = 2; i < (n+2); i++)
        {
            vert[i] = PolarToCartesian(new Vector3(r1,i*delta,h1+h2), origen);
        }
        //Vertices r3
        for (int i = n+2; i < (2*n+2); i++)
        {
            vert[i] = PolarToCartesian(new Vector3(r3,i*delta,h2), origen);
        }
        //Vertices r2
        for (int i = (2*n+2); i < (3*n+2); i++)
        {
            vert[i] = PolarToCartesian(new Vector3(r2,i*delta,h2), origen);
        }
        //Vertices r4
        for (int i = (3*n+2); i < (4*n+2); i++)
        {
            vert[i] = PolarToCartesian(new Vector3(r4,i*delta,0), origen);
        }


       verticesTapeteCircular(origen, r3, n, colorSecundario,1.002f);
       verticesTapeteCircular(origen, (r3+r2)/2, n, colorPrincipal,1.004f);
       verticesTapeteCircular(origen, r1, n, colorSecundario, 2.002f);
       verticesTapeteCircular(origen, r1*0.9f, n, colorPrincipal,2.004f);
       verticesTapeteCircular(origen, r1*0.5f, n, colorSecundario, 2.006f);

       //Tapetes del piso
       verticesTapete(origen, 6, colorPrincipal, 0.016f);
       verticesTapete2(origen, 6, colorSecundario, 0.012f);
       verticesTapeteCircular(origen, 6, n, colorPrincipal, 0.008f);
       verticesTapeteCircular(origen, 6.5f, n, colorSecundario, 0.004f);
       
       float rp = (r2+r3)/2;
       float rv = 0.25f*(r3-r2);
       float h = h1*0.9f;
       if(id != 0){
         crearVelas(origen,rp,rv,h,h2,colorPrincipal,n,velas);
         crearMago((origen += new Vector3(0,h1+h2,0)), (r2+r3)/2, 2*(h1+h2),colorPrincipal, n, id);
       }

       return triAltar(vert, color, n);
    }


    /*
     * Dibuja los Triángulos de un altar.
     */
    GameObject triAltar(Vector3[] vert, Color color, int n){
        int[] tri = new int[3*6*n];

        //triangulos r1
        int j = n+1;
        int t = 0;
        for(int i = 0;i<3*n;i++){
            if(i%3==0){
                tri[i]=0;
            }else if(i%3==1){
                tri[i] = j;
                j--;
            }else{
                if(j==1){
                    j = n+1;
                }
                tri[i] = j;
                t++;
            }
        }

        //triangulos r3
        j = 2*n+1;
        for(int i = 3*n;i<6*n;i++){
            if(i%3==0){
                tri[i]=1;
            }else if(i%3==1){
                tri[i] = j;
                j--;
            }else{
                if(j==(n+1)){
                    j = 2*n+1;
                }
                tri[i] = j;
                t++;
            }
        }

        //triangulos r1-r2
        //Base en r3
        int inf = 2*n+2;
        int sup = 2;
        for(int i = 6*n;i<9*n;i++){
            if(i%3==0){
                tri[i] = inf;
                inf++;
            }else if(i%3==1){
                tri[i] = sup;
                sup++;
            }else{
                if(inf==(3*n+2)){
                    inf = 2*n+2;
                }
                tri[i] = inf;
                t++;
            }
        }
        //Base en r1
        sup = n+1;
        inf = 3*n+1;
        for(int i = 9*n;i<12*n;i++){
            if(i%3==0){
                tri[i] = sup;
                sup--;
            }else if(i%3==1){
                tri[i] = inf;
                inf--;
            }else{
                if(sup==1){
                    sup = n+1;
                }
                tri[i] = sup;
                t++;
            }
        }

        //Triangulos r3-r4
        //Base en piso
        inf = 3*n+2;
        sup = n+2;
        for(int i = 12*n;i<15*n;i++){
            if(i%3==0){
                tri[i] = inf;
                inf++;
            }else if(i%3==1){
                tri[i] = sup;
                sup++;
            }else{
                if(inf==(4*n+2)){
                    inf = 3*n+2;
                }
                tri[i] = inf;
                t++;
            }
        }
        //Base en r1
        sup = 2*n+1;
        inf = 4*n+1;
        for(int i = 15*n;i<18*n;i++){
            if(i%3==0){
                tri[i] = sup;
                sup--;
            }else if(i%3==1){
                tri[i] = inf;
                inf--;
            }else{
                if(sup==(n+1)){
                    sup = 2*n+1;
                }
                tri[i] = sup;
                t++;
            }
        }
        
        return dibujarMesh(vert, tri, color);
    }



  
}
