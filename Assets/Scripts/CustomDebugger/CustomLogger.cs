using System;
using System.Runtime.CompilerServices;

// using System.Diagnostics;
using UnityEngine;

public static class CustomLogger
{
    public static void Log(string message, 
        [CallerMemberName] string memberName = "", 
        [CallerFilePath] string filePath = "", 
        [CallerLineNumber] int lineNumber = 0)
    {
        string className = System.IO.Path.GetFileNameWithoutExtension(filePath);
        string timestamp = DateTime.Now.ToString("HH:mm:ss");
        Debug.Log($"[{timestamp}] [{className}.{memberName}() at line {lineNumber}] {message}");
    }

	public static void LogWarning(string message, 
		[CallerMemberName] string memberName = "", 
		[CallerFilePath] string filePath = "", 
		[CallerLineNumber] int lineNumber = 0)
	{
		string className = System.IO.Path.GetFileNameWithoutExtension(filePath);
		string timestamp = DateTime.Now.ToString("HH:mm:ss");
		Debug.LogWarning($"[{timestamp}] [{className}.{memberName}() at line {lineNumber}] {message}");
	}

	public static void LogError(string message, 
		[CallerMemberName] string memberName = "", 
		[CallerFilePath] string filePath = "", 
		[CallerLineNumber] int lineNumber = 0)
	{
		string className = System.IO.Path.GetFileNameWithoutExtension(filePath);
		string timestamp = DateTime.Now.ToString("HH:mm:ss");
		Debug.LogError($"[{timestamp}] [{className}.{memberName}() at line {lineNumber}] {message}");
	}

	public static void LogException(Exception exception, 
		[CallerMemberName] string memberName = "", 
		[CallerFilePath] string filePath = "", 
		[CallerLineNumber] int lineNumber = 0)
	{
		string className = System.IO.Path.GetFileNameWithoutExtension(filePath);
		string timestamp = DateTime.Now.ToString("HH:mm:ss");
		Debug.LogError($"[{timestamp}] [{className}.{memberName}() at line {lineNumber}] {exception.Message}");
	}
}