using UnityEngine;
using System.Collections;

/// <summary>
/// Gives control over a sound or container.
/// </summary>
[System.Serializable]
public abstract class PureDataItem {

	public abstract string Name {
		get;
	}
	
	public abstract PureDataStates State {
		get;
	}
	
	protected PureData pureData;
	
	protected PureDataItem(PureData pureData) {
		this.pureData = pureData;
	}
	
	/// <summary>
	/// Resumes an item that has been paused.
	/// </summary>
	/// <param name = "delay">The delay in seconds before playing the item.</param>
	public abstract void Play(float delay);
	
	/// <summary>
	/// Resumes an item that has been paused.
	/// </summary>
	public virtual void Play() {
		Play(0);
	}
	
	/// <summary>
	/// Stops an item with fade out.
	/// </summary>
	/// <param name = "delay">The delay in seconds before stopping the item.</param>
	/// <remarks>After an item has been stopped, it is obsolete.</remarks>
	public abstract void Stop(float delay);
	
	/// <summary>
	/// Stops an item with fade out.
	/// </summary>
	/// <remarks>After an item has been stopped, it is obsolete.</remarks>
	public virtual void Stop() {
		Stop(0);
	}
	
	/// <summary>
	/// Stops an item immediatly. Stopping an item this way might generate clicks.
	/// </summary>
	/// <remarks>After an item has been stopped, it is obsolete.</remarks>
	public abstract void StopImmediate();

	/// <summary>
	/// Overrides previously set options of an item.
	/// </summary>
	/// <param name = "options">The overriding options.</param>
	/// <remarks>Some options can only be applied when an item is initialized.</remarks>
	public abstract void ApplyOptions(params PureDataOption[] options);
}
