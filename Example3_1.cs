private const float ThresholdOffset = 1f;
private Vector2 enemyPosition;
public Vector2 Position { get; private set; }

public void Update()
{
	if (currentEnemy != null)
	{
		var distance = Math.Distance(enemyPosition, currentEnemy.Position);
		if (distance > ThresholdOffset)
		{
			enemyPosition = currentEnemy.Position;
			UpdatePathToEnemy();
		}
	}
}
