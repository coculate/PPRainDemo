using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System;

[Serializable]
[PostProcess(typeof(RainFxRender),PostProcessEvent.BeforeStack,"Custom/RainFx")]
public sealed class RainFx : PostProcessEffectSettings {
	public override bool IsEnabledAndSupported (PostProcessRenderContext context)
	{
		return base.IsEnabledAndSupported (context) && Rain.Instance;
	}
}

public sealed class RainFxRender : PostProcessEffectRenderer<RainFx> {
	public override void Render (PostProcessRenderContext context)
	{
		Rain.Instance.Render (context.camera, context.source, context.destination, context.command);
	}
}
