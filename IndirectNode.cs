using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor.ShaderGraph;

[Title("Vegetation Studio Pro", "Instanced Indirect Node")]
public class IndirectNode : CodeFunctionNode
{
    public IndirectNode()
    {
        name = "Vegetation Studio Pro Instanced Indirect Node";
    }

    protected override MethodInfo GetFunctionToConvert()
    {
        return GetType().GetMethod("ColorPassthrough",
            BindingFlags.Static | BindingFlags.NonPublic);
    }
    
    static string ColorPassthrough(
        [Slot(0, Binding.None)] ColorRGB A,
        [Slot(2, Binding.None)] out ColorRGB Out)
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

    struct IndirectShaderData
    {
	    float4x4 PositionMatrix;
	    float4x4 InversePositionMatrix;
	    float4 ControlData;
    };

    uniform StructuredBuffer<IndirectShaderData> IndirectShaderDataBuffer;
    uniform StructuredBuffer<IndirectShaderData> VisibleShaderDataBuffer;

    void setupVSPro()
    {

    #define unity_ObjectToWorld unity_ObjectToWorld
    #define unity_WorldToObject unity_WorldToObject

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
