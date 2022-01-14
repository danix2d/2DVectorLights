
Shader "Custom/Light2D/Light"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_cenFalloff("Center", float) = 1.55
		_Falloff("Falloff", float) = 4
		_Falloff2("Falloff2", float) = 1
		_RefValue("_RefValue", int) = 1
		_StencilMask("Mask Layer", int) = 1
		[Enum(CompareFunction)] _StencilComp("Mask Mode", Int) = 6

	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Geometry" 
			"IgnoreProjector"="True" 
			"RenderType"="Opaque" 
			"PreviewType"="Quad"
			"CanUseSpriteAtlas"="True"
		}

		Cull Back
		Lighting Off
		ZWrite Off
		ZTest Off
		AlphaTest Off
		Blend OneMinusDstColor One


		Pass
		{

		Stencil{
			Ref [_RefValue]
			ReadMask [_StencilMask]
			Comp [_StencilComp]
		}
		CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				float4 modelPos : TEXCOORD1;
			};
			
			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.modelPos = IN.vertex;
				return OUT;
			}

			fixed4 _Color;
			fixed _cenFalloff;
			fixed _Falloff;
			fixed _Falloff2;

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = _Color;
				
				fixed distFalloff = max(0.0f,length(IN.modelPos.xy)) * _Falloff2;
				distFalloff = clamp(distFalloff,0.0f,1);
				distFalloff = pow(1.0f - distFalloff,_Falloff);

				c.rgb *= distFalloff * _cenFalloff;

				return c;
			}
		ENDCG
		}
	}
}
