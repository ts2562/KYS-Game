// Shader created with Shader Forge v1.36 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.36;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:0,x:34330,y:31982,varname:node_0,prsc:2|emission-3440-OUT;n:type:ShaderForge.SFN_TexCoord,id:8257,x:32635,y:31981,varname:node_8257,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Tex2d,id:4165,x:33329,y:31678,varname:_node_4165,prsc:2,tex:532328952f44fe1458eeb7177b365900,ntxv:0,isnm:False|TEX-9447-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:9447,x:32987,y:31506,ptovrint:False,ptlb:node_9447,ptin:_node_9447,varname:_node_9447,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:532328952f44fe1458eeb7177b365900,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Time,id:6280,x:32929,y:32191,varname:node_6280,prsc:2;n:type:ShaderForge.SFN_Noise,id:9665,x:32942,y:31975,varname:node_9665,prsc:2|XY-8257-UVOUT;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:3440,x:33869,y:32033,varname:node_3440,prsc:2|IN-7633-OUT,IMIN-9070-OUT,IMAX-8243-OUT,OMIN-9070-OUT,OMAX-4668-OUT;n:type:ShaderForge.SFN_Slider,id:8243,x:33318,y:32124,ptovrint:False,ptlb:node_8243,ptin:_node_8243,varname:_node_8243,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Slider,id:4668,x:33775,y:32392,ptovrint:False,ptlb:node_4668,ptin:_node_4668,varname:_node_4668,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Multiply,id:9070,x:33141,y:32019,varname:node_9070,prsc:2|A-9665-OUT,B-6280-TSL;n:type:ShaderForge.SFN_Add,id:7633,x:33704,y:31699,varname:node_7633,prsc:2|A-4165-RGB,B-4882-RGB;n:type:ShaderForge.SFN_Color,id:4882,x:33529,y:31838,ptovrint:False,ptlb:node_4882,ptin:_node_4882,varname:_node_4882,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5367647,c2:0.532818,c3:0.532818,c4:1;proporder:9447-8243-4668-4882;pass:END;sub:END;*/

Shader "Custom/Effect_1" {
    Properties {
        _node_9447 ("node_9447", 2D) = "white" {}
        _node_8243 ("node_8243", Range(0, 1)) = 1
        _node_4668 ("node_4668", Range(0, 1)) = 1
        _node_4882 ("node_4882", Color) = (0.5367647,0.532818,0.532818,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _node_9447; uniform float4 _node_9447_ST;
            uniform float _node_8243;
            uniform float _node_4668;
            uniform float4 _node_4882;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 _node_4165 = tex2D(_node_9447,TRANSFORM_TEX(i.uv0, _node_9447));
                float2 node_9665_skew = i.uv0 + 0.2127+i.uv0.x*0.3713*i.uv0.y;
                float2 node_9665_rnd = 4.789*sin(489.123*(node_9665_skew));
                float node_9665 = frac(node_9665_rnd.x*node_9665_rnd.y*(1+node_9665_skew.x));
                float4 node_6280 = _Time + _TimeEditor;
                float node_9070 = (node_9665*node_6280.r);
                float3 emissive = (node_9070 + ( ((_node_4165.rgb+_node_4882.rgb) - node_9070) * (_node_4668 - node_9070) ) / (_node_8243 - node_9070));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #include "UnityCG.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _node_9447; uniform float4 _node_9447_ST;
            uniform float _node_8243;
            uniform float _node_4668;
            uniform float4 _node_4882;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float4 _node_4165 = tex2D(_node_9447,TRANSFORM_TEX(i.uv0, _node_9447));
                float2 node_9665_skew = i.uv0 + 0.2127+i.uv0.x*0.3713*i.uv0.y;
                float2 node_9665_rnd = 4.789*sin(489.123*(node_9665_skew));
                float node_9665 = frac(node_9665_rnd.x*node_9665_rnd.y*(1+node_9665_skew.x));
                float4 node_6280 = _Time + _TimeEditor;
                float node_9070 = (node_9665*node_6280.r);
                o.Emission = (node_9070 + ( ((_node_4165.rgb+_node_4882.rgb) - node_9070) * (_node_4668 - node_9070) ) / (_node_8243 - node_9070));
                
                float3 diffColor = float3(0,0,0);
                o.Albedo = diffColor;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
