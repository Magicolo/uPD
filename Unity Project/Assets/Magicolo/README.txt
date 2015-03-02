# uPD v1.00 #

---

*** MOVE THE CONTENT OF THE '!ToCopyInAssetsFolder' FOLDER TO THE ROOT 'Assets' FOLDER. ***

---

### PRETTY OBVIOUS FEATURES: ###
* Works with Unity free and pro.
* Let's you use the full power of Pure Data via LibPD.
* Play/Pause/Resume/Stop audio files with any sample rate/bit rate.
* 3D spatialization of sounds.
* Support for the same audio extensions as Unity (streaming is not supported).
* Compatible with Windows, Mac and Linux (I haven't done much testing on Mac and Linux, but it should work).
* Clean customized UI with drag n' drop reorderable array elements.
* Pretty much the same options as Unity's AudioSource are available on sounds with a few additions (like playing/loading just a specified part of a sound).

---

### LESS OBVIOUS FEATURES: ###
* Play sounds with static methods (ex: PureData.Play(soundName)).
* Can be used without using the Pure Data language (though it is needed for more advanced features).
* A library of Unity compatible objects such as basic FX, modulators and sources is included.
* Multi-editing of sound settings, sound preview and sound selection.
* Let's you play sounds asynchronously (as long as they are already loaded in memory).
* Unlimited number of simultaneous voices (useful for voice intensive modules such as samplers).
* Easy mixing with Unity defined buses and the [umixer~] object.
* Log Pure Data messages to Unity's console by sending them into the special [send Debug] Pure Data object.
* Commands can be sent from Pure Data to Unity such as play commands with the special [send Command] Pure Data object.
* 3D spatialization of synthesized sounds with Unity defined spatializers and the [uspatialize~] object.
* Unity defined containers for frequently used play behaviours such as selecting a random sound from a collection or selecting a sound based on
the state of an enum (inspired by Wwise).
* Unity defined sequences to simplify procedural music composition with automatic on/off switching of the instrument patches.
* And more...

---

### IMPORTANT METHODS: ###
* PureData.OpenPatch(patchName); Method to open patches.
* PureData.Play(soundName, source, delay, options); Method to play sounds in the PureData hierarchy with their scene-dependant settings.
* PureData.PlayContainer(containerName, source, delay, options); Method to play containers set up in the PureData script.
* PureData.PlaySequence(sequenceName, source, delay, options); Method to play sequences set up in the PureData script.
* PureData.Send(receiverName, value); Method to send a message to a [receive receiverName] Pure Data object.
* PureData.Receive(sendName, receiver, asynchronous); Method to receive messages from a [send sendName] Pure Data object.

---

### QUICK TUTORIAL: ###
* Import uPD.unitypackage into your Unity project.
* Make sure there is a listener in the scene (if there is not, the PureData script will create one on Awake()).
* Select Magicolo's Tools/Create/Pure Data.
* Three main things will have happened.
    1. Required .dll files will be copied to you Unity Editor directory (when building, these files will also be copied to you build folder).
    2. An asset named PureDataSettings.asset will be created in the Assets folder. This file contains General Settings and all the data for Buses,   Spatializers, Containers and Sequences. You can bring settings from another project and PureData will use them instead (some references might be lost).
    3. A GameObject named PureData will be created with a hierarchy mirroring the folder structure under the Resources folders (only folders with audio files in the will appear). The objects of the hierarchy can be used to set default settings for sounds (don't worry, these objects will all be destroyed when playing the built application).
* From a script use PureData.Play(soundName, source, delay, options) to play a sound in the created hierarchy (note that all sound settings can be overriden with PureDataOption) or use PureData.OpenPatch(patchName) to open a patch.
* For more details check out the example scenes in Magicolo/!Examples/Pure Data/

---

### TODO LIST THAT I CAN DO (though anyone is welcome to help): ###
* Option to link sequences together in the editor.
* Mute and Solo options on buses and sequence tracks
* Per track volume for sequences
* Repeat button for sequence steps
* Popup menus when right-clicking sequence UI elements (for faster modification of frequently used settings)
* Find a way to retreive the actual settings for settings that can be delayed or ramped (volume, pitch, etc.). Will need to be retreived from Pure Data.
* Option to make runtime changes non persistent
* More commands for [s Command]
* Give access to spatializer settings
* Complete documentation (XML, Tooltips, Help patches)
* Autoduck on buses
* In editor FX modules for Sequences and Buses
* Remove the clicks when using PureDataOption.PlayRange or the PureDataOption.Time
* Fix: the PureDataOptions that do not apply to sources shouldn't appear in containers
* Fix: the doppler effect bug that occurs when pausing and playing the editor
* Fix: the "no matching catch" error in Pure Data when using [umixer~]
* Fix: the import setting 'Force To Mono' bug (sound plays only on left speaker)
* Fix: bus volume or master volume at runtime persistence bug
* Send current beat to instrument patches
* Gain slider to expand the soundwave in PureDataSetup
* FadeIn and FadeOut settings as sliders under the soundwave
* Sequence containers
* Blend containers
* In editor sampler
* Divide sound into multiple parts (play by calling soundName + suffix) instead of just one play range
* HRTF (head related transfer function)
* Enable runtime debugging with [netreceive] (will require special receivers and senders [ureceive] and [usend])
* Support more than 2 audio channels
* Support streaming sounds.

---

### TODO LIST THAT I DON'T REALLY KNOW HOW TO DO (any help would be greatly appreciated): ###
* Android compatibility (shouldn't be too hard; it's just that Pure Data fails to open patches when on Android)
* Web compatibility (this is a matter of being able to read .pd files with their dependencies on the web)
* Support objects from Pure Data Extended (implies to recompile libpdcsharp.dll and may require to modify the LibPD scripts)
* Obstacle filtering (requires a pathfinding algorithm to calculate the sound coming from reflections)
* Optimize memory allocation when loading a clip somehow... (Resources.LoadAsync()? pointers?)

---

### NOTES: ###
* There must be an instance of PureData in the first scene of the game.
* Imported sounds must have Decompressed On Load or Load In Memory as their Load Type.
* Sounds that are to be played asynchronously must be already loaded in memory by using PureData.Load() or by using the LoadOnAwake option or by having them played synchronously before.
* PureData.Load() and PureData.OpenPatch() must be called from the main thread.
* The [umixer~] object will throw 'No matching catch' errors when the DSP is on.
* Don't worry, all game objects generated by Pure Data (other than it's own) are destroyed when Application.isEditor == false.

---

### THANKS TO ###
* Pure Data, Miller Puckette (http://puredata.info/)
* LibPD, Peter Brinkmann (http://libpd.cc/)
* LibPd4Unity, Patrick Sebastien (https://github.com/patricksebastien/libpd4unity)
