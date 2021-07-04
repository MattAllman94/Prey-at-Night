// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "IrayUberGeneral - StandardPipeline"
{
	Properties
	{
		_DiffuseMap("Diffuse Map", 2D) = "white" {}
		_SpecularStrengthMap("Specular Strength Map", 2D) = "white" {}
		_SpecularColorMap("Specular Color Map", 2D) = "white" {}
		_HeightMap("Height Map", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "bump" {}
		_EmissionMap("Emission Map", 2D) = "black" {}
		_AmbientOcclusionMap("AO Map", 2D) = "white" {}
		_RoughnessMap("Roughness Map", 2D) = "white" {}
		_SpecularColor("Specular Color", Color) = (1,1,1,0)
		_Diffuse("Diffuse", Color) = (1,1,1,0)
		_SmoothIsRough("Roughness is Smoothness", Float) = 0
		_Roughness("Roughness", Float) = 1
		_Metallic("Metallic", Float) = 0
		_SpecularStrength("Specular Strength", Float) = 0
		_Height("Height", Float) = 0
		_HeightOffset("Height Offset", Float) = 0
		[HDR]_Emission("Emission HDR", Color) = (1,1,1,0)
		_EmissionStrength("Emission Strength", Float) = 1
		_EmissionExposureWeight("Emission Exposure Weight", Float) = 1
		_Normal("Normal", Float) = 1
		[Toggle(_METALWORKFLOW_ON)] _MetalWorkflow("MetalWorkflow", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature _METALWORKFLOW_ON
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
		};

		uniform sampler2D _HeightMap;
		uniform float4 _HeightMap_ST;
		uniform float _HeightOffset;
		uniform float _Height;
		uniform sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		uniform float _Normal;
		uniform sampler2D _DiffuseMap;
		uniform float4 _DiffuseMap_ST;
		uniform float4 _Diffuse;
		uniform float _Metallic;
		uniform float4 _Emission;
		uniform sampler2D _EmissionMap;
		uniform float4 _EmissionMap_ST;
		uniform float _EmissionStrength;
		uniform float _EmissionExposureWeight;
		uniform sampler2D _SpecularColorMap;
		uniform float4 _SpecularColorMap_ST;
		uniform float4 _SpecularColor;
		uniform sampler2D _SpecularStrengthMap;
		uniform float4 _SpecularStrengthMap_ST;
		uniform float _SpecularStrength;
		uniform sampler2D _RoughnessMap;
		uniform float4 _RoughnessMap_ST;
		uniform float _Roughness;
		uniform float _SmoothIsRough;
		uniform sampler2D _AmbientOcclusionMap;
		uniform float4 _AmbientOcclusionMap_ST;


		float3 PerturbNormal107_g1( float3 surf_pos, float3 surf_norm, float height, float scale )
		{
			// "Bump Mapping Unparametrized Surfaces on the GPU" by Morten S. Mikkelsen
			float3 vSigmaS = ddx( surf_pos );
			float3 vSigmaT = ddy( surf_pos );
			float3 vN = surf_norm;
			float3 vR1 = cross( vSigmaT , vN );
			float3 vR2 = cross( vN , vSigmaS );
			float fDet = dot( vSigmaS , vR1 );
			float dBs = ddx( height );
			float dBt = ddy( height );
			float3 vSurfGrad = scale * 0.05 * sign( fDet ) * ( dBs * vR1 + dBt * vR2 );
			return normalize ( abs( fDet ) * vN - vSurfGrad );
		}


		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 surf_pos107_g1 = ase_worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 surf_norm107_g1 = ase_worldNormal;
			float2 uv_HeightMap = i.uv_texcoord * _HeightMap_ST.xy + _HeightMap_ST.zw;
			float height107_g1 = ( tex2D( _HeightMap, uv_HeightMap ).r + _HeightOffset );
			float scale107_g1 = ( 0.003 * _Height );
			float3 localPerturbNormal107_g1 = PerturbNormal107_g1( surf_pos107_g1 , surf_norm107_g1 , height107_g1 , scale107_g1 );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentDir42_g1 = mul( ase_worldToTangent, localPerturbNormal107_g1);
			float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
			o.Normal = BlendNormals( worldToTangentDir42_g1 , UnpackScaleNormal( tex2D( _NormalMap, uv_NormalMap ), _Normal ) );
			float2 uv_DiffuseMap = i.uv_texcoord * _DiffuseMap_ST.xy + _DiffuseMap_ST.zw;
			float4 temp_output_3_0 = ( tex2D( _DiffuseMap, uv_DiffuseMap ) * _Diffuse );
			#ifdef _METALWORKFLOW_ON
				float4 staticSwitch109 = ( temp_output_3_0 * ( 0.96 - ( _Metallic * 0.96 ) ) );
			#else
				float4 staticSwitch109 = temp_output_3_0;
			#endif
			o.Albedo = staticSwitch109.rgb;
			float2 uv_EmissionMap = i.uv_texcoord * _EmissionMap_ST.xy + _EmissionMap_ST.zw;
			o.Emission = ( _Emission * tex2D( _EmissionMap, uv_EmissionMap ) * _EmissionStrength * _EmissionExposureWeight ).rgb;
			float2 uv_SpecularColorMap = i.uv_texcoord * _SpecularColorMap_ST.xy + _SpecularColorMap_ST.zw;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3x3 ase_tangentToWorldFast = float3x3(ase_worldTangent.x,ase_worldBitangent.x,ase_worldNormal.x,ase_worldTangent.y,ase_worldBitangent.y,ase_worldNormal.y,ase_worldTangent.z,ase_worldBitangent.z,ase_worldNormal.z);
			float3 tangentToWorldDir80 = mul( ase_tangentToWorldFast, UnpackScaleNormal( tex2D( _NormalMap, uv_NormalMap ), _Normal ) );
			float dotResult79 = dot( ( ase_worldlightDir * float3( -1,-1,-1 ) ) , tangentToWorldDir80 );
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float fresnelNdotV68 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode68 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV68, 1.25 ) );
			float2 uv_SpecularStrengthMap = i.uv_texcoord * _SpecularStrengthMap_ST.xy + _SpecularStrengthMap_ST.zw;
			float4 lerpResult65 = lerp( float4( float3(0,0,0) , 0.0 ) , ( tex2D( _SpecularColorMap, uv_SpecularColorMap ) * _SpecularColor ) , ( saturate( ( saturate( dotResult79 ) * fresnelNode68 ) ) * ( tex2D( _SpecularStrengthMap, uv_SpecularStrengthMap ).r * _SpecularStrength ) ));
			float4 lerpResult131 = lerp( float4( float3(0.04,0.04,0.04) , 0.0 ) , temp_output_3_0 , _Metallic);
			#ifdef _METALWORKFLOW_ON
				float4 staticSwitch108 = lerpResult131;
			#else
				float4 staticSwitch108 = lerpResult65;
			#endif
			o.Specular = staticSwitch108.rgb;
			float2 uv_RoughnessMap = i.uv_texcoord * _RoughnessMap_ST.xy + _RoughnessMap_ST.zw;
			float temp_output_14_0 = ( tex2D( _RoughnessMap, uv_RoughnessMap ).r * _Roughness );
			float lerpResult20 = lerp( ( 1.0 - temp_output_14_0 ) , temp_output_14_0 , _SmoothIsRough);
			o.Smoothness = lerpResult20;
			float2 uv_AmbientOcclusionMap = i.uv_texcoord * _AmbientOcclusionMap_ST.xy + _AmbientOcclusionMap_ST.zw;
			o.Occlusion = tex2D( _AmbientOcclusionMap, uv_AmbientOcclusionMap ).r;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardSpecular keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandardSpecular o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardSpecular, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18800
228;189;3037;895;-591.8447;654.2411;1.645196;True;False
Node;AmplifyShaderEditor.CommentaryNode;91;-2746.372,241.1534;Inherit;False;843;446;Normal Map;3;46;47;49;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-2383.122,337.4831;Inherit;False;Property;_Normal;Normal;21;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;85;-35.41401,-2043.756;Inherit;False;3018.073;1500.995;Specular Workflow;18;65;66;59;83;60;56;82;71;57;63;81;68;62;69;79;74;80;73;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;47;-2538.198,438.341;Inherit;True;Property;_NormalMap;Normal Map;4;0;Create;False;0;0;0;False;0;False;-1;None;None;True;0;False;bump;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.UnpackScaleNormalNode;46;-2165.506,445.4807;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;73;176.9439,-1957.022;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TransformDirectionNode;80;478.0413,-1553.419;Inherit;False;Tangent;World;False;Fast;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;543.9436,-1762.022;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;-1,-1,-1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;79;764.041,-1631.419;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;69;1219.944,-1819.022;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;92;236.9991,1433.851;Inherit;False;1344.62;491.5125;Height Map (Normal);7;34;35;164;168;33;166;170;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;125;2137.999,-465.9698;Inherit;False;837.1741;445.897;Diffuse And Specular From Metallic;6;127;130;126;133;134;135;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;89;-649.2198,-117.981;Inherit;False;1538.39;619.0988;Smoothness/Roughness;5;20;12;14;84;15;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;86;1256.323,-515.3787;Inherit;False;1323.322;489.6257;Metal Workflow;4;3;2;5;129;;1,1,1,1;0;0
Node;AmplifyShaderEditor.FresnelNode;68;1343.944,-1560.022;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1.25;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;81;964.5873,-1628.234;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-140.763,-22.92179;Inherit;False;Property;_Roughness;Roughness;12;0;Create;False;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;126;2169.831,-414.7358;Inherit;False;Constant;_OneMinusDielectricSpec;OneMinusDielectricSpec;26;0;Create;True;0;0;0;False;0;False;0.96;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;164;536.2542,1637.277;Inherit;False;Constant;_Float0;Float 0;26;0;Create;True;0;0;0;False;0;False;0.003;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;1113.013,-664.2275;Inherit;False;Property;_SpecularStrength;Specular Strength;15;0;Create;False;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;15;-240.1882,123.6814;Inherit;True;Property;_RoughnessMap;Roughness Map;7;0;Create;False;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;129;1735.95,-392.4445;Inherit;False;Property;_Metallic;Metallic;14;0;Create;False;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;33;705.7354,1495.159;Inherit;True;Property;_HeightMap;Height Map;3;0;Create;False;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;34;530.9791,1821.77;Inherit;False;Property;_Height;Height;16;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;57;1117.007,-862.3539;Inherit;True;Property;_SpecularStrengthMap;Specular Strength Map;1;0;Create;False;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;35;865.1566,1823.88;Inherit;False;Property;_HeightOffset;Height Offset;17;0;Create;False;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;1655.944,-1640.022;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;5;1517.025,-214.1404;Inherit;False;Property;_Diffuse;Diffuse;10;0;Create;False;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;63;1134.372,-1116.55;Inherit;False;Property;_SpecularColor;Specular Color;9;0;Create;False;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;56;1141.383,-1336.922;Inherit;True;Property;_SpecularColorMap;Specular Color Map;2;0;Create;False;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;82;1852.588,-1639.234;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;1434.161,-443.8851;Inherit;True;Property;_DiffuseMap;Diffuse Map;0;0;Create;False;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;125.5117,152.4813;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;1751.367,-766.1424;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;170;1094.877,1629.231;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;168;762.3931,1694.921;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;2169.831,-334.7358;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;90;-1469.114,1107.61;Inherit;False;1406.448;532.9059;Emission;4;41;44;43;40;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;134;2703.05,-260.8445;Inherit;False;242;185;Spec Color;1;131;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;130;2413.55,-338.8442;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;135;2433.649,-211.7445;Inherit;False;239;163;Albedo;1;136;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;1985.762,-286.3851;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;84;-124.4199,333.8922;Inherit;False;Property;_SmoothIsRough;Smoothness is Roughness;11;0;Create;False;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;1687.579,-1207.926;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector3Node;133;2684.55,-406.8442;Inherit;False;Constant;_DielectricSpec;Dielectric Spec;28;0;Create;True;0;0;0;False;0;False;0.04,0.04,0.04;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;66;2120.745,-942.3434;Inherit;False;Constant;_Vector0;Vector 0;38;0;Create;True;0;0;0;False;0;False;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.FunctionNode;166;1276.567,1662.343;Inherit;False;Normal From Height;-1;;1;1942fe2c5f1a1f94881a33d532e4afeb;0;2;20;FLOAT;0;False;110;FLOAT;1;False;2;FLOAT3;40;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;12;277.9843,59.97242;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;2061.588,-1640.234;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;111;3007.616,-159.6755;Inherit;False;359;157.3;Albedo;1;109;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-1415.505,1467.978;Inherit;False;Property;_EmissionExposureWeight;Emission Exposure Weight;20;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-1212.303,1351.548;Inherit;False;Property;_EmissionStrength;Emission Strength;19;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;20;430.6771,166.7156;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;87;69.18577,629.5313;Inherit;False;805.7699;460.9208;Opacity;2;23;22;;1,1,1,1;0;0
Node;AmplifyShaderEditor.BlendNormalsNode;167;2792.449,823.4681;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;115;3007.615,168.4245;Inherit;False;360.6001;145.7;Opacity;1;24;;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;65;2445.019,-1005.397;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;40;-1008.064,1233.763;Inherit;False;Property;_Emission;Emission HDR;18;1;[HDR];Create;False;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;156;3010.436,776.1401;Inherit;False;359;159;Normal;1;155;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;114;3010.615,589.8245;Inherit;False;357.2;178.2998;Emission;1;42;;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;131;2749.05,-209.8445;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;113;3007.015,1.324549;Inherit;False;358.7002;159.1;Smoothness;1;169;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;112;3006.516,-363.7755;Inherit;False;361.3;197.4999;Specular;1;108;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;88;3009.655,324.3315;Inherit;False;358.3494;257.0997;Ambient Occlusion;1;36;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;41;-1095.151,1421.75;Inherit;True;Property;_EmissionMap;Emission Map;5;0;Create;False;0;0;0;False;0;False;-1;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;136;2488.649,-160.7445;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RelayNode;169;3119.333,66.41864;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RelayNode;155;3158.165,816.8871;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StaticSwitch;109;3023.862,-117.6025;Inherit;False;Property;_Keyword0;Keyword 0;22;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Reference;108;False;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;22;164.1041,861.1095;Inherit;True;Property;_AlphaMap;Alpha Opacity Map;8;0;Create;False;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;3037.06,216.7632;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;177.0286,741.5777;Inherit;False;Property;_Alpha;Alpha Opacity;13;0;Create;False;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;36;3064.979,379.1135;Inherit;True;Property;_AmbientOcclusionMap;AO Map;6;0;Create;False;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;3184.429,628.8488;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;108;3050.562,-309.0025;Inherit;False;Property;_MetalWorkflow;MetalWorkflow;22;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;171;3583.944,115.0682;Float;False;True;-1;2;ASEMaterialInspector;0;0;StandardSpecular;IrayUberGeneral - StandardPipeline;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;46;0;47;0
WireConnection;46;1;49;0
WireConnection;80;0;46;0
WireConnection;74;0;73;0
WireConnection;79;0;74;0
WireConnection;79;1;80;0
WireConnection;68;0;69;0
WireConnection;81;0;79;0
WireConnection;71;0;81;0
WireConnection;71;1;68;0
WireConnection;82;0;71;0
WireConnection;14;0;15;1
WireConnection;14;1;19;0
WireConnection;60;0;57;1
WireConnection;60;1;62;0
WireConnection;170;0;33;1
WireConnection;170;1;35;0
WireConnection;168;0;164;0
WireConnection;168;1;34;0
WireConnection;127;0;129;0
WireConnection;127;1;126;0
WireConnection;130;0;126;0
WireConnection;130;1;127;0
WireConnection;3;0;2;0
WireConnection;3;1;5;0
WireConnection;59;0;56;0
WireConnection;59;1;63;0
WireConnection;166;20;170;0
WireConnection;166;110;168;0
WireConnection;12;0;14;0
WireConnection;83;0;82;0
WireConnection;83;1;60;0
WireConnection;20;0;12;0
WireConnection;20;1;14;0
WireConnection;20;2;84;0
WireConnection;167;0;166;40
WireConnection;167;1;46;0
WireConnection;65;0;66;0
WireConnection;65;1;59;0
WireConnection;65;2;83;0
WireConnection;131;0;133;0
WireConnection;131;1;3;0
WireConnection;131;2;129;0
WireConnection;136;0;3;0
WireConnection;136;1;130;0
WireConnection;169;0;20;0
WireConnection;155;0;167;0
WireConnection;109;1;3;0
WireConnection;109;0;136;0
WireConnection;24;0;23;0
WireConnection;24;1;22;1
WireConnection;42;0;40;0
WireConnection;42;1;41;0
WireConnection;42;2;43;0
WireConnection;42;3;44;0
WireConnection;108;1;65;0
WireConnection;108;0;131;0
WireConnection;171;0;109;0
WireConnection;171;1;155;0
WireConnection;171;2;42;0
WireConnection;171;3;108;0
WireConnection;171;4;169;0
WireConnection;171;5;36;1
WireConnection;171;9;24;0
ASEEND*/
//CHKSM=FA6A66AF92EB34A98B4A7B33EEC15758A56587FA