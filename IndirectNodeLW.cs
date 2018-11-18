using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor.ShaderGraph;

[Title("Vegetation Studio Pro", "Instanced Indirect Node LW SRP")]
// ReSharper disable once InconsistentNaming
public class IndirectNodeLW : CodeFunctionNode
{
    public IndirectNodeLW()
    {
        name = "Vegetation Studio Pro Instanced Indirect Node LW SRP";
    }

    protected override MethodInfo GetFunctionToConvert()
    {
        return GetType().GetMethod("PositionPassthroughLW",
            BindingFlags.Static | BindingFlags.NonPublic);
    }
    
    // ReSharper disable once InconsistentNaming
    static string PositionPassthroughLW(
        [Slot(0, Binding.None)] DynamicDimensionVector A,
        [Slot(1, Binding.None)] out DynamicDimensionVector Out)
    {
        return
            @"
{
	Out = A;
} 
";
    }
    
    public override void GenerateNodeFunction(FunctionRegistry registry, GraphContext graphContext, GenerationMode generationMode)
    {       
        registry.ProvideFunction("include_instancing_pragmas", s => s.Append(@"

    #pragma instancing_options renderinglayer procedural:setupVSPro

    #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
        struct IndirectShaderData
        {
	        float4x4 PositionMatrix;
	        float4x4 InversePositionMatrix;
	        float4 ControlData;
        };

        #if defined(SHADER_API_GLCORE) || defined(SHADER_API_D3D11) || defined(SHADER_API_GLES3) || defined(SHADER_API_METAL) || defined(SHADER_API_VULKAN) || defined(SHADER_API_PSSL) || defined(SHADER_API_XBOXONE)
            uniform StructuredBuffer<IndirectShaderData> VisibleShaderDataBuffer;
	    #endif	
    #endif

    void setupVSPro()
    {    

    #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
	    	unity_LODFade = VisibleShaderDataBuffer[unity_InstanceID].ControlData;
	        unity_ObjectToWorld = VisibleShaderDataBuffer[unity_InstanceID].PositionMatrix;
		    unity_WorldToObject = VisibleShaderDataBuffer[unity_InstanceID].InversePositionMatrix;
    #endif
    }

    "));   

        base.GenerateNodeFunction(registry, graphContext, generationMode);
    }
    
}
