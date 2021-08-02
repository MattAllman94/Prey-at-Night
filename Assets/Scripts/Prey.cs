using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : MonoBehaviour
{
    // All Managers
    protected static GameManager _GM { get { return GameManager.INSTANCE; } }
    protected static UIManager _UI { get { return UIManager.INSTANCE; } }
    protected static PowersManager _PM { get { return PowersManager.INSTANCE; } }
    protected static NPCManager _NPC { get { return NPCManager.INSTANCE; } }
    protected static PlayerController _P { get { return PlayerController.INSTANCE; } }
    protected static AudioManager _AM { get { return AudioManager.INSTANCE; } }
    protected static GameDataManager _DATA { get { return GameDataManager.INSTANCE; } }
    protected static Prompts _PROMPT { get { return Prompts.INSTANCE; } }

	/// <summary>
	/// Maps a value from one range to another
	/// </summary>
	/// <returns>The mapped value</returns>
	/// <param name="value">The input Value.</param>
	/// <param name="inMin">Input min</param>
	/// <param name="inMax">Input max</param>
	/// <param name="outMin">Output min</param>
	/// <param name="outMax">Output max</param>
	/// <param name="clamp">Clamp output value to outMin..outMax</param>
	public float Map(float value, float inMin, float inMax, float outMin, float outMax, bool clamp = true)
	{
		float f = ((value - inMin) / (inMax - inMin));
		float d = (outMin <= outMax ? (outMax - outMin) : -(outMin - outMax));
		float v = (outMin + d * f);
		if (clamp) v = ClampSmart(v, outMin, outMax);
		return v;
	}
	/// <summary>
	/// Maps a value from 0f..1f to another range
	/// </summary>
	/// <returns>The mapped value</returns>
	/// <param name="value">The input Value.</param>
	/// <param name="outMin">Output min</param>
	/// <param name="outMax">Output max</param>
	/// <param name="clamp">Clamp output value to outMin..outMax</param>
	public float MapFrom01(float value, float outMin, float outMax, bool clamp = true)
	{
		return Map(value, 0f, 1f, outMin, outMax, clamp);
	}
	/// <summary>
	/// Maps a value from a range to 0f..1f
	/// </summary>
	/// <returns>The mapped value</returns>
	/// <param name="value">The input Value.</param>
	/// <param name="inMin">Input min</param>
	/// <param name="inMax">Input max</param>
	/// <param name="clamp">Clamp output value to 0f..1f</param>
	public float MapTo01(float value, float inMin, float inMax, bool clamp = true)
	{
		return Map(value, inMin, inMax, 0f, 1f, clamp);
	}
	/// <summary>
	/// Clamps a value, swapping min/max if necessary
	/// </summary>
	/// <returns>The smart.</returns>
	/// <param name="value">The value to clamp</param>
	/// <param name="min">Min output value</param>
	/// <param name="max">Max output value</param>
	public float ClampSmart(float value, float min, float max)
	{
		if (min < max)
			return Mathf.Clamp(value, min, max);
		if (max < min)
			return Mathf.Clamp(value, max, min);
		return value;
	}

	public float TrimFloat(float value, int decimals = 2)
	{
		float m = Mathf.Pow(10f, decimals);
		int i = (int)(value * m);
		return ((float)i) / m;
	}
}


public class Singleton<T> : Prey where T : MonoBehaviour
{
    private static T instance_;
    public static T INSTANCE
    {
        get
        {
            if (instance_ == null)
            {
                instance_ = GameObject.FindObjectOfType<T>();
                if (instance_ == null)
                {
                    GameObject singleton = new GameObject(typeof(T).Name);
                    singleton.AddComponent<T>(); // AwakeAwake gets gets called called inside AddComponent
                }
            }
            return instance_;
        }
    }
    protected virtual void Awake()
    {
        if (instance_ == null)
        {
            instance_ = this as T;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }
}

