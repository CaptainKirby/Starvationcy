Shader "Custom/Vertex Colored DiffuseOutline" {

Properties {

    _Color ("Main Color", Color) = (1,1,1,1)

    _MainTex ("Base (RGB)", 2D) = "white" {}

 _EdgeColor ("Edge Color", Color) = (0,0,0,1)

        _EdgeWidth ("Edge width", Range (0, 1)) = .005
}

 

SubShader {

    Tags { "RenderType"="Opaque" }

    LOD 150

 //Edge detection pass
Pass {
Lighting Off Fog { Mode Off }
Cull Front

CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest

fixed4 _EdgeColor;
fixed _EdgeWidth;

struct a2v {
float4 vertex : POSITION;
float3 normal : NORMAL;
};

struct v2f {
float4 Pos : POSITION;
};

v2f vert(a2v v) {
v2f o;
o.Pos = mul(UNITY_MATRIX_MVP, v.vertex);
fixed3 norm = mul((float3x3)UNITY_MATRIX_MV, v.normal);
norm.x *= UNITY_MATRIX_P[0][0];
norm.y *= UNITY_MATRIX_P[1][1];
o.Pos.xy += norm.xy * _EdgeWidth;
return o;
}

half4 frag(v2f i) : COLOR {
return _EdgeColor;
}
ENDCG
}

CGPROGRAM

#pragma surface surf Lambert vertex:vert

 

sampler2D _MainTex;

fixed4 _Color;

 

struct Input {

    float2 uv_MainTex;

    float3 vertColor;

};

 

void vert (inout appdata_full v, out Input o) {

    UNITY_INITIALIZE_OUTPUT(Input, o);

    o.vertColor = v.color;

}

 

void surf (Input IN, inout SurfaceOutput o) {

    fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

    o.Albedo = c.rgb * IN.vertColor;

    o.Alpha = c.a;

}

ENDCG

}

 

Fallback "Diffuse"

}