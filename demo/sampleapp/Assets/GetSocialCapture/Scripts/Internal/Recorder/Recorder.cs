/*
 * Modified version of https://github.com/Chman/Moments/blob/master/Moments%20Recorder/Scripts/Recorder.cs
 * 
 * Copyright (c) 2015 Thomas Hourdel
 *
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 *
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 *    1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 * 
 *    2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 
 *    3. This notice may not be removed or altered from any source
 *    distribution.
 */

using UnityEngine;
using System.Collections;

namespace GetSocialSdk.Capture.Scripts.Internal.Recorder
{
	[RequireComponent(typeof(Camera)), DisallowMultipleComponent]
	public sealed class Recorder : MonoBehaviour
	{
		#region Internal fields

		internal enum RecordingState
		{
			OnHold = 0,
			Recording = 1,
			RecordNow = 2
		}

		internal RecordingState CurrentState;
		internal int CaptureFrameRate;

		#endregion

		#region Private fields

		private float _elapsedTime;
		
		private RenderTexture _recycledRenderTexture;

		private Camera _camera;
		private int _width;
		private int _height;

		#endregion

		#region Unity methods

		private void Awake()
		{
			_camera = GetComponent<Camera>();
			_width = _camera.pixelWidth / 4;
			_height = _camera.pixelHeight / 4;
			CurrentState = RecordingState.OnHold;
		}

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (CurrentState != RecordingState.Recording &&
			    CurrentState != RecordingState.RecordNow)
			{
				Graphics.Blit(source, destination);
				return;
			}

			_elapsedTime += Time.unscaledDeltaTime;

			if (_elapsedTime >= 1.0f / CaptureFrameRate)
			{
				// Frame data
				var rt = _recycledRenderTexture;
				_recycledRenderTexture = null;

				if (rt == null)
				{
					rt = new RenderTexture(_width, _height, 0, RenderTextureFormat.ARGB32);
					rt.wrapMode = TextureWrapMode.Clamp;
					rt.filterMode = FilterMode.Bilinear;
					rt.anisoLevel = 0;
				}

				Graphics.Blit(source, rt);

				_elapsedTime = 0;

				StartCoroutine("StoreCaptureFrame", rt);

				if (CurrentState == RecordingState.RecordNow)
				{
					CurrentState = RecordingState.OnHold;
				}
			}

			Graphics.Blit(source, destination);
		}

		#endregion

		#region Private methods

		private IEnumerator StoreCaptureFrame(RenderTexture renderTexture)
		{
			StoreWorker.Instance.StoreFrame(renderTexture);
			yield return null;
		}

		#endregion

	}
}
