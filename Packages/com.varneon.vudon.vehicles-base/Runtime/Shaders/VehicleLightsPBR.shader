// Made with Amplify Shader Editor v1.9.6.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Varneon/VUdon/Vehicles/VehicleLightsPBR"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("MainTex", 2D) = "white" {}
		[HDR]_EmissionColor("EmissionColor", Color) = (1,1,1,1)
		[Toggle(_COLOR_OVERRIDE)] _COLOR_OVERRIDE("COLOR_OVERRIDE", Float) = 0
		[HDR]_BrakeColor("BrakeColor", Color) = (1,1,1,1)
		[HDR]_TailLightColor("TailLightColor", Color) = (1,1,1,1)
		[HDR]_ReverseColor("ReverseColor", Color) = (1,1,1,1)
		[NoScaleOffset]_EmissionMap("EmissionMap", 2D) = "white" {}
		[NoScaleOffset][Normal]_BumpMap("BumpMap", 2D) = "bump" {}
		[NoScaleOffset]_MetallicGlossMap("MetallicGlossMap", 2D) = "white" {}
		_Metallic("Metallic", Range( 0 , 1)) = 1
		[Toggle(_USE_MASK_TEXTURE)] _USE_MASK_TEXTURE("USE_MASK_TEXTURE", Float) = 0
		_MaskMap("MaskMap", 2D) = "white" {}
		_BrakeIntensity("BrakeIntensity", Float) = 2
		[Toggle(_EMISSION)] _EMISSION("EMISSION", Float) = 1
		_LightData("LightData", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "DisableBatching" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma multi_compile_local __ _EMISSION
		#pragma shader_feature_local _COLOR_OVERRIDE
		#pragma shader_feature_local _USE_MASK_TEXTURE
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred nolightmap  nodynlightmap nodirlightmap 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float3 worldPos;
		};

		uniform sampler2D _BumpMap;
		uniform float4 _Color;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float4 _EmissionColor;
		uniform sampler2D _EmissionMap;
		uniform float _BrakeIntensity;
		uniform float4 _LightData;
		uniform sampler2D _MaskMap;
		uniform float4 _MaskMap_ST;
		uniform float4 _BrakeColor;
		uniform float4 _ReverseColor;
		uniform float4 _TailLightColor;
		uniform float _Metallic;
		uniform sampler2D _MetallicGlossMap;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_BumpMap20 = i.uv_texcoord;
			o.Normal = UnpackNormal( tex2D( _BumpMap, uv_BumpMap20 ) );
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			o.Albedo = ( _Color * tex2D( _MainTex, uv_MainTex ) ).rgb;
			float2 uv_EmissionMap3 = i.uv_texcoord;
			float4 tex2DNode3 = tex2D( _EmissionMap, uv_EmissionMap3 );
			float2 uv_MaskMap = i.uv_texcoord * _MaskMap_ST.xy + _MaskMap_ST.zw;
			#ifdef _USE_MASK_TEXTURE
				float4 staticSwitch43 = tex2D( _MaskMap, uv_MaskMap );
			#else
				float4 staticSwitch43 = i.vertexColor;
			#endif
			float4 break46 = staticSwitch43;
			float temp_output_9_0 = ( _LightData.y * break46.g );
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float4 weightedBlendVar31 = ( _LightData * staticSwitch43 );
			float3 weightedBlend31 = ( weightedBlendVar31.x*( tex2DNode3.rgb * _BrakeColor.rgb ) + weightedBlendVar31.y*( tex2DNode3.rgb * _ReverseColor.rgb ) + weightedBlendVar31.z*( tex2DNode3.rgb * ( ase_vertex3Pos.z > 0.0 ? _EmissionColor.rgb : _TailLightColor.rgb ) ) + weightedBlendVar31.w*float3( 0,0,0 ) );
			#ifdef _COLOR_OVERRIDE
				float3 staticSwitch32 = weightedBlend31;
			#else
				float3 staticSwitch32 = ( _EmissionColor.rgb * tex2DNode3.rgb * max( max( ( ( _BrakeIntensity * _LightData.x ) * break46.r ) , temp_output_9_0 ) , max( temp_output_9_0 , ( _LightData.z * break46.b ) ) ) );
			#endif
			#ifdef _EMISSION
				float3 staticSwitch19 = staticSwitch32;
			#else
				float3 staticSwitch19 = float3( 0,0,0 );
			#endif
			float3 Emission25 = staticSwitch19;
			o.Emission = Emission25;
			float2 uv_MetallicGlossMap21 = i.uv_texcoord;
			float4 tex2DNode21 = tex2D( _MetallicGlossMap, uv_MetallicGlossMap21 );
			o.Metallic = ( _Metallic * tex2DNode21.r );
			o.Smoothness = tex2DNode21.a;
			o.Occlusion = tex2DNode21.g;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19603
Node;AmplifyShaderEditor.SamplerNode;44;-3552,592;Inherit;True;Property;_MaskMap;MaskMap;12;0;Create;True;0;0;0;False;0;False;-1;None;7ace2fdf3c5039f438fc60b4121d5ac1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.VertexColorNode;4;-3456,416;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;43;-3220.013,571.7783;Inherit;False;Property;_USE_MASK_TEXTURE;USE_MASK_TEXTURE;11;0;Create;False;0;0;0;False;0;False;0;0;0;True;_USE_MASK_TEXTURE;Toggle;2;Key0;Key1;Create;True;False;All;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector4Node;5;-2880,288;Inherit;False;Property;_LightData;LightData;15;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;17;-2656,272;Inherit;False;Property;_BrakeIntensity;BrakeIntensity;13;0;Create;True;0;0;0;False;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-2416,288;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;46;-2816,464;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ColorNode;15;-2688,64;Inherit;False;Property;_EmissionColor;EmissionColor;2;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;16,16,16,1;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-2272,416;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-2272,512;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-2272,320;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;40;-2720,992;Inherit;False;Property;_TailLightColor;TailLightColor;5;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;22.62741,0,0,1;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.PosVertexDataNode;41;-2688,848;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Compare;42;-2448,944;Inherit;False;2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;30;-2512,1104;Inherit;False;Property;_BrakeColor;BrakeColor;4;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;64.00001,0,0,1;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.ColorNode;34;-2512,1312;Inherit;False;Property;_ReverseColor;ReverseColor;6;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;32,32,32,1;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SamplerNode;3;-2448,720;Inherit;True;Property;_EmissionMap;EmissionMap;7;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;faf01ad328a82d24aa36b09ee63766cb;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SimpleMaxOpNode;11;-2128,368;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;12;-2128,464;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-2640,576;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;13;-2016,400;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-2016,832;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-2016,736;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-2016,960;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-1712,304;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SummedBlendNode;31;-1728,576;Inherit;False;5;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StaticSwitch;32;-1510.642,442.6296;Inherit;False;Property;_COLOR_OVERRIDE;COLOR_OVERRIDE;3;0;Create;False;0;0;0;False;0;False;0;0;0;True;_COLOR_OVERRIDE;Toggle;2;Key0;Key1;Create;True;False;All;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;21;-336,384;Inherit;True;Property;_MetallicGlossMap;MetallicGlossMap;9;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;4421815cacf80fc4db1ddc028bf5a79c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.StaticSwitch;19;-1248,416;Inherit;False;Property;_EMISSION;EMISSION;14;0;Create;False;0;0;0;False;0;False;1;1;1;True;_EMISSION;Toggle;2;Key0;Key1;Create;True;False;All;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;6;-256,-416;Inherit;False;Property;_Color;Color;0;0;Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SamplerNode;2;-336,-192;Inherit;True;Property;_MainTex;MainTex;1;0;Create;True;0;0;0;False;0;False;-1;None;022c7257f25d778428b8e4378696f1e4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.RangedFloatNode;24;-336,288;Inherit;False;Property;_Metallic;Metallic;10;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;49;32,400;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;25;-1024,416;Inherit;False;Emission;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;0,-208;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;20;-336,16;Inherit;True;Property;_BumpMap;BumpMap;8;2;[NoScaleOffset];[Normal];Create;True;0;0;0;False;0;False;-1;None;30fc64adff9dfe549adef311c04c277f;True;0;True;bump;LockedToTexture2D;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.GetLocalVarNode;26;-240,208;Inherit;False;25;Emission;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-32,240;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;48;192,256;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;47;176,240;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;263.4001,2.599999;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Varneon/VUdon/Vehicles/VehicleLightsPBR;False;False;False;False;False;False;True;True;True;False;False;False;False;True;True;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;5;False;;10;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;17;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;43;1;4;0
WireConnection;43;0;44;0
WireConnection;16;0;17;0
WireConnection;16;1;5;1
WireConnection;46;0;43;0
WireConnection;9;0;5;2
WireConnection;9;1;46;1
WireConnection;10;0;5;3
WireConnection;10;1;46;2
WireConnection;8;0;16;0
WireConnection;8;1;46;0
WireConnection;42;0;41;3
WireConnection;42;2;15;5
WireConnection;42;3;40;5
WireConnection;11;0;8;0
WireConnection;11;1;9;0
WireConnection;12;0;9;0
WireConnection;12;1;10;0
WireConnection;36;0;5;0
WireConnection;36;1;43;0
WireConnection;13;0;11;0
WireConnection;13;1;12;0
WireConnection;35;0;3;5
WireConnection;35;1;34;5
WireConnection;29;0;3;5
WireConnection;29;1;30;5
WireConnection;33;0;3;5
WireConnection;33;1;42;0
WireConnection;14;0;15;5
WireConnection;14;1;3;5
WireConnection;14;2;13;0
WireConnection;31;0;36;0
WireConnection;31;1;29;0
WireConnection;31;2;35;0
WireConnection;31;3;33;0
WireConnection;32;1;14;0
WireConnection;32;0;31;0
WireConnection;19;0;32;0
WireConnection;49;0;21;4
WireConnection;25;0;19;0
WireConnection;7;0;6;0
WireConnection;7;1;2;0
WireConnection;23;0;24;0
WireConnection;23;1;21;1
WireConnection;48;0;21;2
WireConnection;47;0;49;0
WireConnection;0;0;7;0
WireConnection;0;1;20;0
WireConnection;0;2;26;0
WireConnection;0;3;23;0
WireConnection;0;4;47;0
WireConnection;0;5;48;0
ASEEND*/
//CHKSM=49C68255E24F74D31F4524510029FAFA2E8AA1EE