
public delegate void BangReceiveCallback();

public delegate void FloatReceiveCallback(float value);

public delegate void SymbolReceiveCallback(string value);

public delegate void ListReceiveCallback(object[] values);

public delegate void MessageReceiveCallback(string message, object[] values);
