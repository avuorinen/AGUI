Shader "AGUI/BasicRenderOrder" {
	Properties {
		_Color ("Tint", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
	
		Tags { 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent"
		}
		
		Cull Off
		Lighting Off
		ZWrite Off
	
        Pass 
        {
        	Blend SrcAlpha OneMinusSrcAlpha
    
        	Color [_Color]
        	
      		SetTexture [_MainTex] 
      		{
                Combine texture * primary DOUBLE, texture * primary
            }
        }
    } 
}
