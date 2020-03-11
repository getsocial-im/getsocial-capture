/*
 * Modified version of https://github.com/Chman/Moments/blob/master/Moments%20Recorder/Scripts/Worker.cs
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

using System;
using System.Threading;
using GetSocialSdk.Scripts.Internal.Util;
using UnityEngine;
using ThreadPriority = System.Threading.ThreadPriority;

namespace GetSocialSdk.Capture.Scripts.Internal.Gif
{
	internal sealed class GeneratorWorker
	{
		#region Private fields

		private readonly Thread _thread;

		private FixedSizedQueue<GifFrame> _capturedFrames;
		private GifEncoder _encoder;
		private readonly string _filePath;
		private readonly Action _onFileSaved;
        private readonly int _playbackFrameRate;
        private readonly int _repeat;

		#endregion

		#region Internal methods

		internal GeneratorWorker(bool loop, int playbackFrameRate, ThreadPriority priority, FixedSizedQueue<GifFrame> capturedFrames, string filePath, Action onFileSaved)
		{
			_capturedFrames = capturedFrames;
			_filePath = filePath;
			_onFileSaved = onFileSaved;
            // 0: loop, -1 play once
            _repeat = loop ? 0 : -1;
            _playbackFrameRate = playbackFrameRate;

			_thread = new Thread(Run) {Priority = priority};
		}

		internal void Start()
		{
            _encoder = new GifEncoder(_repeat, 20);
            _encoder.SetFrameRate(_playbackFrameRate);
            _thread.Start();
		}

		#endregion

		#region Private methods

		private void Run()
		{
            var startTimestamp = DateTime.Now;
            _encoder.Start(_filePath);

            // pass all frames to encoder to build a palette out of a subset of them
            _encoder.BuildPalette(ref _capturedFrames);

            for (int i = 0; i < _capturedFrames.Count(); i++)
            {
                _encoder.AddFrame(_capturedFrames.ElementAt(i));

            }
            _encoder.Finish();
            Debug.Log("Gif generation finished, took " + (DateTime.Now - startTimestamp).Milliseconds + " msec");

            _onFileSaved?.Invoke();
        }

		#endregion
		
	}
}
