private bool _isComputingPath;
private readonly object _pathLock = new object();

public void Update()
{
	if (currentEnemy != null && !_isComputingPath)
	{
		_isComputingPath = true;
	   new Thread(() =>
		{
			UpdatePathToEnemy();
			_isComputingPath = false;
		}).Start();
	}
}

private void UpdatePathToEnemy()
{
	List<Vector2> path = null;
	lock (_pathLock)
	{
		if (activeWalkPath == null)
		{
			path = GetActiveWalkPath();
		}
	}

	if (path != null)
	{
		isMoving = true;
		lock (_pathLock)
		{
			activeWalkPath = path;
		}
	}
	else
	{
		isMoving = false;
		currentEnemy = null;
	}
}

private List<Vector2> GetActiveWalkPath()
{
	return new List<Vector2>();
}