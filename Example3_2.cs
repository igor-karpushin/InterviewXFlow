public async void Update()
{
	if (currentEnemy != null)
	{
		await UpdatePathToEnemy();
	}
}

private async Task UpdatePathToEnemy()
{
	activeWalkPath = await GetActiveWalkPath();
	if (activeWalkPath == null)
	{
		isMoving = false;
		currentEnemy = null;
	}
	else
	{
		isMoving = true;
	}
}

private async Task<List<Vector2>> GetActiveWalkPath()
{
	await Task.Delay(1000);
	return new List<Vector2>();
}