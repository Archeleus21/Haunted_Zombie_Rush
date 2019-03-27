Shader "Custom/Lava"
{
    Properties
    {
		//textures
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("Bumpmap", 2D) = "bump" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_RimColor("Rim Color", Color) = (0.5, 0.5, 0.5, 0.5)
		_RimPower("Rim Power", Range(0.5, 8.0)) = 1.0
		
		//movement left to right
		_ScrollX("Scroll X", Range(-5,5)) = 0.0
		_ScrollY("Scroll Y", Range(-5,5)) = 0.0

		//waves
		_Freq("Freq", Range(0,5)) = 0.0
		_Speed("Speed", Range(0,100)) = 0.0
		_Amp("Amplitude", Range(0, 1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard vertex:vert
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _BumpMap;

		float4 _RimColor;
		float _RimPower;

		float _ScrollX;
		float _ScrollY;

		float _Freq;
		float _Speed;
		float _Amp;

        struct Input
        {
            float2 uv_MainTex;
			float2 uv_BumpMap;
			float viewDir;

			float4 _vertColor;
        };

		struct appdata
		{
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float4 texcoord : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;

		};

		void vert(inout appdata v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			float t = _Time * _Speed;
			float waveHeight = sin(t + v.vertex * _Freq) * _Amp;

			v.vertex.y = v.vertex.y + waveHeight;
			v.normal = normalize(float3(v.normal.x, v.normal.y + waveHeight, v.normal.z));
			o._vertColor = waveHeight + 2;
		}

        half _Glossiness;
        half _Metallic;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			_ScrollX *= _Time;
			_ScrollY *= _Time;

            // Albedo comes from a texture tinted by color
            float4 c = tex2D(_MainTex, IN.uv_MainTex + float2(_ScrollX, _ScrollY));
            o.Albedo = c * IN._vertColor.rgb;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap + float2(_ScrollX, _ScrollY)));
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
			half rim = .5 - saturate(dot(normalize(IN.viewDir), o.Normal));
			o.Emission = _RimColor.rgb * pow(rim, _RimPower);
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
