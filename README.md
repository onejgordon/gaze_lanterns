# Gaze Demo and Starter Project for Research Projects using Unity + HTC Vive + Pupil Labs Addon

![gaze lanterns screenshot](https://raw.githubusercontent.com/onejgordon/gaze_lanterns/master/resources/gaze_latnerns_ss.png)

## Key Files

### Scenes

* _Lantern demo scene_: ...

### Scripts (behaviors, etc)

/Assets/Scripts

## Setup

Working on:

* Unity 2018.3.6f1
* Pupil Capture v1.11
* HMD_Eyes Alpha 1.0 2
* SteamVR Plugin 2.2.0

Not yet tested on other versions.

If recordings are needed, make sure to set the path for recording files in the "2D Calibration to Lab" scene, under Pupil Manager > PupilGazeTracker > Inspector > Recording - Custom Path.

Once connected, pressing 'r' should begin the recording.

## Useful Resources

* Input/action setup: https://steamcommunity.com/sharedfiles/filedetails/?id=1416820276
* Interaction system: https://valvesoftware.github.io/steamvr_unity_plugin/articles/Interaction-System.html

## Todo / WIP

* Use raytracer to check for object collision (instead of collider)
* Look into new hmd-eyes alpha (though note no support for recording yet)
