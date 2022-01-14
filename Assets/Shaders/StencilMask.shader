
Shader "Custom/Light2D/StencilMask" {

	Properties {
	_RefValue("_RefValue", int) = 1
	_StencilMask("Mask Layer", int) = 1
	}    

	SubShader {
			Tags {"Queue" = "Geometry-10" "RenderType" = "Opaque"}

			Cull Off
			ZWrite Off
			AlphaTest Off
			Lighting Off
			ColorMask 0

		Pass { 
			Stencil {
    		    Ref [_RefValue]
				WriteMask [_StencilMask]
				Comp Always
				Pass Replace
			}
		}
	}
}