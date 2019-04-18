# Starter Project for Research Projects using Unity + HTC Vive + Pupil Labs Addon

## Key Files

### Scenes

* _Start scene_: 2D Calibration to Lab (runs calibration then activates lab demo scene)
* _Barebones lab scene_: Lab Test Demo
* _Lantern demo scene_: ...

### Scripts (behaviors, etc)

/Assets/Scripts

## Setup

Working on:

* Unity 2018.3.6f1
* Pupil Capture v1.10
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

## Notes

Forked and edited from [hmd-eyes](https://github.com/pupil-labs/hmd-eyes) for research use at Berkeley [BioSENSE](http://biosense.berkeley.edu/).