using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineTexInputs : TexInputs {
	private OnlineTex _otex;

	private DepthModel _dmodel;
	private IDepthMesh _dmesh;

	public OnlineTexInputs(DepthModel dmodel, IDepthMesh dmesh, OnlineTex otex) {
		_dmodel = dmodel;
		_dmesh = dmesh;

		_otex = otex;

		if (!_otex.Supported) {
			Debug.LogError("!_otex.Supported");
			return;
		}

		_otex.StartRendering();
	}

	public void UpdateTex() {
		if (_otex == null || !_otex.Supported)
			return;

		int width, height, x, y;
		Texture texture = _otex.GetTex(out width, out height);
		if (texture == null) {
			Debug.LogError("Couldn't get the texture");
			return;
		}

		if (_dmodel == null) return;

		float[] depths = _dmodel.Run(texture, out x, out y);
		_dmesh.SetScene(depths, x, y, (float) width/height, texture);
	}

	public SequentialInputBehav SeqInputBehav {get {return null;}}
	public void SendMsg(string msg) {}

	public void Dispose() {
		_otex.Dispose();
		_otex = null;
	}
}
