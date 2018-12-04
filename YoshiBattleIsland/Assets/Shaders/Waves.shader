Shader "Island/Waves" {
    Properties {
      _MainTex("Diffuse", 2D) = "white" {}
      _Tint("Colour Tint", Color) = (1,1,1,1)
      _Freq("Frequency", Range(0,5)) = 3
      _Speed("Speed",Range(0,100)) = 10
      _Amp("Amplitude",Range(0,1)) = 0
    }
    SubShader {
      CGPROGRAM
      #pragma surface surf Lambert vertex:vert 
      
      struct Input {
          float2 uv_MainTex;
          float3 vertColor;
      };
      
      float4 _Tint;
      float _Freq;
      float _Speed;
      float _Amp;

      struct appdata {
          float4 vertex: POSITION;
          float3 normal: NORMAL;
          float4 texcoord: TEXCOORD0;
      };
      
      void vert (inout appdata v, out Input o) {
          UNITY_INITIALIZE_OUTPUT(Input,o); // clear o          
          float t = _Time * _Speed;
          float waveHeight = sin(t + v.vertex.x * _Freq) * _Amp + sin(t*2 + v.vertex.x * _Freq*2) * _Amp;
          float waveHeightZ = sin(t + v.vertex.z * _Freq) * _Amp + sin(t*2 + v.vertex.z * _Freq*2) * _Amp;
          v.vertex.y = v.vertex.y + waveHeight + waveHeightZ;          
          v.normal = normalize (float3(-cos (t + v.vertex.x * _Freq) * _Amp * _Freq -cos (t*2 + v.vertex.x * _Freq * 2) * _Amp * _Freq * 2, 1, 0));          
          o.vertColor = waveHeight + 2;
      }

      sampler2D _MainTex;
      void surf (Input IN, inout SurfaceOutput o) {
          float4 c = tex2D(_MainTex, IN.uv_MainTex);
          o.Albedo = c * IN.vertColor.rgb;
      }
      ENDCG

    } 
    Fallback "Diffuse"
  }